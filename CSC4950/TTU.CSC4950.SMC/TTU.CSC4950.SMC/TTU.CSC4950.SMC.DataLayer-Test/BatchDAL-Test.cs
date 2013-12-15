using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTU.CSC4950.SMC.FrameWork;
using TTU.CSC4950.SMC.DataLayer;
using NUnit.Framework;

namespace TTU.CSC4950.SMC.DataLayer_Test
{
    [TestFixture]
    public class BatchDAL_Test
    {
        private BatchType type1, type2, type3, type4;
        private Batch _batch1, _batch2, _batch3, _batch4, _batch5, _batch6;

        [SetUp]
        public void SetUp()
        {
            type1 = new BatchType("Test1", 12.34, 56.7, 123, null, 65, 142424);
            type2 = new BatchType("Test2", 442.2, 2521.11, 1223, 473, 765, 1212424);
            type3 = new BatchType("Test3", 675.43, 3900.03444, 123, 43, 65, 5787878);
            type4 = new BatchType("Test4", 49.03, 98.48, 123, 43, 65, null);
            BatchTypeDAL.AddBatchType(type1);
            BatchTypeDAL.AddBatchType(type2);
            BatchTypeDAL.AddBatchType(type3);
            BatchTypeDAL.AddBatchType(type4);

            _batch1 = new Batch("Machine1", Convert.ToDateTime("2001-01-01"), type1, true);
            _batch2 = new Batch("Machine2", Convert.ToDateTime("2001-01-01"), type2, true);
            _batch3 = new Batch("Machine3", Convert.ToDateTime("2001-01-01"), type3, true);
            _batch4 = new Batch("Machine4", Convert.ToDateTime("2001-01-01"), type1, true);
            _batch5 = new Batch("Machine5", Convert.ToDateTime("2001-01-01"), type1, true);
            _batch6 = new Batch("Machine6", Convert.ToDateTime("2001-01-01"), type4, false);
        }

        [Test]
        public void TestAddBatch()
        {
            bool success1, success2, success3, success4, success5, success6;
            success1 = success2 = success3 = success4 = success5 = success6 = false;

            success1 = BatchDAL.AddBatch(_batch1);
            success2 = BatchDAL.AddBatch(_batch2);
            success3 = BatchDAL.AddBatch(_batch3);
            success4 = BatchDAL.AddBatch(_batch4);
            success5 = BatchDAL.AddBatch(_batch5);
            success6 = BatchDAL.AddBatch(_batch6);

            Assert.IsTrue(success1 && success2 && success3 && success4 && success5 && success6);
        }

        [Test]
        public void TestDeleteBatch()
        {
            TestAddBatch();

            bool success1, success2, success3, success4, success5, success6;
            success1 = success2 = success3 = success4 = success5 = success6 = false;

            success1 = BatchDAL.DeleteBatch(_batch1);
            success2 = BatchDAL.DeleteBatch(_batch2);
            success3 = BatchDAL.DeleteBatch(_batch3);
            success4 = BatchDAL.DeleteBatch(_batch4);
            success5 = BatchDAL.DeleteBatch(_batch5);
            success6 = BatchDAL.DeleteBatch(_batch6);

            Assert.IsTrue(success1 && success2 && success3 && success4 && success5 && success6);
        }

        [Test]
        public void TestGetAllBatches()
        {
            TestAddBatch();

            List<Batch> batches = BatchDAL.GetAllBatches();

            Assert.IsTrue(_batch1.Equals(batches[0]));
            Assert.IsTrue(_batch2.Equals(batches[1]));
            Assert.IsTrue(_batch3.Equals(batches[2]));
            Assert.IsTrue(_batch4.Equals(batches[3]));
            Assert.IsTrue(_batch5.Equals(batches[4]));
            Assert.IsTrue(_batch6.Equals(batches[5]));
        }

        [Test]
        public void TestEmptyGetAllBatches()
        {
            CleanUp();
            List<Batch> batches = BatchDAL.GetAllBatches();

            Assert.AreEqual(null, batches);
        }

        [TearDown]
        public void CleanUp()
        {
            try
            {
                BatchDAL.DeleteBatch(_batch1);
                BatchDAL.DeleteBatch(_batch2);
                BatchDAL.DeleteBatch(_batch3);
                BatchDAL.DeleteBatch(_batch4);
                BatchDAL.DeleteBatch(_batch5);
                BatchDAL.DeleteBatch(_batch6);
            }
            catch { }

            try
            {
                BatchTypeDAL.DeleteBatchType(type1.Name);
                BatchTypeDAL.DeleteBatchType(type2.Name);
                BatchTypeDAL.DeleteBatchType(type3.Name);
                BatchTypeDAL.DeleteBatchType(type4.Name);
            }
            catch { }
        }
    }
}
