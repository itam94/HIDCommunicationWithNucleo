using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIDConf.Models
{
    class Profile : ModelBase
    {
        private string _Name;
        public string Name { get { return _Name; } set { _Name = value; NotifyPropertyChanged("Name"); } }

       /* private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value;
                NotifyPropertyChanged("id");
            }
        }*/
        public List<byte> key { get; set; }

        public Profile()
        {
            key = new List<byte>();
            }
    }


}
