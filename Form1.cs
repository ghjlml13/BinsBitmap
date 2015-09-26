using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Arinc708;
using System.Globalization;

namespace BinsBitmap
{
    public partial class Form1 : Form
    {

        #region " Properties "

        private bool firstTime = false;
        private bool mask = false;

        #endregion

        #region " Variables "

        private IsoArgumentParser ap = new IsoArgumentParser();

        private string[,] collinsBin = {
            {"555555555555555555555555555555555555", "AAAAAAA~"},
            {"FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "FFFFFFF~"},
            {"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", "5555555~"},
            {"000000000000000000000000000000000000", "~0000000"},
            {"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", "5551555~"},
            {"000000000000000000000000000000000000", "~4000000"}
        };

        private string[,] honeywellBin = {
            {"AAAAAAAAAAAA8002000200FFFFFF00000000", "'VECTOR-TAB-A'"},
            {"FFFFFFFFFFFF8007000200FFFFFF00000000", "'VECTOR-TAB-F'"},
            {"5555555555550005000200FFFFFF00000000", "'VECTOR-TAB-5'"},
            {"0000000000000000000200FFFFFF00000000", "'VECTOR-TAB-0'"},
            {"8AAAAAAAAAAA8002000200FFFFFF00000000", "'VECTOR-TAB-51'"},
            {"0000000004000000000200FFFFFF00000000", "'VECTOR-TAB-02'"}
        };

        #endregion

        public Form1(string[] args)
        {
            InitializeComponent();

            ParseAppArguments(args);

            if (ap.RadarType == "H")
            {
                cboRadar.SelectedIndex = 1;
            }
            else if (ap.RadarType == "C")
            {
                cboRadar.SelectedIndex = 0;
            }
            else
            {
                cboRadar.SelectedIndex = 2;
            }
        }

        private void ParseAppArguments(string[] args)
        {
            // Parse the command-line arguments
            if (!ap.Parse(args))
            {
                switch (ap.ProcessType)
                {
                    case -1:
                        MessageBox.Show(ap.AppUsage, "BinsBitmap parser", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    default:
                        MessageBox.Show(ap.ProcessTypeMsg, "BinsBitmap parser", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
            firstTime = true;
            switch (ap.RadarType)
            {
                case "H":
                    break;
                case "C":
                    break;
                default:
                    break;
            }
        }

        private void Initialize()
        {
            this.Text = ap.RadarTypeMsg;
            maskedBitmap.Text = ap.LongHexStr;
            cboBinSamples.Enabled = true;
            cboBinSamples.Items.Clear();
            cboBinSamples.Items.Add("<select pattern>");
            if (ap.RadarType == "C")
            {
                for (int i = 0; i < collinsBin.Length/2; i++)
                {
                    cboBinSamples.Items.Add(collinsBin[i, 0].Substring(0, 4) + " " + collinsBin[i, 1]);
                }
            }
            else if (ap.RadarType == "H")
            {
                for (int i = 0; i < honeywellBin.Length/2; i++)
    			{
                    cboBinSamples.Items.Add(honeywellBin[i, 0].Substring(0, 4) + " " + honeywellBin[i, 1]);
	    		}
            }
            else
            {
                cboBinSamples.Enabled = false;
            }
            cboBinSamples.SelectedIndex = 0;
            label1.Text = " 1   2   3   4   5   6   7   8   9  10  11  12  13  14  15  16";
            label2.Text = "17  18  19  20  21  22  23  24  25  26  27  28  29  30  31  32";
            label3.Text = "33  34  35  36  37  38  39  40  41  42  43  44  45  46  47  48";
        }

        private void UpdateBinData()
        {
            foreach (Control ctrl in grpBins.Controls)
            {
                try
                {
                    ((TextBox)ctrl).Text = "XXX";
                }
                catch { }
            }
            label5.Text = string.Empty;
            label6.Text = string.Empty;
            lblStatusMsg.Text = string.Empty;
            if (ap.LongHexStr.Length == ap.HexStrLenght)
            {
                try
                {
                    UInt64 bmULong1 = UInt64.Parse(ap.LongHexStr.Substring(0, 16), NumberStyles.HexNumber);
                    UInt64 bmULong2 = UInt64.Parse(ap.LongHexStr.Substring(16, 16), NumberStyles.HexNumber);
                    UInt16 bmUInt16 = UInt16.Parse(ap.LongHexStr.Substring(32), NumberStyles.HexNumber);

                    string bmULong1Bs = UsTools.GetBinaryRepresentation(bmULong1, 64, true);
                    string bmULong2Bs = UsTools.GetBinaryRepresentation(bmULong2, 64, true);
                    string bmUInt16Bs = UsTools.GetBinaryRepresentation((UInt64)bmUInt16, 16, true);

                    lblBinaryRep.Text = bmULong1Bs + "\n" + bmULong2Bs + "\n" + bmUInt16Bs;

                    int j = 0;
                    string bin = string.Empty;
                    string tbn = string.Empty;
                    for (int i = 0; i < 22; i++)
                    {
                        j = i * 3;
                        if (i == 21)
                        {
                            bin = bmULong1Bs.Substring(j, 1);
                        }
                        else
                        {
                            bin = bmULong1Bs.Substring(j, 3);
                        }
                        tbn = string.Format("textBox{0}", i + 1);

                        TextBox tb = new TextBox();
                        tb = (TextBox)grpBins.Controls[tbn];
                        if (tbn.Equals("textBox22"))
                        {
                            bin += tb.Text.Substring(1);
                        }
                        tb.Text = bin;
                    }
                    for (int i = 0; i < 22; i++)
                    {
                        if (i == 0)
                        {
                            bin = bmULong2Bs.Substring(i, 2);
                        }
                        else if (i == 21)
                        {
                            j = i * 3 - 1;
                            bin = bmULong2Bs.Substring(j, 2);
                        }
                        else
                        {
                            j = i * 3 - 1;
                            bin = bmULong2Bs.Substring(j, 3);
                        }
                        tbn = string.Format("textBox{0}", i + 22);

                        TextBox tb = new TextBox();
                        tb = (TextBox)grpBins.Controls[tbn];
                        if (tbn.Equals("textBox22"))
                        {
                            bin = tb.Text.Substring(0, 1) + bin;
                        }
                        else if (tbn.Equals("textBox43"))
                        {
                            bin = tb.Text.Substring(0, 1) + bin;
                        }
                        tb.Text = bin;
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        j = i * 3;
                        if (i == 5)
                        {
                            bin = bmUInt16Bs.Substring(j, 1);
                        }
                        else
                        {
                            bin = bmUInt16Bs.Substring(j, 3);
                        }
                        tbn = string.Format("textBox{0}", i + 43);

                        TextBox tb = new TextBox();
                        tb = (TextBox)grpBins.Controls[tbn];
                        if (tbn.Equals("textBox48"))
                        {
                            bin += tb.Text.Substring(1);
                        }
                        tb.Text = bin;
                    }
                }
                catch (Exception ex)
                {
                    lblStatusMsg.Text = ex.Message;
                }
            }
        }

        private void cboRadar_SelectedIndexChanged(object sender, EventArgs e)
        {
            mask = false;
            if (firstTime)
            {
                firstTime = false;
            }
            else
            {
                string[] args = new string[2];
                int radar = cboRadar.SelectedIndex;

                switch (radar)
                {
                    case 0:     // Collins
                        args[0] = "-rC";
                        args[1] = "-s555555555555555555555555555555555555";     // pattern AAAAAAAAAAAAAAAA
                        break;
                    case 1:     // Honeywell
                        args[0] = "-rH";
                        args[1] = "-sAAAAAAAAAAAA8002000200FFFFFF00000000";     // 'S-VECTOR-TAB-A'
                        break;
                    default:    // Other
                        args[0] = "-rO";
                        args[1] = "-s000000000000000000000000000000000000";
                        break;
                }
                ap.Parse(args);
            }
            Initialize();
            UpdateBinData();
            btnDo.Enabled = false;
            mask = true;
        }

        private void cboBinSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            int bs = cboBinSamples.SelectedIndex;
            if (bs > 0)
            {
                mask = true;
                if (ap.RadarType == "C")
                {
                    maskedBitmap.Text = collinsBin[bs - 1, 0];
                }
                else
                {
                    maskedBitmap.Text = honeywellBin[bs - 1, 0];
                }
            }
        }

        private void maskedBitmap_TextChanged(object sender, EventArgs e)
        {
            if (mask)
            {
                ap.LongHexStr = maskedBitmap.Text;
                btnDo.Enabled = (ap.LongHexStr.Length != ap.HexStrLenght);
                UpdateBinData();
            }
            mask = true;
        }

        private void GetReflectivity(object sender, EventArgs e)
        {
            string bin = ((TextBox)sender).Text;
            string title = string.Empty;

            switch (ap.RadarType)
            {
                case "C":
                    title = "REFLECTIVITY DATA: LEVEL      RAINFALL WX MODES  MAP MAP CTR TEST TEST";

                    label6.Text = CollinsBinData(bin);
                    break;
                case "H":
                    title = "FUNCTION/LEVEL: " + HoneywellBinData(bin);
                    break;
                default:
                    break;
            }
            label5.Text = title;
        }

        private static string HoneywellBinData(string bin)
        {
            string data = string.Empty;

            switch (bin)
            {
                case "000":
                    data += "LESS THAN Z2";
                    break;
                case "100":
                    data += "Z2 TO Z3";
                    break;
                case "010":
                    data += "Z3 TO Z4";
                    break;
                case "110":
                    data += "Z4 TO Z5";
                    break;
                case "001":
                    data += "GREATER THAN Z5";
                    break;
                default:
                    data += "UNKNOWN";
                    break;
            }
            return data;
        }

        private static string CollinsBinData(string bin)
        {
            string data = string.Empty;

            switch (bin)
            {
                case "000":
                    data += "Z1=10 dbZ  .006-.03    BLK    BLK BLK BLK  BLK  BLK";
                    break;
                case "100":
                    data += "Z2=20 dbZ   .03-.15    GRN    GRN GRN BLK  GRN  GRN";
                    break;
                case "010":
                    data += "Z3=30 dbZ   .15-.5     YEL    GRN YEL BLK  GRN  YEL";
                    break;
                case "110":
                    data += "Z4=40 dbZ    .5-2      RED    YEL RED BLK  YEL  RED";
                    break;
                case "001":
                    data += "Z5=50 dbZ     2-8      RED    YEL RED BLK  YEL  RED";
                    break;
                case "101":
                    data += "                       BLK    BLK BLK BLK  RED  BLK";
                    break;
                case "011":
                    data += "TURBULANCE  MEDIUM     MAG    BLK BLK BLK  RED  MAG";
                    break;
                case "111":
                    data += "TURBULANCE  HEAVY      MAG    BLK BLK BLK  MAG  MAG";
                    break;
                default:
                    data += "Unknown";
                    break;
            }
            return data;
        }

        private void grpBins_Leave(object sender, EventArgs e)
        {
            label5.Text = string.Empty;
            label6.Text = string.Empty;
        }

        private void lblBinaryRep_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, lblBinaryRep.Text.Replace("\n", ""));
        }

        private void label4_DoubleClick(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder("REFLECTIVITY DATA:" + Environment.NewLine);
            switch (ap.RadarType)
            {
                case "C":
                    sb.Append("Bin Code LEVEL       RAINFALL WX MODES MAP MAP CTR TEST TEST");
                    break;
                case "H":
                    sb.Append("Bin Code FUNCTION/LEVEL");
                    break;
                default:
                    sb.Append("Bin DATA");
                    break;
            }
            sb.Append(Environment.NewLine);

            for (int i = 1; i < 48; i++)
            {
                string tbn = string.Format("textBox{0}", i);

                TextBox tb = new TextBox();
                tb = (TextBox)grpBins.Controls[tbn];

                switch (ap.RadarType)
                {
                    case "C":
                        sb.Append(string.Format("{0,3:D}  {1} {2}", i, tb.Text, CollinsBinData(tb.Text)));
                        break;
                    case "H":
                        sb.Append(string.Format("{0,3:D}  {1} {2}", i, tb.Text, HoneywellBinData(tb.Text)));
                        break;
                    default:
                        sb.Append(string.Format("{0,3:D}  {1}", i, tb.Text));
                        break;
                }
                sb.Append(Environment.NewLine);
            }
            Clipboard.SetData(DataFormats.Text, sb.ToString());
        }

        private void btnDo_Click(object sender, EventArgs e)
        {
            maskedBitmap.Text = maskedBitmap.Text.PadRight(ap.HexStrLenght, '0').Substring(0, ap.HexStrLenght);
            mask = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}