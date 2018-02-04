using System;
using System.Collections.Generic;
using System.Text;

namespace ECO
{
    class Kmeans
    {
        private double[][] centroids;
        public Kmeans(Dictionary<long, PlayerData> rawData, int k)
        {
            Dictionary<long, double[]> data = Normalized(rawData);
            bool changed = true; bool success = true;
            Dictionary<long, int> clustering = InitClustering(data.Keys.Count, k, 0, rawData);
            centroids = Allocate(k, data[0].Length);
            int maxCount = data.Keys.Count * 50;
            int iteration = 0;
            while (changed == true && success == true && iteration < maxCount)
            {
                iteration++;
                success = UpdateMeans(data, clustering, centroids);
                changed = UpdateClustering(data, clustering, centroids);
            }
            
        }

        private static Dictionary<long, double[]> Normalized(Dictionary<long, PlayerData> rawData)
        {
            Dictionary <long, double[]> result = new Dictionary<long, double[]>(); 
            
            foreach(var key in rawData.Keys)
            {
                double[] temp = new double[rawData[key].getFullData().Length];
                temp = rawData[key].getFullData();
                result.Add(key, temp);
            }


            int count = result.Keys.Count;
            for (int j = 0; j < result[0].Length; j++)
            {
                double colSum = 0.0;
                foreach (var key in result.Keys)
                    colSum += result[key][j];
                double mean = colSum / count;
                double sum = 0.0;
                foreach (var key in result.Keys)
                    sum += (result[key][j] - mean) * (result[key][j] - mean);
                double sd = sum / count;
                foreach (var key in result.Keys)
                    result[key][j] = (result[key][j] - mean) / sd;
            }
            return result;
        }

        private static Dictionary<long, int> InitClustering(int numPoints, int k, int seed, Dictionary<long, PlayerData> rawData)
        {
            Random random = new Random(seed);
            Dictionary<long, int> clustering = new Dictionary<long, int>(); ;
            foreach (var key in rawData.Keys)
                clustering.Add(key, (int)key);
            foreach (var key in rawData.Keys)
                clustering[key] = random.Next(0, k);
            return clustering;
        }

        private static bool UpdateMeans(Dictionary<long, double[]> data, Dictionary<long, int> clustering, double[][] centroids)
        {
            int count = data.Keys.Count;
            int numClusters = centroids.Length;
            int[] clusterCounts = new int[numClusters];
            foreach (var key in data.Keys)
            {
                int cluster = clustering[key];
                clusterCounts[cluster]++;
            }

            for (int k = 0; k < numClusters; k++)
                if (clusterCounts[k] == 0)
                    return false;

            for (int k = 0; k < centroids.Length; k++)
                for (int j = 0; j < centroids[k].Length; j++)
                    centroids[k][j] = 0.0;

            foreach (var key in data.Keys)
            {
                int cluster = clustering[key]; // find me
                for (int j = 0; j < data[key].Length; ++j)
                    centroids[cluster][j] += data[key][j]; // accumulate sum
            }

            for (int k = 0; k < centroids.Length; ++k)
                for (int j = 0; j < centroids[k].Length; ++j)
                    centroids[k][j] /= clusterCounts[k]; // danger of div by 0
            return true;
        }

        private static double[][] Allocate(int numClusters, int numColumns)
        {
            double[][] result = new double[numClusters][];
            for (int k = 0; k < numClusters; k++)
                result[k] = new double[numColumns];
            return result;
        }


        private static bool UpdateClustering(Dictionary<long, double[]> data, Dictionary<long, int> clustering, double[][] centroids)
        {
            int count = data.Keys.Count;
            int numClusters = centroids.Length;
            bool changed = false;

            Dictionary<long, int> newClustering = new Dictionary<long, int>();
            foreach (var key in clustering.Keys)
                newClustering[key] = clustering[key];

            double[] distances = new double[numClusters];

            foreach (var key in data.Keys)
            {
                for (int k = 0; k < numClusters; k++)
                    distances[k] = Distance(data[key], centroids[k]);

                int newClusterID = MinIndex(distances);
                if (newClusterID != newClustering[key])
                {
                    changed = true;
                    newClustering[key] = newClusterID;
                }
            }

            if (changed == false)
                return false;

            Dictionary<long, int> clusterCounts = new Dictionary<long, int>();
            foreach (var key in clustering.Keys)
            {
                if (!clusterCounts.ContainsKey(key))
                    clusterCounts.Add(key, 0);

                clusterCounts[key]++;
            }


            foreach (var key in clustering.Keys)
                if (clusterCounts[key] == 0)
                    return false;

            foreach (var key in newClustering.Keys)
                clustering[key] = newClustering[key];
            return true; // no zero-counts and at least one change
        }


        private static double Distance(double[] dataPoint, double[] centroid)
        {
            double sumSquaredDiffs = 0.0;
            for (int j = 0; j < dataPoint.Length; ++j)
                sumSquaredDiffs += Math.Pow((dataPoint[j] - centroid[j]), 2);
            return Math.Sqrt(sumSquaredDiffs);
        }

        private static int MinIndex(double[] distances)
        {
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }

        public double[][] getCentroids()
        {
            return centroids;
        }

        static void ShowData(double[][] data, int decimals,
  bool indices, bool newLine)
        {
            for (int i = 0; i < data.Length; ++i)
            {
                if (indices) Console.Write(i.ToString().PadLeft(3) + " ");
                for (int j = 0; j < data[i].Length; ++j)
                {
                    if (data[i][j] >= 0.0) Console.Write(" ");
                    Console.Write(data[i][j].ToString("F" + decimals) + " ");
                }
                Console.WriteLine("");
            }
            if (newLine) Console.WriteLine("");
        }

        static void ShowVector(int[] vector, bool newLine)
        {
            for (int i = 0; i < vector.Length; ++i)
                Console.Write(vector[i] + " ");
            if (newLine) Console.WriteLine("\n");
        }

        static void ShowClustered(double[][] data, int[] clustering,
          int numClusters, int decimals)
        {
            for (int k = 0; k < numClusters; ++k)
            {
                Console.WriteLine("===================");
                for (int i = 0; i < data.Length; ++i)
                {
                    int clusterID = clustering[i];
                    if (clusterID != k) continue;
                    Console.Write(i.ToString().PadLeft(3) + " ");
                    for (int j = 0; j < data[i].Length; ++j)
                    {
                        if (data[i][j] >= 0.0) Console.Write(" ");
                        Console.Write(data[i][j].ToString("F" + decimals) + " ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("===================");
            } // k
        }
    }
}
