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
            string[] filePaths = Directory.GetFiles(@"C:\Users\fredr\OneDrive\Dokument\KandidatArbetet\Demo Filer\", "*.dem");
            ParserThread temp = new ParserThread(@"C:\Users\fredr\OneDrive\Dokument\KandidatArbetet\Demo Filer\", "*.dem");
            Console.ReadKey();
        }
    }
}
