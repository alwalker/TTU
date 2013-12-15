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

namespace TTU.CSC2400.Program3
{
    /// <summary>
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        #region Properties

        public string FileName;

        #endregion Properties

        #region Constructor

        public LoadWindow()
        {
            InitializeComponent();
        }

        #endregion Constructor
                
        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtFileName.Focus();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(txtFileName.Text))
            {
                MessageBox.Show("File not found.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                FileName = txtFileName.Text;
                DialogResult = true;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion Events        
    }
}
