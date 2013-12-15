using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLCSimulator;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            bool testVal1, testVal2;
            PLC plc = new PLC("PLC.txt");
            
            while(!plc.Connect()){}
            while (!plc.WriteBit("OUT[4]", true)) { }
            while (!plc.ReadBit("IN[5]", out testVal1)) { }
            while (!plc.ReadBit("IN[6]", out testVal2)) { }

            while (!plc.WriteBit("OUT[3]", true)) { }
            while (!plc.ReadBit("IN[5]", out testVal1)) { }
            while (!plc.ReadBit("IN[6]", out testVal2)) { }

            Console.Write("Closed: " + testVal1.ToString() + "\nOpen: " + testVal2.ToString());
            Console.Read();
        }
    }
}
