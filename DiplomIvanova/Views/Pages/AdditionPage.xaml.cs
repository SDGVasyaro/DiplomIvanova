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
        BindingContext = new TripRequestVM();
        InitializeComponent();
        
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
    private async void ChangeBindingContext(string itemsType)
    {
        AddStack.Children.Clear();
        if (string.IsNullOrWhiteSpace(itemsType))
            return;
        switch (itemsType)
        {
            case "ТС":
                var viewModel= new EntityItemsVM<CarEntity>();
                CollectionViewTemplates.DeleteCommand=viewModel.DeleteCommand;
                BindingContext = viewModel;
                listView.ItemTemplate = CollectionViewTemplates.CarsDataTemplate;
                listView.Header = CollectionViewTemplates.GetCarsHeaderTemplate();
                AddStack.Children.Add(viewModel.CarsAddDataTemplate.content);
                break;
            case "Водители":
                var viewModel1 = new EntityItemsVM<DriverEntity>();
                CollectionViewTemplates.DeleteCommand = viewModel1.DeleteCommand;
                BindingContext = viewModel1;
                listView.ItemTemplate = CollectionViewTemplates.DriversDataTemplate;
                listView.Header = CollectionViewTemplates.GetDriversHeaderTemplate();
                AddStack.Children.Add(viewModel1.DriversAddDataTemplate.content);
                break;
            case "Пункты":
                var viewModel2 = new EntityItemsVM<PickUpPointEntity>();
                viewModel2.MapView = routeMap;
                CollectionViewTemplates.DeleteCommand = viewModel2.DeleteCommand;
                BindingContext = viewModel2;
                listView.ItemTemplate = CollectionViewTemplates.PickUpPointDataTemplate;
                listView.Header = CollectionViewTemplates.GetPickUpPointHeaderTemplate();
                AddStack.Children.Add(viewModel2.PointsAddDataTemplate.content);
                break;
            case "Рейсы":
                var viewModel3 = new EntityItemsVM<TripEntity>();
                CollectionViewTemplates.DeleteCommand = viewModel3.DeleteCommand;
                BindingContext = viewModel3;
                listView.ItemTemplate = CollectionViewTemplates.TripsDataTemplate;
                listView.Header = CollectionViewTemplates.GetTripsHeaderTemplate();
                listView.SelectionMode = SelectionMode.Single;
                listView.SelectionChanged += async (_, __) => await DrawRoute(__.CurrentSelection.First() as TripEntity);
                break;
            default:
                return;
        }
    }

    

    private async Task DrawRoute(TripEntity trip)
    {
        if (trip == null)
            return;
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
                x.Route.IntermediatePointsId,
            }).FirstOrDefaultAsync(default);
        List<PickUpPointEntity> IntermediatePoints = [];
        if (route.IntermediatePointsId.Any())
        {
            IntermediatePoints = await db.PickUpPoints.Where(x => route.IntermediatePointsId.Contains(x.Id)).ToListAsync(default);
        }
        IntermediatePoints.Insert(0,route.DeparturePoint);
        IntermediatePoints.Add(route.ArrivalPoint);
        var mapPoints = IntermediatePoints.OrderByDescending(x=>true).Select(x=>new MapPoint(x.Longitude, x.Latitude)).ToList();
        var first = new Stop(IntermediatePoints.Select(x => new MapPoint(x.Longitude, x.Latitude)).First());
        // Установка исходной и конечной точек
        var solve = SolveTravellingSalesmanProblem(mapPoints).Select(x => new Stop(x)).ToList();
        var polylines = new List<Polyline>();
        _viewModel.GraphicsOverlays![1].Graphics.Clear();
        while (solve.Count > 1)
        {
            routeParams.ClearStops();
            var stops = new List<Stop>()
                {
                    solve.First(),
                    solve[1],
                };
            routeParams.SetStops(stops);
            RouteResult routeResult = await routeTask.SolveRouteAsync(routeParams);
            Esri.ArcGISRuntime.Geometry.Polyline routePath = routeResult?.Routes?.First()?.RouteGeometry!;
            polylines.Add(routePath);
            var polylineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Purple, 3.0);
            var polylineGraphic = new Graphic(routePath, polylineSymbol);
            _viewModel.GraphicsOverlays![1].Graphics.Add(polylineGraphic);
            solve.RemoveAt(0);
        }
        routeParams.ClearStops();
        var stops1 = new List<Stop>()
                {
                    solve.First(),
                    first,
                };
        routeParams.SetStops(stops1);
        var routeResult1 = await routeTask.SolveRouteAsync(routeParams);
        var routePath1 = routeResult1?.Routes?.First()?.RouteGeometry!;
        polylines.Add(routePath1);
        var polylineSymbol1 = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Purple, 3.0);
        var polylineGraphic1 = new Graphic(routePath1, polylineSymbol1);
        _viewModel.GraphicsOverlays![1].Graphics.Add(polylineGraphic1);
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
        //ChangePageContent();
    }
}