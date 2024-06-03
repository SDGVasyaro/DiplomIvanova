using DiplomIvanova.ViewModels.BaseViewModels;
using DiplomIvanova.ViewModels.MainPage;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace DiplomIvanova
{
    public partial class MainPage : ContentPage
    {
        MainPageVM viewModel;
        public MainPage()
        {
            BindingContext = viewModel = new();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private void routeMap_MapClicked(object sender, Microsoft.Maui.Controls.Maps.MapClickedEventArgs e)
        {
            

                // Add the Polyline to the map's MapElements collection
                //routeMap.MapElements.Add(polyline);

        }

        private async void DrawRoute(Location start, Location end)
        {
            // Используйте подходящий API для Bing Maps, 
            // чтобы получить данные о маршруте.
            // Пример с использованием HttpClient:
            var httpClient = new HttpClient();
            var bingMapsApiKey = "u7EGInfKGuIz5RygcVVH~Z56wGuow2DopibKxwXC6Mg~AgA16K0pMysGzoPNo4co02s9iYElmAQ7727E5EMHsmcw4jVnfkSqIXGZENZEbWk_"; // Замените на ваш ключ API
            var requestUri = $"https://dev.virtualearth.net/REST/v1/Routes?wp.0={start.Latitude},{start.Longitude}&wp.1={end.Latitude},{end.Longitude}&key={bingMapsApiKey}";

            try
            {
                var response = await httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                // Обработайте ответ API Bing Maps (responseString) и извлеките 
                // координаты маршрута. Формат ответа зависит от 
                // используемого API Bing Maps.

                // Пример: предположим, что координаты маршрута хранятся 
                // в свойстве "routeCoordinates" JSON-ответа.
                var routeData = JsonConvert.DeserializeObject<dynamic>(responseString);
                var positions = new List<Location>();

                // Проходим по всем наборам ресурсов
                foreach (var resourceSet in routeData.resourceSets)
                {
                    // Проходим по всем ресурсам в наборе
                    foreach (var resource in resourceSet.resources)
                    {
                        // Проходим по всем сегментам маршрута
                        foreach (var routeLeg in resource.routeLegs)
                        {
                            // Проходим по всем точкам маршрута в сегменте
                            foreach (var point in routeLeg.itineraryItems)
                            {
                                // Получаем координаты точки
                                var latitude = (double)point.maneuverPoint.coordinates[0];
                                var longitude = (double)point.maneuverPoint.coordinates[1];

                                // Создаем объект Position и добавляем его в список
                                positions.Add(new Location(latitude, longitude));
                            }
                        }
                    }
                }
         
            }
            catch (Exception ex)
            {
                // Обработайте ошибки запроса к API Bing Maps
                Console.WriteLine($"Ошибка при получении маршрута: {ex.Message}");
            }
        }

        // Вспомогательный метод для извлечения координат маршрута 
        // из JSON-ответа Bing Maps API (адаптируйте под формат ответа)
        private List<Location> ExtractRouteCoordinatesFromJson(string json)
        {
            // TODO: Реализуйте парсинг JSON-ответа Bing Maps API 
            // и извлечение координат маршрута.
            // Используйте библиотеку для работы с JSON, например, Newtonsoft.Json.
            return null;
        }
    }

}
