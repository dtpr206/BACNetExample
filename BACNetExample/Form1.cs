using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.BACnet;
using System.Threading;
using System.Diagnostics;

///Folosim biblioteca BACnet preluata cu Nuget
///BACnet protocol library for .NET
///Ela-compil and contributors
///https://github.com/ela-compil
///https://github.com/ela-compil/BACnet


namespace BACNetExample
{
    public partial class Form1 : Form
    {

        static BacnetClient bacnet_client;

        // All the present Bacnet Device List
        static List<BacNode> DevicesList = new List<BacNode>();
        Thread listeningThread;
        private Stopwatch sw;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                StartActivity();
                Console.WriteLine("Started");

                Thread.Sleep(1000); // Wait a fiew time for WhoIs responses (managed in handler_OnIam)

                ReadWriteExample();
            }
            catch { }
        }

        private void StartActivity()
        {
            // Bacnet on UDP/IP/Ethernet
            bacnet_client = new BacnetClient(new BacnetIpUdpProtocolTransport(0xBAC0, false));
            // or Bacnet Mstp on COM4 à 38400 bps, own master id 8
            // m_bacnet_client = new BacnetClient(new BacnetMstpProtocolTransport("COM4", 38400, 8);
            // Or Bacnet Ethernet
            // bacnet_client = new BacnetClient(new BacnetEthernetProtocolTransport("Connexion au réseau local"));          
            // Or Bacnet on IPV6
            // bacnet_client = new BacnetClient(new BacnetIpV6UdpProtocolTransport(0xBAC0));

            bacnet_client.Start();    // go

            // Send WhoIs in order to get back all the Iam responses :  
            bacnet_client.OnIam += new BacnetClient.IamHandler(handler_OnIam);

            bacnet_client.WhoIs();

            /* Optional Remote Registration as A Foreign Device on a BBMD at @192.168.1.1 on the default 0xBAC0 port
                           
            bacnet_client.RegisterAsForeignDevice("192.168.1.1", 60);
            Thread.Sleep(20);
            bacnet_client.RemoteWhoIs("192.168.1.1");
            */
        }

        /*****************************************************************************************************/
        private void ReadWriteExample()
        {

            BacnetValue Value;
            bool ret;
            button_citeste_putere.Enabled = false;
            try
            {
                listeningThread = new Thread(() =>
                {
                    appendTextRichtextBox1("Start fir executie citire BACNet\n");
                    sw = Stopwatch.StartNew();

                    while (true)
                    {
                        // Read Present_Value property on the object ANALOG_INPUT:0 provided by the device 12345
                        // Scalar value only
                        //AICI E VALOAREA pe care o citim din server. trebuie cautata puterea/curentul instantaneu - 12345
                        ret = ReadScalarValue(12345, new BacnetObjectId(BacnetObjectTypes.OBJECT_ANALOG_INPUT, 0), BacnetPropertyIds.PROP_PRESENT_VALUE, out Value);

                        if (ret == true)
                        {
                            //Console.WriteLine("Read value : " + Value.Value.ToString());
                            appendTextRichtextBox1(sw.ElapsedMilliseconds.ToString() + ": " + Value.ToString() + "\n");

                            // Write Present_Value property on the object ANALOG_OUTPUT:0 provided by the device 4000
                            //BacnetValue newValue = new BacnetValue(Convert.ToSingle(Value.Value));   // expect it's a float
                            //ret = WriteScalarValue(4000, new BacnetObjectId(BacnetObjectTypes.OBJECT_ANALOG_OUTPUT, 0), BacnetPropertyIds.PROP_PRESENT_VALUE, newValue);

                            //Console.WriteLine("Write feedback : " + ret.ToString());
                        }
                        else
                            appendTextRichtextBox1("Error somewhere !");
                        Thread.Sleep(1);
                    }
                });
                listeningThread.IsBackground = true;
                listeningThread.Start();
            }
            catch (Exception ex)
            {
                appendTextRichtextBox1("Eroare deschidere socket. Verifica daca mai e deschis un alt server in paralel.\n");
            }

            

        }

        /*****************************************************************************************************/
        private bool ReadScalarValue(int device_id, BacnetObjectId BacnetObjet, BacnetPropertyIds Propriete, out BacnetValue Value)
        {
            BacnetAddress adr;
            IList<BacnetValue> NoScalarValue;

            Value = new BacnetValue(null);

            // Looking for the device
            adr = DeviceAddr((uint)device_id);
            if (adr == null) return false;  // not found

            // Property Read
            if (bacnet_client.ReadPropertyRequest(adr, BacnetObjet, Propriete, out NoScalarValue) == false)
                return false;

            Value = NoScalarValue[0];
            return true;
        }
        /*****************************************************************************************************/
        private void handler_OnIam(BacnetClient sender, BacnetAddress adr, uint device_id, uint max_apdu, BacnetSegmentations segmentation, ushort vendor_id)
        {
            lock (DevicesList)
            {
                // Device already registred ?
                foreach (BacNode bn in DevicesList)
                    if (bn.getAdd(device_id) != null) return;   // Yes

                // Not already in the list
                DevicesList.Add(new BacNode(adr, device_id));   // add it
            }
        }

        /*****************************************************************************************************/
        private BacnetAddress DeviceAddr(uint device_id)
        {
            BacnetAddress ret;

            lock (DevicesList)
            {
                foreach (BacNode bn in DevicesList)
                {
                    ret = bn.getAdd(device_id);
                    if (ret != null) return ret;
                }
                // not in the list
                return null;
            }
        }

        /// <summary>
        /// adaugare text la richtextbox din alt thread
        /// </summary>
        /// <param name="s">sirul de caractere ce trebuie adaugat</param>
        private void appendTextRichtextBox1(string s)
        {
            richTextBox1.Invoke((MethodInvoker)delegate
            {
                richTextBox1.Text += s;
            });

        }

        private void button_stop_citeste_putere_Click(object sender, EventArgs e)
        {
            if(listeningThread!=null)
            {
                if (listeningThread.IsAlive)
                {
                    listeningThread.Abort();
                    button_citeste_putere.Enabled = true;
                    sw.Reset();
                }
            }
        }
    }

    class BacNode
    {
        BacnetAddress adr;
        uint device_id;

        public BacNode(BacnetAddress adr, uint device_id)
        {
            this.adr = adr;
            this.device_id = device_id;
        }

        public BacnetAddress getAdd(uint device_id)
        {
            if (this.device_id == device_id)
                return adr;
            else
                return null;
        }
    }
}
