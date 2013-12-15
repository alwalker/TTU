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
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TTU.CSC4950.SMC.PresentationLayer
{
    /// <summary>
    /// Interaction logic for NewBatchWindow.xaml
    /// </summary>
    public partial class NewBatchWindow : Window
    {
        #region Fields

        private BatchType _type;
        private List<BatchType> _batchTypes;
        private Machine _machine;

        private Task<Batch> _task;

        private Dispatcher _lblStatus1Dispatcher;
        private Dispatcher _lblStatus2Dispatcher;
        private Dispatcher _lblStatus3Dispatcher;
        private Dispatcher _lblStatus4Dispatcher;
        private Dispatcher _lblStatus5Dispatcher;
        private Dispatcher _lblStatus6Dispatcher;

        private delegate void _lblStatus1ChangeVisibleDelegate(Visibility vis);
        private delegate void _lblStatus2ChangeVisibleDelegate(Visibility vis);
        private delegate void _lblStatus3ChangeVisibleDelegate(Visibility vis);
        private delegate void _lblStatus4ChangeVisibleDelegate(Visibility vis);
        private delegate void _lblStatus5ChangeVisibleDelegate(Visibility vis);
        private delegate void _lblStatus6ChangeVisibleDelegate(Visibility vis);

        private BackgroundWorker _worker;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NewBatchWindow"/> class.
        /// </summary>
        public NewBatchWindow()
        {
            InitializeComponent();

            _machine = MachineController.InitializeMachine();

            _lblStatus1Dispatcher = lblStatus1.Dispatcher;
            _lblStatus2Dispatcher = lblStatus2.Dispatcher;
            _lblStatus3Dispatcher = lblStatus3.Dispatcher;
            _lblStatus4Dispatcher = lblStatus4.Dispatcher;
            _lblStatus5Dispatcher = lblStatus5.Dispatcher;
            _lblStatus6Dispatcher = lblStatus6.Dispatcher;

            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += worker_MakeBatch;
            _worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            //Load batch types into combo box
            try
            {
                _batchTypes = BatchTypeController.GetAllBatchTypes();
            }
            catch
            {
                MessageBox.Show("Error retrieving batch types from database.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_batchTypes.Count <= 0)
            {
                MessageBox.Show("No batch types found.", "Notice!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            cboBatchTypes.ItemsSource = _batchTypes;
            cboBatchTypes.SelectedIndex = 0;
        }

        #endregion Constructor

        #region BackGroundWorker Methods

        /// <summary>
        /// Handles the ProgressChanged event of the _worker control. Updates the UI of batch progress.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                lblStatus1.Visibility = System.Windows.Visibility.Visible;
            }
            else if (e.ProgressPercentage == 2)
            {
                lblStatus2.Visibility = System.Windows.Visibility.Visible;
            }
            else if (e.ProgressPercentage == 3)
            {
                lblStatus3.Visibility = System.Windows.Visibility.Visible;
            }
            else if (e.ProgressPercentage == 4)
            {
                lblStatus4.Visibility = System.Windows.Visibility.Visible;
            }
            else if (e.ProgressPercentage == 5)
            {
                lblStatus5.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the _worker control.  Adds finished batch to the database.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Batch newBatch = e.Result as Batch;

            lblStatus6.Visibility = System.Windows.Visibility.Visible;

            try
            {
                BatchController.AddBatch(newBatch);
            }
            catch
            {
                MessageBox.Show("Error adding batch to the database.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MessageBox.Show("Done With BackgroundWorker @: " + newBatch.BatchDate.ToString());
        }

        /// <summary>
        /// Handles the DoWork event of the _worker control.  Makes the batch.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void worker_MakeBatch(object sender, DoWorkEventArgs e)
        {
            PLCController plc = new PLCController("PLC.txt");

            Batch newBatch = BatchController.MakeBatch_EmptyTank(plc, _type);
            _worker.ReportProgress(1);
            BatchController.MakeBatchFillFirstTank(plc);
            _worker.ReportProgress(2);
            BatchController.MakeBatchFillSecondTank(plc);
            _worker.ReportProgress(3);
            BatchController.MakeBatchFillAdditives(plc);
            _worker.ReportProgress(4);
            BatchController.MakeBatchStart(plc);
            _worker.ReportProgress(5);
            BatchController.MakeBatchWaitForFinish(plc, newBatch);

            e.Result = newBatch;
        }

        #endregion BackGroundWorker Methods

        #region Events

        /// <summary>
        /// Handles the Click event of the btnStart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            _type = cboBatchTypes.SelectedItem as BatchType;

            //_worker.RunWorkerAsync();

            _task = Task<Batch>.Factory.StartNew(() => TaskStuff());
            Task.Factory.StartNew(() => BatchDone());
        }

        #endregion Events

        #region Delegate Methods

        /// <summary>
        /// Changes the visibility of lblStatus1.
        /// </summary>
        /// <param name="vis">The vis.</param>
        private void ChangelblStatus1Visibility(Visibility vis)
        {
            lblStatus1.Visibility = vis;
        }

        /// <summary>
        /// Changes the visibility of lblStatus2.
        /// </summary>
        /// <param name="vis">The vis.</param>
        private void ChangelblStatus2Visibility(Visibility vis)
        {
            lblStatus2.Visibility = vis;
        }

        /// <summary>
        /// Changes the visibility of lblStatus3.
        /// </summary>
        /// <param name="vis">The vis.</param>
        private void ChangelblStatus3Visibility(Visibility vis)
        {
            lblStatus3.Visibility = vis;
        }

        /// <summary>
        /// Changes the visibility of lblStatus4.
        /// </summary>
        /// <param name="vis">The vis.</param>
        private void ChangelblStatus4Visibility(Visibility vis)
        {
            lblStatus4.Visibility = vis;
        }

        /// <summary>
        /// Changes the visibility of lblStatus5.
        /// </summary>
        /// <param name="vis">The vis.</param>
        private void ChangelblStatus5Visibility(Visibility vis)
        {
            lblStatus5.Visibility = vis;
        }

        /// <summary>
        /// Changes the visibility of lblStatus6.
        /// </summary>
        /// <param name="vis">The vis.</param>
        private void ChangelblStatus6Visibility(Visibility vis)
        {
            lblStatus6.Visibility = vis;
        }

        #endregion Delegate Methods

        #region Task Methods

        /// <summary>
        /// Waits for the batch to finish then adds it to the database.
        /// </summary>
        private void BatchDone()
        {
            Batch test = _task.Result;

            if (test.Success)
            {
                _lblStatus6Dispatcher.Invoke(new _lblStatus6ChangeVisibleDelegate(ChangelblStatus6Visibility), Visibility.Visible);

                try
                {
                    BatchController.AddBatch(test);
                }
                catch
                {
                    MessageBox.Show("Error adding batch to the database.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                MessageBox.Show("Finished with Tasks @: " + test.BatchDate.ToString());
            }
        }

        /// <summary>
        /// Method given to the global Task object for making the batch.
        /// </summary>
        /// <returns></returns>
        private Batch TaskStuff()
        {
            PLCController plc = new PLCController("PLC.txt");

            Batch newBatch = BatchController.MakeBatch_EmptyTank(plc, _type);
            _lblStatus1Dispatcher.Invoke(new _lblStatus1ChangeVisibleDelegate(ChangelblStatus1Visibility), Visibility.Visible);
            BatchController.MakeBatchFillFirstTank(plc);
            _lblStatus2Dispatcher.Invoke(new _lblStatus2ChangeVisibleDelegate(ChangelblStatus2Visibility), Visibility.Visible);
            BatchController.MakeBatchFillSecondTank(plc);
            _lblStatus3Dispatcher.Invoke(new _lblStatus3ChangeVisibleDelegate(ChangelblStatus3Visibility), Visibility.Visible);
            BatchController.MakeBatchFillAdditives(plc);
            _lblStatus4Dispatcher.Invoke(new _lblStatus4ChangeVisibleDelegate(ChangelblStatus4Visibility), Visibility.Visible);
            BatchController.MakeBatchStart(plc);
            _lblStatus5Dispatcher.Invoke(new _lblStatus5ChangeVisibleDelegate(ChangelblStatus5Visibility), Visibility.Visible);
            BatchController.MakeBatchWaitForFinish(plc, newBatch);

            return newBatch;
        }

        #endregion Task Methods
    }
}
