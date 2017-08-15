using System.Collections.Generic;
using System.IO;

namespace ComunityDetector
{
    public class TextReader
    {

        private string GetPath()
        {
            string appPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            string newPath = Path.GetFullPath(Path.Combine(appPath, @"..\..\..\..\TestData1"));

            return newPath;
        }
        public List<string> GetTextFiles()
        {
            List<string> files = new List<string>();

            foreach (string file in Directory.EnumerateFiles(GetPath(), "*.txt"))
            {
                string contents = File.ReadAllText(file);
                files.Add(contents);
            }

            return files;
        }
    }
}
