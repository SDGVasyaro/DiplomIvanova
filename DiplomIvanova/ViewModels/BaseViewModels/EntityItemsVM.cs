using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.Helpers;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Microsoft.Maui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomIvanova.ViewModels.BaseViewModels
{
    public class EntityItemsVM<T>:ItemsViewModel<T> where T : class, IEntityBase, new()
    {
        private MapPoint _currPointLocation;

        public Command<Guid> DeleteCommand { get; }

        public Command UpdateCommand { get; }

        public EntityItemsVM() : base()
        {
            _entity = new();
            UpdateCommand = new(OnUpdate);
            DeleteCommand = new(OnDelete);
        }
        protected override async Task LoadItemsAsync()
        {
            (await GetDbItemsAsync<T>()).ForEach(PreloadItems.Add);
            Items.Clear();
            PreloadItems.ForEach(Items.Add);
        }

        private async void OnUpdate()
        {
            try
            {
                foreach (var item in Items)
                {
                    await DataBaseHelper.UpdateItemAsync(item);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "Ок");
                return;
            }
            await Shell.Current.DisplayAlert("Данные обновлены", "Данные обновлены", "Ок");
        }

        private async void OnDelete(Guid id)
        {
            try
            {
                await DataBaseHelper.DeleteItemAsync<T>(id);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "Ок");
                return;
            }
            var item = Items.FirstOrDefault(x => x.Id == id);
            Items.Remove(item!);
        }

        protected override void OnItemTapped(T item)
        {
        }

        public (View content, Dictionary<Entry, string> fields) CarsAddDataTemplate
        {
            get
            {
                var dictionary = new Dictionary<Entry, string>();
                var stack = new Grid()
                {
                    Padding = 10
                };
                ImageButton add = new()
                {
                    Source = ImageSource.FromFile("add.png"),
                    Command = SaveCommand
                };
                stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.33, GridUnitType.Star) });
                stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.33, GridUnitType.Star) });
                stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.25, GridUnitType.Star) });
                stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.08, GridUnitType.Star) });

                Entry nameEntry = new();
                Entry numberEntry = new();
                Entry mileageEntry = new();

                dictionary.Add(nameEntry, nameof(CarEntity.Name));
                dictionary.Add(numberEntry, nameof(CarEntity.Number));
                dictionary.Add(mileageEntry, nameof(CarEntity.Mileage));
                _properties= dictionary;
                
                stack.Add(nameEntry);
                stack.Add(numberEntry,1);
                stack.Add(mileageEntry,2);
                stack.Add(add, 3);

                return (stack, dictionary);
            }
        }
        public (View content, Dictionary<Entry, string> fields) DriversAddDataTemplate
        {
            get
            {
                var dictionary = new Dictionary<Entry, string>();
                var stack = new Grid()
                {
                    Padding = 10
                };
                ImageButton add = new()
                {
                    Source = ImageSource.FromFile("add.png"),
                    Command = SaveCommand
                };
                stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.5, GridUnitType.Star) });
                stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.42, GridUnitType.Star) });
                stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.08, GridUnitType.Star) });

                Entry name = new() { IsSpellCheckEnabled = false };
                Entry exp = new() { IsSpellCheckEnabled = false };

                stack.Add(name);
                stack.Add(exp, 1);
                stack.Add(add, 2);


                dictionary.Add(name, nameof(DriverEntity.Name));
                dictionary.Add(exp, nameof(DriverEntity.Experience));
                _properties = dictionary;

                return (stack, dictionary);
            }
        }

        public (View content, Dictionary<Entry, string> fields) PointsAddDataTemplate
        {
            get
            {
                var dictionary = new Dictionary<Entry, string>();
                var stack = new Grid()
                {
                    Padding = 10
                };
                ImageButton add = new()
                {
                    Source = ImageSource.FromFile("add.png"),
                    Command = SaveCommand
                };
                stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.5, GridUnitType.Star) });
                stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.42, GridUnitType.Star) });
                stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.08, GridUnitType.Star) });

                Entry name = new() { IsSpellCheckEnabled = false };
                Entry address = new() { IsSpellCheckEnabled = false };
                address.TextChanged += Address_TextChanged;

                stack.Add(name);
                stack.Add(address, 1);
                stack.Add(add, 2);


                dictionary.Add(name, nameof(PickUpPointEntity.Name));
                dictionary.Add(address, nameof(PickUpPointEntity.Adress));
                _properties = dictionary;

                return (stack, dictionary);
            }
        }
        CancellationTokenSource _cts = new();
        string currQuery;
        private async void Address_TextChanged(object? sender, TextChangedEventArgs e)
        {
            try
            {
                _cts.Cancel();
                _cts = new();
                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                    return;
                currQuery = e.NewTextValue;
                await Task.Delay(3000);
                await GetAdressAsync(currQuery, _cts.Token);
            }
            catch (Exception ex)
            {
                _cts.Cancel();
            }
        }

        private T _entity;
        private Dictionary<Entry, string> _properties;
        public Command SaveCommand => new(AddEntityToDb);

        private async Task FillPropertiesAsync()
        {
            foreach (var property in _properties)
            {
                if (string.IsNullOrWhiteSpace(property.Key.Text))
                    continue;
                try
                {
                    var prop = _entity.GetType().GetProperty(property.Value);
                    if (prop != null)
                    {
                        if (prop.PropertyType.IsAssignableFrom(typeof(double)))
                        {
                            prop.SetValue(_entity, double.Parse(property.Key.Text));
                        }
                        if (prop.PropertyType.IsAssignableFrom(typeof(int?))|| prop.PropertyType.IsAssignableFrom(typeof(int)))
                        {
                            prop.SetValue(_entity, int.Parse(property.Key.Text));
                        }
                        if (prop.PropertyType.IsAssignableFrom(typeof(string)))
                        {
                            prop.SetValue(_entity, property.Key.Text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Ошибка", "Поле не должно содержать буквы", "Ок");
                    throw;
                }
            }
        }

        private async Task GetAdressAsync(string query,CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query)||cancellationToken.IsCancellationRequested)
                return;
            Uri _serviceUri = new Uri("https://geocode-api.arcgis.com/arcgis/rest/services/World/GeocodeServer");
            LocatorTask _geocoder = await LocatorTask.CreateAsync(_serviceUri);
            GeocodeParameters geocodeParameters = new()
            {
                OutputLanguage = new("ru-RU"),
                CountryCode = "+7"
            };
            // Reverse geocode to get addresses.
            IReadOnlyList<GeocodeResult> addresses = await _geocoder.GeocodeAsync(query, geocodeParameters, default);

            // Get the first result.
            GeocodeResult address = addresses.OrderByDescending(x=>x.Score).FirstOrDefault();
            
            // Use the city and region for the Callout Title.
            _currPointLocation = address?.DisplayLocation!;
            ClearMapPoints();
            await ZoomToMapPointAsync(_currPointLocation);
        }

        private void AddPickUpPoint(PickUpPointEntity pickUpPointEntity)
        {
            pickUpPointEntity.Longitude = _currPointLocation.X;
            pickUpPointEntity.Latitude = _currPointLocation.Y;
        }


        private async void AddEntityToDb()
        {
            try
            {
                await FillPropertiesAsync();
                if (_entity is PickUpPointEntity pickUpPointEntity)
                {
                    AddPickUpPoint(pickUpPointEntity);
                }
            }
            catch
            {
                return;
            }
            foreach (var item in _properties.Keys)
            {
                item.Text = "";
            }
            if (await DataBaseHelper.AddItemAsync(_entity))
            {
                Items.Add(_entity);
            }
        }
    }
}
