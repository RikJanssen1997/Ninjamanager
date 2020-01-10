using GalaSoft.MvvmLight.Command;
using NinjaManager.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NinjaManager.ViewModel
{
    public class EditEquipmentVM
    {

        private EquipmentOverviewVM _equipmentOverviewVM;
        public EquipmentVM Equipment { get; set; }
        public ObservableCollection<string> Categories { get; set; }
        public ICommand LoadImageCommand { get; }
        public ICommand SaveEquipmentCommand { get; }
        public EditEquipmentVM(EquipmentOverviewVM equipmentOverviewVM)
        {
            _equipmentOverviewVM = equipmentOverviewVM;
            Equipment = _equipmentOverviewVM.SelectedEquipment;
            using (var context = new NinjaManagerEntities())
            {
                var categories = context.Category.ToList().Select(c => new CategoryVM(c).Name);
                Categories = new ObservableCollection<string>(categories);
            }
            LoadImageCommand = new RelayCommand(() =>
            {
                Equipment.Picture = FileLoader.LoadImage();

            });
            SaveEquipmentCommand = new RelayCommand(editEquipment);

        }   
        private void editEquipment()
        {
                if(Equipment.Name != null && Equipment.Picture != null)
                {
                    using (var context = new NinjaManagerEntities())
                    {
                              int EquipmentId = _equipmentOverviewVM.SelectedEquipment.ToModel().Id;
                              var item = context.Equipment.Where(Item => Item.Id == EquipmentId).Single();
                              item.Name = Equipment.Name;
                              item.GoldValue = Equipment.Gold;
                              item.Intelligence = Equipment.Intelligence;
                              item.Strenght = Equipment.Strenght;
                              item.Agility = Equipment.Agility;
                              item.Category = Equipment.Category;
                              item.Picture = Equipment.DatabasePicture;
                    Equipment.UpdateValues();
                              context.SaveChanges();
                    }
                }
                
        }
    }
}
