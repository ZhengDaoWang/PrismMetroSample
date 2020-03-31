using Prism.Mvvm;
using PrismMetroSample.Infrastructure.Models;
using PrismMetroSample.Infrastructure.Services;
using System.Collections.ObjectModel;
using Prism.Events;
using PrismMetroSample.Infrastructure.Events;
using System;
using Prism;
using System.Windows;

namespace PrismMetroSample.MedicineModule.ViewModels
{
    public class MedicineMainContentViewModel : BindableBase,IActiveAware
    {
        IMedicineSerivce _medicineSerivce;
        IEventAggregator _ea;

        private ObservableCollection<Medicine> _allMedicines;

        public event EventHandler IsActiveChanged;

        public ObservableCollection<Medicine> AllMedicines
        {
            get { return _allMedicines; }
            set { SetProperty(ref _allMedicines, value); }
        }

        bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                if (_isActive)
                {
                    MessageBox.Show("视图被激活了");
                }
                else
                {
                    MessageBox.Show("视图失效了");
                }
                IsActiveChanged?.Invoke(this, new EventArgs());
            }
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
