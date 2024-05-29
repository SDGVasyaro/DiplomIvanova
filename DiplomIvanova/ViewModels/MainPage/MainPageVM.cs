using DiplomIvanova.ViewModels.BaseViewModels;
using DiplomIvanova.Views.Pages;
using DiplomIvanova.Views.Pages.ItemsViewPages;

namespace DiplomIvanova.ViewModels.MainPage
{
    public class MainPageVM:BaseViewModel
    {
        #region commands
        public Command<string> GoToCommand => new(async (type) => await Shell.Current.GoToAsync($"{nameof(ItemsViewPage)}?{nameof(ItemsViewPage.ItemsType)}={type}"));
        public Command GoToAddCommand => new(async () => await Shell.Current.GoToAsync($"{nameof(AdditionPage)}"));
        public Command AddRouteCommand => new(async () => await Shell.Current.GoToAsync($"{nameof(RoutePage)}"));
        
        #endregion
    }
}
