using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeBook.ViewModel
{
    public interface ICustomNavigationService :INavigationService
    {
        object Parameter { get; }
    }
}
