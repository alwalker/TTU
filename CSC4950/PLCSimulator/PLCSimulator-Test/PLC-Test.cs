using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PLCSimulator;

namespace PLCSimulator_Test
{
    [TestFixture]
    public class PLC_Test
    {
        private PLC _plc;

        [SetUp]
        public void Init()
        {
            _plc = new PLC("PLC.txt");

            while (!_plc.Connect()) { }
        }

        [Test]
        public void WriteReadBit()
        {
            bool testVal = false;
            bool read = false, wrote = false;

            while (!read && !wrote)
            {
                wrote = _plc.WriteBit("IN[1]", true);
                read = _plc.ReadBit("IN[1]", out testVal);

                if (!read || !wrote)
                {
                    wrote = read = false;
                }
            }

            Assert.AreEqual(testVal, true);
        }

        [Test]
        public void WriteReadWord()
        {
            double testVal = 0;
            bool read = false, wrote = false;

            while (!read && !wrote)
            {
                wrote = _plc.WriteWord("OUT[16]", 25.9);
                read = _plc.ReadWord("OUT[16]", out testVal);

                if (!read || !wrote)
                {
                    wrote = read = false;
                }
            }

            Assert.AreEqual(25.9, testVal);
        }

        [Test]
        public void TestRule()
        {
            bool testVal1, testVal2;
            bool read = false, wrote = false;

            while (!_plc.WriteBit("OUT[4]", true)) { }
            while (!_plc.ReadBit("IN[5]", out testVal1)) { }
            while (!_plc.ReadBit("IN[6]", out testVal2)) { }
            Assert.IsTrue(testVal1);
            Assert.IsFalse(testVal2);

            while (!_plc.WriteBit("OUT[3]", true)) { }
            while (!_plc.ReadBit("IN[5]", out testVal1)) { }
            while (!_plc.ReadBit("IN[6]", out testVal2)) { }
            Assert.IsFalse(testVal1);
            Assert.IsTrue(testVal2);

            
        }
    }
}
