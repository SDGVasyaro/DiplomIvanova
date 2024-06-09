using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Syncfusion.Maui.Core.Carousel;
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
        private MapView _mapView;
        public MapView MapView
        {
            get => _mapView;
            set
            {
                SetProperty(ref _mapView, value);
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

        protected void ClearMapPoints()
        {
            GraphicsOverlays![0].Graphics.Clear();
        }

        protected async Task ZoomToMapPointAsync(MapPoint point)
        {
            var pointSymbol = new SimpleMarkerSymbol
            {
                Style = SimpleMarkerSymbolStyle.Diamond,
                Color = System.Drawing.Color.Orange,
                Size = 10.0,
                // Add an outline to the symbol.
                Outline = new SimpleLineSymbol
                {
                    Style = SimpleLineSymbolStyle.Solid,
                    Color = System.Drawing.Color.Purple,
                    Width = 2.0
                }
            };
            var pointGraphic = new Graphic(point, pointSymbol);

            // Add the point graphic to graphics overlay.
            GraphicsOverlays![0].Graphics.Add(pointGraphic);

            // Создаем viewpoint на основе этой точки
            Viewpoint viewpoint = new Viewpoint(point, 12000);

            // Перемещаем карту к этому viewpoint
            await MapView?.SetViewpointAsync(viewpoint);
            //Map.
        }
        
    }
}
