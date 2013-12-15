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

namespace TTU.CSC4950.SMC.PresentationLayer
{
    /// <summary>
    /// Interaction logic for EditBatchTypeWindow.xaml
    /// </summary>
    public partial class EditBatchTypeWindow : Window
    {
        #region Fields

        private List<BatchType> _batchTypes;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EditBatchTypeWindow"/> class.
        /// </summary>
        public EditBatchTypeWindow()
        {
            InitializeComponent();

            try
            {
                _batchTypes = BatchTypeController.GetAllBatchTypes();
            }
            catch
            {
                MessageBox.Show("Error retrieving batch types from the database.  Please Try again.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            if (_batchTypes.Count > 0)
            {
                cboBatchTypes.ItemsSource = _batchTypes;
                cboBatchTypes.SelectedIndex = 0;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            double variableA, variableB, temp;
            double? tempReq = null;
            double? viscosityReq = null;
            double? timeReq = null;
            double? speedReq = null;
            bool success = true;

            if (!double.TryParse(txtVariableA.Text, out variableA))
            {
                success = false;
            }
            if (!double.TryParse(txtVariableB.Text, out variableB))
            {
                success = false;
            }

            if(txtTemperatureRequirement.Text != "" && txtViscosityRequirement.Text == "" && txtSpeedRequirement.Text == "" && txtTimeRequirement.Text == "")
            {
                if (!double.TryParse(txtTemperatureRequirement.Text, out temp))
                {
                    success = false;
                }
                else
                {
                    tempReq = temp;
                }
            }
            else if (txtTemperatureRequirement.Text == "" && txtViscosityRequirement.Text != "" && txtSpeedRequirement.Text == "" && txtTimeRequirement.Text == "")
            {
                if (!double.TryParse(txtViscosityRequirement.Text, out temp))
                {
                    success = false;
                }
                else
                {
                    viscosityReq = temp;
                }
            }
            else if (txtTemperatureRequirement.Text == "" && txtViscosityRequirement.Text == "" && txtSpeedRequirement.Text != "" && txtTimeRequirement.Text != "")
            {
                if (!double.TryParse(txtTimeRequirement.Text, out temp))
                {
                    success = false;
                }
                else
                {
                    timeReq = temp;
                }

                if (!double.TryParse(txtSpeedRequirement.Text, out temp))
                {
                    success = false;
                }
                else
                {
                    speedReq = temp;
                }
            }
            else
            {
                MessageBox.Show("Invalid requirements.  Please specify either Temperature, Viscosity, or Speed and Time.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (success)
            {
                try
                {
                    BatchTypeController.EditBatchType(cboBatchTypes.SelectedItem.ToString(), variableA, variableB, tempReq, viscosityReq, speedReq, timeReq);
                }
                catch
                {
                    MessageBox.Show("Error editing batch", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid data entered.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Batch edited succesfully!");
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            BatchType newType = new BatchType();
            AddBatchTypeWindow newWindow = new AddBatchTypeWindow(newType);

            if (newWindow.ShowDialog().Value)
            {
                try
                {
                    BatchTypeController.AddBatchType(newType);
                }
                catch
                {
                    MessageBox.Show("Error adding batch type.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _batchTypes.Add(newType);
                MessageBox.Show("Added batch type!");
            }
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Handles the SelectionChanged event of the cboBatchTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void cboBatchTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BatchType type = cboBatchTypes.SelectedItem as BatchType;

            if (type != null)
            {
                txtVariableA.Text = type.VariableA.ToString();
                txtVariableB.Text = type.VariableB.ToString();

                txtViscosityRequirement.Text = string.Empty;
                txtSpeedRequirement.Text = string.Empty;
                txtTemperatureRequirement.Text = string.Empty;
                txtTimeRequirement.Text = string.Empty;

                if (type.SpeedRequirement.HasValue)
                {
                    txtSpeedRequirement.Text = type.SpeedRequirement.Value.ToString();
                }
                if (type.TimeRequirement.HasValue)
                {
                    txtTimeRequirement.Text = type.TimeRequirement.Value.ToString();
                }
                if (type.TemperatureRequirement.HasValue)
                {
                    txtTemperatureRequirement.Text = type.TemperatureRequirement.Value.ToString();
                }
                if (type.ViscosityRequirement.HasValue)
                {
                    txtViscosityRequirement.Text = type.ViscosityRequirement.Value.ToString();
                }
            }
        }

        #endregion
    }
}
