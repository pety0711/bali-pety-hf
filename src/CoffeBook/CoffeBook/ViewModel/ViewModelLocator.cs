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

using CoffeBook.ViewModels;
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
        public const string MainKey = "Main";
        public const string AuthenticatedKey = "Authenticated";

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = CreateNavigationService();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AuthenticatedViewModel>();

            Messenger.Default.Register<NotificationMessage>(this, NotifyUser);
        }

        private INavigationService CreateNavigationService()
        {
            var navService = new NavigationService();
            navService.
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public AuthenticatedViewModel Authenticated
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AuthenticatedViewModel>();
            }
        }


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