using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace CoffeBook.ViewModels
{
    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            Task.Run(() =>
            {
                try
                {
                    DbHelper.TestDb();
                }
                catch (Exception e)
                {
                    var msg = e.Message;
                }
            });
        }
    }
}
