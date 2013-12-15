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
    public class BatchController_Test
    {
        private BatchType _type1, _type2, _type3, _type4;
        private Batch _batch1, _batch2, _batch3, _batch4, _batch5, _batch6;

        [SetUp]
        public void SetUp()
        {
            _type1 = new BatchType("Test1", 12.34, 56.7, 123, null, 65, 142424);
            _type2 = new BatchType("Test2", 442.2, 2521.11, 1223, 473, 765, 1212424);
            _type3 = new BatchType("Test3", 675.43, 3900.03444, 123, 43, 65, 5787878);
            _type4 = new BatchType("Test4", 49.03, 98.48, 123, null, null, null);
            BatchTypeDAL.AddBatchType(_type1);
            BatchTypeDAL.AddBatchType(_type2);
            BatchTypeDAL.AddBatchType(_type3);
            BatchTypeDAL.AddBatchType(_type4);

            _batch1 = new Batch("Machine1", Convert.ToDateTime("2001-01-01"), _type1, true);
            _batch2 = new Batch("Machine2", Convert.ToDateTime("2001-01-01"), _type2, true);
            _batch3 = new Batch("Machine3", Convert.ToDateTime("2001-01-01"), _type3, true);
            _batch4 = new Batch("Machine4", Convert.ToDateTime("2001-01-01"), _type1, true);
            _batch5 = new Batch("Machine5", Convert.ToDateTime("2001-01-01"), _type1, true);
            _batch6 = new Batch("Machine6", Convert.ToDateTime("2001-01-01"), _type4, false);

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
                BatchTypeDAL.DeleteBatchType(_type1.Name);
                BatchTypeDAL.DeleteBatchType(_type2.Name);
                BatchTypeDAL.DeleteBatchType(_type3.Name);
                BatchTypeDAL.DeleteBatchType(_type4.Name);
            }
            catch { }
        }

        [Test]
        public void TestAddBatch()
        {
            bool success1, success2, success3, success4, success5, success6;

            success1 = BatchController.AddBatch(_batch1);
            success2 = BatchController.AddBatch(_batch2);
            success3 = BatchController.AddBatch(_batch3);
            success4 = BatchController.AddBatch(_batch4);
            success5 = BatchController.AddBatch(_batch5);
            success6 = BatchController.AddBatch(_batch6);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddBatchError()
        {
            BatchController.AddBatch(null);
        }

        [Test]
        public void TestGetAllBatches()
        {
            TestAddBatch();

            List<Batch> batches, emptyBatches;
            batches = BatchController.GetAllBatches();
            CleanUp();
            emptyBatches = BatchController.GetAllBatches();
            
            Assert.IsTrue(batches[0].Equals(_batch1));
            Assert.IsTrue(batches[1].Equals(_batch2));
            Assert.IsTrue(batches[2].Equals(_batch3));
            Assert.IsTrue(batches[3].Equals(_batch4));
            Assert.IsTrue(batches[4].Equals(_batch5));
            Assert.IsTrue(batches[5].Equals(_batch6));
            Assert.AreEqual(null, emptyBatches);
        }

        [Test]
        public void TestMakeBatchEmptyTank()
        {
            Machine mac = MachineController.InitializeMachine();
            Batch newBatchFromEmpty;
            Batch newBatchFromFull;
            PLCController plcFullTank = new PLCController("PLCFullTank.txt");
            PLCController plcEmptyTank = new PLCController("PLCEmptyTank.txt");

            newBatchFromFull = BatchController.MakeBatch_EmptyTank(plcFullTank, _type4);
            newBatchFromEmpty = BatchController.MakeBatch_EmptyTank(plcEmptyTank, _type4);

            Assert.AreEqual(mac.ID, newBatchFromEmpty.MachineID);
            Assert.AreEqual(mac.ID, newBatchFromFull.MachineID);

            Assert.IsTrue(plcFullTank.CheckMainTankIsEmpty());
            Assert.IsFalse(plcFullTank.CheckDischargeGateIsOpen());
            Assert.IsTrue(plcEmptyTank.CheckMainTankIsEmpty());
            Assert.IsFalse(plcEmptyTank.CheckDischargeGateIsOpen());
        }

        [Test]
        public void TestMakeBatchFillTank()
        {
            PLCController plc = new PLCController("PLCEmptyTankDoorClosed.txt");
            
            BatchController.MakeBatchFillFirstTank(plc);

            Assert.IsFalse(plc.CheckMainTankIsEmpty());
            Assert.IsFalse(plc.CheckLargeValveIsOpen());
        }

        [Test]
        public void TestMakeBatchUseSmallValve()
        {
            PLCController plc = new PLCController("PLCFullTank1.txt");

            BatchController.MakeBatchFillSecondTank(plc);

            Assert.IsFalse(plc.CheckMainTankIsEmpty());
            Assert.IsFalse(plc.CheckSmallValveIsOpen());
        }

        [Test]
        public void TestMakeBatchFillAdditives()
        {
            PLCController plc = new PLCController("PLCFullTank2.txt");

            BatchController.MakeBatchFillAdditives(plc);

            Assert.IsFalse(plc.CheckAdditiveValvesAreOpen());
        }

        [Test]
        public void TestMakeBatchStartMixing()
        {
            PLCController plc = new PLCController("PLCFullTank2.txt");

            BatchController.MakeBatchStart(plc);

            Assert.IsTrue(plc.CheckMotorIsOn());
            Assert.IsTrue(plc.CheckFurnaceIsOn());
        }

        [Test]
        public void TestMakeBatchWaitForFinish()
        {
            PLCController plc = new PLCController("PLCFullTankMixStarted.txt");
            Batch newBatch = BatchController.MakeBatch_EmptyTank(plc, _type4);

            BatchController.MakeBatchWaitForFinish(plc, newBatch);

            Assert.IsFalse(plc.CheckMotorIsOn());
            Assert.IsFalse(plc.CheckFurnaceIsOn());
            Assert.IsTrue(plc.CheckMainTankIsEmpty());
            Assert.IsFalse(plc.CheckDischargeGateIsOpen());
            Assert.IsFalse(plc.CheckIsFinishedAcknowledged());
        }
    }
}
