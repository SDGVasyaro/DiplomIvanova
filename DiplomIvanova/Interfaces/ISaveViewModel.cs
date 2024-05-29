using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomIvanova.Interfaces
{
    public interface ISaveViewModel
    {
        public Command SaveCommand { get; }
    }
}
