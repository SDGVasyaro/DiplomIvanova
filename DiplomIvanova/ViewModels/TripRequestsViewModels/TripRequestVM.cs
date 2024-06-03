using DiplomIvanova.DataBase.Context;
using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.ViewModels.BaseViewModels;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.NetworkAnalysis;
using Esri.ArcGISRuntime.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using static System.Formats.Asn1.AsnWriter;

namespace DiplomIvanova.ViewModels.TripRequestsViewModels
{
    public class PickUpPointEntityExtension : PickUpPointEntity
    {
        public bool IsChecked { get; set; }
    }
    public class TripRequestVM:MapVM
    {

        private DateTime? _date;
        public ObservableCollection<CarEntity> Cars { get; set; }
        public ObservableCollection<DriverEntity> Drivers { get; set; }
        
        public ObservableCollection<PickUpPointEntityExtension> PickUpPoints { get; set; }

        public CarEntity? Car { get; set; }

        public DateTime? Date { get => _date; set => SetProperty(ref _date, value); }

        public DriverEntity? Driver { get; set; }
        public List<PickUpPointEntity>? ChoosedPoints { get; set; }
        public Command SaveCommand { get; }
        public List<MapPoint> Pins { get; }


        public TripRequestVM()
        {
            Cars = [];
            Pins = [];
            Drivers = [];
            PickUpPoints = [];
            ChoosedPoints = [];
            SaveCommand = new Command(OnSave,CanExecute);
            PropertyChanged += (_,__)=>SaveCommand.ChangeCanExecute();
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
            foreach (var point in pickUpPoints.OrderBy(x=>x.Name))
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

        private bool CanExecute()
        {
            return Car is not null
                && Driver is not null
                && PickUpPoints.Count(x => x.IsChecked) >1;
        }

        private async void OnSave()
        {
            using var db = new AppDbContext();
            ChoosedPoints = PickUpPoints.Where(x=>x.IsChecked).Select(x=>x as PickUpPointEntity).ToList();

            var intermediate = ChoosedPoints.Skip(1).SkipLast(1).Select(x => x.Id).ToList();
            RouteEntity route = new()
            {
                DeparturePointId = ChoosedPoints[0].Id,
                ArrivalPointId = ChoosedPoints[^1].Id,
                IntermediatePointsId = intermediate??Enumerable.Empty<Guid>().ToList(),
            };
            var trip = new TripEntity()
            {
                CarId = Car!.Id,
                DriverId = Driver!.Id,
                CarName = Car!.Name,
                DriverName = Driver!.Name,
                StartAt = Date??DateTime.Now,
                Route=route,
                Status=Enums.TripStatus.Запланирован,
            };
            await db.Trips.AddAsync(trip, default);
            await db.SaveChangesAsync(default);
            await Shell.Current.GoToAsync("..");
        }

        public async Task DrawRoute()
        {
            // Используйте подходящий API для Bing Maps, 
            // чтобы получить данные о маршруте.
            // Пример с использованием HttpClient:
            if (PickUpPoints.Where(x => x.IsChecked).Count() < 2)
                return;
            var routeTask = await RouteTask.CreateAsync(new Uri("https://route-api.arcgis.com/arcgis/rest/services/World/Route/NAServer/Route_World"));
            RouteParameters routeParams = await routeTask.CreateDefaultParametersAsync();
            var routes = new List<MapPoint>(PickUpPoints.Where(x => x.IsChecked).Select(x => new MapPoint(x.Longitude, x.Latitude)));
            var first = new Stop(routes.First());
            // Установка исходной и конечной точек
            var solve=SolveTravellingSalesmanProblem(routes).Select(x => new Stop(x)).ToList();
            var polylines = new List<Polyline>();
            GraphicsOverlays![1].Graphics.Clear();
            while (solve.Count>1)
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
                GraphicsOverlays![1].Graphics.Add(polylineGraphic);
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
            GraphicsOverlays![1].Graphics.Add(polylineGraphic1);
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

}
