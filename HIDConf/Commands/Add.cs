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
    internal class AddCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        ObservableCollection<Profile> _profileData;

        public AddCommand(ObservableCollection<Profile> ProfileData)
        {
            _profileData = ProfileData;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
           
        Profile newProfile = new Profile();
        newProfile.Name = "Bez tytulu";
        for (int i = 0; i < 10; i++)
        {
            newProfile.key.Add(0x0);
        }

            

            _profileData.Add(newProfile);
           
        }
    }
}
