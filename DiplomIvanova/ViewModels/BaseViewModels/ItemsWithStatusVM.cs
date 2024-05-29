using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.Enums;

namespace DiplomIvanova.ViewModels.BaseViewModels
{
    public class ItemsWithStatusVM<T> : ItemsViewModel<T> where T : EntityWithStatusBase, new()
    {
        private TripStatus requestStatus;
        public TripStatus RequestStatus { get => requestStatus; set { requestStatus = value; FilterItems(); } }
        public ItemsWithStatusVM() : base()
        {
        }
        protected override async Task LoadItemsAsync()
        {
            (await GetDbItemsAsync<T>()).ForEach(PreloadItems.Add);
        }
        protected override void FilterItems()
        {
            base.FilterItems();
            var filter = PreloadItems.Where(x => x.Status == RequestStatus).ToList();
            Items.Clear();
            filter.ForEach(Items.Add);
        }

        protected override void OnItemTapped(T item)
        {
        }

    }
}
