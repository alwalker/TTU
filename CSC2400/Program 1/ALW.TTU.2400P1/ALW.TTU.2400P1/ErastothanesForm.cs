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
    public partial class ErastothanesForm : Form
    {
        #region Fields

        private int _gcd;

        private enum Steps { NotStarted = 0, FindPrimes = 1, FindFactors = 2, FindCommonFactors = 3, Solution = 4, Clear = 5 };
        private Steps _currentStep;

        private List<int> _nPrimes;
        private List<int> _mPrimes;
        private Dictionary<int, int> _mFactors;
        private Dictionary<int, int> _nFactors;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ErastothanesForm"/> class.
        /// </summary>
        public ErastothanesForm()
        {
            InitializeComponent();

            _currentStep = Steps.NotStarted;
            _mFactors = new Dictionary<int, int>();
            _nFactors = new Dictionary<int, int>();
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
            //Switch on the current step in the algorithm. First step runs both the seive and the gcd algorithm.  The rest simply display the steps taken in those methods.
            switch (_currentStep)
            {
                case Steps.NotStarted:

                    int n, m;
                    bool validM = int.TryParse(txtM.Text, out m);
                    bool validN = int.TryParse(txtN.Text, out n);

                    if (!validM || !validN)
                    {
                        MessageBox.Show("Invalid data entered, please make sure both numbers are valid integers.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (m < n)
                    {
                        int temp = n;
                        n = m;
                        m = temp;

                        txtM.Text = m.ToString();
                        txtN.Text = n.ToString();
                    }

                    _nPrimes = Sieve(n);
                    _mPrimes = Sieve(m);

                    FindGCD(m, n, _mPrimes, _nPrimes);

                    btnStep.Text = "Step";
                    lblStep.Text = "Current Step: Algorithm completed succesfully, press step to walk through results.";
                    lsbM.Items.Clear();
                    lsbN.Items.Clear();
                    txtM.Enabled = false;
                    txtN.Enabled = false;
                    _currentStep = Steps.FindPrimes;
                    break;
                case Steps.FindPrimes:
                    lblStep.Text = "Current Step: Find all primes less than or equal to M and N.";
                    lsbM.Items.Add("Primes <= M");
                    foreach (int x in _mPrimes)
                    {
                        lsbM.Items.Add(x);
                    }
                    lsbN.Items.Add("Primes <= N");
                    foreach (int x in _nPrimes)
                    {
                        lsbN.Items.Add(x);
                    }
                    _currentStep = Steps.FindFactors;
                    break;
                case Steps.FindFactors:
                    lblStep.Text = "Current Step: Find Prime factorization of N and M.";
                    lsbM.Items.Clear();
                    lsbM.Items.Add("Prime Factors:");
                    foreach (int x in _mFactors.Keys)
                    {
                        if (_mFactors[x] > 0)
                        {
                            lsbM.Items.Add(x.ToString() + "^" + _mFactors[x].ToString());
                        }
                    }
                    lsbN.Items.Clear();
                    lsbN.Items.Add("Prime Factors:");
                    foreach (int x in _nFactors.Keys)
                    {
                        if (_nFactors[x] > 0)
                        {
                            lsbN.Items.Add(x.ToString() + "^" + _nFactors[x].ToString());
                        }
                    }
                    _currentStep = Steps.Solution;
                    break;
                case Steps.Solution:
                    lblStep.Text = "Current Step: Multiply lowest power of common factors to find solution of: " + _gcd;
                    btnStep.Text = "Clear";
                    _currentStep = Steps.Clear;
                    break;
                case Steps.Clear:
                    CleanUp();
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Implements the Sieve of Erastothanes.
        /// </summary>
        /// <param name="m">A positive integer m.</param>
        /// <returns>a List of all the primes less than or equal to M</in></returns>
        private List<int> Sieve(int m)
        {
            if (m <= 0)
            {
                return null;
            }

            List<int> primeFactors = new List<int>();

            //Make the initial list of all number less than equal to M.
            //This is done w/o even numbers(Except 2) since the first step is to eliminate them.
            for (int i = 2; i <= m; i++)
            {
                if ((i % 2 != 0) || (i == 2))
                {
                    primeFactors.Add(i);
                }
            }

            //For each integer in the list divide every other intger by it and remove any that divide evenly.
            for (int i = 0; i < primeFactors.Count; i++)
            {
                for (int j = 0; j < primeFactors.Count; j++)
                {
                    if ((primeFactors[j] % primeFactors[i] == 0) && (i != j)) //Make sure not to remove the number itself!
                    {
                        primeFactors.Remove(primeFactors[j]);
                    }
                }
            }

            return primeFactors;
        }

        /// <summary>
        /// Finds the GCD by multiplying the common prime factors.
        /// Results are stores in the _gcd, _mfactors, and _nfactors variables.
        /// </summary>
        /// <param name="m">m</param>
        /// <param name="n">n</param>
        /// <param name="mPrimes">The primes less than or equal to m.</param>
        /// <param name="nPrimes">The primes less than or equal to n.</param>
        private void FindGCD(int m, int n, List<int> mPrimes, List<int> nPrimes)
        {
            _gcd = 1;

            //Dictionaries are used to keep track of the powers of prime factors.
            //The keys are the primes themselves, the value associated with each key is 
            //power that it is raised to.

            //Initialize all powers to 0.
            foreach (int x in mPrimes)
            {
                _mFactors.Add(x, 0);
            }
            foreach (int x in nPrimes)
            {
                _nFactors.Add(x, 0);
            }

            //Loop over each number dividing it by its primes.
            //For each division that results in a non-zero remainder add a power to that prime.
            while (m > 1)
            {
                if (m % mPrimes[0] == 0)
                {
                    _mFactors[mPrimes[0]]++;
                    m = m / mPrimes[0];
                }
                else
                {
                    mPrimes.Remove(mPrimes[0]);
                }
            }
            while (n > 1)
            {
                if (n % nPrimes[0] == 0)
                {
                    _nFactors[nPrimes[0]]++;
                    n = n / nPrimes[0];
                }
                else
                {
                    nPrimes.Remove(nPrimes[0]);
                }
            }

            //Now that we have the prime factors we find the common ones and take the least power of the two.
            //Multiplying them together we have our GCD.
            foreach (int x in _mFactors.Keys)
            {
                foreach (int y in _nFactors.Keys)
                {
                    if ((x == y) && (_mFactors[x] > 0) && (_nFactors[y] > 0))
                    {
                        if (_nFactors[y] > _mFactors[x])
                        {
                            _gcd = _gcd * (int)Math.Pow(x, _mFactors[x]);
                        }
                        else
                        {
                            _gcd = _gcd * (int)Math.Pow(y, _nFactors[y]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cleans up.
        /// </summary>
        private void CleanUp()
        {
            //Reset global variables
            _gcd = 1;
            _mFactors = new Dictionary<int, int>();
            _nFactors = new Dictionary<int, int>();
            _mPrimes = null;
            _nPrimes = null;
            _currentStep = Steps.NotStarted;

            //Reset UI
            txtM.Text = string.Empty;
            txtN.Text = string.Empty;
            txtM.Enabled = true;
            txtN.Enabled = true;
            lblStep.Text = "Current Step: Waiting to start.";
            btnStep.Text = "Start";
            lsbM.Items.Clear();
            lsbN.Items.Clear();
        }

        #endregion
    }
}
