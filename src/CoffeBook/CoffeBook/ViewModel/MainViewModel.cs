using BL;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BL.DTOs;
using CoffeBook.Model;
using CoffeBook.Helpers;

namespace CoffeBook.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private User loginUser;
        private string errorLog;
        private ObservableCollection<Recipe> recipes;
        private ObservableCollection<Coffee> coffees;

        public ICommand RegisterButtonCommand { get; private set; }
        public ICommand LoginButtonCommand { get; private set; }
        public ICommand LogoutButtonCommand { get; private set; }
        public ICommand ShowPropertiesButtonCommand { get; private set; }
        public ICommand ClosePropertiesButtonCommand { get; private set; }
        public ICommand CancelPropertiesButtonCommand { get; private set; }
        public ICommand DeletePropertiesButtonCommand { get; private set; }

        private ObservableCollection<RecipeBook> recipebooks;

        private bool isAuthenticated;
        private bool showProperties;
        private string propertiesParameter;

        private object propertiesObject;

        private string propertiesTitle;

        public string PropertiesTitle
        {
            get { return propertiesTitle; }
            set
            {
                if (value == propertiesTitle)
                    return;
                propertiesTitle = value;
                RaisePropertyChanged("PropertiesTitle");
            }
        }

        private string propertiesName;

        public string PropertiesName
        {
            get { return propertiesName; }
            set
            {
                if (value == propertiesName)
                    return;
                propertiesName = value;
                RaisePropertyChanged("PropertiesName");
            }
        }

        private string propertiesDescription;

        public string PropertiesDescription
        {
            get { return propertiesDescription; }
            set
            {
                if (value == propertiesDescription)
                    return;
                propertiesDescription = value;
                RaisePropertyChanged("PropertiesDescription");
            }
        }

        private List<string> propertiesRecipes;

        public List<string> PropertiesRecipes
        {
            get { return propertiesRecipes; }
            set
            {
                propertiesRecipes = value;
                RaisePropertyChanged("PropertiesRecipes");
            }
        }

        private bool showPropertiesRecipes;

        public bool ShowPropertiesRecipes
        {
            get { return showPropertiesRecipes; }
            set
            {
                if (value == showPropertiesRecipes)
                    return;
                showPropertiesRecipes = value;
                RaisePropertyChanged("ShowPropertiesRecipes");
            }
        }

        private bool showPropertiesDescription;

        public bool ShowPropertiesDescription
        {
            get { return showPropertiesDescription; }
            set
            {
                if (value == showPropertiesDescription)
                    return;
                showPropertiesDescription = value;
                RaisePropertyChanged("ShowPropertiesRecipes");
            }
        }


        private List<string> propertiesCoffees;

        public List<string> PropertiesCoffees
        {
            get { return propertiesCoffees; }
            set
            {
                propertiesCoffees = value;
                RaisePropertyChanged("PropertiesCoffees");
            }
        }

        private bool showPropertiesCoffees;

        public bool ShowPropertiesCoffees
        {
            get { return showPropertiesCoffees; }
            set
            {
                if (value == showPropertiesCoffees)
                    return;
                showPropertiesCoffees = value;
                RaisePropertyChanged("ShowPropertiesCoffees");
            }
        }


        public MainViewModel()
        {
            LoginUser = new User { Name = "", Password = "" };
            errorLog = "";
            recipebooks = new ObservableCollection<RecipeBook>();
            recipes = new ObservableCollection<Recipe>();
            coffees = new ObservableCollection<Coffee>();
            isAuthenticated = false;
            showProperties = false;
            propertiesParameter = "";

            RegisterButtonCommand = new RelayCommand<object>(Register);
            LoginButtonCommand = new RelayCommand<object>(Login);
            LogoutButtonCommand = new RelayCommand<object>(Logout);
            ShowPropertiesButtonCommand = new RelayCommand<object>(ShowPropertiesCommand);
            CancelPropertiesButtonCommand = new RelayCommand(CancelPropertiesCommand);
            ClosePropertiesButtonCommand = new RelayCommand(ClosePropertiesCommand);
            DeletePropertiesButtonCommand = new RelayCommand(DeletePropertiesCommand);


            LoadRecipes();
            LoadCoffees();

        }

        private void LoadRecipes()
        {
            Recipes = (ObservableCollection<Recipe>) RecipeHelper.GetRecipes().Result;
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
                if (value == LoginUser.Name)
                    return;
                LoginUser.Name = value;
                RaisePropertyChanged("InputName");
            }
        }

        public ObservableCollection<Recipe> Recipes
        {
            get { return recipes; }
            set
            {
                recipes.Clear();
                foreach (Recipe r in value)
                {
                    recipes.Add(r);
                }
                RaisePropertyChanged("Recipes");
            }
        }

        public ObservableCollection<Coffee> Coffees
        {
            get { return coffees; }
            set
            {
                coffees.Clear();
                foreach (Coffee c in value)
                {
                    coffees.Add(c);
                }
                RaisePropertyChanged("Coffees");
            }
        }

        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set
            {
                if (value == isAuthenticated)
                    return;
                isAuthenticated = value;
                RaisePropertyChanged("IsAuthenticated");
            }
        }

        public bool IsNotAuthenticated
        {
            get { return !isAuthenticated; }
        }

        public bool ShowProperties
        {
            get { return showProperties; }
            set
            {
                if (value == showProperties)
                    return;
                showProperties = value;
                RaisePropertyChanged("ShowProperties");
            }
        }

        public string PropertiesParameter
        {
            get { return propertiesParameter; }
            set
            {
                if (value == propertiesParameter)
                    return;
                propertiesParameter = value;
                RaisePropertyChanged("PropertiesParameter");
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

        public void Register(object obj)
        {
            PasswordBox pwBox = obj as PasswordBox;

            var valid = ValidateInput(pwBox.Password);
            if (!valid)
                return;
            var user_result = UserHelper.GetUser(LoginUser.Name).Result;
            if (user_result == null)
            {
                UserHelper.RegisterUser(LoginUser);
                Authenticate(LoginUser.Name, pwBox.Password);
            }
            else
            {
                ErrorLog = "User name already exists!";
                return;
            }
        }

        public void Login(object obj)
        {
            PasswordBox pwBox = obj as PasswordBox;

            var valid = ValidateInput(pwBox.Password);
            if (!valid)
                return;

            var user_result = UserHelper.GetUser(LoginUser.Name).Result;
            if (user_result == null)
            {
                ErrorLog = "User not found!";
                return;
            }
            Authenticate(LoginUser.Name, pwBox.Password);
        }

        private bool ValidateInput(string password)
        {
            if (string.IsNullOrWhiteSpace(InputName))
            {
                ErrorLog = "Username field is empty or the input is invalid!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(password))
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

        private void Authenticate(string userName, string password)
        {
            User u = UserHelper.GetUser(userName).Result;
            if (u.Password != password)
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

        private void Logout(object obj)
        {
            InputName = "";
            PasswordBox pwBox = obj as PasswordBox;
            pwBox.Password = "";
            IsAuthenticated = false;
        }

        private void LoadRecipeBooks()
        {
            RecipeBooks = (ObservableCollection<RecipeBook>)LoginUser.RecipeBooks;
        }

        public void ShowPropertiesCommand(object obj)
        {
            if (obj is string)
            {
                string parameter = obj as string;

                switch (parameter)
                {
                    case "AddRecipeBook":
                        NewRecipeBook();
                        break;
                    case "AddRecipe":
                        NewRecipe();
                        break;
                    case "NewCoffee":
                        NewCoffee();
                        break;
                }

            }
            else
            {
                EditProperties(obj);
            }
            ShowProperties = true;
        }

        public void NewRecipeBook()
        {
            propertiesObject = new RecipeBook();
            PropertiesTitle = "Add new recipe book";
            PropertiesName = "";
            PropertiesDescription = "";

            List<string> rs = new List<string>();
            foreach (Recipe r in recipes)
            {
                rs.Add(r.Name);
            }

            PropertiesRecipes = rs;
            ShowPropertiesDescription = true;
            ShowPropertiesRecipes = true;
            ShowPropertiesCoffees = false;
        }

        public void NewRecipe()
        {
            propertiesObject = new Recipe();
            PropertiesTitle = "Add new recipe";
            PropertiesName = "";
            PropertiesDescription = "";

            List<string> cs = new List<string>();
            foreach (Coffee c in CoffeeHelper.GetCoffees().Result)
            {
                cs.Add(c.Name);
            }

            PropertiesCoffees = cs;
            ShowPropertiesDescription = true;
            ShowPropertiesRecipes = false;
            ShowPropertiesCoffees = true;
        }

        public void NewCoffee()
        {
            propertiesObject = new Coffee();
            PropertiesTitle = "Add new coffee";
            PropertiesName = "";

            ShowPropertiesDescription = false;
            ShowPropertiesRecipes = false;
            ShowPropertiesCoffees = false;
        }

        private void EditProperties(object obj)
        {
            propertiesObject = obj;
            if (obj is RecipeBook)
            {
                EditRecipeBook(obj as RecipeBook);
            }
            else if (obj is Recipe)
            {
                EditRecipe(obj as Recipe);
            }
            else if (obj is Coffee)
            {
                EditCoffee(obj as Coffee);
            }
        }

        private void EditCoffee(Coffee coffee)
        {
            PropertiesTitle = "Edit Coffee";
            PropertiesName = coffee.Name == null ? "" : coffee.Name;
            
            ShowPropertiesDescription = false;
            ShowPropertiesRecipes = false;
            ShowPropertiesCoffees = false;
        }

        private void EditRecipe(Recipe recipe)
        {
            PropertiesTitle = "Edit Recipe";
            PropertiesName = recipe.Name == null ? "" : recipe.Name;
            PropertiesDescription = recipe.Description == null ? "" : recipe.Description;

            List<string> cs = new List<string>();
            foreach (Coffee c in CoffeeHelper.GetCoffees().Result)
            {
                cs.Add(c.Name);
            }

            if (recipe.CoffeType != null)
            {
                cs.Remove(recipe.CoffeType.Name);
                cs.Insert(0, recipe.CoffeType.Name);
            }

            PropertiesCoffees = cs;

            ShowPropertiesDescription = true;
            ShowPropertiesRecipes = false;
            ShowPropertiesCoffees = true;
        }
        private void EditRecipeBook(RecipeBook recipeBook)
        {
            PropertiesTitle = "Edit Recipe Book";
            PropertiesName = recipeBook.Name == null ? "" : recipeBook.Name;
            PropertiesDescription = recipeBook.Description == null ? "" : recipeBook.Description;

            List<string> rs = new List<string>();
            foreach (Recipe r in recipes)
            {
                rs.Add(r.Name);
            }

            if (recipeBook.Recipes != null && recipeBook.Recipes.Count > 0)
            {
                foreach (var r in recipeBook.Recipes)
                {
                    rs.Remove(r.Name);
                    rs.Insert(0, r.Name);
                }
            }

            PropertiesRecipes = rs;
            ShowPropertiesDescription = true;
            ShowPropertiesRecipes = true;
            ShowPropertiesCoffees = false;
        }


        private void DeletePropertiesCommand()
        {
            if (propertiesObject is RecipeBook)
            {
                RecipeBookHelper.RemoveRecipeBook(propertiesObject as RecipeBook);
                LoadRecipeBooks();
            }
            else if (propertiesObject is Recipe)
            {
                RecipeHelper.RemoveRecipe(propertiesObject as Recipe);
                LoadRecipes();
            }
            else if (propertiesObject is Coffee)
            {
                CoffeeHelper.RemoveCoffee(propertiesObject as Coffee);
                LoadCoffees();
            }
            ShowProperties = false;
        }

        private void LoadCoffees()
        {
            Coffees = (ObservableCollection<Coffee>) CoffeeHelper.GetCoffees().Result;
        }

        private void ClosePropertiesCommand()
        {
            if (propertiesObject is RecipeBook)
            {
                SaveRecipeBook();
            }
            else if (propertiesObject is Recipe)
            {
                SaveRecipe();
            }
            else if (propertiesObject is Coffee)
            {
                SaveCoffee();
            }
            ShowProperties = false;
        }

        private async void SaveCoffee()
        {
            Coffee coffee = propertiesObject as Coffee;
            coffee.Name = PropertiesName;
            await CoffeeHelper.AddCoffee(coffee);

        }

        private async void SaveRecipe()
        {
            Recipe recipe = propertiesObject as Recipe;
            recipe.Name = PropertiesName;
            recipe.Description = PropertiesDescription;
            // TODO coffetype
            await RecipeHelper.AddRecipe(recipe);
        }

        private async void SaveRecipeBook()
        {
            RecipeBook recipeBook = propertiesObject as RecipeBook;
            recipeBook.Name = PropertiesName;
            recipeBook.Description = PropertiesDescription;
            // TODO recipes!
            await RecipeBookHelper.AddRecipeBook(recipeBook);
            LoginUser.RecipeBooks.Add(recipeBook);
        }

        private void CancelPropertiesCommand()
        {
            ShowProperties = false;
        }

        #endregion
    }
}