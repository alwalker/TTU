using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALW_TTU_CSC2400P2
{
    public class Edge
    {
        #region Properties

        public int Connection { get; set; }

        public int Weight { get; set; }

        #endregion Properties

        #region Constuctor

        public Edge(int edge, int weight)
        {
            Connection = edge;
            Weight = weight;
        }

        #endregion Constructor

        #region Methods

        public override string ToString()
        {
            return Connection.ToString() + ":" + Weight.ToString();
        }

        #endregion Methods
    }
}
