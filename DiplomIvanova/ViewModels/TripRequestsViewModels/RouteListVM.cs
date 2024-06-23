using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.ViewModels.BaseViewModels;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Storage;

namespace DiplomIvanova.ViewModels.TripRequestsViewModels;

public class RouteListVM:BaseViewModel
{
    public ObservableCollection<CarEntity> Cars { get; set; }
    public ObservableCollection<DriverEntity> Drivers { get; set; }

    public ObservableCollection<PickUpPointEntityExtension> PickUpPoints { get; set; }

    public ObservableCollection<PickUpPointEntityExtension> ChoosedPoints { get; set; }

    public RouteListVM()
    {
        Cars = [];
        Drivers = [];
        PickUpPoints = [];
        ChoosedPoints = [];
    }

    public async Task ExecuteLoadItemsAsync()
    {
        Cars.Clear();
        Drivers.Clear();
        PickUpPoints.Clear();
        var cars = await GetDbItemsAsync<CarEntity>();
        var drivers = await GetDbItemsAsync<DriverEntity>();
        var pickUpPoints = await GetDbItemsAsync<PickUpPointEntity>();
        cars.ForEach(Cars.Add);
        drivers.ForEach(Drivers.Add);
        foreach (var point in pickUpPoints.OrderBy(x => x.Name))
        {
            PickUpPoints.Add(new()
            {
                Adress = point.Adress,
                Id = point.Id,
                IsChecked = false,
                Latitude = point.Latitude,
                Longitude = point.Longitude,
                Name = point.Name,
            });
        }
    }

    public void AddPoints()
    {
        ChoosedPoints.Clear();
        foreach (var point in PickUpPoints.Where(x=>x.IsChecked)) 
        {
            ChoosedPoints.Add(point);
        }
    }
}
