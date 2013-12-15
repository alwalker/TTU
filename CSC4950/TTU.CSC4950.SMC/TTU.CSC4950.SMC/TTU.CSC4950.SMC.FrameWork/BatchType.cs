using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTU.CSC4950.SMC.FrameWork
{
    /// <summary>
    /// 
    /// </summary>
    public class BatchType : IComparable, IEqualityComparer<BatchType>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the variable A.
        /// </summary>
        /// <value>The variable A.</value>
        public double VariableA { get; set; }

        /// <summary>
        /// Gets or sets the variable B.
        /// </summary>
        /// <value>The variable B.</value>
        public double VariableB { get; set; }

        /// <summary>
        /// Gets or sets the temperature requirement.
        /// </summary>
        /// <value>The temperature requirement.</value>
        public double? TemperatureRequirement { get; set; }

        /// <summary>
        /// Gets or sets the viscosity requirement.
        /// </summary>
        /// <value>The viscosity requirement.</value>
        public double? ViscosityRequirement { get; set; }

        /// <summary>
        /// Gets or sets the speed requirement.
        /// </summary>
        /// <value>The speed requirement.</value>
        public double? SpeedRequirement { get; set; }

        /// <summary>
        /// Gets or sets the time requirement.
        /// </summary>
        /// <value>The time requirement.</value>
        public double? TimeRequirement { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchType"/> class.
        /// </summary>
        public BatchType()
        {
            Name = string.Empty;
            VariableA = 0;
            VariableB = 0;
            TemperatureRequirement = null;
            ViscosityRequirement = null;
            SpeedRequirement = null;
            TimeRequirement = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchType"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="a">The variable A.</param>
        /// <param name="b">The variable B.</param>
        /// <param name="temp">The temperature requirement.</param>
        /// <param name="viscosity">The viscosity requirement.</param>
        /// <param name="speed">The speed requirement.</param>
        /// <param name="time">The time requirement.</param>
        public BatchType(string name, double a, double b, double? temp, double? viscosity, double? speed, double? time)
        {
            Name = name;
            VariableA = a;
            VariableB = b;
            TemperatureRequirement = temp;
            ViscosityRequirement = viscosity;
            SpeedRequirement = speed;
            TimeRequirement = time;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="obj"/> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            BatchType other = obj as BatchType;

            if (other == null)
            {
                return -2;
            }

            if (other.Name == Name && other.VariableA == VariableA && other.VariableB == VariableB && other.TemperatureRequirement == TemperatureRequirement 
                && other.ViscosityRequirement == ViscosityRequirement && other.SpeedRequirement == SpeedRequirement && other.TimeRequirement == TimeRequirement)
            {
                return 0;
            }
            else
            { return -1; }
        }

        #endregion

        #region IEqualityComparer<BatchType> Members

        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(BatchType x, BatchType y)
        {
            if (x.Name == y.Name && x.VariableA == y.VariableA && x.VariableB == y.VariableB &&
                x.TemperatureRequirement == y.TemperatureRequirement && x.ViscosityRequirement == y.ViscosityRequirement &&
                x.SpeedRequirement == y.SpeedRequirement && x.TimeRequirement == y.TimeRequirement)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(BatchType obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}