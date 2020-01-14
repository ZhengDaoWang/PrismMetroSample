using Prism.Commands;
using Prism.Mvvm;
using PrismMetroSample.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismMetroSample.MedicineModule.ViewModels
{
    
    public class ShowSearchPatientViewModel : BindableBase
    {
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        public ShowSearchPatientViewModel(IApplicationCommands applicationCommands)
        {
            this.ApplicationCommands = applicationCommands;
        }
    }
}
