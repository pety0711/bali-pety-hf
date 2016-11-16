using BL;
using BL.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Collections.Generic;

namespace CoffeBook.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private User loginUser;
        private string errorLog;
        private ObservableCollection<Recipe> recipes;
        //private INavigationService navigationService;

        public ICommand RegisterButtonCommand { get; private set; }
        public ICommand LoginButtonCommand { get; private set; }

        private ObservableCollection<RecipeBook> recipebooks;

        private bool isAuthenticated;


        public MainViewModel()
        {
            DbHelper.TestDb();

            LoginUser = new User { Name = "", Password = "" };
            errorLog = "";
            recipebooks = new ObservableCollection<RecipeBook>();
            recipes = new ObservableCollection<Recipe>();
            isAuthenticated = false;


            //navigationService = navService;

            RegisterButtonCommand = new RelayCommand(Register);
            LoginButtonCommand = new RelayCommand(Login);

            LoadEverything();
        }

        private void LoadEverything()
        {
            Recipes = (ObservableCollection<Recipe>)RecipeHelper.GetRecipes();
        }

        #region getters-setters

        public User LoginUser
        {
            get { return loginUser; }
            set { loginUser = value; }
        }

        public string ErrorLog
        {
            get { return errorLog; }
            set
            {
                if (value != errorLog)
                {
                    errorLog = value;
                    RaisePropertyChanged("ErrorLog");
                }
            }
        }

        private void ClearErrorLog()
        {
            ErrorLog = "";
        }


        public string InputName
        {
            get { return LoginUser.Name; }
            set
            {
                LoginUser.Name = value;
                RaisePropertyChanged("InputName");
            }
        }


        public string InputPassword
        {
            get { return LoginUser.Password; }
            set
            {
                LoginUser.Password = value;
                RaisePropertyChanged("InputPassword");
            }
        }

        public ObservableCollection<Recipe> Recipes
        {
            get { return recipes; }
            set
            {
                recipes.Clear();
                foreach(Recipe r in value)
                {
                    recipes.Add(r);
                }
                RaisePropertyChanged("Recipes");
            }
        }

        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set
            {
                isAuthenticated = value;
                RaisePropertyChanged("IsAuthenticated");
            }
        }


        public ObservableCollection<RecipeBook> RecipeBooks
        {
            get { return recipebooks; }
            set
            {

                recipebooks.Clear();
                if (value == null)
                    return;
                foreach (RecipeBook rb in value)
                {
                    recipebooks.Add(rb);
                }
                RaisePropertyChanged("RecipeBooks");
            }
        }


        #endregion

        #region commands

        public void Register()
        {
            var valid = ValidateInput();
            if (!valid)
                return;
            var user_result = UserHelper.GetUser(LoginUser.Name);
            if (user_result == null)
            {
                UserHelper.RegisterUser(LoginUser);
                Authenticate(LoginUser);
            }
            else
            {
                ErrorLog = "User name already exists!";
                return;
            }
        }

        public void Login()
        {
            var valid = ValidateInput();
            if (!valid)
                return;

            var user_result = UserHelper.GetUser(LoginUser.Name);
            if (user_result == null)
            {
                ErrorLog = "User not found!";
                return;
            }
            Authenticate(LoginUser);
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

        private void Authenticate(User user)
        {
            User u = UserHelper.GetUser(user.Name);
            if (u.Password != user.Password)
            {
                ErrorLog = "Incorrect password";
                return;
            }
            else
            {
                IsAuthenticated = true;
                LoginUser = u;
                LoadRecipeBooks();
            }
        }

        private void LoadRecipeBooks()
        {
            RecipeBooks = (ObservableCollection<RecipeBook>)LoginUser.RecipeBooks;
        }
        
        #endregion

    }
}