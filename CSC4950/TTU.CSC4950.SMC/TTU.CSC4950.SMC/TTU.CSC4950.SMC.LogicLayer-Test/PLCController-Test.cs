using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUnit.Framework;
using TTU.CSC4950.SMC.FrameWork;
using TTU.CSC4950.SMC.LogicLayer;
using PLCSimulator;

namespace TTU.CSC4950.SMC.LogicLayer_Test
{
    [TestFixture]
    public class PLCController_Test
    {
        PLCController _plc;

        [SetUp]
        public void SetUp()
        {
            _plc = new PLCController("PLC.txt");
        }

        [TearDown]
        public void CleanUp()
        {
        }

        [Test]
        public void TestGetSensorData()
        {
            SensorData data = null;

            data = _plc.GetSensorData();

            Assert.AreEqual(123.456, data.Temperature);
            Assert.AreEqual(78.901, data.Speed);
        }

        [Test]
        public void TestCheckMainTankIsEmpty()
        {
            bool tankIsEmpty;

            tankIsEmpty = _plc.CheckMainTankIsEmpty();

            Assert.IsFalse(tankIsEmpty);
        }

        [Test]
        public void TestCheckDischargeGateIsOpen()
        {
            bool gateIsOpen = _plc.CheckDischargeGateIsOpen();

            Assert.IsFalse(gateIsOpen);
        }


        [Test]
        public void TestOpenDischargeGate()
        {
            _plc.OpenDischargeGate();

            Assert.IsTrue(_plc.CheckDischargeGateIsOpen());
        }

        [Test]
        public void TestCloseDischargeGate()
        {
            _plc.CloseDischargeGate();

            Assert.IsFalse(_plc.CheckDischargeGateIsOpen());
        }

        [Test]
        public void TestCheckLargeValveIsOpen()
        {
            Assert.IsTrue(_plc.CheckLargeValveIsOpen());
        }

        [Test]
        public void TestCloseLargeValve()
        {
            _plc.OpenLargeValve();
            _plc.CloseLargeValve();
            
            Assert.IsFalse(_plc.CheckLargeValveIsOpen());
        }

        [Test]
        public void TestOpenLargeValve()
        {
            _plc.CloseLargeValve();
            _plc.OpenLargeValve();

            Assert.IsTrue(_plc.CheckLargeValveIsOpen());
        }

        [Test]
        public void TestCheckSmallValveIsOpen()
        {
            Assert.IsFalse(_plc.CheckSmallValveIsOpen());
        }

        [Test]
        public void TestOpenSmallValve()
        {
            _plc.OpenSmallValve();

            Assert.IsTrue(_plc.CheckSmallValveIsOpen());
        }

        [Test]
        public void TestCloseSmallValve()
        {
            _plc.CloseSmallValve();

            Assert.IsFalse(_plc.CheckSmallValveIsOpen());
        }

        [Test]
        public void TestCheckAdditiveValvesAreOpen()
        {
            Assert.IsTrue(_plc.CheckAdditiveValvesAreOpen());
        }

        [Test]
        public void TestCloseAdditiveValves()
        {
            _plc.CloseAdditiveValves();

            Assert.IsFalse(_plc.CheckAdditiveValvesAreOpen());
        }

        [Test]
        public void TestOpenAdditiveValves()
        {
            _plc.OpenAdditiveValves();

            Assert.IsTrue(_plc.CheckAdditiveValvesAreOpen());
        }

        [Test]
        public void TestCheckMotorIsOn()
        {
            Assert.IsFalse(_plc.CheckMotorIsOn());
        }

        public void TestStartMotor()
        {
            _plc.StartMotor();

            Assert.IsTrue(_plc.CheckMotorIsOn());
        }

        public void TestStopMotor()
        {
            _plc.StopMotor();

            Assert.IsFalse(_plc.CheckMotorIsOn());
        }

        public void TestCheckFurnaceIsOn()
        {
            Assert.IsFalse(_plc.CheckFurnaceIsOn());
        }

        public void TestTurnOnFurnace()
        {
            _plc.TurnOnFurnace();

            Assert.IsTrue(_plc.CheckFurnaceIsOn());
        }

        public void TestTurnOffFurnace()
        {
            _plc.TurnOffFurnace();

            Assert.IsFalse(_plc.CheckFurnaceIsOn());
        }

        [Test]
        public void TestIsFinishedAcknowledged()
        {
            Assert.IsFalse(_plc.CheckIsFinishedAcknowledged());
        }

        [Test]
        public void TestSignalFinishedBatch()
        {
            _plc.SignalFinishedBatch();

            Assert.IsFalse(_plc.CheckIsFinishedAcknowledged());
        }

        [Test]
        public void TestCheckIsAlarmSilenced()
        {
            Assert.IsTrue(_plc.CheckIsAlarmSilenced());
        }

        [Test]
        public void TestSoundAlarm()
        {
            _plc.SoundAlarm();

            Assert.IsFalse(_plc.CheckIsAlarmSilenced());
        }
    }
}
