using NinjaManager.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;

namespace NinjaManager.ViewModel
{
    public class EquipmentOverviewVM : INotifyPropertyChanged
    {
        
        private EquipmentVM _selectedEquipment;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateEquipment { get; set; }
        public ICommand EditEquipment { get; set; }
        public ICommand DeleteEquipment { get; set; }
        public ObservableCollection<EquipmentVM> Equipment { get; set; }
        public EquipmentVM SelectedEquipment
        {
            get { return _selectedEquipment; }
            set
            {
                _selectedEquipment = value;
                NotifyPropertyChanged("Equipment");
                
            }
        }
        public EquipmentOverviewVM()
        {
            
            using (var context = new NinjaManagerEntities())
            {
                var equipment = context.Equipment.ToList().Select(e => new EquipmentVM(e));
                Equipment = new ObservableCollection<EquipmentVM>(equipment);
            }

            CreateEquipment = new RelayCommand(() =>
            {
                new CreateEquipmentWindow().Show();
            });
            EditEquipment = new RelayCommand(() =>
            {
                new EditEquipmentWindow().Show();
            });
            DeleteEquipment = new RelayCommand(deleteEquipment);

        }

        private void deleteEquipment()
        {
            using (var context = new NinjaManagerEntities())
            {
                int EquipmentId = _selectedEquipment.ToModel().Id;
                var item = context.Equipment.Where(Item => Item.Id == EquipmentId).Single();
                context.Equipment.Remove(item);
                context.SaveChanges();
                Equipment.Remove(_selectedEquipment);
            }
        }

        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


    }
}
