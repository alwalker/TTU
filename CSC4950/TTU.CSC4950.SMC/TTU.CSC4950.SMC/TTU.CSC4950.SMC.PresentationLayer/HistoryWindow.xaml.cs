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
using TTU.CSC4950.SMC.FrameWork;
using TTU.CSC4950.SMC.LogicLayer;
using System.Data;

namespace TTU.CSC4950.SMC.PresentationLayer
{
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        #region Fields

        private List<string> _machineIDs;
        private List<Batch> _batches;
        private List<BatchType> _batchTypes;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryWindow"/> class.
        /// </summary>
        public HistoryWindow()
        {
            InitializeComponent();

            try
            {
                _machineIDs = MachineController.GetMachineIDs();
            }
            catch
            {
                MessageBox.Show("Error getting Machine IDs.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                _batches = BatchController.GetAllBatches();
            }
            catch
            {
                MessageBox.Show("Error getting Batches.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                _batchTypes = BatchTypeController.GetAllBatchTypes();
            }
            catch
            {
                MessageBox.Show("Error getting Batch Types.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            cboMachines.ItemsSource = _machineIDs;
            cboMachines.SelectedIndex = 0;

            cboBatchTypes.ItemsSource = _batchTypes;
            cboBatchTypes.SelectedIndex = 0;

            dgHistory.ItemsSource = _batches;
        }

        #endregion Constructor

        #region Events

        private void rdoMachines_Checked(object sender, RoutedEventArgs e)
        {
            DataView view = dgHistory.ItemsSource as DataView;

            if (view != null)
            {
                view.RowFilter = "MachineID = '" + cboMachines.SelectedItem.ToString() + "'";
                view.Sort = "BatchDate DESC";
            }
        }

        #endregion Events
    }
}
