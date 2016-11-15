using CoffeBook.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeBook.ViewModels
{
    public class AuthenticatedViewModel : ViewModelBase
    {
        private INavigationService navigationService;

        public AuthenticatedViewModel(ICustomNavigationService navService)
        {
            this.navigationService = navService;
        }
    }
}
