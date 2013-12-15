using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLCSimulator;

namespace PLCSimulator_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting test.");

            PLC testPLC;

            try
            {
                testPLC = new PLC("PLC 1.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
