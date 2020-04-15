using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismMetroSample.Infrastructure.Constants;
using PrismMetroSample.Infrastructure.Models;
using PrismMetroSample.Infrastructure.Services;
using PrismMetroSample.Shell.Views;
using PrismMetroSample.Shell.Views.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrismMetroSample.Shell.ViewModels.Login
{
    public class LoginMainContentViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {


        IRegionNavigationJournal _journal;
        private readonly IRegionManager _regionManager;



        private bool _isCanExcute;
        public bool IsCanExcute
        {
            get { return _isCanExcute; }
            set { SetProperty(ref _isCanExcute, value); }
        }

        private User _currentUser = new User();
        public User CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        private DelegateCommand _createAccountCommand;
        public DelegateCommand CreateAccountCommand =>
            _createAccountCommand ?? (_createAccountCommand = new DelegateCommand(ExecuteCreateAccountCommand));


        private DelegateCommand _goForwardCommand;
        public DelegateCommand GoForwardCommand =>
            _goForwardCommand ?? (_goForwardCommand = new DelegateCommand(ExecuteGoForwardCommand));

        private DelegateCommand<PasswordBox> _loginCommand;
        public DelegateCommand<PasswordBox> LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand<PasswordBox>(ExecuteLoginCommand, CanExecuteGoForwardCommand));

        public bool KeepAlive => true;

        void ExecuteCreateAccountCommand()
        {
            Navigate("CreateAccount");
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate(RegionNames.LoginContentRegion, navigatePath);
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
            else if (Global.AllUsers.Where(t => t.LoginId == this.CurrentUser.LoginId && t.PassWord == this.CurrentUser.PassWord).Count() == 0)
            {
                MessageBox.Show("LoginId 或者 PassWord 错误!");
                return;
            }
            ShellSwitcher.Switch<LoginWindow, MainWindow>();
        }

       private void ExecuteGoForwardCommand()
        {
            _journal.GoForward();
        }

       private bool CanExecuteGoForwardCommand(PasswordBox passwordBox)
        {
            this.IsCanExcute=_journal != null && _journal.CanGoForward;
            return true;
        }


        public LoginMainContentViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //MessageBox.Show("退出了LoginMainContent");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //MessageBox.Show("从CreateAccount导航到LoginMainContent");
            _journal = navigationContext.NavigationService.Journal;

            var loginId= navigationContext.Parameters["loginId"] as string;
            if (loginId!=null)
            {
                this.CurrentUser = new User() { LoginId=loginId};
            }
            LoginCommand.RaiseCanExecuteChanged();
        }
    }
}
