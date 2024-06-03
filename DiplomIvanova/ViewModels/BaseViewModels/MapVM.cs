using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace DiplomIvanova.ViewModels.BaseViewModels
{
    public class MapVM:BaseViewModel
    {

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
        public MapVM() 
        {
            SetupMap();
            CreateGraphics();
        }

        private void SetupMap()
        {
            BasemapStyleParameters basemapStyleParameters = new BasemapStyleParameters();
            basemapStyleParameters.SpecificLanguage = new CultureInfo("ru");
            // Create a new map with a 'topographic vector' basemap.
            Map = new Map(BasemapStyle.ArcGISTopographic);
            Map.Basemap= new Basemap(BasemapStyle.ArcGISTopographic, basemapStyleParameters);
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
        
    }
}
