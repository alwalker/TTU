using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TTU.CSC4950.SMC.FrameWork;
using PLCSimulator;

namespace TTU.CSC4950.SMC.LogicLayer
{
    public class PLCController
    {
        #region Fields

        private PLC _plc;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PLCController"/> class.
        /// </summary>
        /// <param name="fileName">Name of the config file for the plc simulator.</param>
        public PLCController(string fileName)
        {
            _plc = new PLC(fileName);

            while (!_plc.Connect()) { }
        }

        #endregion Constructor

        #region Methods 
        /// <summary>
        /// Gets the sensor readings from the PLC.
        /// </summary>
        /// <returns></returns>
        public SensorData GetSensorData()
        {
            double temp, speed;
            SensorData data = new SensorData();

            while (!_plc.ReadWord("OUT[17]", out speed)) { }
            data.Speed = speed;
            while (!_plc.ReadWord("OUT[16]", out temp)) { }
            data.Temperature = temp;

            return data;
        }

        /// <summary>
        /// Check the plc to see if the main tank is empty.
        /// </summary>
        /// <returns></returns>
        public bool CheckMainTankIsEmpty()
        {
            bool isEmpty;

            while (!_plc.ReadBit("IN[1]", out isEmpty)) { }

            return isEmpty;
        }

        /// <summary>
        /// Check the plc to see if the discharge gate is open.
        /// </summary>
        /// <returns></returns>
        public bool CheckDischargeGateIsOpen()
        {
            bool isClosed, isOpen;

            while (!_plc.ReadBit("IN[3]", out isClosed)) { }
            while (!_plc.ReadBit("IN[4]", out isOpen)) { }

            if (isClosed && !isOpen)
            {
                return false;
            }
            else if (isOpen && !isClosed)
            {
                return true;
            }
            else
            {
                throw new ApplicationException("PLC in invalid state!");
            }
        }

        /// <summary>
        /// Tell the plc to open the discharge gate.
        /// </summary>
        public void OpenDischargeGate()
        {
            while (!_plc.WriteBit("OUT[2]", true)) { }
        }


        /// <summary>
        /// Tell the plc to close the discharge gate.
        /// </summary>
        public void CloseDischargeGate()
        {
            while (!_plc.WriteBit("OUT[1]", true)) { }
        }


        /// <summary>
        /// Check the plc to see if the large valve is open.
        /// </summary>
        /// <returns></returns>
        public bool CheckLargeValveIsOpen()
        {
            bool isClosed, isOpen;

            while (!_plc.ReadBit("IN[5]", out isClosed)) { }
            while (!_plc.ReadBit("IN[6]", out isOpen)) { }

            if (isClosed && !isOpen)
            {
                return false;
            }
            else if (isOpen && !isClosed)
            {
                return true;
            }
            else
            {
                throw new ApplicationException("PLC in invalid state!");
            }
        }

        /// <summary>
        /// Tell the plc to close the large valve.
        /// </summary>
        public void CloseLargeValve()
        {
            while (!_plc.WriteBit("OUT[4]", true)) { }
        }

        /// <summary>
        /// Tell the plc to open the large valve.
        /// </summary>
        public void OpenLargeValve()
        {
            while (!_plc.WriteBit("OUT[3]", true)) { }
        }

        /// <summary>
        /// Check the plc to see if the small valve is open.
        /// </summary>
        /// <returns></returns>
        public bool CheckSmallValveIsOpen()
        {
            bool isClosed, isOpen;

            while (!_plc.ReadBit("IN[7]", out isClosed)) { }
            while (!_plc.ReadBit("IN[8]", out isOpen)) { }

            if (isClosed && !isOpen)
            {
                return false;
            }
            else if (isOpen && !isClosed)
            {
                return true;
            }
            else
            {
                throw new ApplicationException("PLC in invalid state!");
            }
        }

