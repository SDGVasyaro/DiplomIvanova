using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.Enums;
using DiplomIvanova.ViewModels.BaseViewModels;

namespace DiplomIvanova.Views.Pages.TripRequestPages;

public partial class TripRequestsTabbedPage : TabbedPage
{
	ItemsWithStatusVM<TripRequestEntity> viewModel;
    private readonly Dictionary<string, TripStatus> tripStatus = new()
    {
        { "��������������", TripStatus.������������ },
        { "� ��������", TripStatus.�_���� },
        { "�����������", TripStatus.�������� }
    };
	public TripRequestsTabbedPage()
	{
		BindingContext = viewModel = new();
        viewModel.RequestStatus = TripStatus.������������;
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