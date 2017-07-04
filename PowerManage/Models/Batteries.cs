using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerManage.Models
{
    public class Batteries : ViewModelBase
    {
        //[DefaultValue(32)]
        private string _BatteryName;
        public string BatteryName
        {
            get
            {
                return _BatteryName;
            }
            set
            {
                if (_BatteryName != value)
                {
                    _BatteryName = value;
                    RaisePropertyChanged("BatteryName");//定义键
                }
            }
        }

        /// <summary>
        /// 集合数据
        /// </summary>
        public ObservableCollection<Electricity> Items { get; set; }
        /// <summary>
        /// 新建集合数据
        /// </summary>
        public Batteries()
        {
            Items = new ObservableCollection<Electricity>();
        }


    }
}
