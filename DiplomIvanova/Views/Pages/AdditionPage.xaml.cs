using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.Interfaces;
using DiplomIvanova.ViewModels;
using DiplomIvanova.ViewModels.TripRequestsViewModels;
using DiplomIvanova.Views.Templates;

namespace DiplomIvanova.Views.Pages;
public partial class AdditionPage : ContentPage
{
    private View? _view;
    public AdditionPage()
	{
		InitializeComponent();
        BindingContext = new TripRequestVM();
        //picker.Items.Add("CarEntity");
        //picker.Items.Add("ClientEntity");
        //picker.Items.Add("DriverEntity");
        //picker.SelectedIndexChanged += Picker_SelectedIndexChanged;
        //picker.SelectedIndex = 0;
    }

    private void Picker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        //ChangePageContent();
    }

    //private void ChangePageContent()
    //{
    //    if (string.IsNullOrWhiteSpace(picker.SelectedItem?.ToString()))
    //        return;
    //    Title = picker.SelectedItem?.ToString()?.Replace("Entity", "s");
    //    contentStack.Children.Remove(_view);
    //    (View content, Dictionary<Entry, string> fields) templ;
    //    switch (picker.SelectedItem?.ToString())
    //    {
    //        case "CarEntity":
    //            templ = AdditionTemplates.CarsDataTemplate;
    //            BindingContext = new AdditionVM<CarEntity>(new CarEntity(), templ.fields);
    //            break;
    //        case "ClientEntity":
    //            templ = AdditionTemplates.ClientsDataTemplate;
    //            BindingContext = new AdditionVM<ClientEntity>(new ClientEntity(), templ.fields);
                
    //            break;
    //        case "DriverEntity":
    //            templ = AdditionTemplates.DriversDataTemplate;
    //            BindingContext = new AdditionVM<DriverEntity>(new DriverEntity(), templ.fields);
    //            break;
    //        //case "PickUpPointEntity":
    //        //    BindingContext = new EntityItemsVM<PickUpPointEntity>();
    //        //    Content = AdditionTemplates.PickUpPointDataTemplate;
    //        //    break;
    //        //case "ProductEntity":
    //        //    BindingContext = new EntityItemsVM<ProductEntity>();
    //        //    Content = AdditionTemplates.ProductsDataTemplate;
    //        //    break;
    //        //case "RouteEntity":
    //        //    BindingContext = new EntityItemsVM<RouteEntity>();
    //        //    Content = AdditionTemplates.RoutesDataTemplate;
    //        //    break;
    //        //case "StorageEntity":
    //        //    BindingContext = new EntityItemsVM<StorageEntity>();
    //        //    Content = AdditionTemplates.StorageDataTemplate;
    //        //    break;
    //        default:
    //            return;
    //    }
    //    _view = templ.content;
    //    contentStack.Children.Add(_view);
    //}
}