using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Globalization;

namespace Arinc708
{
    class IsoArgumentParser : ArgumentParser
    {
        const int HEX_LSTRING_LENGTH = 36;

        const string HONEYWELL_FORMAT_MSG = "Honeywell 453 Display Data Word Format";
        const string COLLINS_FORMAT_MSG = "Collins Data Bus Word Definition";
        const string CUSTOM_FORMAT_MSG = "Custom Data Bus Word Format";
        const UInt64 DEFAULT_LONG_NUMBER = UInt64.MaxValue;
        const string HONEYWELL_HEX_LSTRING = "AAAAAAAAAAAA8002000200FFFFFF00000000";
        const string COLLINS_HEX_LSTRING = "555555555555555555555555555555555555";
        const string CUSTOM_HEX_LSTRING = "000000000000000000000000000000000000";
        const string DEFAULT_RADAR_TYPE = "H";
        const string DEFAULT_HEX_LSTRING = "AAAAAAAAAAAA8002000200FFFFFF00000000";

        #region " Constructor ..."

        // Give the set of valid command-line switches to the base class
        public IsoArgumentParser()
            : base(new string[] { "?", "r", "s" })
        {
            _appUsage =
                "Usage: BinsParser [-?] [-r[radar]] [-s[hex_str]]" + Environment.NewLine +
                string.Format("\t-?\t\tShow this usage information") + Environment.NewLine +
                string.Format("\t-r[radar]\tRadar type (C/H/O: Collins/Honeywell/Other (default H))", DEFAULT_LONG_NUMBER) + Environment.NewLine +
                string.Format("\t-s[hex_str]\tUse hex string (default '{0}')", DEFAULT_HEX_LSTRING) + Environment.NewLine;

            _commandLineError = string.Empty;
            _invalidSwitchMsg = string.Empty;

            _radarType = DEFAULT_RADAR_TYPE;
            _radarTypeMsg = HONEYWELL_FORMAT_MSG;

            _processType = -1;
            _processTypeMsg = "<Default processing>";

            _longHexStr = DEFAULT_HEX_LSTRING;
            _hexStrLenght = HEX_LSTRING_LENGTH;
        }

        #endregion

        #region " Properties ..."

        private string _appUsage;

        public string AppUsage
        {
            get { return _appUsage; }
            set { _appUsage = value; }
        }
        private string _commandLineError;

        public string CommandLineError
        {
            get { return _commandLineError; }
            set { _commandLineError = value; }
        }
        private string _invalidSwitchMsg;

        public string InvalidSwitchMsg
        {
            get { return _invalidSwitchMsg; }
            set { _invalidSwitchMsg = value; }
        }

        private int _processType;

        public int ProcessType
        {
            get { return _processType; }
            set { _processType = value; }
        }
        private string _processTypeMsg;

        public string ProcessTypeMsg
        {
            get { return _processTypeMsg; }
            set { _processTypeMsg = value; }
        }

        private string _longHexStr;
        public string LongHexStr
        {
            get { return _longHexStr; }
            set { _longHexStr = value; }
        }

        private string _radarType;

        public string RadarType
        {
            get { return _radarType; }
            set { _radarType = value; }
        }

        private string _radarTypeMsg;

        public string RadarTypeMsg
        {
            get { return _radarTypeMsg; }
            set { _radarTypeMsg = value; }
        }

        private int _hexStrLenght;

        public int HexStrLenght
        {
            get { return _hexStrLenght; }
            set { _hexStrLenght = value; }
        }

        #endregion

        #region " Override methods ..."

        // Shows application's usage info and also reports command-line argument errors.
        public override void OnUsage(String errorInfo)
        {
            if (errorInfo != null)
            {
                // An command-line argument error occurred, report it to user
                // errInfo identifies the argument that is in error.
                _commandLineError = string.Format("{0}", errorInfo);
            }
        }

        // Called for each non-switch command-line argument (filespecs)
        protected override SwitchStatus OnNonSwitch(String switchValue)
        {
            SwitchStatus ss = SwitchStatus.NoError;
            return (ss);
        }

        // Called for each switch command-line argument
        protected override SwitchStatus OnSwitch(String switchSymbol, String switchValue)
        {
            // NOTE: For case-insensitive switches, 
            //       switchSymbol will contain all lower-case characters
            SwitchStatus ss = SwitchStatus.NoError;

            switch (switchSymbol)
            {
                case "?":   // User wants to see Usage
                    _processType = -1;
                    ss = SwitchStatus.ShowUsage;
                    break;

                case "r":
                    if (switchValue.Length != 1)
                    {
                        _radarTypeMsg = string.Format("Warning: Radar type wrong or not specified (default = {0})", DEFAULT_RADAR_TYPE);
                        switchValue = DEFAULT_RADAR_TYPE;
                        ss = SwitchStatus.Warning;
                    }
                    switch (switchValue)
                    {
                        case "C":
                            _radarTypeMsg = COLLINS_FORMAT_MSG;
                            break;
                        case "H":
                            _radarTypeMsg = HONEYWELL_FORMAT_MSG;
                            break;
                        case "O":
                            _radarTypeMsg = CUSTOM_FORMAT_MSG;
                            break;
                        default:
                            _radarTypeMsg = string.Format("Warning: Radar type wrong (default = {0})", DEFAULT_RADAR_TYPE);
                            switchValue = DEFAULT_RADAR_TYPE;
                            ss = SwitchStatus.Warning;
                            break;
                    }
                    _radarType = switchValue;
                    break;

                case "s":   // User wants to process an hexadecimal string
                    _processType = 2;
                    int xl = switchValue.Length;
                    if (xl < 1)
                    {
                        switch (_radarType)
                        {
                            case "C":
                                switchValue = COLLINS_HEX_LSTRING;
                                break;
                            case "H":
                                switchValue = HONEYWELL_HEX_LSTRING;
                                break;
                            default:
                                switchValue = CUSTOM_FORMAT_MSG;
                                break;
                        }
                        _processTypeMsg = string.Format("Warning: Hex string not specified (default = '{0}')", switchValue);
                        ss = SwitchStatus.Warning;
                    }
                    _longHexStr = switchValue.PadRight(HEX_LSTRING_LENGTH, '0').Substring(0, HEX_LSTRING_LENGTH);
                    break;

                default:
                    _processType = -1;
                    _invalidSwitchMsg = "\"" + switchSymbol + "\"." + Environment.NewLine;
                    ss = SwitchStatus.Error;
                    break;
            }
            return (ss);
        }

        // Called when all command-line arguments have been parsed
        protected override SwitchStatus OnDoneParse()
        {
            SwitchStatus ss = SwitchStatus.NoError;

            return (ss);
        }

        #endregion
    }
}
