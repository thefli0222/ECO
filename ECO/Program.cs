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



            Kmeans kMean = new Kmeans(temp.getPlayerData(), 5);
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
                    foreach (var value in kMean.getClusters()[x])
                    {
                        tempString += value + " | ";
                    }
                    Console.WriteLine(tempString);
                }
            }
            Console.ReadKey();


        }
    }
}
