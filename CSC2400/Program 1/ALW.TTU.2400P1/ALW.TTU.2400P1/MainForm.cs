using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ALW.TTU._2400P1
{
    public partial class MainForm : Form
    {
        #region Fields

        private EuclidForm _euclidForm;
        private ErastothanesForm _erastothanesForm;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the btnEuclid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnEuclid_Click(object sender, EventArgs e)
        {
            _euclidForm = new EuclidForm();
            _euclidForm.ShowDialog();
            _euclidForm = null;
        }

        /// <summary>
        /// Handles the Click event of the btnErastothanes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnErastothanes_Click(object sender, EventArgs e)
        {
            _erastothanesForm = new ErastothanesForm();
            _erastothanesForm.ShowDialog();
            _erastothanesForm = null;
        }

        #endregion
    }
}
