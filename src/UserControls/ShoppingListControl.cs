using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Demo_PIG_Tool.BudgetTool;
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
        private Button btnRemoveItem = new();
        private Button btnDeleteList = new();
        private Panel mainPanel = new Panel();
        private Panel editPanel = new Panel();
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
            btnAddItem.Click += (_, __) => ShowAddItemForm();
            BlueButton(btnAddItem);

            // button to remove an item from a list
            btnRemoveItem.Text = "Remove Item";
            btnRemoveItem.Click += (_, __) => RemoveItemDialog();
            RedButton(btnRemoveItem);

            // button to delete the selected list
            btnDeleteList.Text = "Delete List";
            btnDeleteList.Click += (_, __) => DeleteList();
            RedButton(btnDeleteList);

            top.Controls.Add(lbl);
            top.Controls.Add(cboLists);
            top.Controls.Add(btnCreateList);
            top.Controls.Add(btnAddItem);
            top.Controls.Add(btnRemoveItem);
            top.Controls.Add(btnDeleteList);

            // make a split container for ListView and TextBox
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 350
            };

            // panel for editing an item (initially hidden, shown when adding/editing an item)
            editPanel.Dock = DockStyle.Fill;
            editPanel.Visible = false;
            Controls.Add(editPanel);
            editPanel.BringToFront();

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

            // double-clicking an item in the ListView will open the edit form for that item
            listViewSummary.DoubleClick += (_, __) => ShowEditItemForm();

            // customize TextBox style for the summary
            txtSummary.Dock = DockStyle.Fill;
            txtSummary.Multiline = true;
            txtSummary.ScrollBars = ScrollBars.Vertical;
            txtSummary.ReadOnly = true;
            txtSummary.BackColor = Color.White;
            txtSummary.BorderStyle = BorderStyle.FixedSingle;

            // add ListView and TextBox to the split container panels
            split.Panel1.Controls.Add(listViewSummary);
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

        // styling for the gray buttons
        private static void GrayButton(Button b)
        {
            b.BackColor = Color.Gray;
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

        // shows a form to add a new item to the selected shopping list, then saves and refreshes the UI
        private void ShowAddItemForm()
        {
            var list = GetSelectedList();

            // ensure a list is selected before showing the add item form
            if (list == null)
            {
                MessageBox.Show("Select a list first.");
                return;
            }

            // clear the edit panel and show it, hiding the main list view
            editPanel.Controls.Clear();
            editPanel.Visible = true;
            listViewSummary.Parent.Visible = false;

            int y = 20;

            // create title label for the form
            var title = new Label
            {
                Text = "Add New Item",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, y),
                AutoSize = true
            };
            editPanel.Controls.Add(title);
            y += 40;

            // create input for item name
            var nameBox = new TextBox { Location = new Point(180, y), Width = 250 };
            editPanel.Controls.Add(new Label { Text = "Item Name:", Location = new Point(20, y) });
            editPanel.Controls.Add(nameBox);
            y += 35;

            // create input for best store
            var storeBox = new TextBox { Location = new Point(180, y), Width = 250 };
            editPanel.Controls.Add(new Label { Text = "Best Store:", Location = new Point(20, y) });
            editPanel.Controls.Add(storeBox);
            y += 35;

            // create input for price
            var priceBox = new NumericUpDown
            {
                Location = new Point(180, y),
                Width = 120,
                DecimalPlaces = 2,
                Maximum = 10000
            };
            editPanel.Controls.Add(new Label { Text = "Price:", Location = new Point(20, y) });
            editPanel.Controls.Add(priceBox);
            y += 35;

            // create input for quantity
            var qtyBox = new NumericUpDown
            {
                Location = new Point(180, y),
                Width = 120,
                Maximum = 1000,
                Value = 1
            };
            editPanel.Controls.Add(new Label { Text = "Quantity:", Location = new Point(20, y) });
            editPanel.Controls.Add(qtyBox);
            y += 50;

            // create save button to save the new item to the list
            var saveBtn = new Button
            {
                Text = "Add Item",
                Location = new Point(180, y)
            };
            BlueButton(saveBtn);

            // handle save button click by validating input, adding the new item to the list, saving, refreshing the UI, and closing the form
            saveBtn.Click += (_, __) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text))
                {
                    MessageBox.Show("Item name is required.");
                    return;
                }

                list.AddItem(new Grocery(
                    nameBox.Text,
                    storeBox.Text,
                    (decimal)priceBox.Value,
                    (int)qtyBox.Value
                ));

                SaveLists();
                RenderListItems();

                editPanel.Visible = false;
                listViewSummary.Parent.Visible = true;
            };
            editPanel.Controls.Add(saveBtn);

            // create cancel button to close the form without saving
            var cancelBtn = new Button
            {
                Text = "Cancel",
                Location = new Point(320, y)
            };
            GrayButton(cancelBtn);

            // handle cancel button click by simply closing the form and showing the main list view again
            cancelBtn.Click += (_, __) =>
            {
                editPanel.Visible = false;
                listViewSummary.Parent.Visible = true;
            };

            editPanel.Controls.Add(cancelBtn);
        }

        // shows a dialog to remove an item from the selected shopping list, then saves and refreshes the UI
        private void RemoveItemDialog()
        {
            var list = GetSelectedList();
            var item = GetSelectedItem();

            // ensure an item from the list is selected
            if (list == null || item == null)
            {
                MessageBox.Show(this, "Select an item first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // confirm with the user before deleting the item
            var result = MessageBox.Show(
                $"Delete '{item.Name}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                list.RemoveItem(item.Name);
                SaveLists();
                RenderListItems();
            }
        }

        // shows a form to edit an item in the selected shopping list (change quantity, price, or best store), then saves and refreshes the UI
        private void ShowEditItemForm()
        {
            var list = GetSelectedList();
            var item = GetSelectedItem();

            // ensure an item from the list is selected before showing the edit form
            if (list == null || item == null)
            {
                MessageBox.Show("Select an item first.");
                return;
            }

            // clear the edit panel and show it, hiding the main list view
            editPanel.Controls.Clear();
            editPanel.Visible = true;
            listViewSummary.Parent.Visible = false; 

            int y = 20;

            // create title label for the form with the item name
            var title = new Label
            {
                Text = $"Edit Item: {item.Name}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, y),
                AutoSize = true
            };
            editPanel.Controls.Add(title);
            y += 40;

            // create input for item name
            var nameBox = new TextBox { Location = new Point(180, y), Width = 250, Text = item.Name };
            editPanel.Controls.Add(new Label { Text = "Item Name:", Location = new Point(20, y) });
            editPanel.Controls.Add(nameBox);
            y += 35;

            // create input for best store
            var storeBox = new TextBox { Location = new Point(180, y), Width = 250, Text = item.BestStore };
            editPanel.Controls.Add(new Label { Text = "Best Store:", Location = new Point(20, y) });
            editPanel.Controls.Add(storeBox);
            y += 35;

            // create input for price
            var priceBox = new NumericUpDown
            {
                Location = new Point(180, y),
                Width = 120,
                DecimalPlaces = 2,
                Maximum = 10000,
                Value = (decimal)item.Price
            };
            editPanel.Controls.Add(new Label { Text = "Price:", Location = new Point(20, y) });
            editPanel.Controls.Add(priceBox);
            y += 35;

            // create input for quantity
            var qtyBox = new NumericUpDown
            {
                Location = new Point(180, y),
                Width = 120,
                Maximum = 1000,
                Value = item.Quantity
            };
            editPanel.Controls.Add(new Label { Text = "Quantity:", Location = new Point(20, y) });
            editPanel.Controls.Add(qtyBox);
            y += 50;

            // create save button to save the changes to the item
            var saveBtn = new Button
            {
                Text = "Save",
                Location = new Point(180, y)
            };
            BlueButton(saveBtn);

            // handle save button click by validating input, updating the item in the list, saving, refreshing the UI, and closing the form
            saveBtn.Click += (_, __) =>
            {
                item.Name = nameBox.Text;
                item.BestStore = storeBox.Text;
                item.Price = (decimal)priceBox.Value;
                item.Quantity = (int)qtyBox.Value;

                SaveLists();
                RenderListItems();

                editPanel.Visible = false;
                listViewSummary.Parent.Visible = true;
            };
            editPanel.Controls.Add(saveBtn);

            // create delete button to remove the item from the list
            var deleteBtn = new Button
            {
                Text = "Delete",
                Location = new Point(320, y)
            };
            RedButton(deleteBtn);

            // handle delete button click by confirming with the user, deleting the item from the list, saving, refreshing the UI, and closing the form
            deleteBtn.Click += (_, __) =>
            {
                var confirm = MessageBox.Show("Delete item?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    list.RemoveItem(item.Name);
                    SaveLists();
                    RenderListItems();

                    editPanel.Visible = false;
                    listViewSummary.Parent.Visible = true;
                }
            };
            editPanel.Controls.Add(deleteBtn);

            // create cancel button to close the form without saving
            var cancelBtn = new Button
            {
                Text = "Cancel",
                Location = new Point(460, y)
            };
            GrayButton(cancelBtn);

            // handle cancel button click by simply closing the form and showing the main list view again
            cancelBtn.Click += (_, __) =>
            {
                editPanel.Visible = false;
                listViewSummary.Parent.Visible = true;
            };

            editPanel.Controls.Add(cancelBtn);
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
                lvi.Tag = item; 
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

        // helper to get the currently selected grocery item from the ListView
        private Grocery? GetSelectedItem()
        {
            if (listViewSummary.SelectedItems.Count == 0)
                return null;

            return listViewSummary.SelectedItems[0].Tag as Grocery;
        }

        // helper to refresh the shopping list dropdown, optionally selecting a specific list
        private void RefreshListDropdown(ShoppingList? selectList = null)
        {
            // refresh the dropdown with the current list of shopping lists
            cboLists.BeginUpdate();
            cboLists.DataSource = null;
            cboLists.DisplayMember = "Name";
            cboLists.DataSource = _allShoppingLists.ToList();
            cboLists.EndUpdate();

            // if there are no lists, clear the ListView and summary and show a message
            if (_allShoppingLists.Count == 0)
            {
                listViewSummary.Items.Clear();
                txtSummary.Text = "No shopping list available.";
                return;
            }

            // select the provided list or default to the first one
            if (selectList != null)
                cboLists.SelectedItem = selectList;
            else
                cboLists.SelectedIndex = 0;
        }

        // helper to delete the currently selected shopping list 
        private void DeleteList()
        {
            var list = GetSelectedList();

            // ensure a list is selected before attempting to delete
            if (list == null)
            {
                MessageBox.Show("Select a shopping list first.");
                return;
            }

            // confirm with the user before deleting the list
            var result = MessageBox.Show(
                $"Delete shopping list '{list.Name}'?\n\nThis cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            if (result != DialogResult.Yes)
                return;
            _allShoppingLists.Remove(list);

            // save the updated lists and refresh the dropdown and ListView to reflect the deletion
            SaveLists();
            RefreshListDropdown();
            RenderListItems();
        }


        // helper to show a dialog for creating a new shopping list, returning the user input or null if cancelled
        private static string? Prompt(string title, string message)
        {
            // create a new form to prompt the user for input
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

            // add a label for the shopping list name and a textbox for user input
            var lbl = new Label { Left = 12, Top = 12, Width = 480, Text = message };
            var txt = new TextBox { Left = 12, Top = 40, Width = 480 };

            // add OK and Cancel buttons to the form
            var panel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 54, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(12) };
            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Width = 110, Height = 36 };
            var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 110, Height = 36 };

            BlueButton(ok);
            GrayButton(cancel);

            // add the buttons to the panel and the panel, label, and textbox to the form
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