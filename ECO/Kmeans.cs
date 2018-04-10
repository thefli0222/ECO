using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace ECO
{
    class Kmeans
    {
        private double[][] centroids;
        private static long theKey;
        private Dictionary<long, int> clustering;
        private Dictionary<long, double[]> allPoints;
        public Kmeans(Dictionary<long, PlayerData> rawData, int k)
        {
           if (k < 3)        {
                k = 3;
          }

            Dictionary<long, double[]> data = Normalized(rawData);
            bool changed = true; bool success = true;
            clustering = InitClustering(data.Keys.Count, k, 0, rawData);
            centroids = Allocate(k, data[theKey].Length);
            int maxCount = data.Keys.Count * 50;
            int iteration = 0;
            while (changed == true && success == true && iteration < maxCount)
            {
                iteration++;
                success = UpdateMeans(data, clustering, centroids);
                changed = UpdateClustering(data, clustering, centroids);
            }
            Console.WriteLine("Stop");
            allPoints = data;
        }

        public Dictionary<int, ArrayList> getClusters()
        {
            Dictionary<int, ArrayList> temp = new Dictionary<int, ArrayList>();
            foreach (var key in clustering.Keys)
            {
                if(!temp.ContainsKey(clustering[key]))
                    temp.Add(clustering[key], new ArrayList());
                temp[clustering[key]].Add(key);
            }
            return temp;
        }

        private static Dictionary<long, double[]> Normalized(Dictionary<long, PlayerData> rawData)
        {
            Dictionary <long, double[]> result = new Dictionary<long, double[]>();
            foreach(var key in rawData.Keys)
            {
                double[] temp = new double[rawData[key].getFullData().Length];
                temp = rawData[key].getFullData();
                result.Add(key, temp);
                theKey = key;
            }


            int count = result.Keys.Count;
            for (int j = 0; j < result[theKey].Length; j++)
            {
                double colSum = 0.0;
                foreach (var key in result.Keys)
                    colSum += result[key][j];
                double mean = colSum / count;
                double sum = 0.0;
                foreach (var key in result.Keys)
                    sum += (result[key][j] - mean) * (result[key][j] - mean);
                double sd = sum / count;
                if(sd == 0)
                {
                    sd = 0.00000000000001;
                }
                
                foreach (var key in result.Keys)
                    result[key][j] = (result[key][j] - mean) / sd;
            }
            return result;
        }

        private static Dictionary<long, int> InitClustering(int numPoints, int k, int seed, Dictionary<long, PlayerData> rawData)
        {
            Random random = new Random(seed);
            Dictionary<long, int> clustering = new Dictionary<long, int>();
            Dictionary<long, long> keys = new Dictionary<long, long>();
            int x = 0;
            foreach(var key in rawData.Keys)
            {
                keys.Add(x, key);
                x++;
            }
            for (int i = 0; i < k; i++)
                clustering.Add(keys[i], i);
            for (int i = k; i < numPoints; i++)
                clustering[keys[i]] = random.Next(0, k);
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

          public Dictionary<int, List<double[]>> getClustersAs2DPoints()
  {
      Dictionary<int, List<double[]>> clusterAndPos = new Dictionary<int, List<double[]>>();
      if (allPoints == null)
      {
          return null;
      }



      double[] origo = new double[centroids[0].Length];
      double[] pointY = new double[centroids[0].Length];
      double[] pointX = new double[centroids[0].Length];

            for(int x=0; x<origo.Length; x++)
            {
                origo[x] = 0;
                if (x < (origo.Length / 2))
                {
                    pointY[x] = 1;
                    pointX[x] = 0;

                } else
                {
                    pointY[x] = 0;
                    pointX[x] = 1;
                }

            }

      double distanceY = Distance(origo, pointY);
      double[] origoXY = { 0, 0 };
      double[] pointYxY = { 0, distanceY };
      double[] pointXxY = { 0, 0 };


            int i = 0;
      double disY;
      double disO;

      double Y;
      double X;



      foreach(double[] cluster in centroids)
      {
          clusterAndPos.Add(i, new List<double[]>());
          disY = Distance(cluster, pointY);
          disO = Distance(cluster, origo);
                /* X^2+(distanceY - Y)^2 = disY^2;
                Y = distanceY - sqrt(disY^2 - X^2)
                  X^2  + Y^2 = disO^2;
                 X^2 = disO^2 - (distanceY - sqrt(disY^2 - X^2))^2
                 X = -(i sqrt(distanceY^4 - 2 distanceY^2 disO^2 - 2 distanceY^2 disY^2 + disO^4 - 2 disO^2 disY^2 + disY^4))/(2*distanceY)
                X = -(-sqrt(Math.Abs(distanceY^4 - 2 distanceY^2 disO^2 - 2 distanceY^2 disY^2 + disO^4 - 2 disO^2 disY^2 + disY^4))/(2*distanceY)
                 X = -sqrt(disY ^ 2 - (distanceY - Y) ^ 2);
                 X^2 = disY^2 - (distanceY - Y) ^ 2
                 (distanceY - Y) ^ 2 = disY^2 - X^2
                  Y = -sqrt(disY^2 - X^2) - distanceY;
                 (X) = -sqrt((disY ^ 2 - distanceY ^ 2 + disO ^ 2)/2);
                 (0 - X) ^ 2 + (0 - Y) ^ 2 = disO ^ 2;
                 (0-X)^2 = disO ^ 2 - (0 - Y)^2
                 sqrt(disO ^ 2 - (0 - Y) ^ 2) = -X
                 Y = -sqrt(disO ^ 2 - (0 - X) ^ 2);
                 X = -sqrt(disY ^ 2 - (distanceY + sqrt(disO ^ 2 - (0 - X) ^ 2)) ^ 2); */
                
                X = -(-Math.Sqrt(Math.Abs(Math.Pow(distanceY, 4) - (2 * Math.Pow(distanceY, 2) * Math.Pow(disO, 2)) - (2 * Math.Pow(distanceY, 2) * Math.Pow(disY, 2)) + Math.Pow(disO, 4) - (2 * Math.Pow(disO, 2) * Math.Pow(disY, 2)) + Math.Pow(disY, 4)))) / (2 * distanceY);
                Y = distanceY - Math.Sqrt(Math.Abs(Math.Pow(disY, 2) - Math.Pow(X, 2)));

                double[] Temp = { X, Y };
                double[] TempNegative = { -X, Y };
                if (i == 2)
                {
                    pointXxY[0] = X;
                    pointXxY[1] = Y;
                } else if (i > 2)
                {
                    if (Math.Abs(Distance(pointXxY,Temp) - Distance(cluster, pointX)) < Math.Abs(Distance(pointXxY, TempNegative) - Distance(cluster, pointX)))
                    {
                        //Temp = Temp;
                        Console.WriteLine("Correct dis");
                    } else
                    {
                        Temp = TempNegative;
                        Console.WriteLine("Not correct dis");
                    }
                }


          clusterAndPos[i].Add(Temp);

          Console.WriteLine("X: " + X + " Y: " + Y + " DRY: " + disY + " DFY: " + Distance(Temp, pointYxY) + " DRO: " + disO + " DFO: " + Distance(Temp, origoXY));
          i++;

      }

            foreach (long key in allPoints.Keys)
            {
                double[] point = allPoints[key];
                //clusterAndPos.Add(i, new List<double[]>());
                disY = Distance(point, pointY);
                disO = Distance(point, origo);

                X = -(-Math.Sqrt(Math.Abs(Math.Pow(distanceY, 4) - (2 * Math.Pow(distanceY, 2) * Math.Pow(disO, 2)) - (2 * Math.Pow(distanceY, 2) * Math.Pow(disY, 2)) + Math.Pow(disO, 4) - (2 * Math.Pow(disO, 2) * Math.Pow(disY, 2)) + Math.Pow(disY, 4)))) / (2 * distanceY);
                Y = distanceY - Math.Sqrt(Math.Abs(Math.Pow(disY, 2) - Math.Pow(X, 2)));

                double[] Temp = { X, Y };
                double[] TempNegative = { -X, Y };

                    if (Math.Abs(Distance(pointXxY, Temp) - Distance(point, pointX)) < Math.Abs(Distance(pointXxY, TempNegative) - Distance(point, pointX)))
                    {
                        //Temp = Temp;
                        Console.WriteLine("Correct dis");
                    }
                    else
                    {
                        Temp = TempNegative;
                        Console.WriteLine("Not correct dis");
                    }



                clusterAndPos[clustering[key]].Add(Temp);

                Console.WriteLine("Key: " + key + "Cluster: " + clustering[key]);

                Console.WriteLine("X: " + X + " Y: " + Y + " DRY: " + disY + " DFY: " + Distance(Temp, pointYxY) + " DRO: " + disO + " DFO: " + Distance(Temp, origoXY));
                //i++;

            }

            return clusterAndPos;
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
