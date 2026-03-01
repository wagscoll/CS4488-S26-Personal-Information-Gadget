using Demo_PIG_Tool.Manager;
using Utils.Docx;
using Utils.Doxc;

namespace Demo_PIG_Tool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //install Docx/ODT Viewer Shahil Kumar from extension in VS Code 
            //to view the created docx files in the file explorer
            SubToolManager.UpdateDocx();
                        //UPDATE DOCX can be called upon every subtool update
                        //to ensure the main docx file is always 
                        //up to date with the latest information
                        //NOT YET IMPLEMENTED IN SUBTOOLS

            SubToolManager.Run();
        }
    }
}
