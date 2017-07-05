using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerManage.Models
{
    public class Electricity : ViewModelBase
    {
        private int? _voltage;
        /// <summary>
        /// 电压
        /// </summary>
        public int? voltage
        {
            get
            {
                return _voltage;
            }
            set
            {
                if (_voltage != value)
                {
                    _voltage = value;
                    RaisePropertyChanged("voltage");
                }
            }
        }

        private string _sn;
        /// <summary>
        /// 电池序号
        /// </summary>
        public string sn
        {
            get
            {
                return _sn;
            }
            set
            {
                if (_sn != value)
                {
                    _sn = value;
                    RaisePropertyChanged("sn");
                }
            }
        }

        public Electricity(string sn,int? Voltage)
        {
            voltage = Voltage;
            this.sn      = sn;
        }

    }
}
