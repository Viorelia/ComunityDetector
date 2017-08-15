using System;
using System.Collections.Generic;
using System.Linq;

namespace ComunityDetectorServices
{
    public class CosineSimilarity
    {
        private Dictionary<string, Dictionary<int, double>> _tfIdf;
        private int _numberDocs;

        public CosineSimilarity(List<string> docs)
        {
            _tfIdf = new Dictionary<string, Dictionary<int, double>>();
            _numberDocs = docs.Count;
            BuildTfIdf(docs);
        }

        public double GetCosineSimilarityBetweenDocs(int indexDoc1, int indexDoc2)
        {
            double sum = 0;
            foreach(var term in _tfIdf)
            {
                var docs = term.Value;
                sum += docs[indexDoc1] * docs[indexDoc2];
            }

            return Math.Acos(sum) * 180 / Math.PI;
        }

        private void BuildTfIdf(List<string> docs)
        {
            var termsFrequencyList = new List<Dictionary<string, double>>();
            var termsFrequencyDictionary = new Dictionary<string, Dictionary<int, double>>();
                  
            foreach (var doc in docs)
            {
                var tf = GetTermFrequencyVector(doc);
                var ntf = GetNormalizedTermFrequencyVector(tf);
                AddMissingTerms(termsFrequencyDictionary, tf);
                termsFrequencyList.Add(ntf);
            }

            GlobalizeTerms(termsFrequencyList, termsFrequencyDictionary);

            foreach (var term in termsFrequencyDictionary)
            {
                _tfIdf.Add(term.Key, new Dictionary<int, double>());
                var idf = GetInverseDocumentFrecuency(term.Key, term.Value);

                foreach (var doc in term.Value)
                {                    
                    _tfIdf[term.Key].Add(doc.Key, doc.Value * idf);
                }                
            }
        }      

        private void GlobalizeTerms(List<Dictionary<string, double>> termsFrequencyList, Dictionary<string, Dictionary<int, double>> termsDictionary)
        {
            for(int i=1; i <= _numberDocs; i++)
            {
                var doc = termsFrequencyList.ElementAt(i-1);
                foreach(var term in termsDictionary.Keys)
                {
                    double termFrequency;
                    doc.TryGetValue(term, out termFrequency);
                    
                    termsDictionary[term].Add(i, termFrequency);
                }
            }
        }

        private Dictionary<string,int> GetTermFrequencyVector(string content)
        {
            var words = content.Split(new char[] { ' ', '.', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            return words.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
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

        private double GetInverseDocumentFrecuency(string term, Dictionary<int, double> termsFrequency)
        {
            var numberDocsWhereTerm = termsFrequency.Count(x => x.Value > 0);
            var value = (double)_numberDocs / (1 + numberDocsWhereTerm);

            return Math.Log(value);
        }
    }
}
