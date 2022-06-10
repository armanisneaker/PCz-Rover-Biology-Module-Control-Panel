using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace RoverBiologyModuleControlPanel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Console.Write("---------------------------\nBiology Module Tutorial\n\nConnect - type in IP address and port number then click Connect button\nSend Frame - select desired module configuration and click Update button\n\nFrame structure:\nthe first 4 bits are information about state of subsequent container lids: true - opened / false - closed\nthe next 4 bits are 0s\nthe next 4 bits encode the rotation position ( 0 - 7 ) of the containers revolver\nthe next 4 bits encode the rotation position ( 0 - 7 ) of the  spectroscope revolver\n---------------------------\n");
        }
        UdpClient udpClient = new UdpClient();
        public string IPAdress;
        public string Port;
        public bool Lid1Opened = false;
        public bool Lid2Opened = false;
        public bool Lid3Opened = false;
        public bool Lid4Opened = false;
        int ContainerValue = 0;
        int SpectroscopeValue = 0;
        private void ButtonClick(object sender, RoutedEventArgs ex)
        {
            IPAdress = IpTextBox.Text;
            Port = PortTextBox.Text;
        }
        
        private void Lid1SwitchClick(object sender, RoutedEventArgs ex)
        {
           
            if (Lid1Opened == false)
            {
                Lid1Opened = true;
            }
            else
            {
                Lid1Opened = false;
            }
        }
        private void Lid2SwitchClick(object sender, RoutedEventArgs ex)
        {
            if (Lid2Opened == false)
            { 
                Lid2Opened = true;
            }
            else
            {
                Lid2Opened = false;
            }
        }
        private void Lid3SwitchClick(object sender, RoutedEventArgs ex)
        {
            if (Lid3Opened == false)
            {
                Lid3Opened = true;
            }
            else
            {
                Lid3Opened = false;
            }

        }
        private void Lid4SwitchClick(object sender, RoutedEventArgs ex)
        {
            if (Lid4Opened == false)
            {
                Lid4Opened = true;
            }
            else
            {
                Lid4Opened = false;
            }
        }

        private void ContainerSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> ex)
        {
            ContainerValue = (int)ContainerRevolverSlider.Value;
        }

        private void SpectroscopeSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> ex)
        {
            SpectroscopeValue = (int)SpectroscopeRevolverSlider.Value;
        }
        
        byte[] MakeFrame(bool switch1, bool switch2, bool switch3, bool switch4, int container, int spectroscope)
        {
            byte[] frame = new byte[2]; // pierwsze 4 bity to informacja o otwarciu kolejnych pokrywek pojemników, następne 4 bity są równe 0, następne 4 bity kodują pozycję obrotu rewolwera z pojemnikami, następne 4 bity kodują pozycję obrotu rewolwera ze spektroskopem

            frame[0] |= (byte)((switch1 ? 1 : 0) << 7);
            frame[0] |= (byte)((switch2 ? 1 : 0) << 6);
            frame[0] |= (byte)((switch3 ? 1 : 0) << 5);
            frame[0] |= (byte)((switch4 ? 1 : 0) << 4);
            frame[1] |= (byte)((container/45) << 4);
            frame[1] |= (byte)((spectroscope/45));

            return frame;
        }
        
        private void SendUpdate(object sender, RoutedEventArgs ex)
        {
            byte[] frame = MakeFrame(Lid1Opened, Lid2Opened, Lid3Opened, Lid4Opened, ContainerValue, SpectroscopeValue);
            try
            {
                udpClient.Send(frame, frame.Length, IPAdress, int.Parse(Port));
                Console.Write("---------------------------\nBiology Module\n\nIP: " + IPAdress + "\nPort: " + Port + "\n\n" + "Container 1 opened: " + Lid1Opened + "\nContainer 2 opened: " + Lid2Opened + "\nContainer 3 opened: " + Lid3Opened + "\nContainer 4 opened: " + Lid4Opened + "\n\nContainer revolver position: " + ContainerValue + "°\nSpectroscope revolver position: " + SpectroscopeValue + "°\n\n" + "Frame: " + frame[0] + " " + frame[1] + "\n---------------------------\n") ;
            }
            catch (Exception e)
            {
                Console.WriteLine("---------------------------\nType in Correct IP and Port\n---------------------------\n");
            }

        }
    } 
}
