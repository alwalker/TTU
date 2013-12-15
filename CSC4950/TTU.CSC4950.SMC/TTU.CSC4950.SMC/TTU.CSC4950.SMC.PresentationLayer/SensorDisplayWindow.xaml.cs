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
using System.Windows.Threading;
using TTU.CSC4950.SMC.FrameWork;
using TTU.CSC4950.SMC.LogicLayer;

namespace TTU.CSC4950.SMC.PresentationLayer
{
    /// <summary>
    /// Interaction logic for SensorDisplayWindow.xaml
    /// </summary>
    public partial class SensorDisplayWindow : Window
    {
        #region Fields

        private DispatcherTimer _getReadings;
        private PLCController _plc;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorDisplayWindow"/> class.
        /// </summary>
        public SensorDisplayWindow()
        {
            InitializeComponent();

            _getReadings = new DispatcherTimer();
            _getReadings.Interval = TimeSpan.FromSeconds(.5);
            _getReadings.Tick += GetReadings;
            _plc = new PLCController("PLC.txt");
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = CommonVariables.MainMenuWidth;
            this.Top = CommonVariables.MainMenuHeight + 10;
            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.Width - 10;

            _getReadings.Start();
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Gets the temperature and speed readings from the plc and displays them.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void GetReadings(object sender, EventArgs e)
        {
            SensorData readings;

            try
            {
                readings = _plc.GetSensorData();
            }
            catch
            {
                MessageBox.Show("Error readings sensors.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            lblSpeed.Content = "Motor Speed: " + readings.Speed.ToString();
            lblTemperature.Content = "Temperature: " + readings.Temperature.ToString();
        }

        #endregion Methods
    }
}
