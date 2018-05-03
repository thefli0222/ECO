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
        private Dictionary<long, double[]>  normalizedData;
        private static double[] weightValues;
        public Kmeans(Dictionary<long, PlayerData> rawData, int k, double[] weights)
        {
            if (k < 3)
            {
                k = 3;
            }
            weightValues = weights;
            if(normalizedData == null)
            {
                normalizedData = Normalized(rawData);
            }

           


            Dictionary<long, double[]> data = withWeights(normalizedData);


            Dictionary<long, double[]> smallData = new Dictionary<long, double[]>();

            


            bool changed = true; bool success = true;
            clustering = InitClustering(data.Keys.Count, k, 0, rawData);

            var startTime = DateTime.Now;
            /*
            List<long> keyTemp = new List<long>();
            keyTemp.AddRange(data.Keys);
            Random random = new Random();
            int tempKey;
            long realKey;
            for (int smallTest = 0; smallTest < data.Count / 20; smallTest++)
            {
                tempKey = random.Next(0, keyTemp.Count);
                realKey = keyTemp[tempKey];
                keyTemp.RemoveAt(tempKey);
                smallData.Add(realKey, data[realKey]);
            }
            */
            centroids = Allocate(k, data[theKey].Length);
            int maxCount = 1;
            int iteration = 0;
            int maxCap = 0;
            /*
            while (changed == true && success == true && iteration < maxCount)// && maxCap < 5)
            {
                iteration++;
                success = UpdateMeans(smallData, clustering, centroids);
                changed = UpdateClustering(smallData, clustering, centroids);
                maxCap++;
            }
            changed = true; success = true;

            //Console.WriteLine("-Fast stage: " + (DateTime.Now - startTime));*/
            var startTimes = DateTime.Now;

            maxCount = data.Keys.Count * 50;
            iteration = 0;
            maxCap = 0;
            while (changed == true && success == true && iteration < maxCount)// && maxCap < 5)
            {
                iteration++;
                //startTimes = DateTime.Now;
                success = UpdateMeans(data, clustering, centroids);
                //Console.WriteLine((DateTime.Now - startTimes).TotalMilliseconds);
                //startTimes = DateTime.Now;
                changed = UpdateClustering(data, clustering, centroids);
                //Console.WriteLine((DateTime.Now - startTimes).TotalMilliseconds);
                maxCap++;
                //Console.WriteLine("-------------------------------------------");
            }
            //Console.WriteLine("--Slow Stage: " + (DateTime.Now - startTimes));
            //Console.WriteLine("---Total: " + (DateTime.Now - startTime));
            //Console.WriteLine((DateTime.Now - startTime).TotalMilliseconds);
            //Console.WriteLine("-------------------------------------------");
            allPoints = data;
        }

        public Dictionary<long, double[]> withWeights(Dictionary<long, double[]> data){

            Dictionary<long, double[]> result = new Dictionary<long, double[]>();
            foreach (var key in data.Keys)
            {
                double[] temp = data[key];
                for(int b=0; b < temp.Length; b++)
                {
                    temp[b] = temp[b] * weightValues[b];
                }
                result.Add(key, temp);
            }
            return result;

        }

        public Dictionary<int, ArrayList> getClusters()
        {
            Dictionary<int, ArrayList> temp = new Dictionary<int, ArrayList>();
            foreach (var key in clustering.Keys)
            {
                if (!temp.ContainsKey(clustering[key]))
                    temp.Add(clustering[key], new ArrayList());
                temp[clustering[key]].Add(key);
            }
            return temp;
        }

        private static Dictionary<long, double[]> Normalized(Dictionary<long, PlayerData> rawData)
        {
            Dictionary<long, double[]> result = new Dictionary<long, double[]>();
            foreach (var key in rawData.Keys)
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
                if (sd == 0)
                {
                    sd = 0.00000000000001;
                }

                foreach (var key in result.Keys)
                    result[key][j] = ((result[key][j] - mean) / sd);
            }
            return result;
        }

        private static Dictionary<long, int> InitClustering(int numPoints, int k, int seed, Dictionary<long, PlayerData> rawData)
        {
            Random random = new Random(seed);
            Dictionary<long, int> clustering = new Dictionary<long, int>();
            Dictionary<long, long> keys = new Dictionary<long, long>();
            int x = 0;
            foreach (var key in rawData.Keys)
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

        public bool doesClusterContain(int cluster, long value)
        {
            if (clustering.ContainsKey(value))
            {
                if (clustering[value] == cluster)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool UpdateMeans(Dictionary<long, double[]> data, Dictionary<long, int> clustering, double[][] centroids)
        {
            int count = data.Keys.Count;
            int numClusters = centroids.Length;
            int[] clusterCounts = new int[numClusters];
            /*foreach (var key in data.Keys)
            {
                int cluster = clustering[key];
                clusterCounts[cluster]++;
            }*/ // might be needed 

            //0 check here ?

            

            for (int k = 0; k < centroids.Length; k++)
                for (int j = 0; j < centroids[k].Length; j++)
                    centroids[k][j] = 0.0;

            foreach (var key in data.Keys)
            {
                int cluster = clustering[key]; // find me
                clusterCounts[cluster]++; // Maybe remove
                for (int j = 0; j < data[key].Length; ++j)
                    centroids[cluster][j] += data[key][j]; // accumulate sum
            }

            for (int k = 0; k < numClusters; k++)
                if (clusterCounts[k] == 0)
                    return false;

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
                sumSquaredDiffs += (dataPoint[j] - centroid[j])*(dataPoint[j] - centroid[j]);
            return Math.Sqrt(sumSquaredDiffs);
            //return Math.Pow(sumSquaredDiffs, 0.5);
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



        /*abs((X-0)^2 + (Y-0)^2 + (X-o)^2 + (Y-0)^2 + (X-0)^2 + (Y-p)^2 - f^2 - g^2 - h^2) = min

                dX = (X * (2 * (X - o) + 4 * X) * (-Math.Pow(f, 2) - Math.Pow(g, 2) - Math.Pow(h, 2) + Math.Pow((X - o), 2) + Math.Pow((Y - p), 2) + 2 * Math.Pow(X, 2) + 2 * Math.Pow(Y, 2))) / Math.Abs(-Math.Pow(f, 2) - Math.Pow(g, 2) - Math.Pow(h, 2) + 2 * Math.Pow(X, 2) + Math.Pow((X - o), 2) + 2 * Math.Pow(Y, 2) + Math.Pow((Y - p), 2)) + Math.Abs(-Math.Pow(f, 2) - Math.Pow(g, 2) - Math.Pow(h, 2) + 2 * Math.Pow(X, 2) + Math.Pow((X - o), 2) + 2 * Math.Pow(Y, 2) + Math.Pow((Y - p), 2));
                dY = (Y * (2*(Y - p) + 4*Y)*(-Math.Pow(f,2) - Math.Pow(g,2) - Math.Pow(h,2) + Math.Pow((X-o),2) + Math.Pow((Y-p),2) + 2*Math.Pow(X,2) + 2*Math.Pow(Y,2)))/Math.Abs(-Math.Pow(f,2) - Math.Pow(g,2) - Math.Pow(h,2) + 2*Math.Pow(X,2) + Math.Pow((X-o),2) + 2*Math.Pow(Y,2) + Math.Pow((Y-p),2)) + Math.Abs(-Math.Pow(f,2) - Math.Pow(g,2) - Math.Pow(h,2) + 2*Math.Pow(X,2) + Math.Pow((X-o),2) + 2*Math.Pow(Y,2) + Math.Pow((Y-p),2));
        
                dX = -((2 (o - 3 X) (-f^2 - g^2 - h^2 + o^2 - 2 o X + p^2 - 2 p Y + 3 X^2 + 3 Y^2))/abs(f^2 + g^2 + h^2 - o^2 - p^2 - 3 X^2 - 3 Y^2 + 2 o X + 2 p Y));
                dY = -((2 (p - 3 Y) (-f^2 - g^2 - h^2 + o^2 - 2 o X + p^2 - 2 p Y + 3 X^2 + 3 Y^2))/abs(f^2 + g^2 + h^2 - o^2 - p^2 - 3 X^2 - 3 Y^2 + 2 o X + 2 p Y))

                dX = -((2*(o - 3*X)*(-Math.Pow(f,2) - Math.Pow(g,2) - Math.Pow(h,2) + Math.Pow(o,2) - 2*o*X + Math.Pow(p,2) - 2*p*Y + 3*Math.Pow(X,2) + 3*Math.Pow(Y,2)))/Math.Abs(Math.Pow(f,2) + Math.Pow(g,2) + Math.Pow(h,2) - Math.Pow(o,2) - Math.Pow(p,2) - 3*Math.Pow(X,2) - 3*Math.Pow(Y,2) + 2*o*X + 2*p*Y));
                dY = -((2*(p - 3*Y) (-Math.Pow(f,2) - Math.Pow(g,2) - Math.Pow(h,2) + Math.Pow(o,2) - 2*o*X + Math.Pow(p,2) - 2*p*Y + 3*Math.Pow(X,2) + 3*Math.Pow(Y,2)))/Math.Abs(Math.Pow(f,2) + Math.Pow(g,2) + Math.Pow(h,2) - Math.Pow(o,2) - Math.Pow(p,2) - 3*Math.Pow(X,2) - 3*Math.Pow(Y,2) + 2*o*X + 2*p*Y));
             
                sqrt(abs((X + Y)^2 - f^2)) + sqrt(abs(((X-o)+Y)^2 - g^2)) + sqrt(abs((X + (Y-p))^2 - h^2))

                (2*X*(-Math.Pow(f,2) + Math.Pow(X,2) + Math.Pow(Y,2)))/Math.Abs(-Math.Pow(f,2) + Math.Pow(X,2) + Math.Pow(Y,2)) + (2*(X - o)*(-Math.Pow(g,2) + Math.Pow((X - o),2) + Math.Pow(Y,2)))/Math.Abs(-Math.Pow(g,2) + Math.Pow((X - o),2) + Math.Pow(Y,2)) + (2*X*(-Math.Pow(h,2) + Math.Pow((Y - p),2) + Math.Pow(X,2)))/Math.Abs(-Math.Pow(h,2) + Math.Pow(X,2) + Math.Pow((Y - p),2));
                (2*Y*(-Math.Pow(f,2) + Math.Pow(X,2) + Math.Pow(Y,2)))/Math.Abs(-Math.Pow(f,2) + Math.Pow(X,2) + Math.Pow(Y,2)) + (2*Y*(-Math.Pow(g,2) + Math.Pow((X - o),2) + Math.Pow(Y,2)))/Math.Abs(-Math.Pow(g,2) + Math.Pow((X - o),2) + Math.Pow(Y,2)) + (2*(Y - p)*(-Math.Pow(h,2) + Math.Pow((Y - p),2) + Math.Pow(X,2)))/Math.Abs(-Math.Pow(h,2) + Math.Pow(X,2) + Math.Pow((Y - p),2));



                dx = ((X + Y) ((X + Y)^2 - f^2))/abs((X + Y)^2 - f^2)^(3/2) + ((-o + X + Y) ((-o + X + Y)^2 - g^2))/abs((-o + X + Y)^2 - g^2)^(3/2) + ((-p + X + Y) ((-p + X + Y)^2 - h^2))/abs((-p + X + Y)^2 - h^2)^(3/2)
                dy = ((X + Y) ((X + Y)^2 - f^2))/abs((X + Y)^2 - f^2)^(3/2) + ((-o + X + Y) ((-o + X + Y)^2 - g^2))/abs((-o + X + Y)^2 - g^2)^(3/2) + ((-p + X + Y) ((-p + X + Y)^2 - h^2))/abs((-p + X + Y)^2 - h^2)^(3/2)

             */
        private double[] getXnY(double o, double p, double f, double g, double h)
        {
            double oldValue;
            double newValue = 0;
            double stepLength = 1;
            double newX = 0;
            double newY = 0;
            double oldXBase = 5;
            double oldYBase = 5;
            double oldX = 15;
            double oldY = 15;
            double bestValue = 99999;
            double multiplier = 1;
            double[] temp = { 0, 0 };


            for (int b = 0; b < 80; b++)
            {
                stepLength = 5;
                if (b % 4 == 0)
                {
                    oldX = -oldXBase * multiplier;
                    oldY = -oldYBase * multiplier;
                    multiplier *= 2;
                }
                else if (b % 3 == 0)
                {
                    oldX = -oldXBase * multiplier;
                    oldY = oldYBase * multiplier;
                }
                else if (b % 2 == 0)
                {
                    oldX = oldXBase * multiplier;
                    oldY = -oldYBase * multiplier;
                }
                else
                {
                    oldX = oldXBase * multiplier;
                    oldY = oldYBase * multiplier;
                }
                int x = 0;
                while (stepLength > 0.0000001)
                {
                    oldValue = functionOfXY(o, p, f, g, h, oldX, oldY);
                    newX = oldX - (derivateX(o, p, f, g, h, oldX, oldY) * stepLength);
                    newY = oldY - (derivateY(o, p, f, g, h, oldX, oldY) * stepLength);


                    if (x > 10000)
                    {
                        break;
                    }
                    if (oldValue == 0)
                    {
                        break;
                    }

                    oldX = newX;
                    oldY = newY;
                    x++;

                    newValue = functionOfXY(o, p, f, g, h, oldX, oldY);

                    if (newValue > oldValue)
                    {
                        stepLength = stepLength / 2;
                    }
                }

                if (newValue < bestValue)
                {
                    newX = oldX - oldX / (derivateX(o, p, f, g, h, oldX, oldY));
                    newY = oldY - oldY / (derivateY(o, p, f, g, h, oldX, oldY));
                    temp = new double[] { oldX, oldY };
                    bestValue = newValue;
                }



            }

            Console.WriteLine("Value of f: " + functionOfXY(o, p, f, g, h, oldX, oldY));

            return temp;
        }

        private double functionOfXY(double o, double p, double f, double g, double h, double X, double Y)
        {
            return Math.Abs(Math.Pow(X + Y, 2) - Math.Pow(f, 2)) + Math.Abs(Math.Pow((X - o) + Y, 2) - Math.Pow(g, 2)) + Math.Abs(Math.Pow(X + (Y - p), 2) - Math.Pow(h, 2));
        }

        private double derivateX(double o, double p, double f, double g, double h, double X, double Y)
        {
            return (2 * X * (-Math.Pow(f, 2) + Math.Pow(X, 2) + Math.Pow(Y, 2))) / Math.Abs(-Math.Pow(f, 2) + Math.Pow(X, 2) + Math.Pow(Y, 2)) + (2 * (X - o) * (-Math.Pow(g, 2) + Math.Pow((X - o), 2) + Math.Pow(Y, 2))) / Math.Abs(-Math.Pow(g, 2) + Math.Pow((X - o), 2) + Math.Pow(Y, 2)) + (2 * X * (-Math.Pow(h, 2) + Math.Pow((Y - p), 2) + Math.Pow(X, 2))) / Math.Abs(-Math.Pow(h, 2) + Math.Pow(X, 2) + Math.Pow((Y - p), 2));
        }

        private double derivateY(double o, double p, double f, double g, double h, double X, double Y)
        {
            return (2 * Y * (-Math.Pow(f, 2) + Math.Pow(X, 2) + Math.Pow(Y, 2))) / Math.Abs(-Math.Pow(f, 2) + Math.Pow(X, 2) + Math.Pow(Y, 2)) + (2 * Y * (-Math.Pow(g, 2) + Math.Pow((X - o), 2) + Math.Pow(Y, 2))) / Math.Abs(-Math.Pow(g, 2) + Math.Pow((X - o), 2) + Math.Pow(Y, 2)) + (2 * (Y - p) * (-Math.Pow(h, 2) + Math.Pow((Y - p), 2) + Math.Pow(X, 2))) / Math.Abs(-Math.Pow(h, 2) + Math.Pow(X, 2) + Math.Pow((Y - p), 2));

        }
        public Dictionary<int, List<double[]>> getClustersAs2DPoints()
        {
            Dictionary<int, List<double[]>> clusterAndPos = new Dictionary<int, List<double[]>>();
            if (allPoints == null)
            {
                return null;
            }



            double[] origo = centroids[0];
            double[] pointY = centroids[1];
            double[] pointX = centroids[2];
            double oldDistance = Distance(centroids[1], centroids[2]);

            double disY;
            double disO;
            double disX;

            double[] pointXxY = { 0, 0 };
            double distanceY = Distance(origo, pointY);


            foreach (var point in centroids)
            {
                if (Distance(centroids[1], point) > oldDistance && Distance(centroids[0], point) != 0)
                {
                    oldDistance = Distance(centroids[1], point);
                    pointX = point;
                    disY = Distance(point, pointY);
                    disO = Distance(point, origo);
                    pointXxY[0] = -(-Math.Sqrt(Math.Abs(Math.Pow(distanceY, 4) - (2 * Math.Pow(distanceY, 2) * Math.Pow(disO, 2)) - (2 * Math.Pow(distanceY, 2) * Math.Pow(disY, 2)) + Math.Pow(disO, 4) - (2 * Math.Pow(disO, 2) * Math.Pow(disY, 2)) + Math.Pow(disY, 4)))) / (2 * distanceY);
                    pointXxY[1] = distanceY - Math.Sqrt(Math.Abs(Math.Pow(disY, 2) - Math.Pow(pointXxY[0], 2)));
                }
            }

            double distanceX = Distance(origo, pointX);
            double[] origoXY = { 0, 0 };
            double[] pointYxY = { 0, distanceY };


            int i = 0;

            double Y;
            double X;

            double tY;
            double tX;



            foreach (double[] cluster in centroids)
            {
                clusterAndPos.Add(i, new List<double[]>());
                disY = Distance(cluster, pointY);
                disO = Distance(cluster, origo);
                disX = Distance(cluster, pointX);
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
                 X = -sqrt(disY ^ 2 - (distanceY + sqrt(disO ^ 2 - (0 - X) ^ 2)) ^ 2); 

                (distanceX-X)^2 - Y^2 = disX^2
                 

                Math.Pow(distanceX-X,2) = Math.Pow(disX,2) + Math.Pow(Y,2)


                distanceX = Math.Sqrt(Math.Pow(disX,2) + Math.Pow(Y,2)) + X

                distanceX - Math.Sqrt(Math.Pow(disX,2) + Math.Pow(Y,2)) = X


                Math.Sqrt(Math.Pow(distanceX-X,2) - Math.Pow(disX,2)) = Y

                X = distanceX - Math.Sqrt(Math.Pow(disX,2) + Math.Pow(tY,2));
                Y = Math.Sqrt(Math.Pow(distanceX-tX,2) - Math.Pow(disX,2));

                 X^2 = O^2 -Y^2;

                sqrt(-((C-sqrt((O^2 -Y^2)))^2 - V^2)) = Y^2

                (0-sqrt((O^2 - sqrt(-((C-sqrt((O^2 -Y^2)))^2 - V^2))^2)))^2 + (U-sqrt(-((C-sqrt((O^2 -Y^2)))^2 - V^2)))^2 = I^2
                 
                 
                X^2 + Y^2 + (C-X)^2 + Y^2 = V^2 + O^2
                (U-Y)^2 + x^2 = T^2

                Y = sqrt(T^2-X^2

                X = 1/2 (sqrt(-C^2 + 2 O^2 + 2 V^2) + C)

                X = 0.5 (C - sqrt(-C^2 + 2 O^2 + 2 V^2 - 4 (sqrt(T^2 - X^2) + U)^2))

                Y = (sqrt(T^2 - X^2) + U)

                X = (distanceX - Math.Sqrt(-Math.Pow(distanceX,2) + 2 * Math.Pow(disO,2) + 2 * Math.Pow(disX,2) - 4* Math.Pow(Y,2))/2

                Y = Math.Sqrt(Math.Pow(disY,2)-Math.Pow(X,2)) + distanceY

                (distanceY - Y)^2 = Math.Pow(disY,2)-Math.Pow(X,2)

                Math.Pow(disY,2) - Math.Pow((distanceY - Y),2) = Math.Pow(X,2)

                X = Math.Sqrt(Math.Pow(disY,2) - Math.Pow((distanceY - Y),2))


                X = (distanceX - Math.Sqrt(-Math.Pow(distanceX,2) + 2 * Math.Pow(disO,2) + 2 * Math.Pow(disX,2) - 4* Math.Pow(Math.Sqrt(Math.Pow(disY,2)-Math.Pow(X,2)) + distanceY,2))/2


                X≈(0.5*(-distanceX (-4*Math.Pow(distanceX,2) + 4*Math.Pow(disO,2) - 8*Math.Pow(disY,2) - 8*Math.Pow(distanceY,2) + 4*Math.Pow(disX,2)) - sqrt(-64*Math.Pow(distanceX,4)*Math.Pow(distanceY,2) + 128*Math.Pow(distanceX,2)*Math.Pow(disO,2)*Math.Pow(distanceY,2) - 256*Math.Pow(distanceX,2)*Math.Pow(distanceY,4) + 128*Math.Pow(distanceX,2)*Math.Pow(distanceY,2)*Math.Pow(disX,2) - 64*Math.Pow(disO,4)*Math.Pow(distanceY,2) + 256*Math.Pow(disO,2)*Math.Pow(disY,2)*Math.Pow(distanceY,2) + 256*Math.Pow(disO,2)*Math.Pow(distanceY,4) - 128*Math.Pow(disO,2)*Math.Pow(distanceY,2)*Math.Pow(disX,2) - 256*Math.Pow(disY,4)*Math.Pow(distanceY,2) + 512*Math.Pow(disY,2)*Math.Pow(distanceY,4) + 256*Math.Pow(disY,2)*Math.Pow(distanceY,2)*Math.Pow(disX,2) - 256*Math.Pow(distanceY,6) + 256*Math.Pow(distanceY,4)*Math.Pow(disX,2) - 64*Math.Pow(distanceY,2)*Math.Pow(disX,4))))/(4*Math.Pow(distanceX,2) + 16*Math.Pow(distanceY,2))

                (x−a)^2+(y−b)^2=j^2

                (x−c)^2+(y−d)^2=k^2

                (x−e)^2+(y−f)^2=l^2

                abs((X-0)^2 + (Y-0)^2 + (X-o)^2 + (Y-0)^2 + (X-0)^2 + (Y-p)^2 - f^2 - g^2 - h^2) = min
                abs(X^2 + Y^2 - f^2) + abs((X-o)^2 + Y^2 - g^2) + abs(X^2 + (Y-p)^2 - h^2)

                (2*X*(-Math.Pow(f,2) + Math.Pow(X,2) + Math.Pow(Y,2)))/Math.Abs(-Math.Pow(f,2) + Math.Pow(X,2) + Math.Pow(Y,2)) + (2*(X - o)*(-Math.Pow(g,2) + Math.Pow((X - o),2) + Math.Pow(Y,2)))/Math.Abs(-Math.Pow(g,2) + Math.Pow((X - o),2) + Math.Pow(Y,2)) + (2*X*(-Math.Pow(h,2) + Math.Pow((Y - p),2) + Math.Pow(X,2)))/Math.Abs(-Math.Pow(h,2) + Math.Pow(X,2) + Math.Pow((Y - p),2));
                (2*Y*(-Math.Pow(f,2) + Math.Pow(X,2) + Math.Pow(Y,2)))/Math.Abs(-Math.Pow(f,2) + Math.Pow(X,2) + Math.Pow(Y,2)) + (2*Y*(-Math.Pow(g,2) + Math.Pow((X - o),2) + Math.Pow(Y,2)))/Math.Abs(-Math.Pow(g,2) + Math.Pow((X - o),2) + Math.Pow(Y,2)) + (2*(Y - p)*(-Math.Pow(h,2) + Math.Pow((Y - p),2) + Math.Pow(X,2)))/Math.Abs(-Math.Pow(h,2) + Math.Pow(X,2) + Math.Pow((Y - p),2));


                dX = (X (2 (X - o) + 4 X) (-f^2 - g^2 - h^2 + (X - o)^2 + (Y - p)^2 + 2 X^2 + 2 Y^2))/abs(-f^2 - g^2 - h^2 + 2 X^2 + (X - o)^2 + 2 Y^2 + (Y - p)^2) + abs(-f^2 - g^2 - h^2 + 2 X^2 + (X - o)^2 + 2 Y^2 + (Y - p)^2)
                dY = = (Y (2 (Y - p) + 4 Y) (-f^2 - g^2 - h^2 + (X - o)^2 + (Y - p)^2 + 2 X^2 + 2 Y^2))/abs(-f^2 - g^2 - h^2 + 2 X^2 + (X - o)^2 + 2 Y^2 + (Y - p)^2) + abs(-f^2 - g^2 - h^2 + 2 X^2 + (X - o)^2 + 2 Y^2 + (Y - p)^2)

                 */

                X = -(-Math.Sqrt(Math.Abs(Math.Pow(distanceY, 4) - (2 * Math.Pow(distanceY, 2) * Math.Pow(disO, 2)) - (2 * Math.Pow(distanceY, 2) * Math.Pow(disY, 2)) + Math.Pow(disO, 4) - (2 * Math.Pow(disO, 2) * Math.Pow(disY, 2)) + Math.Pow(disY, 4)))) / (2 * distanceY);
                Y = distanceY - Math.Sqrt(Math.Abs(Math.Pow(disY, 2) - Math.Pow(X, 2)));



                /*Y = 1 / 2 * Math.Sqrt(-(4 * Math.Sqrt(-Math.Pow(distanceX, 2)*(Math.Pow(disO, 4) + 2 * Math.Pow(disO, 2) * Math.Pow(distanceY, 2) + 2 * Math.Pow(disO, 2) + Math.Pow(distanceY, 4) - 4 * Math.Pow(distanceY, 2) * Math.Pow(disX, 2) + 2 * Math.Pow(distanceY, 2) + 1))) / distanceY - 4 * Math.Pow(distanceX, 2) + Math.Pow(disO, 4) / Math.Pow(distanceY, 2) + (2 * Math.Pow(disO, 2)) / Math.Pow(distanceY, 2) + 6 * Math.Pow(disO, 2) + Math.Pow(distanceY, 2) + 1 / Math.Pow(distanceY, 2) - 4 * Math.Pow(disX, 2) + 2);

                X = 1 / 3 * (Math.Sqrt(-2 * Math.Pow(distanceX, 2) + 3 * Math.Pow(disO, 2) + 3 * Math.Pow(disY, 2) - 3 * Math.Pow(distanceY, 2) + 3 * Math.Pow(disX, 2)) + distanceX);

                Y = Math.Sqrt(Math.Pow(disY, 2) - Math.Pow((Math.Sqrt(Math.Abs(-Math.Pow(distanceX, 2) + 2 * Math.Pow(disO, 2) + 2 * Math.Pow(disX, 2)) + distanceX) / 2), 2)) + distanceY;

                X = (Math.Sqrt(-Math.Pow(distanceX, 2) + 2 * Math.Pow(disO, 2) + 2 * Math.Pow(disX, 2)) + distanceX) / 2;*/

                /*X = (0.5 * (-distanceX * (-4 * Math.Pow(distanceX, 2) + 4 * Math.Pow(disO, 2) - 8 * Math.Pow(disY, 2) - 8 * Math.Pow(distanceY, 2) + 4 * Math.Pow(disX, 2)) - Math.Sqrt(-64 * Math.Pow(distanceX, 4) * Math.Pow(distanceY, 2) + 128 * Math.Pow(distanceX, 2) * Math.Pow(disO, 2) * Math.Pow(distanceY, 2) - 256 * Math.Pow(distanceX, 2) * Math.Pow(distanceY, 4) + 128 * Math.Pow(distanceX, 2) * Math.Pow(distanceY, 2) * Math.Pow(disX, 2) - 64 * Math.Pow(disO, 4) * Math.Pow(distanceY, 2) + 256 * Math.Pow(disO, 2) * Math.Pow(disY, 2) * Math.Pow(distanceY, 2) + 256 * Math.Pow(disO, 2) * Math.Pow(distanceY, 4) - 128 * Math.Pow(disO, 2) * Math.Pow(distanceY, 2) * Math.Pow(disX, 2) - 256 * Math.Pow(disY, 4) * Math.Pow(distanceY, 2) + 512 * Math.Pow(disY, 2) * Math.Pow(distanceY, 4) + 256 * Math.Pow(disY, 2) * Math.Pow(distanceY, 2) * Math.Pow(disX, 2) - 256 * Math.Pow(distanceY, 6) + 256 * Math.Pow(distanceY, 4) * Math.Pow(disX, 2) - 64 * Math.Pow(distanceY, 2) * Math.Pow(disX, 4)))) / (4 * Math.Pow(distanceX, 2) + 16 * Math.Pow(distanceY, 2));

                Y = Math.Sqrt(Math.Pow(disY, 2) - Math.Pow(X, 2)) + distanceY;*/



                double[] Temp = { X, Y }; //getXnY(distanceX, distanceY, disO, disX, disY);




                /* if (Math.Abs(Distance(pointXxY, Temp) - Distance(cluster, pointX)) < Math.Abs(Distance(pointXxY, TempNegative) - Distance(cluster, pointX)))
                 {
                     //Temp = Temp;
                     Console.WriteLine("Correct dis");
                 }
                 else
                 {
                     Temp = TempNegative;
                     Console.WriteLine("Not correct dis");

                 } */



                clusterAndPos[i].Add(Temp);

                /*Console.WriteLine("X: " + X + " Y: " + Y + " DRY: " + disY + " DFY: " + Distance(Temp, pointYxY) + " DRO: " + disO + " DFO: " + Distance(Temp, origoXY) + " DRX: " + disX + " DFX: " + Distance(Temp, pointXxY));*/
                i++;

            }

            foreach (long key in allPoints.Keys)
            {
                double[] point = allPoints[key];
                //clusterAndPos.Add(i, new List<double[]>());
                disY = Distance(point, pointY);
                disO = Distance(point, origo);
                disX = Distance(point, pointX);

                X = -(-Math.Sqrt(Math.Abs(Math.Pow(distanceY, 4) - (2 * Math.Pow(distanceY, 2) * Math.Pow(disO, 2)) - (2 * Math.Pow(distanceY, 2) * Math.Pow(disY, 2)) + Math.Pow(disO, 4) - (2 * Math.Pow(disO, 2) * Math.Pow(disY, 2)) + Math.Pow(disY, 4)))) / (2 * distanceY);
                Y = distanceY - Math.Sqrt(Math.Abs(Math.Pow(disY, 2) - Math.Pow(X, 2)));





                double[] Temp = { X, Y };

                /*
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


                double oldvalue;
                double newalue;

                while (true)
                {
                    oldvalue = (Distance(pointXxY, Temp) - disX) + (Distance(pointYxY, Temp) - disY) + (Distance(origoXY, Temp) - disO);
                    TempNegative = new double[] { (X - (disY / disO) * 0.01), (Y - (disO / disY) * 0.01) };
                    newalue = (Distance(pointXxY, TempNegative) - disX) + (Distance(pointYxY, TempNegative) - disY) + (Distance(origoXY, TempNegative) - disO);
                    if (oldvalue > newalue)
                    {
                        Temp = TempNegative;
                    }
                    else
                    {
                        break;
                    }
                } */

                clusterAndPos[clustering[key]].Add(Temp);

                /*Console.WriteLine("Key: " + key + "Cluster: " + clustering[key]);

                Console.WriteLine("X: " + Temp[0] + " Y: " + Temp[1] + " DRY: " + disY + " DFY: " + Distance(Temp, pointYxY) + " DRO: " + disO + " DFO: " + Distance(Temp, origoXY) + " DRX: " + disX + " DFX: " + Distance(Temp, pointXxY));*/
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
