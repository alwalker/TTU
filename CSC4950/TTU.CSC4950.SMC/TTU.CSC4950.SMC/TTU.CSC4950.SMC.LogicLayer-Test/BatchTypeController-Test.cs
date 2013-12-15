using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TTU.CSC4950.SMC.FrameWork;
using TTU.CSC4950.SMC.LogicLayer;
using TTU.CSC4950.SMC.DataLayer;

namespace TTU.CSC4950.SMC.LogicLayer_Test
{
    [TestFixture]
    public class BatchTypeController_Test
    {
        private BatchType type1, type2, type3, type4;

        [SetUp]
        public void SetUp()
        {
            type1 = new BatchType("Test1", 12.34, 56.7, 123, null, 65, 142424);
            type2 = new BatchType("Test2", 442.2, 2521.11, 1223, 473, 765, 1212424);
            type3 = new BatchType("Test3", 675.43, 3900.03444, 123, 43, 65, 5787878);
            type4 = new BatchType("Test4", 49.03, 98.48, 123, 43, 65, null);
        }

        [Test]
        public void TestGetAllBatchTypes()
        {
            TestAddBatchType();
            List<BatchType> tests = BatchTypeController.GetAllBatchTypes();
            CleanUp();
            List<BatchType> emptyTests = BatchTypeController.GetAllBatchTypes();

            Assert.IsTrue(type1.Equals(tests[0], type1));
            Assert.IsTrue(type1.Equals(tests[1], type2));
            Assert.IsTrue(type1.Equals(tests[2], type3));
            Assert.IsTrue(type1.Equals(tests[3], type4));
            Assert.AreEqual(null, emptyTests);
        }

        [Test]
        public void TestAddBatchType()
        {
            bool success1 = BatchTypeController.AddBatchType(type1);
            bool success2 = BatchTypeController.AddBatchType(type2);
            bool success3 = BatchTypeController.AddBatchType(type3);
            bool success4 = BatchTypeController.AddBatchType(type4);

            Assert.IsTrue(success1 && success2 && success3 && success4);
        }
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddBatchError()
        {
            BatchTypeController.AddBatchType(null);
        }

        [Test]
        public void TestEditBatchType()
        {
            TestAddBatchType();

            bool success1 = BatchTypeController.EditBatchType(type2.Name, type1.VariableA, type1.VariableB, type1.TemperatureRequirement, type1.ViscosityRequirement, type1.SpeedRequirement, type1.TimeRequirement);

            Assert.IsTrue(success1);
        }
                
        [Test]
        public void TestGetBachType()
        {
            TestAddBatchType();

            BatchType test = BatchTypeController.GetBatchType(type1.Name);
            BatchType emptyTest = BatchTypeController.GetBatchType("");
            Assert.IsTrue(test.Equals(test, type1));
            Assert.AreEqual(null, emptyTest);
        }

        [TearDown]
        public void CleanUp()
        {
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
