using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismMetroSample.Infrastructure.Constants;
using PrismMetroSample.Infrastructure.Services;

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
            _regionManager.RequestNavigate(RegionNames.LoginContentRegion, "LoginMainContent");
            Global.AllUsers = _userService.GetAllUsers();
        }

        public LoginWindowViewModel(IRegionManager regionManager, IUserService userService)
        {
            _regionManager = regionManager;
            _userService = userService;
            
        }




    }
}