        /// <summary>
        /// Tell the plc to open the small valve.
        /// </summary>
        public void OpenSmallValve()
        {
            while (!_plc.WriteBit("OUT[5]", true)) { }
        }


        /// <summary>
        /// Tell the plc to close the small valve.
        /// </summary>
        public void CloseSmallValve()
        {
            while (!_plc.WriteBit("OUT[6]", true)) { }
        }

        /// <summary>
        /// Check the plc to see if the additive valves are open.
        /// </summary>
        /// <returns></returns>
        public bool CheckAdditiveValvesAreOpen()
        {
            bool open1, open2, open3;

            while (!_plc.ReadBit("IN[9]", out open1)) { }
            while (!_plc.ReadBit("IN[10]", out open2)) { }
            while (!_plc.ReadBit("IN[11]", out open3)) { }

            if (open1 && open2 && open3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tell the plc to close the additive valves.
        /// </summary>
        public void CloseAdditiveValves()
        {
            while (!_plc.WriteBit("OUT[7]", false)) { }
            while (!_plc.WriteBit("OUT[8]", false)) { }
            while (!_plc.WriteBit("OUT[9]", false)) { }
        }

        /// <summary>
        /// Tell the plc to open the additive valves.
        /// </summary>
        public void OpenAdditiveValves()
        {
            while (!_plc.WriteBit("OUT[7]", true)) { }
            while (!_plc.WriteBit("OUT[8]", true)) { }
            while (!_plc.WriteBit("OUT[9]", true)) { }
        }

        /// <summary>
        /// Check the plc to see if the motor is on.
        /// </summary>
        /// <returns></returns>
        public bool CheckMotorIsOn()
        {
            bool isOn;

            while (!_plc.ReadBit("IN[12]", out isOn)) { }

            return isOn;
        }

        /// <summary>
        /// Tell the plc to start the motor.
        /// </summary>
        public void StartMotor()
        {
            while (!_plc.WriteBit("OUT[10]", true)) { }
        }

        /// <summary>
        /// Tell the plc to stop the motor.
        /// </summary>
        public void StopMotor()
        {
            while (!_plc.WriteBit("OUT[10]", false)) { }
        }

        /// <summary>
        /// Check the plc to see if the furnace is on.
        /// </summary>
        /// <returns></returns>
        public bool CheckFurnaceIsOn()
        {
            bool isOn;

            while (!_plc.ReadBit("IN[13]", out isOn)) { }

            return isOn;
        }

        /// <summary>
        /// Tell the plc to turn on the furnace.
        /// </summary>
        public void TurnOnFurnace()
        {
            while (!_plc.WriteBit("OUT[11]", true)) { }
        }

        /// <summary>
        /// Tell the plc to turn off the furnace.
        /// </summary>
        public void TurnOffFurnace()
        {
            while (!_plc.WriteBit("OUT[11]", false)) { }
        }

        /// <summary>
        /// Check the plc to see if the batch finished alarm has been acknowledged
        /// </summary>
        /// <returns></returns>
        public bool CheckIsFinishedAcknowledged()
        {
            bool acked;

            while (!_plc.ReadBit("IN[14]", out acked)) { }

            return acked;
        }

        /// <summary>
        /// Tell the plc to signal the batch finished alarm
        /// </summary>
        public void SignalFinishedBatch()
        {
            while (!_plc.WriteBit("OUT[14]", true)) { }
        }

        /// <summary>
        /// Check the plc to see if the alarm is silenced.
        /// </summary>
        /// <returns></returns>
        public bool CheckIsAlarmSilenced()
        {
            bool isSilenced;

            while (!_plc.ReadBit("IN[15]", out isSilenced)) { }

            return isSilenced;
        }

        /// <summary>
        /// Tell the plc to sound the alarm.
        /// </summary>
        public void SoundAlarm()
        {
            while (!_plc.WriteBit("OUT[15]", true)) { }
        }

        #endregion Methods
    }
}
