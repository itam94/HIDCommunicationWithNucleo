using HIDConf.Models;
using HIDConf.Utils;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HidLibrary;
using System.Threading;
using System.IO;
using HIDConf.Commands;

namespace HIDConf.ViewModels
{
    class MainViewModel : ModelBase
    {
        public MainViewModel()
        {

            ProfileData = new ObservableCollection<Profile>();
            
            LoadReport(ProfileData);
            if(ProfileData.Count != 0)
            {
                SelectedProfile = ProfileData.FirstOrDefault();
            }
            else
            {
                SelectedProfile = new Profile();
            }
        
            
            Scancode = ScancodeData;
            isConected = false;
            hidReport.ReportId = 4;
            hidReport.Data = SelectedProfile.key.ToArray();
            Send = new SendCommand(_device, onDeviceProfile);
            Add = new AddCommand(ProfileData);
            
            Delete = new DeleteCommand(ProfileData,SelectedProfile);
            


    }
        public static Profile onDeviceProfile;
        public static ObservableCollection<Profile> ProfileData { get; set; }
        private const int VendorId = 0x0483;
        private static readonly int ProductIds = 0x5750;
        private static HidDevice _device;
        private HidReport hidReport = new HidReport(10);
        private string ProfilesDirec = "../../Resources/Profiles.csv";
 
       
        private bool _isConected;
        public bool isConected
        {
            set
            {
                _isConected = value;
                NotifyPropertyChanged("isConected");
                NotifyPropertyChanged("notisConected");

            }
            get
            {
                return _isConected;
            }
        }

        public bool notisConected
        {
            set
            {
                _isConected = value;
            }
            get
            {
                return !_isConected;
            }
        }

        public ICommand Send { get; set;  }
        public ICommand Add { get; set; }
       
        public ICommand Delete { get; set; }
        

        private Profile _SelectedProfile;
        public Profile SelectedProfile {
            get{
                return _SelectedProfile;
            }
                set
            {
                _SelectedProfile = value;
                NotifyPropertyChanged("SelectedProfile");
            }
            }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        

        private void LoadReport(ObservableCollection<Profile> ProfileData)
        {
            using (TextFieldParser parser = new TextFieldParser("../../Resources/Profiles.csv"))
            {
                int i = 0;
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                   
                    Profile newProfile = new Profile();
                    string[] fields = parser.ReadFields();
                    foreach (string field in fields)
                    {
                        //Process field
                        byte k;
                        
                        if (Byte.TryParse(field, out k))
                        {
                            newProfile.key.Add(k);
                        }
                        else
                        {
                            newProfile.Name = field;
                        }
                       
                    }
                    //newProfile.id = i;
                    i++;
                    ProfileData.Add(newProfile);
                   // int i = 0;
                }
                parser.Close();
            }
        }
      
    public ICommand  CloseApp { get { return new RelayCommand <Window>(Close, AlwaysTrueWithWindow); } }
     
    public ICommand Connect { get { return new RelayCommand(ConnectToDevice, AlwaysTrue); } }
        

        private void Close(Window window)
        {
            if (window != null)
            {
                SaveProfiles();
                window.Close();
                if (_device!=null && _device.IsConnected)
                {
                    _device.CloseDevice();
                }
            }
        }

        private void SaveProfiles()
        {
            ProfileData.Remove(onDeviceProfile);
            StreamWriter writer = new StreamWriter(ProfilesDirec);
            
            foreach(Profile p in ProfileData)
            {
                string keyLine = "";
                foreach(byte k in p.key)
                {
                    keyLine += "," + k;
                }
                writer.WriteLine(p.Name + keyLine);
            }
            writer.Close();
        }

        private Dictionary<string, byte> _Scancode;
        public Dictionary<string, byte> Scancode
        {
            get { return _Scancode; }
            set
            {
                _Scancode = value;
                OnPropertyChanged("Scancode");

            }
        }


