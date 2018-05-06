using System;
using DemoInfo;
using System.IO;
using System.Collections.Generic;

namespace ECO
{
    class Program
    {
        static int[] clusterValues = { 8, 16, 32 };
        static void Main(string[] args)
        {
            double[][] weights = new double[5][];
            weights[0] = new double[Enum.GetNames(typeof(PlayerData.STAT)).Length * 2];
            int t = 0;
            foreach (var ping in Enum.GetNames(typeof(PlayerData.STAT)))
            {
                weights[0][t] = 1;
                t++;
            }
            foreach (var ping in Enum.GetNames(typeof(PlayerData.STAT)))
            {
                weights[0][t] = 1;
                t++;
            }


            weights[1] = new double[Enum.GetNames(typeof(PlayerData.STAT)).Length * 2];

            int c = 0;
            foreach (var value in System.IO.File.ReadAllText(@"C:\Users\Fredrik\Documents\GitHub\ECO\ECO\Save Files\weights.txt").Split(" "))
            {
                if (value != "")
                {
                    weights[1][c] = double.Parse(value);
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
                    Console.WriteLine(t + ": " + "CT " + ping.ToString() + "W: " + weights[1][t]);
                    t++;
                }
                foreach (var ping in Enum.GetNames(typeof(PlayerData.STAT)))
                {
                    Console.WriteLine(t + ": " + "T " + ping.ToString() + "W: " + weights[1][t]);
                    t++;
                }
                Console.WriteLine("To change a weight input the number of the weight and it's value, example 1 0,5... Input n/N when you are finished.");
                input = Console.ReadLine();

                valuesInput = input.Split(" ");
                if (valuesInput.Length > 1)
                    weights[1][int.Parse(valuesInput[0])] = double.Parse(valuesInput[1]);
                Console.Clear();
            }

            string weightsString = "";
            foreach (var value in weights[1])
            {
                weightsString += value + " ";
            }
            System.IO.File.WriteAllText(@"C:\Users\Fredrik\Documents\GitHub\ECO\ECO\Save Files\weights.txt", weightsString);


            Console.WriteLine(Directory.GetCurrentDirectory() + @"/demos/");
            //string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory() + @"/Demo links/", "*.dem");
            ParserThread temp = new ParserThread("", "*.dem");
            Console.WriteLine("Write start");
            System.IO.File.WriteAllLines(@"C:\Users\Fredrik\Documents\GitHub\ECO\ECO\Save Files\matchresults.txt", temp.GetMatchResults().AsString().Split("\n"));
            Console.WriteLine("Evolution start");
            EA evolution = new EA(0.7, 0.7, (4 / (Enum.GetNames(typeof(PlayerData.STAT)).Length)), 0.3, 25, temp, 10);
            evolution.RunGenerations(15, 8);

            weights[2] = evolution.BestChild.Weights;

            evolution = new EA(0.7, 0.7, (4 / (Enum.GetNames(typeof(PlayerData.STAT)).Length)), 0.3, 25, temp, 10);
            evolution.RunGenerations(15, 16);

            weights[3] = evolution.BestChild.Weights;

            evolution = new EA(0.7, 0.7, (4 / (Enum.GetNames(typeof(PlayerData.STAT)).Length)), 0.3, 25, temp, 10);
            evolution.RunGenerations(15, 32);

            weights[4] = evolution.BestChild.Weights;



            /*foreach(var key in pointsIn2D.Keys)
            {
                Console.WriteLine("X" + " " + "Y");
                foreach (var p in pointsIn2D[key])
                {
                    Console.WriteLine(p[0] + " " + p[1]);
                }
            }*/


            /*foreach (var row in System.IO.File.ReadAllLines(@"C:\Users\Fredrik\Documents\GitHub\ECO\ECO\Save Files\matchresults.txt"))
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
            }*/
            string[] weightNames = { "NON", "SELF", "EAO8", "EAO16", "EAO32" };
            foreach (int clusterAmount in clusterValues)
                for (int testNum = 0; testNum < weights.Length; testNum++)
                {
                    { /*
                Kmeans kMean = new Kmeans(temp.getPlayerData(), numberOfClusters, weights[2]);
                String tempString = "";
                for (int x = 0; x < kMean.getCentroids().Length; x++)
                {
                    tempString = "";
                    tempString += x + ": ";
                    for (int i = 0; i < kMean.getCentroids()[x].Length; i++)
                    {
                        tempString += kMean.getCentroids()[x][i] + " | ";
                    }
                    Console.WriteLine(tempString);

                    {
                        tempString = "";
                        tempString += x + ": ";
                        if (kMean.getClusters()[x] != null)
                            foreach (var value in kMean.getClusters()[x])
                            {
                                tempString += value + " | ";
                            }
                        Console.WriteLine(tempString);
                    }
                */
                    }


                    Kmeans kMean;
                    double winLossFittness;
                    double totalAmountOfPoints = 0;
                    MatchResults theMatchResults = new MatchResults();
                    kMean = new Kmeans(temp.getPlayerData(), clusterAmount, weights[testNum]);
                    theMatchResults = temp.GetMatchResults().Clone() as MatchResults;
                    theMatchResults.ConvertToClassesFromKmeans(kMean);

                    long[] wins = new long[clusterAmount];
                    long[] losses = new long[clusterAmount];
                    long[] row;
                    for (int rowNum = 0; rowNum < theMatchResults.MatchResultList.Count; rowNum++)
                    {
                        row = theMatchResults.MatchResultList[rowNum];
                        //foreach (var row in theMatchResults.MatchResultList)
                        //{ 
                        if (row.Length > 5)
                        {
                            for (int x = 0; x < 10; x++)
                            {
                                if (row[x] < clusterAmount)
                                {
                                    if (x < 5)
                                    {
                                        if (row[10] > row[11])
                                        {
                                            wins[row[x]]++;
                                        }
                                        else
                                        {
                                            losses[row[x]]++;
                                        }
                                    }
                                    else
                                    {
                                        if (row[10] < row[11])
                                        {
                                            wins[row[x]]++;
                                        }
                                        else
                                        {
                                            losses[row[x]]++;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    winLossFittness = 0;
                    for (int x = 0; x < wins.Length; x++)
                    {
                        winLossFittness += (1 - Math.Abs(0.5 - ((double)wins[x] / (double)(wins[x] + losses[x])))) * (double)(wins[x] + losses[x]);
                        totalAmountOfPoints += (wins[x] + losses[x]);
                    }
                    winLossFittness = winLossFittness / (totalAmountOfPoints);
                    if (winLossFittness > 1)
                    {
                        winLossFittness = 1;
                    } else if (winLossFittness == Double.NaN)
                    {
                        winLossFittness = 0;
                    }
                    //Console.WriteLine(winLossFittness);

                    double averageDistance = 0;
                    double[][] oldPos = kMean.getCentroids();
                    List<int> takenNumbers = new List<int>();
                    double lowestDis;
                    double thisDis;
                    int tempVal = 0;
                    double stabilityFittness = 0;
                    for (int x = 0; x < 2; x++)
                    {
                        foreach (double[] p in oldPos)
                        {
                            foreach (double[] cent in kMean.getCentroids())
                            {
                                averageDistance += Math.Sqrt(Math.Pow(p[0] - cent[0], 2) + Math.Pow(p[1] - cent[1], 2));
                            }
                        }
                        averageDistance = averageDistance / (oldPos.Length * oldPos.Length);

                        kMean = new Kmeans(temp.getPlayerData(), clusterAmount, weights[2]);
                        lowestDis = 999999;
                        foreach (double[] p in oldPos)
                        {
                            for (int y = 0; y < oldPos.Length; y++)
                            {

                                if (!takenNumbers.Contains(y))
                                {
                                    thisDis = Math.Sqrt(Math.Pow(kMean.getCentroids()[y][0] - p[0], 2) + Math.Pow(kMean.getCentroids()[y][1] - p[1], 2));
                                    if (thisDis < lowestDis)
                                    {
                                        lowestDis = thisDis;
                                        tempVal = y;
                                    }
                                }
                            }
                            takenNumbers.Add(tempVal);
                            stabilityFittness += 1 - (lowestDis / (averageDistance + lowestDis));
                        }
                    }
                    stabilityFittness = stabilityFittness / (oldPos.Length * 2);
                    if (stabilityFittness > 1)
                    {
                        stabilityFittness = 1;
                    }



                    Dictionary<int, List<double[]>> pointsIn2D = new Dictionary<int, List<double[]>>();
                    


                    pointsIn2D = kMean.getClustersAs2DPoints();
                    //KMeansFunction




                    /*MatchResults matchResultsConversionHolder = temp.GetMatchResults().Clone() as MatchResults;
                    matchResultsConversionHolder.ConvertToClassesFromKmeans(kMean);
                    System.IO.File.WriteAllLines(@"C:\Users\Fredrik\Documents\GitHub\ECO\ECO\Output\WriteLines.txt", temp.GetMatchResults().AsString().Split("\n"));


                    long[] wins = new long[numberOfClusters];
                    long[] losses = new long[numberOfClusters];
                    foreach (var row in matchResultsConversionHolder.AsString().Split("\n"))
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
                    }*/

                    double weightsFittness = 0;
                    foreach (double w in weights[testNum]) weightsFittness += w;
                    weightsFittness = weightsFittness / weights[testNum].Length;


                    double dd = dumbPrediction.calculateDumbPrediction(theMatchResults, wins, losses);
                    
                    System.IO.File.WriteAllLines(@"C:\Users\Fredrik\Documents\GitHub\ECO\ECO\Output\WriteLines" + "C" + clusterAmount + "W" + (testNum + 1) + weightNames[testNum] + "P" + Math.Round((dd * 100), 0) + ".txt", theMatchResults.AsString().Split("\n"));
                    UIGeneratorClass UI = new UIGeneratorClass(pointsIn2D, kMean.getCentroids(), temp.getStats(), wins, losses, "C" + clusterAmount + "W" + (testNum+1) + weightNames[testNum] + "P" + Math.Round((dd * 100), 0), Math.Pow(winLossFittness, 3), Math.Pow(stabilityFittness, 2), weightsFittness, Math.Pow(winLossFittness, 3) * Math.Pow(stabilityFittness, 2) * weightsFittness, dd);
                    UI.generateHTML();
                }
            Console.ReadKey();


        }
    }
}
