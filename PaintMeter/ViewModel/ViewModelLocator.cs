using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using PaintMeter.Classes;
using PaintMeter.Model;

namespace PaintMeter.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<EditionViewModel>();
            SimpleIoc.Default.Register<Communications>();

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public EditionViewModel Edition
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditionViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}