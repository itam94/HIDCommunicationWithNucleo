using HIDConf.Models;
using HIDConf.ViewModels;
using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HIDConf.Commands
{
    internal class SendCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainViewModel _mainViewModel;
        HidDevice _device;
        private HidReport hidReport;
        private Profile _onDeviceProfile;
        private const int VendorId = 0x0483;
        private static readonly int ProductIds = 0x5750;

        public SendCommand(HidDevice device, Profile onDeviceProfile)
        {
            _device = device;
            hidReport = new HidReport(10);
            hidReport.ReportId = 0x04;
            _onDeviceProfile = onDeviceProfile;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _device = HidDevices.Enumerate(VendorId, ProductIds).FirstOrDefault();
            Profile SelectedProfile = (Profile)parameter;
            hidReport.Data = SelectedProfile.key.ToArray();
            _device.OpenDevice();
            bool sended = _device.WriteReport(hidReport);
            if (sended)
            {
                _onDeviceProfile = SelectedProfile;
                MessageBox.Show("Wysłano poprawnie",
                                         "Potwierdzenie",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Information);
            }
            _device.CloseDevice();
        }
    }
}
