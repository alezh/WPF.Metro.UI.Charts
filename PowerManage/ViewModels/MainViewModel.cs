﻿using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PowerManage.Models;

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
        public MainViewModel()
        {
            SwitchSeries = new RelayCommand(switchSeries);
            seriesSwitched = false;
        }        
    }
}
