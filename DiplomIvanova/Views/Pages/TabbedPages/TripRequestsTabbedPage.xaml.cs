using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.Enums;
using DiplomIvanova.ViewModels.BaseViewModels;

namespace DiplomIvanova.Views.Pages.TripRequestPages;

public partial class TripRequestsTabbedPage : TabbedPage
{
	ItemsWithStatusVM<TripRequestEntity> viewModel;
    private readonly Dictionary<string, TripStatus> tripStatus = new()
    {
        { "Сформированные", TripStatus.Formed },
        { "В процессе", TripStatus.InProgress },
        { "Завершенные", TripStatus.Ended }
    };
	public TripRequestsTabbedPage()
	{
		BindingContext = viewModel = new();
        viewModel.RequestStatus = TripStatus.Formed;
        InitializeComponent();
        foreach (var item in Children)
        {
			item.BindingContext = viewModel;
        }
	}
    protected override void OnCurrentPageChanged()
    {
        base.OnCurrentPageChanged();
        viewModel.RequestStatus = tripStatus[CurrentPage.Title];
    }

}