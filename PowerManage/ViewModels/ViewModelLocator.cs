using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace PowerManage.ViewModels
{
    public class ViewModelLocator
    {
        /// <summary>
        /// 初始化设置ViewModelLocator
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                // SimpleIoc.Default.Register<IDataService, DesignDataService>();
                //  SimpleIoc.Default.Register<IGADataService, DataService>();
            }
            else
            {
                // Create run time view services and models
                // SimpleIoc.Default.Register<IGADataService, DataService>();
            }

            //views
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}
