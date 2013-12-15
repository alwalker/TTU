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
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace ALW_TTU_CSC2400P2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private List<Vertex> _vertices;
        private List<Vertex> _oddVertices;
        private int[][] _oddDistanceMatrix;
        private List<int[][]> _distanceMatrices;
        private int step;

        #endregion Fields

        #region Constructor

        public MainWindow()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
            int step = 0;
            _vertices = new List<Vertex>();
            _oddVertices = new List<Vertex>();
            _distanceMatrices = new List<int[][]>();
        }

        #endregion Constructor

        #region Events

        private void btnReadGraph_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string fullFileName, fileName;

            //Get file name from user
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fileChooser.Title = "Select a graph file";

            if (fileChooser.ShowDialog() == true)
            {
                fullFileName = fileChooser.FileName;
                fileName = fullFileName.Substring(fullFileName.LastIndexOf('\\') + 1);
            }
            else
            {
                fullFileName = fileName = string.Empty;
                txtFileName.Text = string.Empty;
            }

            txtFileName.Text = fileName;
            BuildGraph(fullFileName);
            BuildDistanceMatrices();

            btnReadGraph.IsEnabled = false;
            btnClear.IsEnabled = true;
            btnFind.IsEnabled = true;
            btnIdentify.IsEnabled = true;
            btnPair.IsEnabled = true;
            btnStep.IsEnabled = true;
        }

        private void btnClear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _vertices = new List<Vertex>();
            _oddVertices = new List<Vertex>();
            _distanceMatrices = new List<int[][]>();
            step = 0;

            btnReadGraph.IsEnabled = true;
            btnClear.IsEnabled = false;
            btnFind.IsEnabled = false;
            btnIdentify.IsEnabled = false;
            btnPair.IsEnabled = false;
            btnStep.IsEnabled = false;

            txtFileName.Text = string.Empty;
            txbDistanceMatrices.Text = string.Empty;
            txbGraphDescription.Text = string.Empty;
            txbOddVertices.Text = string.Empty;
            txbPaired.Text = string.Empty;
        }

        private void btnIdentify_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PrintOddVertices();
        }

        private void btnPair_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PairOddVertices();
        }

        private void btnStep_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
        }

        private void btnFind_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FindEulerPath();
        }

        #endregion Events

        #region Methods

        private void BuildGraph(string fileName)
        {
            int vertex;
            string line;
            Vertex newVertex;
            Edge newEdge;

            if (!File.Exists(fileName))
            {
                MessageBox.Show("No such file found.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Build initial graph
            using (StreamReader sr = File.OpenText(fileName))
            {
                line = sr.ReadLine();

                while (line != null)
                {
                    vertex = Convert.ToInt32(line.Split(',')[0]);
                    newEdge = new Edge(Convert.ToInt32(line.Split(',')[1]), Convert.ToInt32(line.Split(',')[2]));

                    if (FindVertex(vertex))
                    {

                        AddEdge(vertex, newEdge);
                    }
                    else
                    {
                        newVertex = new Vertex(vertex, newEdge);
                        _vertices.Add(newVertex);
                    }

                    line = sr.ReadLine();
                }
            }

            //Display graph description
            PrintGraphDescription();

            //Build odd vertix list
            foreach (Vertex vert in _vertices)
            {
                if ((vert.Edges.Count % 2) != 0)
                {
                    _oddVertices.Add(vert);
                }
            }
        }

        private void BuildDistanceMatrices()
        {
            int[][] newDistanceMatrix, tempDistanceMatrix;

            //Initialize Distance Matrices
            for (int i = 0; i < _vertices.Count; i++)
            {
                newDistanceMatrix = new int[_vertices.Count][];

                for (int j = 0; j < _vertices.Count; j++)
                {
                    newDistanceMatrix[j] = new int[_vertices.Count];

                    for (int k = 0; k < _vertices.Count; k++)
                    {
                        newDistanceMatrix[j][k] = 1000;
                    }
                }

                _distanceMatrices.Add(newDistanceMatrix);
            }

            //Build D0
            for (int i = 0; i < _vertices.Count; i++)
            {
                for (int j = 0; j < _vertices.Count; j++)
                {
                    foreach (Edge edge in _vertices[i].Edges)
                    {
                        if (i == j)
                        {
                            _distanceMatrices[0][i][j] = 0;
                        }
                        else if (edge.Connection == _vertices[j].Number)
                        {
                            _distanceMatrices[0][i][j] = edge.Weight;
                        }
                    }
                }
            }

            //Build distance matrices
            for (int k = 1; k < _vertices.Count; k++)
            {
                for (int i = 0; i < _vertices.Count; i++)
                {
                    for (int j = 0; j < _vertices.Count; j++)
                    {
                        tempDistanceMatrix = _distanceMatrices[k - 1];
                        _distanceMatrices[k][i][j] =
                            Math.Min(tempDistanceMatrix[i][j], (tempDistanceMatrix[i][k] + tempDistanceMatrix[k][j]));
                    }
                }
            }

            //Build odd distance matrix
            _oddDistanceMatrix = new int[_oddVertices.Count][];
            for (int i = 0; i < _oddVertices.Count; i++)
            {
                _oddDistanceMatrix[i] = new int[_oddVertices.Count];
            }

            for (int i = 0; i < _oddVertices.Count; i++)
            {
                for (int j = 0; j < _oddVertices.Count; j++)
                {
                    _oddDistanceMatrix[i][j] =
                        _distanceMatrices[_vertices.Count - 1][_oddVertices[i].Number][_oddVertices[j].Number];
                }
            }

            PrintDistanceMatrices();
        }

        //Pair odd vertices using the greedy paradigm.
        //Starting with the first vertex pair it with the closest one.
        //Repeat
        private void PairOddVertices()
        {
            Edge newEdge;
            int min = 1000;
            int other = -1;
            List<int> pairs = new List<int>();
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < _oddVertices.Count; i++)
            {
                if (!pairs.Contains(i))
                {
                    for (int j = 0; j < _oddVertices.Count; j++)
                    {
                        if (i != j)
                        {
                            if (_oddDistanceMatrix[i][j] < min)
                            {
                                min = _oddDistanceMatrix[i][j];
                                other = j;
                            }
                        }
                    }

                    pairs.Add(i);
                    pairs.Add(other);
                    other = -1;
                    min = 1000;
                }
            }

            //Display paired vertices and add the corresponding edges
            for (int i = 0; i < pairs.Count; i = i + 2)
            {
                //First edge
                newEdge = new Edge(_vertices[_oddVertices[pairs[i+1]].Number-1].Number, 1);
                _vertices[_oddVertices[pairs[i]].Number-1].Edges.Add(newEdge);

                //Corresponding edge
                newEdge = new Edge(_vertices[_oddVertices[pairs[i]].Number - 1].Number, 1);
                _vertices[_oddVertices[pairs[i+1]].Number - 1].Edges.Add(newEdge);

                str.Append(_oddVertices[pairs[i]].Number.ToString() + ", " + _oddVertices[pairs[i + 1]].Number.ToString() + " ");
            }

            txbPaired.Text = str.ToString();
        }

        private void FindEulerPath()
        {
            StringBuilder str = new StringBuilder();
            str.Append("1->");

            RemoveEdge(_vertices[0], _vertices[0].Edges[0].Connection);
            while (_vertices[0].Edges.Count > 0)
            {
                takeEdge(_vertices[0], str);
                str.Append("\n");
            }

            MessageBox.Show(str.ToString());
        }

        private void takeEdge(Vertex vertex, StringBuilder str)
        {
            int temp;
            if(vertex.Edges.Count > 0)
            {
                str.Append((vertex.Edges[0].Connection).ToString() + "->");
                temp = vertex.Edges[0].Connection;
                RemoveEdge(vertex, vertex.Edges[0].Connection);
                takeEdge(_vertices[temp-1], str);                
            }
            else
                return;
        }

        private void RemoveEdge(Vertex vertex, int edge)
        {
            foreach (Edge ed in vertex.Edges)
            {
                if (ed.Connection == edge)
                {
                    vertex.Edges.Remove(ed);
                    break;
                }
            }

            //remove edge from other vertex
            foreach (Vertex vert in _vertices)
            {
                if (vert.Number == edge)
                {
                    foreach (Edge ed in vert.Edges)
                    {
                        if (ed.Connection == vertex.Number)
                        {
                            vert.Edges.Remove(ed);
                            break;
                        }
                    }
                }
            }
                   
        }

        private bool FindVertex(int vertex)
        {
            foreach (Vertex vert in _vertices)
            {
                if (vert.Number == vertex)
                {
                    return true;
                }
            }

            return false;
        }

        private void AddEdge(int vertex, Edge edge)
        {
            foreach (Vertex vert in _vertices)
            {
                if (vert.Number == vertex)
                {
                    vert.Edges.Add(edge);
                }
            }
        }

        private void PrintGraphDescription()
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < _vertices.Count; i++)
            {
                str.Append(_vertices[i].ToString() + "\n");
            }

            txbGraphDescription.Text = str.ToString();
        }

        private void PrintDistanceMatrices()
        {
            StringBuilder str = new StringBuilder();

            for (int k = 0; k < _distanceMatrices.Count; k++)
            {

                for (int i = 0; i < _vertices.Count; i++)
                {
                    str.Append("\t" + i.ToString());
                }

                str.Append("\n");


                for (int i = 0; i < _vertices.Count; i++)
                {
                    str.Append(i.ToString() + "\t");

                    for (int j = 0; j < _vertices.Count; j++)
                    {
                        str.Append(_distanceMatrices[k][i][j].ToString() + "\t");
                    }

                    str.Append("\n");
                }

                str.Append("\n\n");
            }

            txbDistanceMatrices.Text = str.ToString();
        }

        private void PrintOddVertices()
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < _oddVertices.Count; i++)
            {
                str.Append(_oddVertices[i].ToString() + "\n");
            }

            txbOddVertices.Text = str.ToString();
        }

        private void PrintOddDistanceMatrix()
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < _oddVertices.Count; i++)
            {
                str.Append("\t" + i.ToString());
            }

            str.Append("\n");


            for (int i = 0; i < _oddVertices.Count; i++)
            {
                str.Append(i.ToString() + "\t");

                for (int j = 0; j < _oddVertices.Count; j++)
                {
                    str.Append(_oddDistanceMatrix[i][j].ToString() + "\t");
                }

                str.Append("\n");
            }

            txbPaired.Text = str.ToString();
        }


        #endregion Methods
    }
}