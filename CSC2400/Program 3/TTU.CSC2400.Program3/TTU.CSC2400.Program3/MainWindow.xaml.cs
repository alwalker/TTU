using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace TTU.CSC2400.Program3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private DijkstraWindow _dWindow;
        private PrimmWindow _pWindow;
        private LoadWindow _lWindow;

        private List<Node> _nodes;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            _nodes = new List<Node>();
            _lWindow = new LoadWindow();
        }

        #endregion Constructor

        #region Events

        private void btnDikjstra_Click(object sender, RoutedEventArgs e)
        {
            if (_dWindow.ShowDialog().Value)
            {
                MessageBox.Show(Dijkstra(_dWindow.StartNode, _dWindow.GoalNode));
                _dWindow = null;
                _dWindow = new DijkstraWindow(_nodes);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (_lWindow.ShowDialog().Value)
            {
                if (BuildGraph(_lWindow.FileName))
                {
                    _dWindow = new DijkstraWindow(_nodes);
                    _pWindow = new PrimmWindow(_nodes);
                    btnDikjstra.IsEnabled = true;
                    btnPrimm.IsEnabled = true;
                    btnClear.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Could not load graph from this file.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnPrimm_Click(object sender, RoutedEventArgs e)
        {
            if (_pWindow.ShowDialog().Value)
            {
                MessageBox.Show(Prim(_pWindow.StartNode));
                _pWindow = null;
                _pWindow = new PrimmWindow(_nodes);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _nodes = null;
            _dWindow = null;
            _pWindow = null;
            btnDikjstra.IsEnabled = false;
            btnPrimm.IsEnabled = false;
            btnClear.IsEnabled = false;
        }

        #endregion Events

        #region Methods

        private bool BuildGraph(string fileName)
        {
            string line, node, edge, weight;
            StringBuilder graph = new StringBuilder();
            bool found = false;
            Node currentNode, newNode = null;

            //Build initial graph
            try
            {
                //First pass, get all nodes
                using (StreamReader sr = File.OpenText(fileName))
                {
                    line = sr.ReadLine();

                    while (line != null)
                    {
                        node = line.Split(',')[0];
                        edge = line.Split(',')[1];
                        weight = line.Split(',')[2];

                        graph.Append("Node: " + node + " Edge: " + edge + " Cost: " + weight + "\n");

                        if (newNode == null)
                        {
                            newNode = new Node(node);
                            _nodes.Add(newNode);
                            line = sr.ReadLine();
                            continue;
                        }

                        foreach (Node nde in _nodes)
                        {
                            if (nde.Name == node)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            newNode = new Node(node);
                            _nodes.Add(newNode);
                            found = false;
                        }
                        else
                        {
                            found = false;
                        }


                        line = sr.ReadLine();
                    }

                    sr.Close();
                }

                //Print graph to screen
                txbGraphDescription.Text = graph.ToString();

                //Second pass, build graph
                if (_nodes.Count <= 0)
                {
                    MessageBox.Show("No nodes.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                currentNode = _nodes[0];
                using (StreamReader sr = File.OpenText(fileName))
                {
                    line = sr.ReadLine();

                    while (line != null)
                    {
                        node = line.Split(',')[0];
                        edge = line.Split(',')[1];
                        weight = line.Split(',')[2];

                        if (currentNode.Name != node)
                        {
                            foreach (Node nod in _nodes)
                            {
                                if (nod.Name == node)
                                {
                                    currentNode = nod;
                                    break;
                                }
                            }
                        }

                        foreach (Node nod in _nodes)
                        {
                            if (nod.Name == edge)
                            {
                                currentNode.Weights.Add(Convert.ToDouble(weight));
                                currentNode.Neighbors.Add(nod);
                                break;
                            }
                        }

                        line = sr.ReadLine();
                    }

                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private string Dijkstra(Node start, Node goal)
        {
            int sourceIndex = 0, uIndex = 0, count, prevCount = 0;
            double minDist = 100;
            double alt;
            string temp = string.Empty;
            StringBuilder path = new StringBuilder();
            List<double> dist = new List<double>(_nodes.Count);
            List<Node> previous = new List<Node>(_nodes.Count);
            List<Node> tempNodes = new List<Node>(_nodes.Count);

            //Initialize distance and previous node lists
            for (int i = 0; i < _nodes.Count; i++)
            {
                dist.Add(1000);
                previous.Add(null);

                tempNodes.Add(_nodes[i]);

                if (_nodes[i].Name == start.Name)
                {
                    sourceIndex = i;
                }
            }

            //set distance to source so we have a place ot start
            dist[sourceIndex] = 0;

            //loop through all nodes in graph
            while (tempNodes.Count > 0)
            {
                count = 0;
                minDist = 100;

                //find closest node to starting point
                for (int i = 0; i < tempNodes.Count; i++)
                {
                    if (dist[i] < minDist)
                    {
                        minDist = dist[i];
                        uIndex = i;
                    }
                }

                //if we have no shortest distance then we are stranded
                if (dist[uIndex] == 1000)
                {
                    break;
                }

                //we reached the goal noad build the string representing the shortest path and return it
                if (tempNodes[uIndex].Name == goal.Name)
                {
                    for (int i = sourceIndex; i < previous.Count; i++)
                    {
                        if (previous[i] != null)
                        {
                            path.Append(previous[i].Name + " => ");
                        }
                    }
                    for (int i = 0; i < sourceIndex; i++)
                    {
                        if (previous[i] != null)
                        {
                            path.Append(previous[i].Name + " => ");
                        }
                    }

                    path.Append(goal.Name);
                    return path.ToString();
                }

                //find the closest neighbor
                foreach (Node v in tempNodes[uIndex].Neighbors)
                {
                    alt = dist[uIndex] + tempNodes[uIndex].Weights[count];
                    if (tempNodes.IndexOf(v) >= 0 && alt < dist[tempNodes.IndexOf(v)])
                    {
                        dist[tempNodes.IndexOf(v)] = alt;
                        previous[tempNodes.IndexOf(v)] = tempNodes[uIndex];
                        prevCount++;
                    }

                    count++;
                }

                tempNodes.Remove(tempNodes[uIndex]);
                dist.Remove(dist[uIndex]);
            }
            return null;
        }

        private string Prim(Node start)
        {
            double min = 100;
            string temp;
            StringBuilder tree = new StringBuilder(start.Name + " => ");
            Node minNode = null;
            Dictionary<Node, double> frontierList = new Dictionary<Node, double>();
            List<Node> tempNodes = new List<Node>();

            //build the initial node list
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].Name != start.Name)
                {
                    tempNodes.Add(_nodes[i]);
                }
            }

            //build the initial frontier list
            for (int i = 0; i < start.Neighbors.Count; i++)
            {
                frontierList.Add(start.Neighbors[i], start.Weights[i]);
            }

            //loop until we have a tree with all the nodes
            while (tempNodes.Count > 0)
            {
                //find the closest node in the frontier list
                foreach (Node n in frontierList.Keys)
                {
                    if (frontierList[n] < min)
                    {
                        minNode = n;
                        min = frontierList[n];
                    }
                }

                tree.Append(minNode.Name + " => ");

                //rebuild the frontier list
                for (int i = 0; i < minNode.Neighbors.Count; i++)
                {
                    if (tempNodes.Contains(minNode.Neighbors[i]))
                    {
                        if (frontierList.Keys.Contains(minNode.Neighbors[i]))
                        {
                            if (minNode.Weights[i] < frontierList[minNode.Neighbors[i]])
                            {
                                frontierList.Remove(minNode.Neighbors[i]);
                                frontierList.Add(minNode.Neighbors[i], minNode.Weights[i]);
                            }
                        }
                        else
                        {
                            frontierList.Add(minNode.Neighbors[i], minNode.Weights[i]);
                        }
                    }
                }

                //remove the node we just took from frontier list and node list
                tempNodes.Remove(minNode);
                frontierList.Remove(minNode);
                min = 100;
            }

            //return the tree
            temp = tree.ToString();
            temp = temp.Substring(0, temp.Length - 4);
            return temp;
        }

        #endregion Methods
    }
}
