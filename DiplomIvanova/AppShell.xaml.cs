using DiplomIvanova.Views.Pages;
using DiplomIvanova.Views.Pages.ItemsViewPages;
using DiplomIvanova.Views.Pages.TripRequestPages;

namespace DiplomIvanova
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }
        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(TripRequestsTabbedPage), typeof(TripRequestsTabbedPage));
            Routing.RegisterRoute(nameof(ItemsViewPage), typeof(ItemsViewPage));
            Routing.RegisterRoute(nameof(AdditionPage), typeof(AdditionPage));
            Routing.RegisterRoute(nameof(RoutePage), typeof(RoutePage));
            Routing.RegisterRoute(nameof(RouteListPage), typeof(RouteListPage));
        }
    }
}
