using Prism.Mvvm;
using Prism.Regions;
using PrismMetroSample.Infrastructure;
using PrismMetroSample.MedicineModule.Views;
using PrismMetroSample.Infrastructure.Constants;
using System.Linq;
using Prism.Commands;
using System.Threading.Tasks;

namespace PrismMetroSample.MedicineModule.ViewModels
{

    public class ShowSearchPatientViewModel : BindableBase
    {
        private IApplicationCommands _applicationCommands;
        private readonly IRegionManager _regionManager;
        private ShowSearchPatient _showSearchPatientView;
        private IRegion _region;

        private bool _isShow=true;
        public bool IsShow
        {
            get { return _isShow=true; }
            set 
            { 
                SetProperty(ref _isShow, value);
                if (_isShow)
                {
                    ActiveShowSearchPatient();
                }
                else
                {
                    DeactiveShowSearchPaitent();
                }
            }
        }

        private DelegateCommand _showSearchLoadingCommand;
        public DelegateCommand ShowSearchLoadingCommand =>
            _showSearchLoadingCommand ?? (_showSearchLoadingCommand = new DelegateCommand(ExecuteShowSearchLoadingCommand));

        void ExecuteShowSearchLoadingCommand()
        {
            _region = _regionManager.Regions[RegionNames.ShowSearchPatientRegion];
            _showSearchPatientView = (ShowSearchPatient)_region.Views.Where(t => t.GetType() == typeof(ShowSearchPatient)).FirstOrDefault();
        }

        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        public ShowSearchPatientViewModel(IApplicationCommands applicationCommands,IRegionManager regionManager)
        {
            this.ApplicationCommands = applicationCommands;
            _regionManager = regionManager;
        }

        private void ActiveShowSearchPatient()
        {
            if (!_region.ActiveViews.Contains(_showSearchPatientView))
            {
                _region.Add(_showSearchPatientView);
            }         
        }

        private async void DeactiveShowSearchPaitent()
        {
            _region.Remove(_showSearchPatientView);
            await Task.Delay(2000);
            IsShow = true;
        }
    }
}
