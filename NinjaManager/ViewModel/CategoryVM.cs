using NinjaManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaManager.ViewModel
{
    public class CategoryVM
    {
        private Category _category;
        public string Name
        {
            get { return _category.Category1; }
            set { _category.Category1 = value; }
        }

        public CategoryVM(Category category)
        {
            this._category = category;
        }
    }
}
