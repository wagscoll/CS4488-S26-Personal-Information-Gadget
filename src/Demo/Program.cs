using Demo_PIG_Tool.Manager;
using Utils.Docx;
using Demo_PIG_Tool;


        //UPDATE DOCX can be called upon every subtool update
        //to ensure the main docx file is always 
        //up to date with the latest information
        //NOT YET IMPLEMENTED IN SUBTOOLS

        //install Docx/ODT Viewer Shahil Kumar from extension in VS Code 
        //to view the created docx files in the file explorer\

SubToolManager.UpdateDocx();

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
Application.Run(new ShellForm());

SubToolManager.UpdateDocx();