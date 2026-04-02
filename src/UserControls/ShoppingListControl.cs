using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Demo_PIG_Tool.BudgetTool;
using BudgetDialogs;
using Demo_PIG_Tool.Manager;

/* Author: Gabriel Ory
 * This file contains the code for creating the UserControl for the Shopping sub-tool.
 */

namespace ShoppingTracker
{
    public partial class ShoppingListControl : UserControl
    {

        // shopping list
        private List<ShoppingList> _allShoppingLists = new();

        // helpers to load and save lists
        private readonly LoadList _loadList = new();
        private readonly SaveList _saveList = new();

        // UI components
        private ComboBox cboLists = new();
        private Button btnCreateList = new();
        private Button btnAddItem = new();
        private Button btnEditItem = new();
        private Button btnRemoveItem = new();
        private Panel mainPanel = new Panel();
        private Label titleLabel = new Label();
        private ListView listViewSummary = new();
        private TextBox txtSummary = new();

        // constructor to set up the UI and load data
        public ShoppingListControl()
        {
            Dock = DockStyle.Fill;
            BuildUI();
            LoadData();
        }

        // builds the user interface with a header, top bar for list selection and actions, and a split view for the shopping list in table and text formats
        private void BuildUI()
        {
            // make header panel 
            mainPanel.BackColor = Color.FromArgb(240, 240, 240);
            mainPanel.Dock = DockStyle.Top;
            mainPanel.AutoSize = true;
            mainPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            mainPanel.Padding = new Padding(20, 20, 20, 10);

            // add title label to header
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(0, 120, 215);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Text = "Shopping Tool";

            mainPanel.Controls.Add(titleLabel);

            // make top buttons/dropdown panel
            var top = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10),
                AutoScroll = true
            };

            // label for the dropdown
            var lbl = new Label
            {
                Text = "Select List:",
                AutoSize = true,
                Padding = new Padding(0, 10, 0, 0),
            };

            // customize dropdown
            cboLists.Width = 240;
            cboLists.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLists.SelectedIndexChanged += (_, __) => RenderListItems();

            // button to create a new list
            btnCreateList.Text = "Create List";
            btnCreateList.Click += (_, __) => CreateList();
            BlueButton(btnCreateList);

            // button to add an item to a list
            btnAddItem.Text = "Add Item";
            btnAddItem.Click += (_, __) => AddItemDialog();
            BlueButton(btnAddItem);

            // button to edit an item in a list (change quantity, price, or best store)
            btnEditItem.Text = "Edit Item";
            btnEditItem.Click += (_, __) => EditItemDialog();
            BlueButton(btnEditItem);

            // button to remove an item from a list
            btnRemoveItem.Text = "Remove Item";
            btnRemoveItem.Click += (_, __) => RemoveItemDialog();
            RedButton(btnRemoveItem);

            top.Controls.Add(lbl);
            top.Controls.Add(cboLists);
            top.Controls.Add(btnCreateList);
            top.Controls.Add(btnAddItem);
            top.Controls.Add(btnEditItem);
            top.Controls.Add(btnRemoveItem);

