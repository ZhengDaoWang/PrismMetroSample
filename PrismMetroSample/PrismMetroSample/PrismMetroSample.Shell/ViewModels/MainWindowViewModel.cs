using Prism.Commands;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PrismMetroSample.Shell.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        IModuleManager _moduleManager;
        public MainWindowViewModel(IModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
            _moduleManager.LoadModuleCompleted += _moduleManager_LoadModuleCompleted;
        }

        private void _moduleManager_LoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            MessageBox.Show($"{e.ModuleInfo.ModuleName}模块被加载了");
        }

        private DelegateCommand _loadPatientModuleCommand;
        public DelegateCommand LoadPatientModuleCommand =>
            _loadPatientModuleCommand ?? (_loadPatientModuleCommand = new DelegateCommand(ExecuteLoadPatientModuleCommand));

        void ExecuteLoadPatientModuleCommand()
        {
            _moduleManager.LoadModule("MedicineModule");
        }
    }
}
