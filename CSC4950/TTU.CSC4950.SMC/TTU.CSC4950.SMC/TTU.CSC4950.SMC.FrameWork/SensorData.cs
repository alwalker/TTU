using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTU.CSC4950.SMC.FrameWork
{
    public class SensorData
    {
        #region Property

        /// <summary>
        /// Gets or sets the time the machine has been running.
        /// </summary>
        /// <value>The time.</value>
        public double Speed { get; set; }

        /// <summary>
        /// Gets or sets the temperature of the machine.
        /// </summary>
        /// <value>The temperature.</value>
        public double Temperature { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorData"/> class.
        /// </summary>
        public SensorData()
        {
            Speed = 0;
            Temperature = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorData"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="temp">The temp.</param>
        public SensorData(double speed, double temp)
        {
            Speed = speed;
            Temperature = temp;
        }

        #endregion
    }
}
