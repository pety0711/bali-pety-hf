using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BL;
using System.Windows.Input;
using System.ComponentModel;
using CoffeBook.Commands;
using DB;
using DB.Entities;

namespace CoffeBook.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private string errorLog;

        public string ErrorLog
        {
            get { return errorLog; }
            set {
                if (value != errorLog)
                {
                    errorLog = value;
                    OnPropertyChanged("ErrorLog");
                }
            }
        }

        private void ClearErrorLog()
        {
            ErrorLog = "";
        }


        private ICommand registerButtonCommand;
        
        public ICommand RegisterButtonCommand
        {
            get {
                return registerButtonCommand ?? (registerButtonCommand = new CommandHandler(() => Register(), registerCanExecute));
            }
        }

        private bool registerCanExecute = true;
        public void Register()
        {
            var valid = ValidateInput();
            if (!valid)
                return;

            using (var context = new CoffeBookContext())
            {
                var conn = context.Database.Connection.ConnectionString;

                var user = new User
                {
                    Name = this.InputName,
                    Password = this.InputPassword
                };

                var user_result = context.Users.Where(u => u.Name == user.Name).First();
                if (user_result == null)
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                else
                {
                    ErrorLog = "User name already exists!";
                    return;
                }
            }
        }

        private ICommand loginButtonCommand;

        public ICommand LoginButtonCommand
        {
            get { return loginButtonCommand ?? (loginButtonCommand = new CommandHandler(() => Login(), loginCanExecute)); }
        }

        private bool loginCanExecute = true;
        public void Login()
        {
            var valid = ValidateInput();
            if (!valid)
                return;

            using (var context = new CoffeBookContext())
            {
                var conn = context.Database.Connection.ConnectionString;

                var user = new User
                {
                    Name = this.InputName,
                    Password = this.InputPassword
                };

                var user_result = context.Users.Where(u => u.Name == user.Name).First();
                if (user_result == null)
                {
                    ErrorLog = "User not found!";
                }
                if (user_result.Password == user.Password)
                {
                    
                }
                else
                {
                    ErrorLog = "Incorrect password";
                    return;
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(InputName))
            {
                ErrorLog = "Username field is empty or the input is invalid!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(InputPassword))
            {
                ErrorLog = "Password field is empty or the input is invalid!";
                return false;
            }
            else
            {
                ClearErrorLog();
                return true;
            }
        }

        public string InputName { get; set; }
        public string InputPassword { get; set; }

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
