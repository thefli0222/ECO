using DemoInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ECO
{
    class ParserThread
    {
        public ParserThread(String filePath, String fileType)
        {
            int x = 0;
            Thread[] t = new Thread[8];
            Boolean isFileAdded = false;
            string[] filePaths = Directory.GetFiles(filePath, fileType);
            foreach (var fileName in filePaths)
            {
                getInfoFromFile(fileName);
            }
        }
        public void getInfoFromFile(string fileName)
        {
            Dictionary<string, int> players = new Dictionary<string, int>();
            int demoFileNumber = 1;
            Boolean hasMatchStarted;
            hasMatchStarted = false;
            var parser = new DemoParser(File.OpenRead(fileName));
            parser.ParseHeader();
            parser.MatchStarted += (sender, e) => {
                hasMatchStarted = true;
                Console.WriteLine(parser.Map);

            };

            parser.RoundEnd += (sender, e) => {
                if (!hasMatchStarted)
                    return;

                // We do this in a method-call since we'd else need to duplicate code
                // The much parameters are there because I simply extracted a method
                // Sorry for this - you should be able to read it anywys :)
                //Console.WriteLine("New round");
            };





            parser.ParseToEnd();
            
        }
    }
}
