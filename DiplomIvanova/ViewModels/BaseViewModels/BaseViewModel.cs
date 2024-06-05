using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DiplomIvanova.ViewModels.BaseViewModels
{
    public abstract class BaseViewModel: INotifyPropertyChanged
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        
        protected async Task<List<T>> GetDbItemsAsync<T>() where T : class,IEntityBase,new()
        {
            IsBusy= true;
            var items=await DataBaseHelper.GetItemsAsync<T>();
            IsBusy= false;
            return items;
        }


    }
}
