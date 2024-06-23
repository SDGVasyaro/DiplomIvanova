using DiplomIvanova.ViewModels.BaseViewModels;
using DiplomIvanova.Views.Pages;
using DiplomIvanova.Views.Pages.ItemsViewPages;

namespace DiplomIvanova.ViewModels.MainPage
{
    public class MainPageVM:MapVM
    {
        #region commands
        public Command GoToListCommand => new(async () => await Shell.Current.GoToAsync(nameof(RouteListPage)));
        public Command GoToAddCommand => new(async () => await Shell.Current.GoToAsync(nameof(AdditionPage)));
        public Command AddRouteCommand => new(async () => await Shell.Current.GoToAsync(nameof(RoutePage)));
        
        #endregion
    }
}
