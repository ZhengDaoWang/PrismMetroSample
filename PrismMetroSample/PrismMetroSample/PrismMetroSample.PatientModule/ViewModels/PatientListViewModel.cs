using Prism.Commands;
using Prism.Mvvm;
using PrismMetroSample.Infrastructure.Services;
using PrismMetroSample.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Regions;
using PrismMetroSample.Infrastructure;
using Prism.Events;
using PrismMetroSample.Infrastructure.Events;
using PrismMetroSample.Infrastructure.Constants;
using PrismMetroSample.PatientModule.Views;

namespace PrismMetroSample.PatientModule.ViewModels
{
    public class PatientListViewModel : BindableBase
    {
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        private List<Patient> _allPatients;
        public List<Patient> AllPatients
        {
            get { return _allPatients; }
            set { SetProperty(ref _allPatients, value); }
        }

        private DelegateCommand<Patient> _mouseDoubleClickCommand;
        public DelegateCommand<Patient> MouseDoubleClickCommand =>
            _mouseDoubleClickCommand ?? (_mouseDoubleClickCommand = new DelegateCommand<Patient>(ExecuteMouseDoubleClickCommand));

        IEventAggregator _ea;
        private readonly IRegionManager _regionManager;
        IPatientService _patientService;
        private IRegion _region;
        private PatientList _patientListView;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PatientListViewModel(IPatientService patientService, IEventAggregator ea, IApplicationCommands applicationCommands)
        {
            _ea = ea;
            this.ApplicationCommands = applicationCommands;
            _patientService = patientService;
            this.AllPatients = _patientService.GetAllPatients();         
        }

        /// <summary>
        /// DataGrid 双击按钮命令方法
        /// </summary>
        void ExecuteMouseDoubleClickCommand(Patient patient)
        {
            this.ApplicationCommands.ShowCommand.Execute(FlyoutNames.PatientDetailFlyout);//打开窗体
            _ea.GetEvent<PatientSentEvent>().Publish(patient);//发布消息
        }

    }
}
