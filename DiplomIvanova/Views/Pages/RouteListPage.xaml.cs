using CommunityToolkit.Maui.Storage;
using DiplomIvanova.ViewModels.TripRequestsViewModels;

namespace DiplomIvanova.Views.Pages;

public partial class RouteListPage : ContentPage
{
    private readonly RouteListVM _viewModel;
	public RouteListPage()
	{
        BindingContext = _viewModel = new();
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.ExecuteLoadItemsAsync();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await OnSave(contentStack);
    }

    private static async Task OnSave(View view)
    {
        var screenshotResult = await view.CaptureAsync();

        if (screenshotResult != null)
        {
            var stream = new MemoryStream();
            await screenshotResult.CopyToAsync(stream, ScreenshotFormat.Jpeg);
            stream.Position = 0;
            var date = DateTime.Now.ToString("dd:MM:yyyy");
            var name = "Маршрутный_лист" + date + ".png";
            await FileSaver.Default.SaveAsync(name, stream, default);
        }
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {

    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _viewModel.AddPoints();
    }
}