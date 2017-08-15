using System.Collections.Generic;

namespace ComunityDetectorServices
{
    public class HierarchicalClustering
    {
        private CosineSimilarity _cosineSimilarity;
        private Dictionary<int, List<int>> _clusters;
        private Dictionary<int, Dictionary<int, double>> _similarityMatrix;
        private int _numberClusters; 

        public HierarchicalClustering(List<string> documents)
        {
            _cosineSimilarity = new CosineSimilarity(documents);
            _clusters = new Dictionary<int, List<int>>();
            _similarityMatrix = new Dictionary<int, Dictionary<int, double>>();
            _numberClusters = documents.Count;
        }

        public Dictionary<int, List<int>> ComputeClusters()
        {
            InitialiseClusters();
            InitialiseSimilarityMatrix();

            while (!IsQualityMeet())
            {
                int cluster1, cluster2;
                FindMostSimilarClusters(out cluster1, out cluster2);
                MergeClusters(cluster1, cluster2);
            }

            return _clusters;
        }

        private void InitialiseClusters()
        {
            for(int i=1; i <= _numberClusters; i++)
            {
                _clusters.Add(i, new List<int> {i});
            }
        }

        private void InitialiseSimilarityMatrix()
        {
            for(int i=1; i<_numberClusters; i++)
            {
                var dictionary = new Dictionary<int, double>();
                for(int j=i+1; j<=_numberClusters; j++)
                {
                    var similarity = _cosineSimilarity.GetCosineSimilarityBetweenDocs(i, j);
                    dictionary.Add(j, similarity);
                }
                _similarityMatrix.Add(i, dictionary);
            }
        }

        private void FindMostSimilarClusters(out int cluster1, out int cluster2)
        {
            double minValue = 360;
            cluster1 = 0;
            cluster2 = 0;

            foreach(var k1 in _similarityMatrix)
            {
                foreach(var k2 in k1.Value)
                {
                    if (k2.Value < minValue)
                    {
                        minValue = k2.Value;
                        cluster1 = k1.Key;
                        cluster2 = k2.Key;
                    }
                }
            }
        }

        private void MergeClusters(int cluster1, int cluster2)
        {
            if(cluster1 > cluster2)
            {
                var temp = cluster2;
                cluster2 = cluster1;
                cluster1 = temp;
            }

            var numberPointsCluster1 = _clusters[cluster1].Count;
            var numberPointsCluster2 = _clusters[cluster2].Count;

            for(int index = 1; index <= _numberClusters; index++)
            {
                if(index != cluster2)
                {
                    if(index != cluster1)
                    {
                        var numberPointsRelatedCluster = _clusters[index].Count;
                        double relatedCluster1 = GetSimilarityValue(cluster1, index);
                        double relatedCluster2 = GetSimilarityValue(cluster2, index);

                        var averageSimilarity = (relatedCluster1 * (numberPointsRelatedCluster + numberPointsCluster1 - 1) +
                                                relatedCluster2 * (numberPointsRelatedCluster + numberPointsCluster2 - 1)) /
                                                (numberPointsRelatedCluster + numberPointsCluster1 + numberPointsCluster2 - 1);

                        UpdateSimilarityValue(cluster1, index, averageSimilarity);
                    }
                    
                    _similarityMatrix[index].Remove(cluster2);
                }            
            }

            _similarityMatrix.Remove(cluster2);
            _clusters[cluster1].AddRange(_clusters[cluster2]);
            _clusters.Remove(cluster2);
            _numberClusters--;
            //keep a vector/list/dictionary where to have the association index - key of that dictionary (because ones clusters are removed)
        }

        private double GetSimilarityValue(int cluster1, int cluster2)
        {
            double relatedCluster = cluster1 < cluster2 
                ? _similarityMatrix[cluster1][cluster2] 
                : _similarityMatrix[cluster2][cluster1];

            return relatedCluster;
        }

        private void UpdateSimilarityValue(int cluster1, int cluster2, double value)
        {
            if (cluster1 < cluster2)
            {
                _similarityMatrix[cluster1][cluster2] = value;
            }
            else
            {
                _similarityMatrix[cluster2][cluster1] = value;
            }
        }

        private bool IsQualityMeet()
        {//research best aproach to stop the cluster merge - where the best quality of comunity
            return _clusters.Count == 2;
        }
    }
}
