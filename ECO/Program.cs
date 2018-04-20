using System;
using DemoInfo;
using System.IO;
using System.Collections.Generic;

namespace ECO
{
    class Program
    {
        static int numberOfClusters = 6;
        static void Main(string[] args)
        {
            double[] weights = new double[Enum.GetNames(typeof(PlayerData.STAT)).Length * 2];

            int t = 0;
            foreach (var ping in Enum.GetNames(typeof(PlayerData.STAT)))
            {
                weights[t] = 1;
                t++;
            }
            foreach (var ping in Enum.GetNames(typeof(PlayerData.STAT)))
            {
                weights[t] = 1;
                t++;
            }



            int c = 0;
            foreach (var value in System.IO.File.ReadAllText(@"C:\Users\Fredrik\Documents\GitHub\ECO\ECO\Save Files\weights.txt").Split(" "))
            {
                if (value != "")
                {
                    weights[c] = double.Parse(value);
                }
                c++;
            }

            string input = "";
            string[] valuesInput;
            while (input != "n" && input != "N")
            {
                t = 0;
                foreach (var ping in Enum.GetNames(typeof(PlayerData.STAT)))
                {
                    Console.WriteLine(t + ": " + "CT " + ping.ToString() + "W: " + weights[t]);
                    t++;
                }
                foreach (var ping in Enum.GetNames(typeof(PlayerData.STAT)))
                {
                    Console.WriteLine(t + ": " + "T " + ping.ToString() + "W: " + weights[t]);
                    t++;
                }
                Console.WriteLine("To change a weight input the number of the weight and it's value, example 1 0,5... Input n/N when you are finished.");
                input = Console.ReadLine();

                valuesInput = input.Split(" ");
                if(valuesInput.Length > 1)
                    weights[int.Parse(valuesInput[0])] = double.Parse(valuesInput[1]);
                Console.Clear();
            }

            string weightsString = "";
            foreach (var value in weights)
            {
                weightsString += value + " ";
            }
            System.IO.File.WriteAllText(@"C:\Users\Fredrik\Documents\GitHub\ECO\ECO\Save Files\weights.txt", weightsString);


            Console.WriteLine(Directory.GetCurrentDirectory() + @"/demos/");
            //string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory() + @"/Demo links/", "*.dem");
            ParserThread temp = new ParserThread("", "*.dem");



            Kmeans kMean = new Kmeans(temp.getPlayerData(), numberOfClusters, weights);
            String tempString = "";
            for(int x = 0; x < kMean.getCentroids().Length; x++)
            {
                tempString = "";
                tempString += x + ": " ;
                for (int i = 0; i < kMean.getCentroids()[x].Length; i++)
                {
                    tempString += kMean.getCentroids()[x][i] + " | ";
                }
                Console.WriteLine(tempString);

                {
                    tempString = "";
                    tempString += x + ": ";
                    if(kMean.getClusters()[x] != null)
                        foreach (var value in kMean.getClusters()[x])
                        {
                            tempString += value + " | ";
                        }
                    Console.WriteLine(tempString);
                }
            }


            
            Dictionary<int, List<double[]>> pointsIn2D = new Dictionary<int, List<double[]>>();


            pointsIn2D = kMean.getClustersAs2DPoints();

            /*foreach(var key in pointsIn2D.Keys)
            {
                Console.WriteLine("X" + " " + "Y");
                foreach (var p in pointsIn2D[key])
                {
                    Console.WriteLine(p[0] + " " + p[1]);
                }
            }*/


            foreach (var row in System.IO.File.ReadAllLines(@"C:\Users\Fredrik\Documents\GitHub\ECO\ECO\Save Files\matchresults.txt"))
            {

                long[] ctPlayers = new long[5];
                long[] tPlayers = new long[5];
                long[] results = new long[2];

                string[] matchResults = row.Split(" ");
                if (matchResults.Length > 10) {
                    for (int x = 0; x < 10; x++)
                    {
                        if (x < 5)
                        {
                            ctPlayers[x] = long.Parse(matchResults[x]);
                        }
                        else
                        {
                            tPlayers[x - 5] = long.Parse(matchResults[x]);
                        }
                    }
                    results[0] = long.Parse(matchResults[10]);
                    results[1] = long.Parse(matchResults[11]);
                    temp.GetMatchResults().AddMatchResult(ctPlayers, tPlayers, results);
                }
            }

            System.IO.File.WriteAllLines(@"C:\Users\Fredrik\Documents\GitHub\ECO\ECO\Save Files\matchresults.txt", temp.GetMatchResults().AsString().Split("\n"));

            temp.GetMatchResults().ConvertToClassesFromKmeans(kMean);
            System.IO.File.WriteAllLines(@"C:\Users\Fredrik\Documents\GitHub\ECO\ECO\Output\WriteLines.txt", temp.GetMatchResults().AsString().Split("\n"));


            long[] wins = new long[numberOfClusters];
            long[] losses = new long[numberOfClusters];
            foreach (var row in temp.GetMatchResults().AsString().Split("\n"))
            {
                string[] matchResults = row.Split(" ");
                if (row != "" && matchResults.Length > 5) {
                    for (int x = 0; x < 10; x++)
                    {
                        if(long.Parse(matchResults[x]) < numberOfClusters) {
                            if (x < 5)
                            {
                                if (int.Parse(matchResults[10]) > int.Parse(matchResults[11]))
                                {
                                    wins[long.Parse(matchResults[x])]++;
                                }
                                else
                                {
                                    losses[long.Parse(matchResults[x])]++;
                                }
                            }
                            else
                            {
                                if (int.Parse(matchResults[10]) < int.Parse(matchResults[11]))
                                {
                                    wins[long.Parse(matchResults[x])]++;
                                }
                                else
                                {
                                    losses[long.Parse(matchResults[x])]++;
                                }
                            }
                        }
                    }
                }
            }
           
            UIGeneratorClass UI = new UIGeneratorClass(pointsIn2D, kMean.getCentroids(), temp.getStats(), wins, losses);
            UI.generateHTML();

            Console.ReadKey();


        }
    }
}
