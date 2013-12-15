using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TTU.CSC4950.SMC.FrameWork;
using TTU.CSC4950.SMC.DataLayer;
using NUnit.Framework;

namespace TTU.CSC4950.SMC.DataLayer_Test
{
    [TestFixture]
    public class BatchTypeDAL_Test
    {
        BatchType type1, type2, type3, type4;

        [SetUp]
        public void Init()
        {
            type1 = new BatchType("Test1", 12.34, 56.7, 123, null, 65, 142424);
            type2 = new BatchType("Test2", 442.2, 2521.11, 1223, 473, 765, 1212424);
            type3 = new BatchType("Test3", 675.43, 3900.03444, 123, 43, 65, 5787878);
            type4 = new BatchType("Test4", 49.03, 98.48, 123, 43, 65, null);
        }

        [Test]
        public void TestAddBatchType()
        {
            bool success1 = BatchTypeDAL.AddBatchType(type1);
            bool success2 = BatchTypeDAL.AddBatchType(type2);
            bool success3 = BatchTypeDAL.AddBatchType(type3);
            bool success4 = BatchTypeDAL.AddBatchType(type4);

            Assert.IsTrue(success1 && success2 && success3 && success4);
        }

        [Test]
        public void TestDeleteBatch()
        {
            bool success1 = BatchTypeDAL.DeleteBatchType(type1.Name);
            bool success2 = BatchTypeDAL.DeleteBatchType(type2.Name);
            bool success3 = BatchTypeDAL.DeleteBatchType(type3.Name);
            bool success4 = BatchTypeDAL.DeleteBatchType(type4.Name);

            Assert.IsTrue(success1 && success2 && success3 && success4);
        }

        [Test]
        public void TestEditBatchType()
        {
            TestAddBatchType();
            bool success1 = BatchTypeDAL.EditBatchType(type2.Name, type1.VariableA, type1.VariableB, type1.TemperatureRequirement, type1.ViscosityRequirement, type1.SpeedRequirement, type1.TimeRequirement);

            Assert.IsTrue(success1);
        }

        [Test]
        public void TestGetBachType()
        {
            TestAddBatchType();

            BatchType test = BatchTypeDAL.GetBatchType(type1.Name);
            Assert.IsTrue(test.Equals(test, type1));
        }

        [Test]
        public void TestGetAllBatchTypes()
        {
            TestDeleteBatch();
            TestAddBatchType();
            List<BatchType> batchTypes = BatchTypeDAL.GetAllBatchTypes();

            Assert.IsTrue(type1.Equals(type1, batchTypes[0]));
            Assert.IsTrue(type1.Equals(type2, batchTypes[1]));
            Assert.IsTrue(type1.Equals(type3, batchTypes[2]));
            Assert.IsTrue(type1.Equals(type4, batchTypes[3]));
        }

        [Test]
        public void TestEmptyGetAllBatchTypes()
        {
            CleanUp();
            List<BatchType> batchTypes = BatchTypeDAL.GetAllBatchTypes();

            Assert.AreEqual(null, batchTypes);
        }

        [TearDown]
        public void CleanUp()
        {
            TestDeleteBatch();
        }
    }
}
