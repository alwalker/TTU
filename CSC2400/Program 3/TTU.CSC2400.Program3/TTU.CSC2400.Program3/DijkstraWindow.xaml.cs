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

namespace TTU.CSC2400.Program3
{
    /// <summary>
    /// Interaction logic for DijkstraWindow.xaml
    /// </summary>
    public partial class DijkstraWindow : Window
    {
        #region Fields

        private List<Node> _nodes;

        #endregion

        #region Properties

        public Node StartNode;

        public Node GoalNode;

        #endregion

        #region Constructor

        public DijkstraWindow(List<Node> nodes)
        {
            InitializeComponent();

            _nodes = nodes;
            StartNode = null;
            GoalNode = null;
        }

        #endregion Constructor

        #region Events

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_nodes == null || _nodes.Count <= 0)
            {
                MessageBox.Show("No nodes in graph.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }

            cboGoalNode.ItemsSource = _nodes;
            cboGoalNode.SelectedIndex = 0;
            cboStartNode.ItemsSource = _nodes;
            cboStartNode.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (StartNode == null || GoalNode == null)
            {
                MessageBox.Show("Please select a start and goal node.", "Notice!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            DialogResult = true;
        }

        private void cboStartNode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StartNode = cboStartNode.SelectedItem as Node;
        }

        private void cboGoalNode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GoalNode = cboGoalNode.SelectedItem as Node;
        }

        #endregion Events
    }
}
