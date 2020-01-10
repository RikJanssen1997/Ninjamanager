/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:NinjaManager"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;


namespace NinjaManager.ViewModel
{
    public class ViewModelLocator
    {
        private EquipmentOverviewVM _equipmentOverviewVM;
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            
        }
        

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public CreateNinjaVM CreateNinja
        {
            get 
            {
                return new CreateNinjaVM(Main);
            }
        }
        public EditNinjaVM EditNinja
        {
            get
            {
                return new EditNinjaVM(Main);
            }
        }
        public InventoryVM Inventory
        {
            get
            {
                return new InventoryVM(Main);
            }
        }
        public EquipmentOverviewVM EquipmentOverview
        {
            get
            {

                _equipmentOverviewVM = new EquipmentOverviewVM();
                return _equipmentOverviewVM;
            }
        }

        public CreateEquipmentVM CreateEquipment
        {
            get
            {
                return new CreateEquipmentVM(_equipmentOverviewVM);
            }
        }
        public EditEquipmentVM EditEquipmentVM
        {
            get
            {
                return new EditEquipmentVM(_equipmentOverviewVM);
            }
        }
        public ShopVM Shop
        {
            get
            {
                return new ShopVM(Main);
            }
        }
        public static void Cleanup()
        {

        }
    }
}