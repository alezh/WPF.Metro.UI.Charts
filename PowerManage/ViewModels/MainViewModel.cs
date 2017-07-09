using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PowerManage.Models;
using System.Threading.Tasks;
using System.Threading;

namespace PowerManage.ViewModels
{
    public class MainViewModel : ViewModelBase    
    {
        private Batteries _scatterData;
        public Batteries scatterData
        {
            get { return _scatterData; }
            set
            {
                if (_scatterData != value)
                {
                    _scatterData = value;
                    RaisePropertyChanged("scatterData");
                }
            }
        }

        private Electricity _selectedItem;
        public Electricity selectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged("selectedItem");
                }
            }
        }
        

        #region 行列转换开关

        private bool _seriesSwitched;
        /// <summary>
        /// 行列转换开关
        /// </summary>
        public bool seriesSwitched
        {
            get { return _seriesSwitched; }
            set
            {
                if (_seriesSwitched != value)
                {
                    _seriesSwitched = value;
                    RaisePropertyChanged("seriesSwitched");
                }
            }
        }

        private void switchSeries()
        {
            seriesSwitched = !seriesSwitched;
        }
        #endregion

        public RelayCommand SwitchSeries { get; private set; }

        public RelayCommand updateCommand { get; private set; }
        public MainViewModel()
        {
            SwitchSeries = new RelayCommand(switchSeries);
            updateCommand = new RelayCommand(AsyncAccess);
            seriesSwitched = false;

            scatterData = new Batteries { BatteryName = "sjdh" };
            scatterData.Items.Add(new Electricity("1", 390));
            scatterData.Items.Add(new Electricity("2", 420));
            scatterData.Items.Add(new Electricity("3", 420));
            scatterData.Items.Add(new Electricity("4", 400));
            scatterData.Items.Add(new Electricity("5", 100));
            scatterData.Items.Add(new Electricity("6", 300));
            scatterData.Items.Add(new Electricity("7", 330));
            scatterData.Items.Add(new Electricity("8", 360));
            scatterData.Items.Add(new Electricity("9", 389));
            scatterData.Items.Add(new Electricity("10", 390));
            scatterData.Items.Add(new Electricity("11", 410));
            scatterData.Items.Add(new Electricity("12", 400));
            scatterData.Items.Add(new Electricity("13", 387));
            scatterData.Items.Add(new Electricity("14", 388));
            scatterData.Items.Add(new Electricity("15", 220));
            scatterData.Items.Add(new Electricity("16", 150));

        }

        private void update()
        {
            int row = new System.Random().Next(0, 16);
            scatterData.Items[row].voltage = new System.Random().Next(0, 420);
        }

        public void delete()
        {
            scatterData.Items.RemoveAt(15);
        }

        /// <summary>
        /// 异步
        /// </summary>
        /// <returns></returns>
        private async void AsyncAccess()
        {
            var getDataListTask = new Task(() =>
                {
                    while (true)
                    {
                        GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            update();
                        });
                        Thread.Sleep(100);
                    }
                });
            getDataListTask.Start();
            await getDataListTask;           
        }

        public void addChart()
        {
            
        }
        /// <summary>
        /// ThreadPool 进程池异步
        /// </summary>
        private void eachvol()
        {
            ThreadPool.QueueUserWorkItem(o =>
              {
                  // This is a background operation!
                  while (true)
                  {
                      // Do something
                      GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            // Dispatch back to the main thread
                            //Status = string.Format("Loop # {0}",
                            //   loopIndex++);
                            update();
                        });
                      // Sleep for a while
                      Thread.Sleep(500);
                  }
              });
        }

    }
}
