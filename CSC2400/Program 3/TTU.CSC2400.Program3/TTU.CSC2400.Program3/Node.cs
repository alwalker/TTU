using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTU.CSC2400.Program3
{
    public class Node
    {
        public string Name;
        public List<Node> Neighbors;
        public List<double> Weights;

        public Node(string name)
        {
            Neighbors = new List<Node>();
            Weights = new List<double>();

            if (name == null)
            {
                Name = string.Empty;
            }
            else
            {
                Name = name;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
