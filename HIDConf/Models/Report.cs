using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIDConf.Models
{
    class Report:ModelBase
    {
       
        private List <byte> _key;
        private List<byte>  key { get { return _key; } set { _key = value; NotifyPropertyChanged("key"); } }
        
        public Report()
        {
            _key = new List<byte>();
            
        }

        public void addKey(byte k)
        {
            _key.Add(k);
            NotifyPropertyChanged("key");
            
        }
    }
}
