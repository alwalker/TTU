using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTU.CSC4950.SMC.FrameWork;
using NUnit.Framework;

namespace TTU.CSC4950.SMC.FrameWork_Test
{
    [TestFixture]
    public class Batch_Test
    {
        private BatchType _type1, _type2;
        private Batch _batch1, _batch2, _batch3;

        [SetUp]
        public void SetUp()
        {
            _type1 = new BatchType("Test1", 12.34, 56.7, 123, null, 65, 142424);
            _type2 = new BatchType("Test2", 442.2, 2521.11, 1223, 473, 765, 1212424);

            _batch1 = new Batch("machine1", Convert.ToDateTime("2001-01-01"), _type1, true);
            _batch2 = new Batch("machine2", Convert.ToDateTime("2001-01-01"), _type2, false);
            _batch3 = new Batch("machine1", Convert.ToDateTime("2001-01-01"), _type1, true);
        }

        [Test]
        public void TestEquality()
        {
            Assert.IsTrue(_batch1.Equals(_batch3));
            Assert.IsFalse(_batch1.Equals(_batch2));
        }
    }
}
