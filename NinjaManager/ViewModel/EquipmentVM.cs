using NinjaManager.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace NinjaManager.ViewModel
{
    public class EquipmentVM : INotifyPropertyChanged
    {
        private Equipment _equipment;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _equipment.Name; }
            set { _equipment.Name= value;
            }
        }
        public int Gold
        {
            get { return _equipment.GoldValue; }
            set { _equipment.GoldValue = value; }
        }

        public int Strenght { 
            get { return _equipment.Strenght; }
            set { _equipment.Strenght = value; }
        }
        public int Intelligence
        {
            get { return _equipment.Intelligence; }
            set { _equipment.Intelligence = value; }
        }
        public int Agility
        {
            get { return _equipment.Agility; }
            set { _equipment.Agility = value; }
        }
        public string Category
        {
            get { return _equipment.Category; }
            set { _equipment.Category = value; }
        }
        public BitmapImage Picture
        {
            get
            {
                if(_equipment.Picture != null)
                {
                    
                    using (var ms = new MemoryStream(_equipment.Picture))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad; // here
                        image.StreamSource = ms;
                        image.EndInit();
                        return image;
                    }
                }
                return null;
                
            }
            set 
            {
                byte[] data;
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                if(value!= null)
                {
                    encoder.Frames.Add(BitmapFrame.Create(value));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                        _equipment.Picture = data;
                        OnPropertyChanged("Picture");
                    }
                }
                
                
            }
        }

        public byte[] DatabasePicture
        {
            get { return _equipment.Picture; }
        }



        public EquipmentVM(Equipment equipment)
        {
            this._equipment = equipment;
        }

        public EquipmentVM()
        {
            this._equipment = new Equipment();
        }
        public Equipment ToModel()
        {
            return _equipment;
        }
        private void OnPropertyChanged(string property)
        {
            if(PropertyChanged!= null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public void UpdateValues()
        {
            OnPropertyChanged("Name");
            OnPropertyChanged("Gold");
            OnPropertyChanged("Strenght");
            OnPropertyChanged("Intelligence");
            OnPropertyChanged("Agility");
            OnPropertyChanged("Category");
        }
    }
}
