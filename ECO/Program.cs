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
            Console.WriteLine("Started");
            ParserThread temp = new ParserThread(Directory.GetCurrentDirectory() + @"/demos/", "*.dem");
            Console.ReadKey();
        }
    }
}
