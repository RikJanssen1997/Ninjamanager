using NinjaManager.Domain;
using System.Collections.Generic;
using System.ComponentModel;

namespace NinjaManager.ViewModel
{
    public class NinjaVM : INotifyPropertyChanged
    {
        private Ninja _ninja;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _ninja.Name; }
            set { _ninja.Name = value; }
        }
        
        public int Gold
        {
            get { return _ninja.Gold; }
            set 
            {
                _ninja.Gold = value;
                OnPropertyChanged("Gold");
            }
        }
        public List<EquipmentVM> Inventory
        {
            get
            {
                List<EquipmentVM> inventory = new List<EquipmentVM>();
                foreach(var i in _ninja.Equipment)
                {
                    inventory.Add(new EquipmentVM(i));
                }
                return inventory;
            }
            set 
            {
                List<Equipment> inventory = new List<Equipment>();
                foreach(var i in value)
                {
                    inventory.Add(i.ToModel());
                }
                _ninja.Equipment = inventory;
            }
        }


        public NinjaVM(Ninja ninja)
        {
            this._ninja = ninja;
        }

        public NinjaVM()
        {
            this._ninja = new Ninja();
        }
        public Ninja ToModel()
        {
            return _ninja;
        }
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}