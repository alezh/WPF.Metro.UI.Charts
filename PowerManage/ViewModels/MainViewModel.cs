﻿using System.Diagnostics;
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
            updateCommand = new RelayCommand(addChart);
            seriesSwitched = false;

            scatterData = new Batteries { BatteryName = "sjdh" };
            scatterData.Items.Add(new Electricity("1", 390));
            scatterData.Items.Add(new Electricity("2", 50));
            scatterData.Items.Add(new Electricity("3", 420));
            scatterData.Items.Add(new Electricity("5", 400));
            scatterData.Items.Add(new Electricity("6", 100));
            scatterData.Items.Add(new Electricity("7", 300));
            scatterData.Items.Add(new Electricity("8", 330));
            scatterData.Items.Add(new Electricity("9", 360));
            scatterData.Items.Add(new Electricity("10", 389));
            scatterData.Items.Add(new Electricity("11", 390));
            scatterData.Items.Add(new Electricity("12", 410));
            scatterData.Items.Add(new Electricity("13", 400));
            scatterData.Items.Add(new Electricity("14", 387));
            scatterData.Items.Add(new Electricity("15", 388));
            scatterData.Items.Add(new Electricity("16", 220));
            scatterData.Items.Add(new Electricity("17", 150));

        }

        private void update()
        {
           // GalaSoft.MvvmLight.Threading.DispatcherHelper
            scatterData.Items[0].voltage = new System.Random().Next(100, 420);
        }

        /// <summary>
        /// 异步
        /// </summary>
        /// <returns></returns>
        private async Task AsyncAccess()
        {
            
                var getDataListTask = new Task(() =>
                {
                    while (true)
                    {
                        GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            update();
                        });

                        Thread.Sleep(500);
                    }
                });
                getDataListTask.Start();
                await getDataListTask;
           
        }
        public async void addChart()
        {
            await AsyncAccess();
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
