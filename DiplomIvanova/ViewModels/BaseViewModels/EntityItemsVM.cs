using DiplomIvanova.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomIvanova.ViewModels.BaseViewModels
{
    public class EntityItemsVM<T>:ItemsViewModel<T> where T : class, IEntityBase, new()
    {
        public EntityItemsVM() : base()
        {
        }
        protected override async Task LoadItemsAsync()
        {
            (await GetDbItemsAsync<T>()).ForEach(PreloadItems.Add);
            Items.Clear();
            PreloadItems.ForEach(Items.Add);
        }

        protected override void OnItemTapped(T item)
        {
        }

    }
}
