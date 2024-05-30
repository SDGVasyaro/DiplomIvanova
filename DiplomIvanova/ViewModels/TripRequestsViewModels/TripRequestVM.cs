using DiplomIvanova.DataBase.Context;
using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.ViewModels.BaseViewModels;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using System.Collections.ObjectModel;
using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace DiplomIvanova.ViewModels.TripRequestsViewModels
{
    public class TripRequestVM:BaseViewModel
    {
        public ObservableCollection<CarEntity> Cars { get; set; }
        public ObservableCollection<DriverEntity> Drivers { get; set; }
        public ObservableCollection<PickUpPointEntity> PickUpPoints { get; set; }

        private Map _map;
        public Map Map
        {
            get => _map;
            set
            {
                SetProperty(ref _map, value);
            }
        }
        private GraphicsOverlayCollection? _graphicsOverlays;
        public GraphicsOverlayCollection? GraphicsOverlays
        {
            get { return _graphicsOverlays; }
            set
            {
                SetProperty(ref _graphicsOverlays, value);
            }
        }

        public CarEntity? Car { get; set; }
        public DriverEntity? Driver { get; set; }
        public List<PickUpPointEntity>? ChoosedPoints { get; set; }
        public Command SaveCommand { get; }
        public List<MapPoint> Pins { get; }


        public TripRequestVM()
        {
            SetupMap();
            CreateGraphics();
            Cars = [];
            Pins = [];
            Drivers = [];
            PickUpPoints = [];
            SaveCommand = new Command(OnSave);
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

        private void SetupMap()
        {

            // Create a new map with a 'topographic vector' basemap.
            Map = new Map(BasemapStyle.ArcGISTopographic);
            var mapCenterPoint = new MapPoint(37.555, 55.628, SpatialReferences.Wgs84);
            Map.InitialViewpoint = new Viewpoint(mapCenterPoint, 1000000);
        }

        private void CreateGraphics()
        {
            // Create a new graphics overlay to contain a variety of graphics.
            var pointGraphicsOverlay = new GraphicsOverlay();
            var polylineGraphicsOverlay = new GraphicsOverlay();
            // Add the overlay to a graphics overlay collection.
            GraphicsOverlayCollection overlays = new()
            {
                pointGraphicsOverlay,
                polylineGraphicsOverlay
            };

            // Set the view model's "GraphicsOverlays" property (will be consumed by the map view).
            this.GraphicsOverlays = overlays;

        }
        private async void OnSave()
        {
            using var db = new AppDbContext();
            var depart = new PickUpPointEntity() 
            {
                Longitude = Pins[0].X,
                Latitude = Pins[0].Y,
            };
            var arival = new PickUpPointEntity()
            {
                Longitude = Pins[0].X,
                Latitude = Pins[0].Y,
            };
            var intermediate = Pins.Skip(1).SkipLast(1).Select(x => new PickUpPointEntity()
            {
                Longitude = x.X,
                Latitude = x.Y
            }).ToList();
            RouteEntity route = new()
            {
                DeparturePoint = depart,
                ArrivalPoint = arival,
                IntermediatePoints = intermediate,
            };
            await db.Routes.AddAsync(route);
            var trip = new TripEntity()
            {
                Car = Car,
                Driver = Driver,
                StartAt = DateTime.Now,
                Route = route,
            };
            await db.Trips.AddAsync(trip,default);
        }

    }
}
