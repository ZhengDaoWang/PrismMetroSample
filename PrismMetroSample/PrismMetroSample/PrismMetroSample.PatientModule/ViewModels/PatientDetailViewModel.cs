using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using PrismMetroSample.Infrastructure.Events;
using PrismMetroSample.Infrastructure.Models;
using PrismMetroSample.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace PrismMetroSample.PatientModule.ViewModels
{
    public class PatientDetailViewModel : BindableBase
    {
        IEventAggregator _ea;

        private Patient _currentPatient;
        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set { SetProperty(ref _currentPatient, value); }
        }

        private ObservableCollection<Medicine> _lstMedicines;
        public ObservableCollection<Medicine> lstMedicines
        {
            get { return _lstMedicines; }
            set { SetProperty(ref _lstMedicines, value); }
        }

        private DelegateCommand _cancleSubscribeCommand;
        public DelegateCommand CancleSubscribeCommand =>
            _cancleSubscribeCommand ?? (_cancleSubscribeCommand = new DelegateCommand(ExecuteCancleSubscribeCommand));

        void ExecuteCancleSubscribeCommand()
        {
            _ea.GetEvent<MedicineSentEvent>().Unsubscribe(MedicineMessageReceived);
        }

        IMedicineSerivce _medicineSerivce;
        public PatientDetailViewModel(IEventAggregator ea, IMedicineSerivce medicineSerivce)
        {
            _medicineSerivce = medicineSerivce;
            _ea = ea;
            _ea.GetEvent<PatientSentEvent>().Subscribe(PatientMessageReceived);
            _ea.GetEvent<MedicineSentEvent>().Subscribe(MedicineMessageReceived);
        }

        /// <summary>
        /// 接受事件消息函数
        /// </summary>
        private void MedicineMessageReceived(Medicine  medicine)
        {
            this.lstMedicines?.Add(medicine);
        }

        private void PatientMessageReceived(Patient patient)
        {
            this.CurrentPatient = patient;
            this.lstMedicines = new ObservableCollection<Medicine>(_medicineSerivce.GetRecipesByPatientId(this.CurrentPatient.Id).FirstOrDefault().LstMedicines);
        }
    }
}
