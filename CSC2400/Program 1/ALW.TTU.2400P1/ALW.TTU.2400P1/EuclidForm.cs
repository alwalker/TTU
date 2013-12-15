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
    public partial class EuclidForm : Form
    {
        #region Fields

        private int _step;
        private bool _started;

        private List<string> _steps;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EuclidForm"/> class.
        /// </summary>
        public EuclidForm()
        {
            InitializeComponent();

            _started = false;
            _step = 0;
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the btnStep control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnStep_Click(object sender, EventArgs e)
        {
            //If this is the first time make sure the input is valid then run the algorithm
            if (!_started)
            {
                int n, m;
                bool validM = int.TryParse(txtM.Text, out m);
                bool validN = int.TryParse(txtN.Text, out n);

                if (!validM || !validN)
                {
                    MessageBox.Show("Invalid data entered, please make sure both numbers are valid integers.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //Check to make sure m is bigger, if not switch them
                if (m < n)
                {
                    int temp = n;
                    n = m;
                    m = temp;

                    txtM.Text = m.ToString();
                    txtN.Text = n.ToString();
                }

                EuclidAlgorithm(n, m);
                _started = true;
                txtM.Enabled = false;
                txtN.Enabled = false;
                lstSteps.Items.Clear();
                lblState.Text = "Current State: Algorithm completed, press next to display step.";
            }
            else if (_step < (_steps.Count - 1)) //Display the next step
            {
                lstSteps.Items.Add(_steps[_step]);
                lblState.Text = "Current State: " + _steps[_step].Split('-')[1].Split(':')[0];
                txtM.Text = _steps[_step].Split(' ')[6];
                txtN.Text = _steps[_step].Split(' ')[8];

                _step++;
            }
            else //Last step, display solution and reset gui and variables
            {
                lstSteps.Items.Add(_steps[_step]);
                lblState.Text = "Current State: Solution";
                txtN.Text = string.Empty;
                txtM.Text = string.Empty;
                txtM.Enabled = true;
                txtN.Enabled = true;

                _step = 0;
                _started = false;
            }
        }

        /// <summary>
        /// Handles the Load event of the EuclidForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EuclidForm_Load(object sender, EventArgs e)
        {
            txtM.Focus();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Implementation of Euclids Algorithm for GCD.
        /// Input: Two positive integers, m>n
        /// Output: the GCD and the intermediate steps are stored in the _steps global variable
        /// </summary>
        /// <param name="n">The n.</param>
        /// <param name="m">The m.</param>
        private void EuclidAlgorithm(int n, int m)
        {
            int r;
            _steps = new List<string>();

            //At each step determine the remainder of the larger number devided by the smaller number.
            //Swap the largest number with the smallest number and the smallest number with the remainder.
            //When we have a remainder of 0 we have m as our GCD.
            while (n != 0)
            {
                _steps.Add("State - Divide: Find GCD of " + m + " and " + n);
                r = m % n;
                m = n;
                n = r;
            }

            _steps.Add("State - Solution: GCD = " + m); 
        }

        #endregion
    }
}
