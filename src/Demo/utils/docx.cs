
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Utils.Docx
{
    public static class Docx
    {

        //install Docx/ODT Viewer Shahil Kumar from extension in VS Code 
        //to view the created docx files in the file explore

        // This method creates a new .docx file at the specified path and writes the provided content into it.
        public static void WriteToDocx(string filePath, string content)
        {
            string fullPath = PrepareOutputPath(filePath);

            using var doc = WordprocessingDocument.Create(fullPath, WordprocessingDocumentType.Document);
            MainDocumentPart main = SetupMain(doc);

            Body body = main.Document.Body!;
            ApplyPageLayout(body);
            AppendTextWithLineBreaks(body, content);

            main.Document.Save();
        }

        // This method reads the text content from an existing .docx file at the specified path and returns it as a string.
        private static string PrepareOutputPath(string filePath)
        {
            string fullPath = Path.GetFullPath(filePath);
            string? dir = Path.GetDirectoryName(fullPath);

            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            // If the file exists, delete it so Create(...) won't choke on a locked/readonly file.
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return fullPath;
        }

        // This method sets up the main document part of the .docx file, initializing it with a new Document and Body.
        private static MainDocumentPart SetupMain(WordprocessingDocument doc)
        {
            MainDocumentPart main = doc.AddMainDocumentPart();
            main.Document = new Document(new Body());
            return main;
        }

        // This method applies page layout settings to the Body of the document, including page size, margins, and column configuration.
        private static void ApplyPageLayout(Body body)
        {
            // If you call this multiple times, you’ll keep appending sectPr.
            // You can optionally remove existing SectionProperties first.
            var sectionProperties = new SectionProperties(
                new PageSize
                {
                    Width = 12240U,   // Letter page size - source: https://www.lexjansen.com/nesug/nesug10/cc/cc13.pdf
                    Height = 15840U   // Letter page size
                },
                new PageMargin
                {
                    Top = 720,        // 0.5" margins on all sides
                    Bottom = 720,     // 0.5"
                    Left = 720U,      // 0.5"
                    Right = 720U,     // 0.5"
                    Header = 720U,
                    Footer = 720U,
                    Gutter = 0U
                },
                new Columns
                {
                    ColumnCount = 2,
                    Space = "720"     // 0.5" between columns - padding
                }
            );

            body.Append(sectionProperties);
        }

        // This method appends the provided text content to the Body of the document, preserving line breaks and applying the specified font and size formatting.
        private static void AppendTextWithLineBreaks(Body body, string content)
        {
            var p = body.AppendChild(new Paragraph());
            var run = p.AppendChild(new Run());

            // Times New Roman 9pt == w:sz=18 (half-points)
            run.AppendChild(new RunProperties(
                new RunFonts
                {
                    Ascii = "Times New Roman",
                    HighAnsi = "Times New Roman",
                    EastAsia = "Times New Roman",
                    ComplexScript = "Times New Roman"
                },
                new FontSize { Val = "18" }
            ));

            // Normalize line breaks to \n, then split on \n to preserve empty lines.
            string normalized = (content ?? string.Empty).Replace("\r\n", "\n");
            string[] lines = normalized.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                run.AppendChild(new Text(lines[i])
                {
                    Space = SpaceProcessingModeValues.Preserve
                });

                // Add a line break BETWEEN lines (not after the last line)
                if (i < lines.Length - 1)
                    run.AppendChild(new Break());
            }
        }
    }
}