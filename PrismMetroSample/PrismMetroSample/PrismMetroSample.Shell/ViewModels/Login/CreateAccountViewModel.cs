using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismMetroSample.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PrismMetroSample.Infrastructure.Constants;

namespace PrismMetroSample.Shell.ViewModels.Login
{
    public class CreateAccountViewModel : BindableBase,INavigationAware,IConfirmNavigationRequest
    {

        private string _registeredLoginId;
        public string RegisteredLoginId
        {
            get { return _registeredLoginId; }
            set { SetProperty(ref _registeredLoginId, value); }
        }

        public bool IsUseRequest { get; set; }

        private DelegateCommand _loginMainContentCommand;
        public DelegateCommand LoginMainContentCommand =>
            _loginMainContentCommand ?? (_loginMainContentCommand = new DelegateCommand(ExecuteLoginMainContentCommand));

        private DelegateCommand _goBackCommand;
        public DelegateCommand GoBackCommand =>
            _goBackCommand ?? (_goBackCommand = new DelegateCommand(ExecuteGoBackCommand));

        void ExecuteGoBackCommand()
        {
            _journal.GoBack();
        }

        void ExecuteLoginMainContentCommand()
        {
            Navigate("LoginMainContent");
        }

       

        private DelegateCommand<object> _verityCommand;
        private readonly IRegionManager _regionManager;
        IRegionNavigationJournal _journal;

        public DelegateCommand<object> VerityCommand =>
            _verityCommand ?? (_verityCommand = new DelegateCommand<object>(ExecuteVerityCommand));

        void ExecuteVerityCommand(object parameter)
        {
            if (!VerityRegister(parameter))
            {
                return;
            }
            this.IsUseRequest = true;
            MessageBox.Show("注册成功!");
            //LoginMainContentCommand.Execute();
            _journal.GoBack();
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
            Global.AllUsers.Add(new User()
            {
                Id = Global.AllUsers.Max(t => t.Id) + 1,
                LoginId = this.RegisteredLoginId,
                PassWord = password
            });
            return true;
        }

        public CreateAccountViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate(RegionNames.LoginContentRegion, navigatePath);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //MessageBox.Show("退出了CreateAccount");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //MessageBox.Show("从LoginMainContent导航到CreateAccount");
            _journal = navigationContext.NavigationService.Journal;
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            if (!string.IsNullOrEmpty(RegisteredLoginId) && this.IsUseRequest)
            {
                if (MessageBox.Show("是否需要用当前注册的用户登录?", "Naviagte?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    navigationContext.Parameters.Add("loginId", RegisteredLoginId);
                }
            }
            continuationCallback(true);
            //var result = false;
            //if (MessageBox.Show("是否需要导航到LoginMainContent页面?", "Naviagte?",MessageBoxButton.YesNo) ==MessageBoxResult.Yes)
            //{
            //    result = true;
            //}
            //continuationCallback(result);
        }
    }
}
