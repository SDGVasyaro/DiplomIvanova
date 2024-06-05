using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.Helpers;
using DiplomIvanova.Interfaces;
using DiplomIvanova.ViewModels.BaseViewModels;
using DiplomIvanova.Views.Pages.ItemsViewPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomIvanova.ViewModels
{
    public class AdditionVM<T>:MapVM where T : class,IEntityBase,new()
    {

    }
}
