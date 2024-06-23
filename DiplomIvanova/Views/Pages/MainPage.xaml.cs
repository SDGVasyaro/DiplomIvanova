using DiplomIvanova.ViewModels.BaseViewModels;
using DiplomIvanova.ViewModels.MainPage;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace DiplomIvanova
{
    public partial class MainPage : ContentPage
    {
        MainPageVM viewModel;
        public MainPage()
        {
            BindingContext = viewModel = new();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }

}
