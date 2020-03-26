using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismMetroSample.Infrastructure.Models;
using PrismMetroSample.Infrastructure.Services;
using PrismMetroSample.Shell.Views;
using PrismMetroSample.Shell.Views.Login;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrismMetroSample.Shell.ViewModels.Login
{
    public class LoginWindowViewModel:BindableBase
    {
        private DelegateCommand<PasswordBox> _loginCommand;
        public DelegateCommand<PasswordBox> LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand<PasswordBox>(ExecuteLoginCommand));

        private User _currentUser=new User();
        public User CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        private List<User> _allUsers;
        public List<User> AllUsers
        {
            get { return _allUsers; }
            set { SetProperty(ref _allUsers, value); }
        }

        private string _registeredLoginId;
        public string RegisteredLoginId
        {
            get { return _registeredLoginId; }
            set { SetProperty(ref _registeredLoginId, value); }
        }

        private DelegateCommand _loginMainContentCommand;
        public DelegateCommand LoginMainContentCommand =>
            _loginMainContentCommand ?? (_loginMainContentCommand = new DelegateCommand(ExecuteLoginMainContentCommand));

        void ExecuteLoginMainContentCommand()
        {
            Navigate("LoginMainContent");
        }

        private DelegateCommand<object> _verityCommand;
        public DelegateCommand<object> VerityCommand =>
            _verityCommand ?? (_verityCommand = new DelegateCommand<object>(ExecuteVerityCommand));

        void ExecuteVerityCommand(object parameter)
        {
            if (!VerityRegister(parameter))
            {
                return;
            }
            LoginMainContentCommand.Execute();
            MessageBox.Show("注册成功!");
        }

        private bool VerityRegister(object parameter)
        {
            if (string.IsNullOrEmpty(this.RegisteredLoginId))
            {
                MessageBox.Show("LoginId 不能为空！");
                return false;
            }
            var passwords = parameter as Dictionary<string, PasswordBox>;
            var password = (passwords["Password"] as PasswordBox).Password;
            var confimPassword = (passwords["ConfirmPassword"] as PasswordBox).Password;
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password 不能为空！");
                return false;
            }
            if (string.IsNullOrEmpty(confimPassword))
            {
                MessageBox.Show("ConfirmPassword 不能为空！");
                return false;
            }
            if (password.Trim() != confimPassword.Trim())
            {
                MessageBox.Show("两次密码不一致");
                return false;
            }
            this.AllUsers.Add(new User()
            {
                Id = this.AllUsers.Max(t => t.Id) + 1,
                LoginId = this.RegisteredLoginId,
                PassWord = password
            }) ; 
            return true;
        }

        void ExecuteLoginCommand(PasswordBox passwordBox)
        {
            if (string.IsNullOrEmpty(this.CurrentUser.LoginId))
            {
                MessageBox.Show("LoginId 不能为空!");
                return;
            }
            this.CurrentUser.PassWord = passwordBox.Password;
            if (string.IsNullOrEmpty(this.CurrentUser.PassWord))
            {
                MessageBox.Show("PassWord 不能为空!");
                return;
            }
            else if (this.AllUsers.Where(t => t.LoginId == this.CurrentUser.LoginId && t.PassWord == this.CurrentUser.PassWord).Count() == 0)
            {
                MessageBox.Show("LoginId 或者 PassWord 错误!");
                return;
            }
            ShellSwitcher.Switch<LoginWindow, MainWindow>();
        }

        private DelegateCommand _createAccountCommand;
        public DelegateCommand CreateAccountCommand =>
            _createAccountCommand ?? (_createAccountCommand = new DelegateCommand(ExecuteCreateAccountCommand));

        void ExecuteCreateAccountCommand()
        {
            Navigate("CreateAccount");
        }

        private DelegateCommand _loginLoadingCommand;
        public DelegateCommand LoginLoadingCommand =>
            _loginLoadingCommand ?? (_loginLoadingCommand = new DelegateCommand(ExecuteLoginLoadingCommand));

        void ExecuteLoginLoadingCommand()
        {
            this.LoginMainContentCommand.Execute();
        }

        private readonly IRegionManager _regionManager;
        private readonly IUserService _userService;

        public LoginWindowViewModel(IRegionManager regionManager, IUserService userService)
        {
            _regionManager = regionManager;
            _userService = userService;
            this.AllUsers = _userService.GetAllUsers();
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("LoginContentRegion", navigatePath);
        }
    
}
}
