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
    public class CreateNinjaVM
    {
        private MainViewModel _main;

        public NinjaVM Ninja { get; set; }
        public ICommand SaveNinjaCommand { get; }

        public CreateNinjaVM(MainViewModel main)
        {
            _main = main;
            Ninja = new NinjaVM();
            SaveNinjaCommand = new RelayCommand(() =>
            {
                using (var context = new NinjaManagerEntities())
                {
                    context.Ninja.Add(Ninja.ToModel());
                    context.SaveChanges();
                    _main.Ninjas.Add(Ninja);
                }
            });
        }

    }
}
