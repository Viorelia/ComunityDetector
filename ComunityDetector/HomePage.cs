using ComunityDetectorServices;
using System;
using System.Windows.Forms;

namespace ComunityDetector
{
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextReader reader = new TextReader();
            var textContents = reader.GetTextFiles();

            
            HierarchicalClustering clustering = new HierarchicalClustering(textContents);
            clustering.ComputeClusters();
        }
    }
}
