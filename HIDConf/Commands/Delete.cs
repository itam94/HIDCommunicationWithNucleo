using HIDConf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HIDConf.Commands
{
    internal class DeleteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        ObservableCollection<Profile> ProfileData;
        Profile OnDeviceProfile;

        public DeleteCommand(ObservableCollection<Profile> _ProfileData, Profile _SelectedProfile)
        {
            ProfileData = _ProfileData;
            OnDeviceProfile = _SelectedProfile;
        }

        public bool CanExecute(object parameter)
        {
           
            return true;
        }

        public void Execute(object parameter)
        {
            Profile onDeviceProfile = (Profile)parameter;
            if (onDeviceProfile == null || ProfileData.Count == 0 || onDeviceProfile.Name == "Na urzadzeniu")
            {
                return;
            }
            ProfileData.Remove((Profile)parameter);
            if (ProfileData.Count == 0)
            {
                Profile newProfile = new Profile();
                newProfile.Name = "Bez tytulu";
                for (int i = 0; i < 10; i++)
                {
                    newProfile.key.Add(0x0);
                }



                ProfileData.Add(newProfile); 
            }

            
        }
    }
}
