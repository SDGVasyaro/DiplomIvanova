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


        public TripRequestVM()
        {
            SetupMap();
            CreateGraphics();
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

        private void SetupMap()
        {

            // Create a new map with a 'topographic vector' basemap.
            Map = new Map(BasemapStyle.ArcGISTopographic);
            var mapCenterPoint = new MapPoint(-118.805, 34.027, SpatialReferences.Wgs84);
            Map.InitialViewpoint = new Viewpoint(mapCenterPoint, 100000);
        }

        private void CreateGraphics()
        {
            // Create a new graphics overlay to contain a variety of graphics.
            var pointGraphicsOverlay = new GraphicsOverlay();
            var polylineGraphicsOverlay = new GraphicsOverlay();
            // Add the overlay to a graphics overlay collection.
            GraphicsOverlayCollection overlays = new GraphicsOverlayCollection
            {
                pointGraphicsOverlay,
                polylineGraphicsOverlay
            };

            // Set the view model's "GraphicsOverlays" property (will be consumed by the map view).
            this.GraphicsOverlays = overlays;

        }

    }
}
