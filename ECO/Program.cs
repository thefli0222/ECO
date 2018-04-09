using System;
using DemoInfo;
using System.IO;
using System.Collections.Generic;

namespace ECO
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory() + @"/demos/");
            //string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory() + @"/Demo links/", "*.dem");
            ParserThread temp = new ParserThread("", "*.dem");


            Kmeans kMean = new Kmeans(temp.getPlayerData(), numberOfClusters);
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

            foreach(var key in pointsIn2D.Keys)
            {
                Console.WriteLine("X" + " " + "Y");
                foreach (var p in pointsIn2D[key])
                {
                    Console.WriteLine(p[0] + " " + p[1]);
                }
            }

            temp.GetMatchResults().ConvertToClassesFromKmeans(kMean);
            System.IO.File.WriteAllLines(@"..\ECO\Output\WriteLines.txt", temp.GetMatchResults().AsString().Split("\n"));

            UIGeneratorClass UI = new UIGeneratorClass(pointsIn2D, kMean.getCentroids(), temp.getStats());
            UI.generateHTML();

            Console.ReadKey();


        }
    }
}
