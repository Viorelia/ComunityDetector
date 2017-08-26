using ComunityDetectorServices;
using System;
using System.Collections.Generic;
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
            List<string> fileNames;
            var textContents = reader.GetFiles(out fileNames);

            
            HierarchicalClustering clustering = new HierarchicalClustering(textContents);
            int numberOfClusters = textBox1.Text != "" ? Convert.ToInt32(textBox1.Text) : 2;
            var clusters = clustering.ComputeClusters(numberOfClusters);

            listViewResults.Items.Clear();
            int index = 1;
            foreach(var cluster in clusters)
            {
                listViewResults.Items.Add("Cluster " + index);
                foreach(var item in cluster.Value)
                {
                    listViewResults.Items.Add(fileNames[item-1]);
                }
                listViewResults.Items.Add("");
                index++;
            }
        }
    }
}
