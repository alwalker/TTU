using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALW_TTU_CSC2400P2
{
    public class Vertex
    {
        #region Properties

        public int Number { get; set; }

        public List<Edge> Edges { get; set; }

        #endregion Properties

        #region Constructor

        public Vertex(int num, Edge edge)
        {
            Number = num;
            Edges = new List<Edge>();
            Edges.Add(edge);
        }

        #endregion Constructor

        #region Methods

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(Number.ToString());
            str.Append(": ");

            foreach (Edge edge in Edges)
            {
                str.Append(edge.ToString());
                str.Append(", ");
            }

            return str.ToString().Substring(0, str.ToString().Length - 2);
        }

        #endregion Methods
    }
}
