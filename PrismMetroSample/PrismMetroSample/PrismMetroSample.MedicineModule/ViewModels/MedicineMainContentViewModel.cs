using Prism.Mvvm;
using PrismMetroSample.Infrastructure.Models;
using PrismMetroSample.Infrastructure.Services;
using System.Collections.ObjectModel;
using Prism.Events;
using PrismMetroSample.Infrastructure.Events;
using System;
using Prism;
using System.Windows;
using Prism.Services.Dialogs;
using Prism.Commands;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PrismMetroSample.MedicineModule.ViewModels
{
    public class MedicineMainContentViewModel : BindableBase,IActiveAware
    {
        #region Fields

        private readonly IMedicineSerivce _medicineSerivce;
        private readonly IEventAggregator _ea;
        private readonly IDialogService _dialogService;

        public event EventHandler IsActiveChanged;

        #endregion

        #region Properties


        private ObservableCollection<Medicine> _allMedicines=new ObservableCollection<Medicine>();

        public ObservableCollection<Medicine> AllMedicines
        {
            get { return _allMedicines; }
            set { _allMedicines = value; }
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
                    _dialogService.ShowDialog("SuccessDialog", new DialogParameters($"message={"视图被激活了"}"), null);
                }
                else
                {
                    _dialogService.ShowDialog("WarningDialog", new DialogParameters($"message={"视图失效了"}"), null);
                }
                IsActiveChanged?.Invoke(this, new EventArgs());
            }
        }

        #endregion

        #region Commands

        private DelegateCommand _loadCommand;
        public DelegateCommand LoadCommand =>
            _loadCommand ?? (_loadCommand = new DelegateCommand(ExecuteLoadCommand));

         void ExecuteLoadCommand()
        {
            //TaskExtension for async void Command 
            ALongTask().Await( completedCallback:() =>
            {
                this.AllMedicines.AddRange(_medicineSerivce.GetAllMedicines());
            }, errorCallback:null,configureAwait:true);


        }

        private async Task ALongTask()
        {
            await Task.Delay(3000);//模拟耗时操作
            Debug.WriteLine("耗时操作完成");
        }

        #endregion

        #region  Excutes



        #endregion



        public MedicineMainContentViewModel(IMedicineSerivce medicineSerivce,IEventAggregator ea,IDialogService dialogService)
        {
            _medicineSerivce = medicineSerivce;
            _ea = ea;
            _dialogService = dialogService;           
            _ea.GetEvent<MedicineSentEvent>().Subscribe(MedicineMessageReceived);//订阅事件
            this.AllMedicines = new ObservableCollection<Medicine>();
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
