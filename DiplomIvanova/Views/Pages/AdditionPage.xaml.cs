using DiplomIvanova.DataBase.Context;
using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.ViewModels;
using DiplomIvanova.ViewModels.BaseViewModels;
using DiplomIvanova.ViewModels.TripRequestsViewModels;
using DiplomIvanova.Views.Templates;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Esri.ArcGISRuntime.Tasks.NetworkAnalysis;
using Esri.ArcGISRuntime.UI;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Maui.Core.Carousel;

namespace DiplomIvanova.Views.Pages;
public partial class AdditionPage : ContentPage
{
    private View? _view;

    private List<Guid> _entities;

    private View defaultView;

    private string currentEntity;

    public AdditionPage()
	{
		InitializeComponent();
        BindingContext = new TripRequestVM();
        defaultView = Content;
        //picker.Items.Add("CarEntity");
        //picker.Items.Add("ClientEntity");
        //picker.Items.Add("DriverEntity");
        //picker.SelectedIndexChanged += Picker_SelectedIndexChanged;
        //picker.SelectedIndex = 0;
    }

    private void Picker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        //ChangePageContent();
    }

    private void ChangePageContent()
    {
        //contentStack.Children.Remove(_view);
        (View content, Dictionary<Entry, string> fields) templ;
        switch (currentEntity)
        {
            case "ТС":
                templ = AdditionTemplates.CarsDataTemplate;
                BindingContext = new AdditionVM<CarEntity>(new CarEntity(), templ.fields);
                break;
            case "Водители":
                templ = AdditionTemplates.DriversDataTemplate;
                BindingContext = new AdditionVM<DriverEntity>(new DriverEntity(), templ.fields);
                break;
            //case "PickUpPointEntity":
            //    BindingContext = new EntityItemsVM<PickUpPointEntity>();
            //    Content = AdditionTemplates.PickUpPointDataTemplate;
            //    break;
            //case "ProductEntity":
            //    BindingContext = new EntityItemsVM<ProductEntity>();
            //    Content = AdditionTemplates.ProductsDataTemplate;
            //    break;
            //case "RouteEntity":
            //    BindingContext = new EntityItemsVM<RouteEntity>();
            //    Content = AdditionTemplates.RoutesDataTemplate;
            //    break;
            //case "StorageEntity":
            //    BindingContext = new EntityItemsVM<StorageEntity>();
            //    Content = AdditionTemplates.StorageDataTemplate;
            //    break;
            default:
                return;
        }
        Content = templ.content;
        //contentStack.Children.Add(_view);
    }
    private void ChangeBindingContext(string itemsType)
    {
        if (string.IsNullOrWhiteSpace(itemsType))
            return;
        switch (itemsType)
        {
            case "ТС":
                BindingContext = new EntityItemsVM<CarEntity>();
                listView.ItemTemplate = CollectionViewTemplates.CarsDataTemplate;
                listView.Header = CollectionViewTemplates.GetCarsHeaderTemplate();
                break;
            case "Водители":
                BindingContext = new EntityItemsVM<DriverEntity>();
                listView.ItemTemplate = CollectionViewTemplates.DriversDataTemplate;
                listView.Header = CollectionViewTemplates.GetDriversHeaderTemplate();
                break;
            case "Пункты":
                BindingContext = new EntityItemsVM<PickUpPointEntity>();
                listView.ItemTemplate = CollectionViewTemplates.PickUpPointDataTemplate;
                listView.Header = CollectionViewTemplates.GetPickUpPointHeaderTemplate();
                break;
            case "Рейсы":
                BindingContext = new EntityItemsVM<TripEntity>();
                listView.ItemTemplate = CollectionViewTemplates.TripsDataTemplate;
                listView.Header = CollectionViewTemplates.GetTripsHeaderTemplate();
                listView.SelectionMode = SelectionMode.Single;
                listView.SelectionChanged += async (_, __) => await DrawRoute(__.CurrentSelection as TripEntity);
                break;
            default:
                return;
        }
    }

    private async Task GetAdressAsync()
    {
        MapPoint normalizedPoint = new(1,2);//(MapPoint)e.Location.NormalizeCentralMeridian();
        Uri _serviceUri = new Uri("https://geocode-api.arcgis.com/arcgis/rest/services/World/GeocodeServer");
        LocatorTask _geocoder = await LocatorTask.CreateAsync(_serviceUri);
        // Reverse geocode to get addresses.
        IReadOnlyList<GeocodeResult> addresses = await _geocoder.ReverseGeocodeAsync(normalizedPoint);

        // Get the first result.
        GeocodeResult address = addresses.First();

        // Use the city and region for the Callout Title.
        string calloutTitle = address.Attributes["Address"].ToString();
    }

    private async Task DrawRoute(TripEntity trip)
    {
        // Используйте подходящий API для Bing Maps, 
        // чтобы получить данные о маршруте.
        // Пример с использованием HttpClient:
        using var db = new AppDbContext();
        var _viewModel = BindingContext as EntityItemsVM<TripEntity>;
        var routeTask = await RouteTask.CreateAsync(new Uri(
            "https://route-api.arcgis.com/arcgis/rest/services/World/Route/NAServer/Route_World"));
        RouteParameters routeParams = await routeTask.CreateDefaultParametersAsync();
        // Установка исходной и конечной точек
        var route = await db.Trips.Where(x => x.Id == trip.Id)
            .Select(x => new
            {
                x.Route.ArrivalPoint,
                x.Route.DeparturePoint,
                x.Route.IntermediatePoints
            }).FirstOrDefaultAsync(default);
        var stops = route.IntermediatePoints.Count == 0 ? 
            new List<Stop>()
        {
            new Stop(new MapPoint(route.DeparturePoint.Longitude,route.DeparturePoint.Latitude)),
            new Stop(new MapPoint(route.DeparturePoint.Latitude,route.DeparturePoint.Latitude)),
        }
         : SolveTravellingSalesmanProblem(new List<MapPoint>(route.IntermediatePoints.Select(x => new MapPoint(x.Longitude, x.Latitude)))).Select(x=>new Stop(x)).ToList();

        routeParams.SetStops(stops);
        RouteResult routeResult = await routeTask.SolveRouteAsync(routeParams);

        // Получение маршрутного графика
        Esri.ArcGISRuntime.Geometry.Polyline routePath = routeResult?.Routes?.First()?.RouteGeometry!;
        var polylineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Purple, 3.0);
        var polylineGraphic = new Graphic(routePath, polylineSymbol);
        _viewModel.GraphicsOverlays![1].Graphics.Clear();
        _viewModel.GraphicsOverlays![1].Graphics.Add(polylineGraphic);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        
        foreach (var children in entityButtonStack.Children)
        {
            if (children is Button bt)
            {
                bt.IsEnabled = true;
            }
        }
        btn!.IsEnabled = false;
        currentEntity = btn!.Text;
        ChangeBindingContext(btn!.Text);
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {

    }
    public static List<MapPoint> SolveTravellingSalesmanProblem(List<MapPoint> points)
    {
        int n = points.Count;
        double[,] distances = new double[n, n];

        // Вычисление матрицы расстояний между всеми точками
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                distances[i, j] = CalculateDistance(points[i], points[j]);
            }
        }

        // Инициализация начального пути
        List<int> path = Enumerable.Range(0, n).ToList();

        // Применение алгоритма Дейкстры для оптимизации пути
        OptimizePath(path, distances);

        // Формирование списка MapPoint в оптимальном порядке
        List<MapPoint> optimizedPath = new List<MapPoint>();
        foreach (int index in path)
        {
            optimizedPath.Add(points[index]);
        }

        return optimizedPath;
    }

    private static void OptimizePath(List<int> path, double[,] distances)
    {
        int n = path.Count;
        double totalDistance = 0;

        // Вычисление общего расстояния для начального пути
        for (int i = 0; i < n - 1; i++)
        {
            int currentIndex = path[i];
            int nextIndex = path[i + 1];
            totalDistance += distances[currentIndex, nextIndex];
        }

        bool isOptimized = false;
        int iterationCount = 0;
        int maxIterations = 100; // Ограничение на количество итераций

        while (!isOptimized && iterationCount < maxIterations)
        {
            isOptimized = true;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    double newDistance = distances[path[i], path[j]];
                    double oldDistance = distances[path[i], path[i + 1]] + distances[path[j - 1], path[j]];

                    if (newDistance < oldDistance)
                    {
                        // Обновление пути
                        path.RemoveAt(j);
                        path.Insert(i + 1, j);
                        totalDistance -= oldDistance;
                        totalDistance += newDistance;
                        isOptimized = false;
                        break;
                    }
                }

                if (!isOptimized)
                    break;
            }

            iterationCount++;
        }
    }

    private static double CalculateDistance(MapPoint point1, MapPoint point2)
    {
        // Реализация вычисления расстояния между двумя точками
        // Используйте соответствующие методы из Esri.ArcGISRuntime.Geometry.MapPoint
        double distance = Math.Sqrt(Math.Pow(Math.Abs(point1.X - point2.X), 2) + Math.Pow(Math.Abs(point1.Y - point2.Y), 2));
        return distance;
    }

    private void Button_Clicked_2(object sender, EventArgs e)
    {
        ChangePageContent();
    }
}