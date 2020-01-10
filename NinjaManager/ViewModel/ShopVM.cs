using GalaSoft.MvvmLight.Command;
using NinjaManager.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NinjaManager.ViewModel
{
    public class ShopVM : INotifyPropertyChanged
    {
        private MainViewModel _main;
        private NinjaVM _selectedNinja;
        private bool _canBuy;
        public NinjaVM Ninja
        {
            get
            {
                return _selectedNinja;
            }
            set
            {
                _selectedNinja = value;
                OnPropertyChanged("Ninja");
            }
        }
        public bool CanBuy
        {
            get
            {
                return _canBuy;
            }
            set
            {
                _canBuy = value;
                OnPropertyChanged("CanBuy");
            }
        }
        public bool CanSell
        {
            get
            {
                return _canSell;
            }
            set
            {
                _canSell = value;
                OnPropertyChanged("CanSell");
            }
        }
        public ICommand SelectHead { get; set; }
        public ICommand SelectShoulders { get; set; }
        public ICommand SelectChest { get; set; }
        public ICommand SelectBelt { get; set; }
        public ICommand SelectLegs { get; set; }
        public ICommand SelectBoots { get; set; }
        public ICommand BuyCommand { get; set; }
        public ICommand SellCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<EquipmentVM> _selectedCategoryEquipment;
        private EquipmentVM _selectedEquipment;
        private bool _canSell;

        public ObservableCollection<EquipmentVM> SelectedCategoryEquipment 
        {
            get 
            {
                return _selectedCategoryEquipment;
            }
            set
            {
                _selectedCategoryEquipment = value;
                OnPropertyChanged("SelectedCategoryEquipment");

            } 
        }
        public EquipmentVM SelectedEquipment
        {
            get { return _selectedEquipment; }
            set
            {
                
                _selectedEquipment = value;
                CanBuy = buyCheck();
                CanSell = sellCheck();
                OnPropertyChanged("SelectedCategoryEquipment");
                OnPropertyChanged("SelectedEquipment");

            }
        }
        public ShopVM(MainViewModel main)
        {
            _main = main;
            _selectedNinja = _main.SelectedNinja;
            SelectHead = new RelayCommand(() =>
            {
                changeSelectedCategoryEquipment("Head");
            });
            SelectShoulders = new RelayCommand(() =>
            {
                changeSelectedCategoryEquipment("Shoulders");
            });
            SelectChest = new RelayCommand(() =>
            {
                changeSelectedCategoryEquipment("Chest");
            });
            SelectBelt = new RelayCommand(() =>
            {
                changeSelectedCategoryEquipment("Belt");
            });
            SelectLegs = new RelayCommand(() =>
            {
                changeSelectedCategoryEquipment("Legs");
            });
            SelectBoots = new RelayCommand(() =>
            {
                changeSelectedCategoryEquipment("Boots");
            });
            BuyCommand = new RelayCommand(buy);
            SellCommand = new RelayCommand(sell);
        }
        private void changeSelectedCategoryEquipment(string value)
        {
            using (var context = new NinjaManagerEntities())
            {
                var equipment = context.Equipment.Where(e => e.Category == value).ToList().Select(e => new EquipmentVM(e));
                SelectedCategoryEquipment = new ObservableCollection<EquipmentVM>(equipment);
            }
        }
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        private bool buyCheck()
        {
            if (SelectedEquipment == null)
            {
                return false;
            }
            if(SelectedEquipment.Gold > Ninja.Gold)
            {
                return false;
            }
            foreach(EquipmentVM equipment in Ninja.Inventory)
            {
                if(equipment.Category == SelectedEquipment.Category)
                {
                    return false;
                }
            }
            return true;
        }
        private bool sellCheck()
        {
            if(SelectedEquipment == null)
            {
                return false;
            }
            foreach(EquipmentVM equipment in Ninja.Inventory)
            {
                if(equipment.ToModel().Id == SelectedEquipment.ToModel().Id)
                {
                    return true;
                }
            }
            return false;
        }
        private void buy()
        {
            using (var context = new NinjaManagerEntities())
            {
                int ninjaId = _main.SelectedNinja.ToModel().Id;
                var item = context.Ninja.Include("Equipment").Where(Item => Item.Id == ninjaId).Single();
                Equipment eq = SelectedEquipment.ToModel();
                Equipment equipment = context.Equipment.Where(e => e.Id == eq.Id).Single();
                equipment.Ninja.Add(item);
                item.Gold -= SelectedEquipment.Gold;
                context.SaveChanges();
                
                Ninja = new NinjaVM(item);
                Ninja.Inventory.Add(SelectedEquipment);
                CanBuy = buyCheck();
                CanSell = sellCheck();
            }
        }
        private void sell()
        {
            using (var context = new NinjaManagerEntities())
            {
                int ninjaId = _main.SelectedNinja.ToModel().Id;
                var item = context.Ninja.Include("Equipment").Where(Item => Item.Id == ninjaId).Single();
                Equipment eq = SelectedEquipment.ToModel();
                Equipment equipment = context.Equipment.Where(e => e.Id == eq.Id).Single();
                equipment.Ninja.Remove(item);
                item.Gold += equipment.GoldValue;

                context.SaveChanges();

                Ninja = new NinjaVM(item);
                Ninja.Inventory.Remove(SelectedEquipment);
                CanBuy = buyCheck();
                CanSell = sellCheck();
            }
        }
    }
}
