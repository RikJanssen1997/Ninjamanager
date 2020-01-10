using GalaSoft.MvvmLight.Command;
using NinjaManager.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NinjaManager.ViewModel
{
    public class InventoryVM : INotifyPropertyChanged
    {
        private MainViewModel _main;

        private EquipmentVM _head;
        private EquipmentVM _chest;
        private EquipmentVM _shoulders;
        private EquipmentVM _belt;
        private EquipmentVM _legs;
        private EquipmentVM _boots;
        private string _headString;
        private string _chestString;
        private string _shouldersString;
        private string _beltString;
        private string _legString;
        private string _bootsString;
        private string _totalStatString;
        private string _totalGoldString;
        private int totalsStrength = 0;
        private int totalsIntelligence = 0;
        private int totalsAgillity = 0;
        private int totalGold = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand DeleteAllEquipment { get; set; }
        public NinjaVM Ninja { get; set; }
        public string Values
        {
            get
            {
                return _totalStatString;

            }
            set
            {
                _totalStatString = value;
                OnPropertyChanged("Values");
            }
            
        }
        
        public string TotalGold
        {
            get
            {
                return _totalGoldString;
            }
            set
            {
                _totalGoldString = value;
                OnPropertyChanged("TotalGold");
            }
        }
        public string Head 
        {
            get 
            {
                return _headString;
            }
            set
            {
                _headString = value;

                OnPropertyChanged("Head");
            }
        }
        public string Chest
        {
            get
            {
                return _chestString;
            }
            set
            {
                 _chestString = value;

                OnPropertyChanged("Chest");
            }


        }
        public string Shoulders
        {
            get
            {
                return _shouldersString;
            }
            set
            {
                _shouldersString = value;
                OnPropertyChanged("Shoulders");
            }
        }
        public string Belt
        {
            get
            {
                return _beltString;
            }
            set
            {
                _beltString = value;
                OnPropertyChanged("Belt");
            }

        }
        public string Legs
        {
            get
            {
                return _legString;
            }
            set
            {
                _legString = value;
                OnPropertyChanged("Legs");
            }
        }
        public string Boots
        {
            get
            {
                return _bootsString;
            }
            set
            {
                _bootsString = value;
                OnPropertyChanged("Boots");
            }
        }

        public InventoryVM(MainViewModel main)
        {
            _main = main;
            Ninja = _main.SelectedNinja;
            DeleteAllEquipment = new RelayCommand(deleteAllEquipment);
            foreach (EquipmentVM equipmentVM in Ninja.Inventory)
            {
                totalsStrength += equipmentVM.Strenght;
                totalsIntelligence += equipmentVM.Intelligence;
                totalsAgillity += equipmentVM.Agility;
                totalGold += equipmentVM.Gold;
                if (equipmentVM.Category == "Head")
                {
                    _head = equipmentVM;
                    
                }
                if (equipmentVM.Category == "Chest")
                {
                    _chest = equipmentVM;
                    
                }
                if (equipmentVM.Category == "Shoulders")
                {
                    _shoulders = equipmentVM;
                }
                if (equipmentVM.Category == "Legs")
                {
                    _legs = equipmentVM;
                    
                }
                if (equipmentVM.Category == "Belt")
                {
                    _belt = equipmentVM;
                    
                }
                if (equipmentVM.Category == "Boots")
                {
                    _boots = equipmentVM;
                    
                }
                
            }
                Head = output(_head);
                Chest = output(_chest);
                Shoulders = output(_shoulders);
                Legs = output(_legs);
                Belt = output(_belt);
                Boots = output(_boots);
                TotalGold = updateGold();
                Values = updateStats();
        }

        private void deleteAllEquipment()
        {
            using (var context = new NinjaManagerEntities())
            {
                Ninja ninja = Ninja.ToModel();
                int ninjaId = Ninja.ToModel().Id;
                int money = 0;
                foreach(var item in ninja.Equipment.ToList())
                {
                    money += item.GoldValue;
                    totalGold -= item.GoldValue;
                    totalsAgillity -= item.Agility;
                    totalsIntelligence -= item.Intelligence;
                    totalsStrength -= item.Strenght;
                    int itemId = item.Id;
                    var loadedNinja = context.Ninja.Find(ninjaId);
                    var equipment = context.Equipment.Find(itemId);
                    context.Entry(loadedNinja).Collection("Equipment").Load();
                    loadedNinja.Equipment.Remove(equipment);
                    context.SaveChanges();
                }
                ninja.Equipment.Clear();
                ninja.Gold += money;
                context.SaveChanges();
                deleteFields();
                
            }
        }

        private string output(EquipmentVM equipment)
        {
            if(equipment != null)
            {
                        return      equipment.Category + Environment.NewLine +
                        equipment.Name + Environment.NewLine +
                        "Strength = " + equipment.Strenght + Environment.NewLine +
                        "Intelligence = " + equipment.Intelligence + Environment.NewLine +
                        "Agility = " + equipment.Agility; 
            }
            else
            {
                return "geen item";
            }
            
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        private void deleteFields()
        {
            _head = null;
            Head = output(null);
            _chest = null;
            Chest = output(null);
            _boots = null;
            Boots = output(null);
            _belt = null;
            Belt = output(null);
            _legs = null;
            Legs = output(null);
            _shoulders = null;
            Shoulders = output(null);
            TotalGold = updateGold();
            Values = updateStats();

        }
        private string updateGold()
        {
            return "Gold: " + totalGold;
        }
        private string updateStats()
        {
             return "Strengt: " + totalsStrength + Environment.NewLine +
                        "Intelligence: " + totalsIntelligence + Environment.NewLine +
                        "Agility: " + totalsAgillity;
        }
    }
}
