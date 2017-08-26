using System.Collections.Generic;
using System.IO;

namespace ComunityDetector
{
    public class TextReader
    {

        private string GetPath()
        {
            string appPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            string newPath = Path.GetFullPath(Path.Combine(appPath, @"..\..\..\..\TestData"));

            return newPath;
        }
        public List<string> GetFiles(out List<string> fileNames)
        {
            List<string> files = new List<string>();
            fileNames = new List<string>();

            foreach (string file in Directory.EnumerateFiles(GetPath(), "*.txt"))
            {
                string name = Path.GetFileName(file);
                string contents = File.ReadAllText(file);
                files.Add(contents);
                fileNames.Add(name);
            }

            return files;
        }
    }
}
