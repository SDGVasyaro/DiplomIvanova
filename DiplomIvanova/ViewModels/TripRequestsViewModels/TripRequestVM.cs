using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.ViewModels.BaseViewModels;
using System.Collections.ObjectModel;

namespace DiplomIvanova.ViewModels.TripRequestsViewModels
{
    public class TripRequestVM:BaseViewModel
    {
        public ObservableCollection<CarEntity> Cars { get; set; }
        public ObservableCollection<DriverEntity> Drivers { get; set; }
        public ObservableCollection<PickUpPointEntity> PickUpPoints { get; set; }

        public CarEntity? Car { get; set; }
        public DriverEntity? Driver { get; set; }
        public List<PickUpPointEntity>? ChoosedPoints { get; set; }


        public TripRequestVM()
        {
            Cars = [];
            Drivers = [];
            PickUpPoints = [];
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
            pickUpPoints.ForEach(PickUpPoints.Add);
        }
    }
}
