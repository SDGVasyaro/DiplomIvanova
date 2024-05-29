using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.ViewModels.TripRequestsViewModels;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;

namespace DiplomIvanova.Views.Pages;

public partial class RoutePage : ContentPage
{

    private readonly TripRequestVM _viewModel;
	public RoutePage()
	{
		InitializeComponent();
        BindingContext = _viewModel = new();
	}

    protected override async void OnAppearing()
    {
        await _viewModel.ExecuteLoadItemsAsync();
    }

    private void routeMap_MapClicked(object sender, Microsoft.Maui.Controls.Maps.MapClickedEventArgs e)
    {
        var pin = new Pin()
        {
            Location = new(e.Location.Latitude, e.Location.Longitude),
            Label = string.Empty,
        };
        //pin.MarkerClicked += Pin_MarkerClicked;

        //routeMap.Pins.Add(pin);
        //if(routeMap.Pins.Count == 2 ) 
        //{
        //    Polyline polyline = new Polyline
        //    {
        //        StrokeColor = Colors.Red,
        //        StrokeWidth = 8
        //    };

        //    foreach (var waypoint in routeMap.Pins)
        //    {
        //        polyline.Geopath.Add(new Location(waypoint.Location.Latitude, waypoint.Location.Longitude));
        //    }

        //    // Add the Polyline to the map's MapElements collection
        //    routeMap.MapElements.Add(polyline);
            //routeMap.UpdateLayout()
        //}
        
    }

    private void Pin_MarkerClicked(object? sender, PinClickedEventArgs e)
    {
        //routeMap.Pins.Remove((Pin)sender!);
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        _viewModel.Driver = (sender as Picker)!.SelectedItem as DriverEntity;
    }

    private void Picker_SelectedIndexChanged_1(object sender, EventArgs e)
    {
        _viewModel.Car = (sender as Picker)!.SelectedItem as CarEntity;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
       
    }
}