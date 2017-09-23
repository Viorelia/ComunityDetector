using System;
using System.Collections.Generic;
using System.Linq;

namespace ComunityDetectorServices
{
    public class CosineSimilarity
    {
        private Dictionary<string, Dictionary<int, double>> _termFrequency;
        private int _numberDocs;

        public CosineSimilarity(List<string> docs)
        {
            _termFrequency = new Dictionary<string, Dictionary<int, double>>();
            _numberDocs = docs.Count;
            BuildTf(docs);
        }

        public double GetCosineSimilarityBetweenDocs(int indexDoc1, int indexDoc2)
        {
            double sum = 0;
            foreach(var term in _termFrequency)
            {
                var docs = term.Value;
                sum += docs[indexDoc1] * docs[indexDoc2];
            }

            return Math.Acos(sum) * 180 / Math.PI;
        }

        private void BuildTf(List<string> docs)
        {
            var termsFrequencyList = new List<Dictionary<string, int>>();
                  
            foreach (var doc in docs)
            {
                var tf = GetTermFrequencyVector(doc);
                AddMissingTerms(_termFrequency, tf);
                termsFrequencyList.Add(tf);
            }

            GlobalizeTerms(termsFrequencyList, _termFrequency);
        }      

        private void GlobalizeTerms(List<Dictionary<string, int>> termsFrequencyList, Dictionary<string, Dictionary<int, double>> termsDictionary)
        {
            for(int i=1; i <= _numberDocs; i++)
            {
                var tf = termsFrequencyList.ElementAt(i-1);
                var ntf = GetNormalizedTermFrequencyVector(tf);

                foreach(var term in termsDictionary.Keys)
                {
                    double termFrequency;
                    ntf.TryGetValue(term, out termFrequency);
                                        
                    termsDictionary[term].Add(i, termFrequency);
                }
            }
        }

        private Dictionary<string,int> GetTermFrequencyVector(string content)
        {
            var words = content.Split(new char[] { ' ', '.', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            return words.GroupBy(x => x, StringComparer.OrdinalIgnoreCase).ToDictionary(g => g.Key.ToLowerInvariant(), g => g.Count());
        }

        private void AddMissingTerms(Dictionary<string, Dictionary<int, double>> termsDictionary, Dictionary<string, int> terms)
        {
            foreach(var term in terms.Keys)
            {
                if (!termsDictionary.Keys.Contains(term))
                {
                    termsDictionary.Add(term, new Dictionary<int, double>());
                }
            }
        }

        private Dictionary<string, double> GetNormalizedTermFrequencyVector(Dictionary<string, int> vector)
        {
            var normalizedVector = new Dictionary<string, double>();
            var norm = GetVectorNorm(vector);

            foreach(var key in vector.Keys)
            {
                normalizedVector.Add(key, vector[key] / norm);
            }

            return normalizedVector;
        }

        private double GetVectorNorm(Dictionary<string, int> vector)
        {
            double sum = 0;

            foreach(var key in vector.Keys)
            {
                sum += Math.Pow(vector[key], 2);
            }

            return Math.Sqrt(sum);
        }
    }
}
