using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismMetroSample.Infrastructure.Constants;
using PrismMetroSample.Infrastructure.Services;
using System.Threading;
using System.Windows;

namespace PrismMetroSample.Shell.ViewModels.Login
{
    public class LoginWindowViewModel:BindableBase
    {

        private readonly IRegionManager _regionManager;
        private readonly IUserService _userService;
        private DelegateCommand _loginLoadingCommand;
        public DelegateCommand LoginLoadingCommand =>
            _loginLoadingCommand ?? (_loginLoadingCommand = new DelegateCommand(ExecuteLoginLoadingCommand));

        void ExecuteLoginLoadingCommand()
        {
            //_regionManager.RequestNavigate(RegionNames.LoginContentRegion, "LoginMainContent");
            IRegion region = _regionManager.Regions[RegionNames.LoginContentRegion];
            region.RequestNavigate("LoginMainContent", NavigationCompelted);
            Global.AllUsers = _userService.GetAllUsers();
        }

        private void NavigationCompelted(NavigationResult result)
        {
            if (result.Result==true)
            {
                MessageBox.Show("导航到LoginMainContent页面成功");
            }
            else
            {
                MessageBox.Show("导航到LoginMainContent页面失败");
            }
        }

        public LoginWindowViewModel(IRegionManager regionManager, IUserService userService)
        {
            _regionManager = regionManager;
            _userService = userService;
            
        }

    }
}