        public Dictionary<string, byte> ScancodeData = new Dictionary<string, byte>
        {
              { "LCTRL" , 0x66},
            { "LSHIFT" , 0x67},
            { "LALT" , 0x68},
            { "LMETA" , 0x69},
            { "RCTRL" , 0x6A},
            { "RSHIFT" , 0x6B},
            { "RALT" , 0x6C},
            { "RMETA" , 0x6D},

            /**
             * Scan codes - last N slots in the HID report (usually 6).
             * 0x00 if no { "KEY pressed.
             * 
             * If more than N { "KEYs are pressed}, the HID reports 
             * { "ERR_OVF in all slots to indicate this condition.
             */

            { "NONE" , 0x00}, // No { "KEY pressed
            { "ERR_OVF" , 0x01}, //  { "KEYboard Error Roll Over - used for all slots if too many { "KEYs are pressed ("Phantom { "KEY")
                                // 0x02 //  { "KEYboard POST Fail
                                // 0x03 //  { "KEYboard Error Undefined
            { "A" , 0x04}, // { "KEYboard a and A
            { "B" , 0x05},// { "KEYboard b and B
            { "C" , 0x06}, // { "KEYboard c and C
            { "D" , 0x07}, // { "KEYboard d and D
            { "E" , 0x08}, // { "KEYboard e and E
            { "F" , 0x09}, // { "KEYboard f and F
            { "G" , 0x0a}, // { "KEYboard g and G
            { "H" , 0x0b}, // { "KEYboard h and H
            { "I" , 0x0c}, // { "KEYboard i and I
            { "J" , 0x0d}, // { "KEYboard j and J
            { "K" , 0x0e}, // { "KEYboard k and K
            { "L" , 0x0f}, // { "KEYboard l and L
            { "M" , 0x10}, // { "KEYboard m and M
            { "N" , 0x11}, // { "KEYboard n and N
            { "O" , 0x12}, // { "KEYboard o and O
            { "P" , 0x13}, // { "KEYboard p and P
            { "Q" , 0x14}, // { "KEYboard q and Q
            { "R" , 0x15}, // { "KEYboard r and R
            { "S" , 0x16}, // { "KEYboard s and S
            { "T" , 0x17}, // { "KEYboard t and T
            { "U" , 0x18}, // { "KEYboard u and U
            { "V" , 0x19}, // { "KEYboard v and V
            { "W" , 0x1a}, // { "KEYboard w and W
            { "X" , 0x1b}, // { "KEYboard x and X
            { "Y" , 0x1c}, // { "KEYboard y and Y
            { "Z" , 0x1d}, // { "KEYboard z and Z

            { "1" , 0x1e}, // { "KEYboard 1 and !
            { "2" , 0x1f}, // { "KEYboard 2 and @
            { "3" , 0x20}, // { "KEYboard 3 and #
            { "4" , 0x21}, // { "KEYboard 4 and $
            { "5" , 0x22}, // { "KEYboard 5 and %
            { "6" , 0x23}, // { "KEYboard 6 and ^
            { "7" , 0x24}, // { "KEYboard 7 and &
            { "8" , 0x25}, // { "KEYboard 8 and *
            { "9" , 0x26}, // { "KEYboard 9 and (
            { "0" , 0x27}, // { "KEYboard 0 and )

            { "ENTER" , 0x28}, // { "KEYboard Return (ENTER)
            { "ESC" , 0x29}, // { "KEYboard ESCAPE
            { "BACKSPACE" , 0x2a}, // { "KEYboard DELETE (Backspace)
            { "TAB" , 0x2b}, // { "KEYboard Tab
            { "SPACE" , 0x2c}, // { "KEYboard Spacebar
            { "MINUS" , 0x2d}, // { "KEYboard - and _
            { "EQUAL" , 0x2e}, // { "KEYboard" , and +
            { "LEFTBRACE" , 0x2f}, // { "KEYboard [ and {
            { "RIGHTBRACE" , 0x30}, // { "KEYboard ] and }
            { "BACKSLASH" , 0x31}, // { "KEYboard \ and |
            { "HASHTILDE" , 0x32}, // { "KEYboard Non-US # and ~
            { "SEMICOLON" , 0x33}, // { "KEYboard ; and :
            { "APOSTROPHE" , 0x34}, // { "KEYboard ' and "
            { "GRAVE" , 0x35}, // { "KEYboard ` and ~
            { "COMMA" , 0x36}, // { "KEYboard }, and <
            { "DOT" , 0x37}, // { "KEYboard . and >
            { "SLASH" , 0x38}, // { "KEYboard / and ?
            { "CAPSLOCK" , 0x39}, // { "KEYboard Caps Lock

            { "F1" , 0x3a}, // { "KEYboard F1
            { "F2" , 0x3b}, // { "KEYboard F2
            { "F3" , 0x3c}, // { "KEYboard F3
            { "F4" , 0x3d}, // { "KEYboard F4
            { "F5" , 0x3e}, // { "KEYboard F5
            { "F6" , 0x3f}, // { "KEYboard F6
            { "F7" , 0x40}, // { "KEYboard F7
            { "F8" , 0x41}, // { "KEYboard F8
            { "F9" , 0x42}, // { "KEYboard F9
            { "F10" , 0x43}, // { "KEYboard F10
            { "F11" , 0x44}, // { "KEYboard F11
            { "F12" , 0x45}, // { "KEYboard F12

            { "PRT SCR" , 0x46}, // { "KEYboard Print Screen
            { "SCROLLLOCK" , 0x47}, // { "KEYboard Scroll Lock
            { "PAUSE" , 0x48}, // { "KEYboard Pause
            { "INSERT" , 0x49}, // { "KEYboard Insert
            { "HOME" , 0x4a}, // { "KEYboard Home
            { "PAGEUP" , 0x4b}, // { "KEYboard Page Up
            { "DELETE" , 0x4c}, // { "KEYboard Delete Forward
            { "END" , 0x4d}, // { "KEYboard End
            { "PAGEDOWN" , 0x4e}, // { "KEYboard Page Down
            { "RIGHT" , 0x4f}, // { "KEYboard Right Arrow
            { "LEFT" , 0x50}, // { "KEYboard Left Arrow
            { "DOWN" , 0x51}, // { "KEYboard Down Arrow
            { "UP" , 0x52}, // { "KEYboard Up Arrow

            { "NUMLOCK" , 0x53}, // { "KEYboard Num Lock and Clear
            { "KPSLASH" , 0x54}, // { "KEYpad /
            { "KPASTERISK" , 0x55}, // { "KEYpad *
            { "KPMINUS" , 0x56}, // { "KEYpad -
            { "KPPLUS" , 0x57}, // { "KEYpad +
            { "KPENTER" , 0x58}, // { "KEYpad ENTER
            { "KP1" , 0x59}, // { "KEYpad 1 and End
            { "KP2" , 0x5a}, // { "KEYpad 2 and Down Arrow
            { "KP3" , 0x5b}, // { "KEYpad 3 and PageDn
            { "KP4" , 0x5c}, // { "KEYpad 4 and Left Arrow
            { "KP5" , 0x5d}, // { "KEYpad 5
            { "KP6" , 0x5e}, // { "KEYpad 6 and Right Arrow
            { "KP7" , 0x5f}, // { "KEYpad 7 and Home
            { "KP8" , 0x60}, // { "KEYpad 8 and Up Arrow
            { "KP9" , 0x61}, // { "KEYpad 9 and Page Up
            { "KP0" , 0x62}, // { "KEYpad 0 and Insert
            { "KPDOT" , 0x63}, // { "KEYpad . and Delete

            { "102ND" , 0x64}, // { "KEYboard Non-US \ and |
            { "COMPOSE" , 0x65} // { "KEYboard Application
			};
        private bool AlwaysTrueWithWindow(Window w) { return true; }
        private bool AlwaysTrue() { return true; }
        private bool AlwaysFalse() { return false; }
        

        void ConnectToDevice()
        {
            _device = HidDevices.Enumerate(VendorId, ProductIds).FirstOrDefault();
            if (_device != null)
            {
                isConected = true;
                _device.OpenDevice();
            
                byte[] bytes ={ 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0};

                hidReport.Data = bytes;
                if(_device.WriteReport(hidReport))
                {
                    _device.ReadReport(OnReport);
                }
            }
            else
            {
                MessageBox.Show("Nie można połączyć",
                                          "Błąd",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
            }
        }

       
        private static void OnReport(HidReport report)
        {
            if (report.Data[0] == 0) { return; }
            onDeviceProfile = new Profile();
            onDeviceProfile.Name = "Na urzadzeniu";
            for (int i = 0; i < report.Data.Length; i++)
            {
                onDeviceProfile.key.Add(report.Data[i]);
            }
            //newProfile.id = 0;
            
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                ProfileData.Add(onDeviceProfile);
            });
           
            _device.CloseDevice();
        }
    }
}