            // make a split container for ListView and TextBox
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 350
            };

            // customize ListView style
            listViewSummary = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
            };

            // show columns for item name, best store, price, quantity, and total cost
            listViewSummary.Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader { Text = "Item", Width = 200 },
                new ColumnHeader { Text = "Best Store", Width = 200 },
                new ColumnHeader { Text = "Price", Width = 200 },
                new ColumnHeader { Text = "Quantity", Width = 200 },
                new ColumnHeader { Text = "Total Cost", Width = 200 }
            });

            split.Panel1.Controls.Add(listViewSummary);

            // customize TextBox style for the summary
            txtSummary.Dock = DockStyle.Fill;
            txtSummary.Multiline = true;
            txtSummary.ScrollBars = ScrollBars.Vertical;
            txtSummary.ReadOnly = true;
            txtSummary.BackColor = Color.White;
            txtSummary.BorderStyle = BorderStyle.FixedSingle;

            split.Panel2.Controls.Add(txtSummary);

            // add all panels to the main control
            Controls.Add(split);
            Controls.Add(top);
            Controls.Add(mainPanel);

        }

 
        // styling for the blue buttons
        private static void BlueButton(Button b)
        {
            b.BackColor = Color.FromArgb(0, 120, 215);
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Size = new Size(130, 35);
            b.Font = new Font("Segoe UI", 10F);
        }

        // styling for the red buttons
        private static void RedButton(Button b)
        {
            b.BackColor = Color.Red;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Size = new Size(130, 35);
            b.Font = new Font("Segoe UI", 10F);
        }


        // loads the shopping lists from storage and refreshes the UI
        private void LoadData()
        {
            try
            {
                _allShoppingLists = _loadList.LoadLists() ?? new List<ShoppingList>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to load shopping lists.\n\n{ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _allShoppingLists = new List<ShoppingList>();
            }

            RefreshListDropdown();
            RenderListItems();
        }

        // saves the shopping lists to storage and updates the docx 
        private void SaveLists()
        {
            try {
                _saveList.SaveLists(_allShoppingLists); 
                SubToolManager.UpdateDocx();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to save shopping lists.\n\n{ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // creates a new shopping list with a name provided by the user and refreshes the UI
        private void CreateList()
        {
            var name = Prompt("Shopping List Name", "Enter the name of the shopping list:");
            if (string.IsNullOrWhiteSpace(name)) return;

            var list = new ShoppingList(name.Trim(), new List<Grocery>());
            _allShoppingLists.Add(list);
            SaveLists();
            RefreshListDropdown(list);
            RenderListItems();
        }

        // shows a dialog to add a new item to the selected shopping list, then saves and refreshes the UI
        private void AddItemDialog()
        {
            var list = GetSelectedList();
            if (list == null)
            {
                MessageBox.Show(this, "Create/select a shopping list first.", "No List", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new GroceryAddForm();
            if (dlg.ShowDialog() != DialogResult.OK) return;

            list.AddItem(new Grocery(dlg.ItemName, dlg.BestStore, dlg.Price, dlg.Quantity));
            SaveLists();
            RenderListItems();
        }

        // shows a dialog to remove an item from the selected shopping list, then saves and refreshes the UI
        private void RemoveItemDialog()
        {
            var list = GetSelectedList();
            if (list == null) return;

            var name = Prompt("Remove Item", "Enter the name of the item to remove:");
            if (string.IsNullOrWhiteSpace(name)) return;

            list.RemoveItem(name.Trim());
            SaveLists();
            RenderListItems();
        }

        // shows a dialog to edit an item in the selected shopping list (change quantity, price, or best store), then saves and refreshes the UI
        private void EditItemDialog()
        {
            var list = GetSelectedList();
            if (list == null) return;

            using var dlg = new GroceryEditForm();
            if (dlg.ShowDialog() != DialogResult.OK) return;

            if (dlg.Action == GroceryEditAction.ChangeQuantity)
                list.changeItemQuantity(dlg.TargetName, dlg.NewQuantity);
            else if (dlg.Action == GroceryEditAction.ChangePrice)
                list.changeItemPrice(dlg.TargetName, dlg.NewPrice);
            else if (dlg.Action == GroceryEditAction.ChangeBestStore)
                list.changeItemBestStore(dlg.TargetName, dlg.NewBestStore);

            SaveLists();
            RenderListItems();
        }

        // renders the items of the selected shopping list in the ListView and updates the text summary
        private void RenderListItems()
        {
            var list = GetSelectedList();
            if (list == null)
            {
                listViewSummary.Items.Clear();
                txtSummary.Text = "No shopping list selected.";
                return;
            }

            // clear previous items
            listViewSummary.Items.Clear();

            // add items to ListView
            foreach (var item in list.Items)
            {
                var lvi = new ListViewItem(item.Name);
                lvi.SubItems.Add(item.BestStore);
                lvi.SubItems.Add(item.Price.ToString("C"));
                lvi.SubItems.Add(item.Quantity.ToString());
                lvi.SubItems.Add(item.TotalCost().ToString("C"));
                listViewSummary.Items.Add(lvi);
            }

            // update the text preview
            txtSummary.Text = list.ToString();
        }

        // helper to get the currently selected shopping list from the dropdown
        private ShoppingList? GetSelectedList() => cboLists.SelectedItem as ShoppingList;

        // helper to refresh the shopping list dropdown, optionally selecting a specific list
        private void RefreshListDropdown(ShoppingList? selectList = null)
        {
            cboLists.BeginUpdate();
            cboLists.DataSource = null;
            cboLists.DisplayMember = "Name";
            cboLists.DataSource = _allShoppingLists.ToList();
            cboLists.EndUpdate();

            if (selectList != null) cboLists.SelectedItem = selectList;
            else if (_allShoppingLists.Count > 0) cboLists.SelectedIndex = 0;
        }

        // helper to show a simple prompt dialog with a title and message, returning the user input or null if cancelled
        private static string? Prompt(string title, string message)
        {
            using var form = new Form
            {
                Text = title,
                Width = 520,
                Height = 170,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
            };

            var lbl = new Label { Left = 12, Top = 12, Width = 480, Text = message };
            var txt = new TextBox { Left = 12, Top = 40, Width = 480 };

            var panel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 54, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(12) };
            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Width = 110, Height = 36 };
            var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 110, Height = 36 };

            ok.BackColor = Color.FromArgb(0, 120, 215);
            ok.ForeColor = Color.White;
            ok.FlatStyle = FlatStyle.Flat;
            ok.FlatAppearance.BorderSize = 0;

            cancel.BackColor = Color.Gray;
            cancel.ForeColor = Color.White;
            cancel.FlatStyle = FlatStyle.Flat;
            cancel.FlatAppearance.BorderSize = 0;

            panel.Controls.Add(ok);
            panel.Controls.Add(cancel);
            form.Controls.Add(panel);
            form.Controls.Add(lbl);
            form.Controls.Add(txt);
            form.AcceptButton = ok;
            form.CancelButton = cancel;

            return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }
    }
}