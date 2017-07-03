using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace GAMandelkowMetroChartsTestApplication.Models
{
    /// <summary>
    /// 图表信息模型
    /// </summary>
    public class GraphSeriesInformation : ViewModelBase
    {
        private string _seriesDisplayName;
        /// <summary>
        /// 表名
        /// </summary>
        public string seriesDisplayName
        {
            get
            {
                return _seriesDisplayName;
            }
            set
            {
                if (_seriesDisplayName != value)
                {
                    _seriesDisplayName = value;
                    RaisePropertyChanged("seriesDisplayName");//定义键
                }
            }
        }
        /// <summary>
        /// 集合数据
        /// </summary>
        public ObservableCollection<GraphSeriesDataPoint> Items { get; set; }
        /// <summary>
        /// 新建集合数据
        /// </summary>
        public GraphSeriesInformation()
        {
            Items = new ObservableCollection<GraphSeriesDataPoint>();
        }
    }
}