using Prism.Commands;
using Prism.Mvvm;
using PrismMetroSample.Infrastructure.Models;
using PrismMetroSample.Infrastructure.Services;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using Prism.Events;
using PrismMetroSample.Infrastructure.Events;
using System;

namespace PrismMetroSample.MedicineModule.ViewModels
{
    public class MedicineMainContentViewModel : BindableBase
    {
        IMedicineSerivce _medicineSerivce;
        IEventAggregator _ea;

        private ObservableCollection<Medicine> _allMedicines;
        public ObservableCollection<Medicine> AllMedicines
        {
            get { return _allMedicines; }
            set { SetProperty(ref _allMedicines, value); }
        }
        public MedicineMainContentViewModel(IMedicineSerivce medicineSerivce,IEventAggregator ea)
        {
            _medicineSerivce = medicineSerivce;
            _ea = ea;
            this.AllMedicines = new ObservableCollection<Medicine>(_medicineSerivce.GetAllMedicines());
            _ea.GetEvent<MedicineSentEvent>().Subscribe(MedicineMessageReceived);//订阅事件
        }

        /// <summary>
        /// 事件消息接受函数
        /// </summary>
        private void MedicineMessageReceived(Medicine medicine)
        {
            this.AllMedicines?.Add(medicine);
        }
    }
}
