using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoffeBook.Helpers
{
    public class NavigationService : INavigationService
    {
        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void Navigate(Type sourcePage)
        {
        }

        public void Navigate(Type sourcePage, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
