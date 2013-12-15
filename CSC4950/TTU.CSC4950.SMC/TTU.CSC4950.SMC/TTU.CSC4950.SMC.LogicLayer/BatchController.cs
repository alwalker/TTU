using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTU.CSC4950.SMC.FrameWork;
using TTU.CSC4950.SMC.DataLayer;
using System.Threading;
using System.ComponentModel;

namespace TTU.CSC4950.SMC.LogicLayer
{
    public class BatchController
    {
        /// <summary>
        /// Adds the batch.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <returns></returns>
        public static bool AddBatch(Batch batch)
        {
            try
            {
                return BatchDAL.AddBatch(batch);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all batches.
        /// </summary>
        /// <returns></returns>
        public static List<Batch> GetAllBatches()
        {
            try
            {
                return BatchDAL.GetAllBatches();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Makes the batch.
        /// </summary>
        /// <param name="mac">The machine the batch is being mixed on.</param>
        /// <param name="type">The type of batch.</param>
        /// <param name="worker">The worker.</param>
        /// <returns></returns>
        public static Batch MakeBatch(Machine mac, BatchType type, BackgroundWorker worker)
        {
            DateTime start, stop;
            SensorData readings;
            PLCController plc = new PLCController("PLC.txt");
            Batch newBatch = new Batch();
            newBatch.MachineID = mac.ID;
            newBatch.Type = type;

            //If the main tank is not empty drain it
            if (!plc.CheckMainTankIsEmpty())
            {
                plc.OpenDischargeGate();

                while (!plc.CheckMainTankIsEmpty()) { Thread.Sleep(500); }
            }

            worker.ReportProgress(1);

            //Close the gate
            if (plc.CheckDischargeGateIsOpen())
            {
                plc.CloseDischargeGate();
            }

            worker.ReportProgress(2);

            //Open large valve until main tank is full
            plc.OpenLargeValve();
            worker.ReportProgress(3);

            Thread.Sleep(1000);

            plc.CloseLargeValve();
            worker.ReportProgress(4);

            //Open small valve for a set time
            plc.OpenSmallValve();
            worker.ReportProgress(5);

            Thread.Sleep(1000);

            plc.CloseSmallValve();
            worker.ReportProgress(6);


            //Open additive valves for a set time
            plc.OpenAdditiveValves();
            worker.ReportProgress(7);

            Thread.Sleep(1000);

            plc.CloseAdditiveValves();
            worker.ReportProgress(8);
            
            //start motor
            plc.StartMotor();
            worker.ReportProgress(9);
                        
            //start furnace
            plc.TurnOnFurnace();
            worker.ReportProgress(10);

            start = DateTime.Now;

            //stop furnace and motor and open dishcarge gate once requirements are met
            if (!type.TimeRequirement.HasValue)
            {
                if (type.TemperatureRequirement.HasValue)
                {
                    while (true)
                    {
                        readings = plc.GetSensorData();
                        if (readings.Temperature >= type.TemperatureRequirement)
                        {
                            break;
                        }
                    }                    
                }
                else if (type.ViscosityRequirement.HasValue)
                {
                    //calculate viscosity
                }
            }
            else
            {
                while (true)
                {
                    readings = plc.GetSensorData();
                    stop = DateTime.Now;
                    if (readings.Speed >= type.SpeedRequirement && (stop - start).TotalSeconds >= type.TimeRequirement)
                    {
                        break;
                    }
                }
            }

            plc.StopMotor();
            plc.TurnOffFurnace();
            plc.OpenDischargeGate();

            //signal batch done
            plc.SignalFinishedBatch();

            return newBatch;
        }

        /// <summary>
        /// Stage 1.
        /// </summary>
        /// <param name="plc">The PLC.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Batch MakeBatch_EmptyTank(PLCController plc, BatchType type)
        {
            Batch newBatch = new Batch();
            newBatch.BatchDate = DateTime.Now;
            newBatch.MachineID = MachineController.InitializeMachine().ID;
            newBatch.Type = type;

            if (!plc.CheckMainTankIsEmpty())
            {
                plc.OpenDischargeGate();

                while (!plc.CheckMainTankIsEmpty()) { }
            }

            plc.CloseDischargeGate();

            return newBatch;
        }

        /// <summary>
        /// Stage 2.
        /// </summary>
        /// <param name="plc">The PLC.</param>
        public static void MakeBatchFillFirstTank(PLCController plc)
        {
            plc.OpenLargeValve();

            while (plc.CheckMainTankIsEmpty())
            {
            }

            plc.CloseLargeValve();
        }

        /// <summary>
        /// Stage 3.
        /// </summary>
        /// <param name="plc">The PLC.</param>
        public static void MakeBatchFillSecondTank(PLCController plc)
        {
            plc.OpenSmallValve();

            while (plc.CheckMainTankIsEmpty())
            { }

            plc.CloseSmallValve();
        }

        /// <summary>
        /// Stage 4.
        /// </summary>
        /// <param name="plc">The PLC.</param>
        public static void MakeBatchFillAdditives(PLCController plc)
        {
            plc.OpenAdditiveValves();

            System.Threading.Thread.Sleep(500);

            plc.CloseAdditiveValves();
        }

        /// <summary>
        /// Stage 5.
        /// </summary>
        /// <param name="plc">The PLC.</param>
        public static void MakeBatchStart(PLCController plc)
        {
            plc.StartMotor();
            plc.TurnOnFurnace();
        }

        /// <summary>
        /// Stage 6.
        /// </summary>
        /// <param name="plc">The PLC.</param>
        /// <param name="newBatch">The new batch.</param>
        public static void MakeBatchWaitForFinish(PLCController plc, Batch newBatch)
        {
            DateTime start = DateTime.Now;
            DateTime stop;
            SensorData readings;
            //stop furnace and motor and open dishcarge gate once requirements are met
            if (!newBatch.Type.TimeRequirement.HasValue)
            {
                if (newBatch.Type.TemperatureRequirement.HasValue)
                {
                    while (true)
                    {
                        readings = plc.GetSensorData();
                        if (readings.Temperature >= newBatch.Type.TemperatureRequirement)
                        {
                            break;
                        }
                    }
                }
                else if (newBatch.Type.ViscosityRequirement.HasValue)
                {
                    //calculate viscosity
                }
            }
            else
            {
                while (true)
                {
                    readings = plc.GetSensorData();
                    stop = DateTime.Now;
                    if (readings.Speed >= newBatch.Type.SpeedRequirement && (stop - start).TotalSeconds >= newBatch.Type.TimeRequirement)
                    {
                        break;
                    }
                }
            }

            newBatch.Success = true;
            newBatch.BatchDate = DateTime.Now;
            plc.TurnOffFurnace();
            plc.StopMotor();
            plc.OpenDischargeGate();
            plc.SignalFinishedBatch();
        }
    }
}
