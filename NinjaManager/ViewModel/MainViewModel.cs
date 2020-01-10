using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NinjaManager.Domain;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace NinjaManager.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private EquipmentOverviewWindow _equipmentOverviewWindow;
        private NinjaVM _selectedNinja;
        public ICommand ShowCreateNinjaCommand { get; set;}
        public ICommand DeleteNinjaCommand { get; set; }
        public ICommand EditNinjaCommand { get; set; }
        public ICommand InventoryCommand { get; set; }
        public RelayCommand ShopCommand { get; set; }
        public ICommand ShowEquipmentCommand { get; set; }

        public ObservableCollection<NinjaVM> Ninjas { get; set; }

        public NinjaVM SelectedNinja
        {
            get { return _selectedNinja; }
            set { _selectedNinja = value;
                base.RaisePropertyChanged();
            }
        }
        public MainViewModel()
        {
            using(var context = new NinjaManagerEntities())
            {
                var ninjas = context.Ninja.Include("Equipment").ToList().Select(n => new NinjaVM(n));
                Ninjas = new ObservableCollection<NinjaVM>(ninjas);
            }
            ShowCreateNinjaCommand = new RelayCommand(() =>
            {
                new CreateNinjaWindow().Show();
            });
            EditNinjaCommand = new RelayCommand(() =>
            {
                new EditNinjaWindow().Show();
            });
            InventoryCommand = new RelayCommand(() =>
            {
                reloadInventory();
                new InventoryWindow().Show();
            });
            ShopCommand = new RelayCommand(() =>
            {
                reloadInventory();
                new ShopWindow().Show();
            });
            DeleteNinjaCommand = new RelayCommand(deleteNinja);
            ShowEquipmentCommand = new RelayCommand(showEquipment, canShowEquipment);
        }
        private void showEquipment()
        {
            _equipmentOverviewWindow = new EquipmentOverviewWindow();
            _equipmentOverviewWindow.Show();
        }
        private bool canShowEquipment()
        {
            if(_equipmentOverviewWindow == null || !_equipmentOverviewWindow.IsVisible)
            {
                return true;
            }
            return false;
        }
        private void deleteNinja()
        {
            using (var context = new NinjaManagerEntities())
            {
                //delete ninja from ninja table
                
                int ninjaId = _selectedNinja.ToModel().Id;
                
                var item = context.Ninja.Where(Item => Item.Id == ninjaId).Single();
                
                context.Ninja.Remove(item);
                context.SaveChanges();
                Ninjas.Remove(_selectedNinja);
            }
        }
        private void reloadInventory()
        {
            using(var context = new NinjaManagerEntities())
            {
                int ninjaID = SelectedNinja.ToModel().Id;
                var ninja = context.Ninja.Include("Equipment").Where(n => n.Id == ninjaID).Single();
                _selectedNinja = new NinjaVM(ninja);

            }
        }
    }
}