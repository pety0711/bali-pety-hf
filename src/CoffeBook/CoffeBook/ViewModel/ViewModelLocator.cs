/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:CoffeBook"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CoffeBook.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using System.Windows;
using System;
using GalaSoft.MvvmLight.Views;
using System.Windows.Navigation;

namespace CoffeBook.ViewModel
{
    public class ViewModelLocator
    {
        public const string MainKey = "MainViewModel";
        public const string AuthenticatedKey = "AuthenticatedViewModel";

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //CreateNavigationService();

            SimpleIoc.Default.Register<MainViewModel>();
            //SimpleIoc.Default.Register<AuthenticatedViewModel>();

            Messenger.Default.Register<NotificationMessage>(this, NotifyUser);
        }

        private void CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure(MainKey, new Uri("Views/MainWindow.xaml", UriKind.Relative));
            //navigationService.Configure(AuthenticatedKey, new Uri("Views/AuthenticatedView.xaml", UriKind.Relative));

            SimpleIoc.Default.Register<ICustomNavigationService>(() => navigationService);
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        //public AuthenticatedViewModel AuthenticatedViewModel
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<AuthenticatedViewModel>();
        //    }
        //}


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        public void NotifyUser(NotificationMessage msg)
        {
            MessageBox.Show(msg.Notification);
        }
    }
}