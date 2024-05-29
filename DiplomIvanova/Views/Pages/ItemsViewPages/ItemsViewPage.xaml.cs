using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.ViewModels.BaseViewModels;
using DiplomIvanova.Views.Templates;

namespace DiplomIvanova.Views.Pages.ItemsViewPages;

[QueryProperty(nameof(ItemsType), nameof(ItemsType))]
public partial class ItemsViewPage : ContentPage
{
	public string? itemsType;

    public string? ItemsType { get => itemsType; set { itemsType = value; ChangeBindingContext(); } }
	public ItemsViewPage()
	{
		InitializeComponent();
	}
	private void ChangeBindingContext()
	{
		if (string.IsNullOrWhiteSpace(ItemsType))
			return;
        Title = ItemsType.Replace("Entity", "s");
        switch (ItemsType)
        {
            case "CarEntity":
                BindingContext = new EntityItemsVM<CarEntity>();
                ItemsCollectionView.ItemTemplate = CollectionViewTemplates.CarsDataTemplate;
                break;
            case "ClientEntity":
                BindingContext = new EntityItemsVM<ClientEntity>();
                ItemsCollectionView.ItemTemplate = CollectionViewTemplates.ClientsDataTemplate;
                break;
            case "DriverEntity":
                BindingContext = new EntityItemsVM<DriverEntity>();
                ItemsCollectionView.ItemTemplate = CollectionViewTemplates.DriversDataTemplate;
                break;
            case "PickUpPointEntity":
                BindingContext = new EntityItemsVM<PickUpPointEntity>();
                ItemsCollectionView.ItemTemplate = CollectionViewTemplates.PickUpPointDataTemplate;
                break;
            case "ProductEntity":
                BindingContext = new EntityItemsVM<ProductEntity>();
                ItemsCollectionView.ItemTemplate = CollectionViewTemplates.ProductsDataTemplate;
                break;
            case "RouteEntity":
                BindingContext = new EntityItemsVM<RouteEntity>();
                ItemsCollectionView.ItemTemplate = CollectionViewTemplates.RoutesDataTemplate;
                break;
            case "StorageEntity":
                BindingContext = new EntityItemsVM<StorageEntity>();
                ItemsCollectionView.ItemTemplate = CollectionViewTemplates.StorageDataTemplate;
                break;
            default:
                return;
                //case "TripEntity":
                //    break;
                //case "TripEntity":
                //    break;
        }
    }
}