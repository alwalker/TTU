// <copyright file="PLCControllerTest.cs" company="Microsoft">Copyright © Microsoft 2010</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TTU.CSC4950.SMC.FrameWork;
using TTU.CSC4950.SMC.LogicLayer;

namespace TTU.CSC4950.SMC.LogicLayer
{
    /// <summary>This class contains parameterized unit tests for PLCController</summary>
    [PexClass(typeof(PLCController))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class PLCControllerTest
    {
        /// <summary>Test stub for CheckAdditiveValvesAreOpen()</summary>
        [PexMethod]
        public bool CheckAdditiveValvesAreOpen([PexAssumeUnderTest]PLCController target)
        {
            bool result = target.CheckAdditiveValvesAreOpen();
            return result;
            // TODO: add assertions to method PLCControllerTest.CheckAdditiveValvesAreOpen(PLCController)
        }

        /// <summary>Test stub for CheckDischargeGateIsOpen()</summary>
        [PexMethod]
        public bool CheckDischargeGateIsOpen([PexAssumeUnderTest]PLCController target)
        {
            bool result = target.CheckDischargeGateIsOpen();
            return result;
            // TODO: add assertions to method PLCControllerTest.CheckDischargeGateIsOpen(PLCController)
        }

        /// <summary>Test stub for CheckFurnaceIsOn()</summary>
        [PexMethod]
        public bool CheckFurnaceIsOn([PexAssumeUnderTest]PLCController target)
        {
            bool result = target.CheckFurnaceIsOn();
            return result;
            // TODO: add assertions to method PLCControllerTest.CheckFurnaceIsOn(PLCController)
        }

        /// <summary>Test stub for CheckIsAlarmSilenced()</summary>
        [PexMethod]
        public bool CheckIsAlarmSilenced([PexAssumeUnderTest]PLCController target)
        {
            bool result = target.CheckIsAlarmSilenced();
            return result;
            // TODO: add assertions to method PLCControllerTest.CheckIsAlarmSilenced(PLCController)
        }

        /// <summary>Test stub for CheckIsFinishedAcknowledged()</summary>
        [PexMethod]
        public bool CheckIsFinishedAcknowledged([PexAssumeUnderTest]PLCController target)
        {
            bool result = target.CheckIsFinishedAcknowledged();
            return result;
            // TODO: add assertions to method PLCControllerTest.CheckIsFinishedAcknowledged(PLCController)
        }

        /// <summary>Test stub for CheckLargeValveIsOpen()</summary>
        [PexMethod]
        public bool CheckLargeValveIsOpen([PexAssumeUnderTest]PLCController target)
        {
            bool result = target.CheckLargeValveIsOpen();
            return result;
            // TODO: add assertions to method PLCControllerTest.CheckLargeValveIsOpen(PLCController)
        }

        /// <summary>Test stub for CheckMainTankIsEmpty()</summary>
        [PexMethod]
        public bool CheckMainTankIsEmpty([PexAssumeUnderTest]PLCController target)
        {
            bool result = target.CheckMainTankIsEmpty();
            return result;
            // TODO: add assertions to method PLCControllerTest.CheckMainTankIsEmpty(PLCController)
        }

        /// <summary>Test stub for CheckMotorIsOn()</summary>
        [PexMethod]
        public bool CheckMotorIsOn([PexAssumeUnderTest]PLCController target)
        {
            bool result = target.CheckMotorIsOn();
            return result;
            // TODO: add assertions to method PLCControllerTest.CheckMotorIsOn(PLCController)
        }

        /// <summary>Test stub for CheckSmallValveIsOpen()</summary>
        [PexMethod]
        public bool CheckSmallValveIsOpen([PexAssumeUnderTest]PLCController target)
        {
            bool result = target.CheckSmallValveIsOpen();
            return result;
            // TODO: add assertions to method PLCControllerTest.CheckSmallValveIsOpen(PLCController)
        }

        /// <summary>Test stub for CloseAdditiveValves()</summary>
        [PexMethod]
        public void CloseAdditiveValves([PexAssumeUnderTest]PLCController target)
        {
            target.CloseAdditiveValves();
            // TODO: add assertions to method PLCControllerTest.CloseAdditiveValves(PLCController)
        }

        /// <summary>Test stub for CloseDischargeGate()</summary>
        [PexMethod]
        public void CloseDischargeGate([PexAssumeUnderTest]PLCController target)
        {
            target.CloseDischargeGate();
            // TODO: add assertions to method PLCControllerTest.CloseDischargeGate(PLCController)
        }

        /// <summary>Test stub for CloseLargeValve()</summary>
        [PexMethod]
        public void CloseLargeValve([PexAssumeUnderTest]PLCController target)
        {
            target.CloseLargeValve();
            // TODO: add assertions to method PLCControllerTest.CloseLargeValve(PLCController)
        }

        /// <summary>Test stub for CloseSmallValve()</summary>
        [PexMethod]
        public void CloseSmallValve([PexAssumeUnderTest]PLCController target)
        {
            target.CloseSmallValve();
            // TODO: add assertions to method PLCControllerTest.CloseSmallValve(PLCController)
        }

        /// <summary>Test stub for .ctor(String)</summary>
        [PexMethod]
        public PLCController Constructor(string fileName)
        {
            PLCController target = new PLCController(fileName);
            return target;
            // TODO: add assertions to method PLCControllerTest.Constructor(String)
        }

        /// <summary>Test stub for GetSensorData()</summary>
        [PexMethod]
        public SensorData GetSensorData([PexAssumeUnderTest]PLCController target)
        {
            SensorData result = target.GetSensorData();
            return result;
            // TODO: add assertions to method PLCControllerTest.GetSensorData(PLCController)
        }

        /// <summary>Test stub for OpenAdditiveValves()</summary>
        [PexMethod]
        public void OpenAdditiveValves([PexAssumeUnderTest]PLCController target)
        {
            target.OpenAdditiveValves();
            // TODO: add assertions to method PLCControllerTest.OpenAdditiveValves(PLCController)
        }

        /// <summary>Test stub for OpenDischargeGate()</summary>
        [PexMethod]
        public void OpenDischargeGate([PexAssumeUnderTest]PLCController target)
        {
            target.OpenDischargeGate();
            // TODO: add assertions to method PLCControllerTest.OpenDischargeGate(PLCController)
        }

        /// <summary>Test stub for OpenLargeValve()</summary>
        [PexMethod]
        public void OpenLargeValve([PexAssumeUnderTest]PLCController target)
        {
            target.OpenLargeValve();
            // TODO: add assertions to method PLCControllerTest.OpenLargeValve(PLCController)
        }

        /// <summary>Test stub for OpenSmallValve()</summary>
        [PexMethod]
        public void OpenSmallValve([PexAssumeUnderTest]PLCController target)
        {
            target.OpenSmallValve();
            // TODO: add assertions to method PLCControllerTest.OpenSmallValve(PLCController)
        }

        /// <summary>Test stub for SignalFinishedBatch()</summary>
        [PexMethod]
        public void SignalFinishedBatch([PexAssumeUnderTest]PLCController target)
        {
            target.SignalFinishedBatch();
            // TODO: add assertions to method PLCControllerTest.SignalFinishedBatch(PLCController)
        }

        /// <summary>Test stub for SoundAlarm()</summary>
        [PexMethod]
        public void SoundAlarm([PexAssumeUnderTest]PLCController target)
        {
            target.SoundAlarm();
            // TODO: add assertions to method PLCControllerTest.SoundAlarm(PLCController)
        }

        /// <summary>Test stub for StartMotor()</summary>
        [PexMethod]
        public void StartMotor([PexAssumeUnderTest]PLCController target)
        {
            target.StartMotor();
            // TODO: add assertions to method PLCControllerTest.StartMotor(PLCController)
        }

        /// <summary>Test stub for StopMotor()</summary>
        [PexMethod]
        public void StopMotor([PexAssumeUnderTest]PLCController target)
        {
            target.StopMotor();
            // TODO: add assertions to method PLCControllerTest.StopMotor(PLCController)
        }

        /// <summary>Test stub for TurnOffFurnace()</summary>
        [PexMethod]
        public void TurnOffFurnace([PexAssumeUnderTest]PLCController target)
        {
            target.TurnOffFurnace();
            // TODO: add assertions to method PLCControllerTest.TurnOffFurnace(PLCController)
        }

        /// <summary>Test stub for TurnOnFurnace()</summary>
        [PexMethod]
        public void TurnOnFurnace([PexAssumeUnderTest]PLCController target)
        {
            target.TurnOnFurnace();
            // TODO: add assertions to method PLCControllerTest.TurnOnFurnace(PLCController)
        }
    }
}
