using GalaSoft.MvvmLight.Command;
using NinjaManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NinjaManager.ViewModel
{
    public class EditNinjaVM
    {
        private MainViewModel _main;
        public NinjaVM Ninja { get; set; }
        public ICommand EditNinjaCommand { get; }

        public EditNinjaVM(MainViewModel main)
        {
            this._main = main;
            Ninja = _main.SelectedNinja;
            EditNinjaCommand = new RelayCommand(() =>
            {
                using (var context = new NinjaManagerEntities())
                {
                    int ninjaId = _main.SelectedNinja.ToModel().Id;
                    var item =context.Ninja.Where(Item => Item.Id == ninjaId).Single();
                    item.Name = Ninja.Name;
                    item.Gold = Ninja.Gold;
                    context.SaveChanges();
                   
                }
            });
        }
    }
}
