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
using TTU.CSC4950.SMC.FrameWork;
using TTU.CSC4950.SMC.LogicLayer;

namespace TTU.CSC4950.SMC.PresentationLayer
{    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        Machine _machine;

        SensorDisplayWindow _sensorDisplayWindow;
        AddBatchTypeWindow _addBatchWindow;
        HistoryWindow _historyWindow;
        EditBatchTypeWindow _editBatchTypeWindow;
        NewBatchWindow _newBatchWindow;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _machine = MachineController.InitializeMachine();
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Handles the Click event of the btnExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Handles the Click event of the btnViewsensors control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnViewsensors_Click(object sender, RoutedEventArgs e)
        {
            _sensorDisplayWindow = new SensorDisplayWindow();
            _sensorDisplayWindow.Show();
        }

        /// <summary>
        /// Handles the Click event of the btnAddEditBatchTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAddEditBatchTypes_Click(object sender, RoutedEventArgs e)
        {
            _editBatchTypeWindow = new EditBatchTypeWindow();
            _editBatchTypeWindow.Show();
        }

        /// <summary>
        /// Handles the Click event of the btnHistory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            _historyWindow = new HistoryWindow();
            _historyWindow.Show();
        }

        /// <summary>
        /// Handles the Click event of the btnNewBatch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnNewBatch_Click(object sender, RoutedEventArgs e)
        {
            _newBatchWindow = new NewBatchWindow();
            _newBatchWindow.Show();
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Height = CommonVariables.MainMenuHeight;
            this.Width = CommonVariables.MainMenuWidth;
            this.Top = 0;
            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.Width - 10;
        }

        #endregion Events        
    }
}
