using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CoffeBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Frame CurrentFrame { get; set; }

        public static NavigationService Navigation;
        

        public static bool Navigate(Type sourcePage, object parameter)
        {
            return CurrentFrame.Navigate(sourcePage, parameter);
        }

        public static void GoBack()
        {
            throw new NotImplementedException();
        }
    }
}
