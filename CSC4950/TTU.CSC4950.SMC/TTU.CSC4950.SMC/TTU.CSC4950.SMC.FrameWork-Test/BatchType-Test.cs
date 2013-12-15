using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTU.CSC4950.SMC.FrameWork;
using NUnit.Framework;

namespace TTU.CSC4950.SMC.FrameWork_Test
{
    [TestFixture]
    class BatchType_Test
    {
        private BatchType _batchType1, _batchType2, _batchType3;

        [SetUp]
        public void Init()
        {
            _batchType1 = new BatchType("Test1", 1, 2, 3, 4, 5, 6);
            _batchType2 = new BatchType("Test2", 2, 3, null, null, null, null);
            _batchType3 = new BatchType("Test1", 1, 2, 3, 4, 5, 6);
        }

        [Test]
        public void TestToString()
        {
            Assert.AreEqual("Test1", _batchType1.Name);
        }

        [Test]
        public void TestCompareTo()
        {
            int notEqual, equal;

            notEqual = _batchType1.CompareTo(_batchType2);
            equal = _batchType1.CompareTo(_batchType3);

            Assert.AreEqual(-1, notEqual);
            Assert.AreEqual(0, equal);
        }

        [Test]
        public void TestEqual()
        {
            bool notEqual, equal;

            notEqual = _batchType1.Equals(_batchType1, _batchType2);
            equal = _batchType1.Equals(_batchType1, _batchType3);
        }

    }
}
