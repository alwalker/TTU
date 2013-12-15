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

namespace TTU.CSC4950.SMC.PresentationLayer
{
    /// <summary>
    /// Interaction logic for AddBatchTypeWindow.xaml
    /// </summary>
    public partial class AddBatchTypeWindow : Window
    {
        #region Fields

        private BatchType _newType;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddBatchTypeWindow"/> class.
        /// </summary>
        public AddBatchTypeWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="AddBatchTypeWindow"/> class.
        /// </summary>
        /// <param name="newType">The new type.</param>
        public AddBatchTypeWindow(BatchType newType)
        {
            InitializeComponent();

            _newType = newType;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Handles the Click event of the btnConfirm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            double variableA, variableB, temp;
            double? tempReq = null;
            double? viscosityReq = null;
            double? timeReq = null;
            double? speedReq = null;
            bool success = true;

            if (txtName.Text == "")
            {
                success = false;
            }
            if (!double.TryParse(txtVariableA.Text, out variableA))
            {
                success = false;
            }
            if (!double.TryParse(txtVariableB.Text, out variableB))
            {
                success = false;
            }

            if (txtTemperatureRequirement.Text != "" && txtViscosityRequirement.Text == "" && txtSpeedRequirement.Text == "" && txtTimeRequirement.Text == "")
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
                _newType.Name = txtName.Text;
                _newType.VariableA = variableA;
                _newType.VariableB = variableB;
                _newType.SpeedRequirement = speedReq;
                _newType.TemperatureRequirement = tempReq;
                _newType.TimeRequirement = timeReq;
                _newType.ViscosityRequirement = viscosityReq;
            }
            else
            {
                MessageBox.Show("Invalid data entered.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion Events
    }
}
