using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace PLCSimulator
{
    public class PLC
    {
        #region Fields

        private bool _isConnected;
        private Dictionary<string, bool> _bits;
        private Dictionary<string, double> _words;
        private List<Rule> _rules;
        private List<Timer> _timers;

        private Random _rnd;

        #endregion Fields

        #region Properties

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        #endregion

        #region Constructor

        public PLC(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new ApplicationException("Invalid File!");
            }

            string[] pieces;
            string line;
            _isConnected = false;

            _bits = new Dictionary<string, bool>();
            _words = new Dictionary<string, double>();
            _rules = new List<Rule>();
            _timers = new List<Timer>();
            _rnd = new Random(DateTime.Now.Millisecond);

            using (StreamReader sr = File.OpenText(filename))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    pieces = line.Split(' ');

                    if (pieces[0].CompareTo("bit") == 0)
                    {
                        try
                        {
                            _bits.Add(pieces[1], Convert.ToBoolean(pieces[2]));
                        }
                        catch
                        {
                            throw new ApplicationException("Tried to add same address twice! in bit" + pieces[1]);
                        }
                    }
                    else if (pieces[0].CompareTo("word") == 0)
                    {
                        try
                        {
                            _words.Add(pieces[1], Convert.ToDouble(pieces[2]));
                        }
                        catch
                        {
                            throw new ApplicationException("Tried to add same address twice! in word");
                        }
                    }
                    else if (pieces[0].CompareTo("rule") == 0)
                    {
                        if (!AddRule(pieces[1], Convert.ToBoolean(pieces[2]), pieces[3], Convert.ToBoolean(pieces[4])))
                        {
                            throw new ApplicationException("Invalid Rule!");
                        }
                    }
                    else if (pieces[0].CompareTo("timer") == 0)
                    {

                    }
                }

                sr.Close();
            }
        }

        #endregion Constructor

        #region Private Methods

        private bool AddRule(string addr1, bool val1, string addr2, bool val2)
        {
            if (_bits.Keys.Contains(addr1) && _bits.Keys.Contains(addr2))
            {
                _rules.Add(new Rule(addr1, val1, addr2, val2));
                return true;
            }
            else
            {
                return false;
            }
        }

        private void EvaluateRules()
        {
            foreach (Rule rl in _rules)
            {
                if (_bits[rl.Address1] == rl.Value1)
                {
                    _bits[rl.Address2] = rl.Value2;
                }
            }
        }

        #endregion Private Methods

        #region Public Methods

        public bool Connect()
        {
            //Check for fail (5% chance)
            int fail = _rnd.Next(99);
            if (fail < 4)
            {
                _isConnected = false;
                return false;
            }
            else
            {
                _isConnected = true;
                return true;
            }
        }

        public bool Disconnect()
        {
            //Check for fail (5% chance)
            int fail = _rnd.Next(99);
            if (fail < 4)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ReadBit(string address, out bool bit)
        {
            if (!_isConnected)
            {
                bit = false;
                throw new ApplicationException("PLC Not Connected!");
            }

            //Check for fail (5% chance)
            int fail = _rnd.Next(99);
            if (fail < 4)
            {
                bit = false;
                return false;
            }

            if (_bits.Keys.Contains(address))
            {
                bit = _bits[address];
                return true;
            }
            else
            {
                bit = false;
                throw new ApplicationException("Invalid address!");
            }
        }

        public bool WriteBit(string address, bool bit)
        {
            if (!_isConnected)
            {
                throw new ApplicationException("PLC Not Connected!");
            }

            //Check for fail (5% chance)
            int fail = _rnd.Next(99);
            if (fail < 4)
            {
                return false;
            }

            if (_bits.Keys.Contains(address))
            {
                _bits[address] = bit;
                EvaluateRules();
                return true;
            }
            else
            {
                throw new ApplicationException("Invalid address!");
            }
        }

        public bool ReadWord(string address, out double word)
        {
            if (!_isConnected)
            {
                word = double.NaN;
                throw new ApplicationException("PLC Not Connected!");
            }

            //Check for fail (5% chance)
            int fail = _rnd.Next(99);
            if (fail < 4)
            {
                word = double.NaN;
                return false;
            }

            if (_words.Keys.Contains(address))
            {
                word = _words[address];
                return true;
            }
            else
            {
                word = double.NaN;
                throw new ApplicationException("Invalid address!");
            }
        }

        public bool WriteWord(string address, double word)
        {
            if (!_isConnected)
            {
                throw new ApplicationException("PLC Not Connected!");
            }

            //Check for fail (5% chance)
            int fail = _rnd.Next(99);
            if (fail < 4)
            {
                return false;
            }

            if (_words.Keys.Contains(address))
            {
                _words[address] = word;
                EvaluateRules();
                return true;
            }
            else
            {
                throw new ApplicationException("Invalid address!");
            }
        }

        #endregion Public Methods
    }
}
