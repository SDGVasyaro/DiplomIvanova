using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.ViewModels.TripRequestsViewModels;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.NetworkAnalysis;
using Esri.ArcGISRuntime.UI;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;
using Newtonsoft.Json;
using System.Net;

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

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        _viewModel.Driver = (sender as Picker)!.SelectedItem as DriverEntity;
        _viewModel.SaveCommand.ChangeCanExecute();
    }

    private void Picker_SelectedIndexChanged_1(object sender, EventArgs e)
    {
        _viewModel.Car = (sender as Picker)!.SelectedItem as CarEntity;
        _viewModel.SaveCommand.ChangeCanExecute();
    }

    private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _viewModel.SaveCommand.ChangeCanExecute();
        await _viewModel.DrawRoute();
    }

    private async void routeMap_GeoViewTapped(object sender, Esri.ArcGISRuntime.Maui.GeoViewInputEventArgs e)
    {
        // Создаем точку, к которой необходимо переместить карту



        if (_viewModel.Pins.Count >= 2)
        {
            DrawRoute();
        }
    }
    private async void DrawRoute()
    {
        // Используйте подходящий API для Bing Maps, 
        // чтобы получить данные о маршруте.
        // Пример с использованием HttpClient:
        var routeTask = await RouteTask.CreateAsync(new Uri(
            "https://route-api.arcgis.com/arcgis/rest/services/World/Route/NAServer/Route_World"));
        RouteParameters routeParams = await routeTask.CreateDefaultParametersAsync();
        // Установка исходной и конечной точек
        var stops = _viewModel.Pins.Count == 2 ? _viewModel.Pins.Select(x => new Stop(x)) : SolveTravellingSalesmanProblem(_viewModel.Pins).Select(x => new Stop(x));
        routeParams.SetStops(stops);
        RouteResult routeResult = await routeTask.SolveRouteAsync(routeParams);

        // Получение маршрутного графика
        Esri.ArcGISRuntime.Geometry.Polyline routePath = routeResult?.Routes?.First()?.RouteGeometry!;
            var polylineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Purple, 3.0);
            var polylineGraphic = new Graphic(routePath, polylineSymbol);
        _viewModel.GraphicsOverlays![1].Graphics.Clear();
        _viewModel.GraphicsOverlays![1].Graphics.Add(polylineGraphic);
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

}
