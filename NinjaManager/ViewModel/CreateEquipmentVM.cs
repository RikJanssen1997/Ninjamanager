using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using NinjaManager.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NinjaManager.ViewModel
{
    public class CreateEquipmentVM
    {
        private EquipmentOverviewVM _equipmentOverview;


        public EquipmentVM Equipment { get; set; }
        public ICommand SaveEquipmentCommand { get; }
        public ICommand LoadImageCommand { get; }
        public ObservableCollection<string> Categories { get; set; }

        public CreateEquipmentVM(EquipmentOverviewVM equipmentOverview)
        {
            _equipmentOverview = equipmentOverview;
            Equipment = new EquipmentVM();
            using (var context = new NinjaManagerEntities())
            {
                var categories = context.Category.ToList().Select(c => new CategoryVM(c).Name);
                Categories = new ObservableCollection<string>(categories);
            }
            SaveEquipmentCommand = new RelayCommand(() =>
            {
                
                using (var context = new NinjaManagerEntities())
                {
                    context.Equipment.Add(Equipment.ToModel());
                    if(Equipment.Name != null && Equipment.Picture != null)
                    {
                        context.SaveChanges();
                        _equipmentOverview.Equipment.Add(Equipment);

                    }
                   
                }
            });
            LoadImageCommand = new RelayCommand(() =>
            {
                Equipment.Picture = FileLoader.LoadImage();

            });
            
        }

        
    }
}
