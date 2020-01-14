using Prism.Ioc;
using Prism.Unity;
using PrismMetroSample.Shell.Views;
using Prism.Modularity;
using System;
using System.Windows;
using PrismMetroSample.Infrastructure.Services;
using PrismMetroSample.Infrastructure;

namespace PrismMetroSample.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IMedicineSerivce, MedicineSerivce>();
            containerRegistry.Register<IPatientService, PatientService>();

            //注册全局命令
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
            containerRegistry.RegisterInstance<IFlyoutService>(Container.Resolve<FlyoutService>());
        }

        //protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        //{
        //    moduleCatalog.AddModule<PrismMetroSample.PatientModule.PatientModule>();
        //    var MedicineModuleType = typeof(PrismMetroSample.MedicineModule.MedicineModule);
        //    moduleCatalog.AddModule(new ModuleInfo()
        //    {
        //        ModuleName= MedicineModuleType.Name,
        //        ModuleType=MedicineModuleType.AssemblyQualifiedName,
        //        InitializationMode=InitializationMode.OnDemand
        //    });

        //}

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
            //return new ConfigurationModuleCatalog();
        }
    }
}
