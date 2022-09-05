using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace mitsubishiComm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void viewCom()
        {
            comboBox1.Items.Clear();
            comboBox1.Text = "\0";
            try
            {
                foreach(string s in SerialPort.GetPortNames())
                {
                    comboBox1.Items.Add(s);
                }
                comboBox1.Text = comboBox1.Items[0].ToString();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            frameMitsubishi a = new frameMitsubishi();
            //label2.Text = a.sum("5");
            //label2.Text = String.Join("/", a.sendtoread(0,'d',123,2));
            //label2.Text = String.Join("/", a.inttoHexinByte(0x1245,4));
            //label2.Text = a.exponent(2,5).ToString();
            //label2.Text = a.checkSum(new byte[] { 0x02, 0x30, 0x31, 0x30, 0x46, 0x36, 0x30, 0x32, 0x03, 0x37, 0x32 }).ToString();
            label2.Text = a.hex1dtoInt('G').ToString();
            label2.Text = a.hexToInt(new byte[] { 0x31, 0x31,0x32,0x33,0x34 },0,2).ToString();
            viewCom();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.Open();
                    if (serialPort1.IsOpen)
                    {
                        button1.Text = "DISCONNECT";
                    }
                }
                else
                {
                    serialPort1.Close();
                    button1.Text = "CONNECT";
                }
            }catch(Exception a)
            {
                MessageBox.Show(a.ToString());
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int j;
            int i;
            string hasil;
            j = serialPort1.BytesToRead;
            byte[] buffer = new byte[j];
            for (i = 0; i < j; i++)
            {
                buffer[i] = (byte)serialPort1.ReadByte();
                
            }
            this.Invoke((MethodInvoker)delegate ()
            {
                label1.Text = label1.Text + BitConverter.ToString(buffer).Replace("-","");
                label1.Text = label1.Text + " ";
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                //var dataByte = new byte[] { 0x02,0x30,0x31,0x30,0x46,0x36,0x30,0x32,0x03,0x37,0x32 };
                //var dataByte = new byte[] {0x02,0x30,0x31,0x46,0x45,0x31,0x32,0x33,0x34,0x35, 0x36, 0x37, 0x38, 0x03, 0x39, 0x32 };
                frameMitsubishi a = new frameMitsubishi();
                List<byte> dataByte = new List<byte>();
                dataByte = a.sendtoread(0, 'd', 0, 2);
                serialPort1.Write(dataByte.ToArray(), 0, dataByte.Count);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "";
        }
    }
}
