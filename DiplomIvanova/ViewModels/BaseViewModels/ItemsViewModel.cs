using System.Collections.ObjectModel;

namespace DiplomIvanova.ViewModels.BaseViewModels
{
    public abstract class ItemsViewModel<T>: MapVM
    {
        protected List<T> PreloadItems { get;} = [];
        public ObservableCollection<T> Items { get; } = [];
        public bool IsRefreshing { get; set; }
        public T? SelectedItem { get; set; }
        public ItemsViewModel()
        {
            Init();
        }
        private async void Init()
        {
            await LoadItemsAsync();
            FilterItems();
        }
        protected abstract Task LoadItemsAsync();
        protected virtual void FilterItems()
        {

        }
        protected abstract void OnItemTapped(T item);
        public Command<T> ItemTappedCommand => new(OnItemTapped);
        public Command RefreshCommand => new(RefreshItems);
        private async void RefreshItems()
        {
            IsRefreshing = false;
            await LoadItemsAsync();
            FilterItems();
        }
    }
}
