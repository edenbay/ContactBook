using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.ViewModels.Base
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel
    {
        public virtual void Dispose() { }

    }
}
