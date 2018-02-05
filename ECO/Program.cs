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
<<<<<<< HEAD
            Console.WriteLine("Started");
            ParserThread temp = new ParserThread(Directory.GetCurrentDirectory() + @"/demos/", "*.dem");
=======
            string[] filePaths = Directory.GetFiles(@"C:\Users\fredr\OneDrive\Dokument\KandidatArbetet\Demo Filer\", "*.dem");
            ParserThread temp = new ParserThread(@"C:\Users\fredr\OneDrive\Dokument\KandidatArbetet\Demo Filer\", "*.dem");



            Kmeans kMean = new Kmeans(temp.getPlayerData(), 5);
            String tempString = "";
            for(int x = 0; x < kMean.getCentroids().Length; x++)
            {
                tempString = "";
                tempString += kMean.getCentroids()[x] + ": " ;
                for (int i = 0; i < kMean.getCentroids()[x].Length; i++)
                {
                    tempString += kMean.getCentroids()[x][i] + " | ";
                }
                Console.WriteLine(tempString);
            }


>>>>>>> 28041a1aa2fd1607686ee9ba5e1c884f388c11e4
            Console.ReadKey();


        }
    }
}
