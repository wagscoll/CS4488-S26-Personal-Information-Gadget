using Demo_PIG_Tool.Manager;
using Utils.Docx;
using Demo_PIG_Tool;


SubToolManager.UpdateDocx();            // "Auto-save" - updates the docx file with the latest logs from all tools before launching the UI

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
Application.Run(new ShellForm());       // Launches the main shell form, acting as a UI wrapper for the various tools

SubToolManager.UpdateDocx();            // "Auto-save" - updates the docx file with the latest logs from all tools after closing the UI