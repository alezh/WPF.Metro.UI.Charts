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
        private Double? _Voltage;
        /// <summary>
        /// 电压
        /// </summary>
        public Double? Voltage
        {
            get
            {
                return _Voltage;
            }
            set
            {
                if (_Voltage != value)
                {
                    _Voltage = value;
                    RaisePropertyChanged("Voltage");
                }
            }
        }

        private int? _sn;
        /// <summary>
        /// 电池序号
        /// </summary>
        public int? sn
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

        public Electricity(int sn,double Voltage)
        {
            this.Voltage = Voltage;
            this.sn      = sn;
        }

    }
}
