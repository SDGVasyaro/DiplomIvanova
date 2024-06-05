using DiplomIvanova.DataBase.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomIvanova.Views.Templates
{
    public  class AdditionTemplates
    {
        public static (View content,Dictionary<Entry,string> fields) CarsDataTemplate {
            get  
            {
                var dictionary = new Dictionary<Entry,string>();
                var stack = new Grid()
                {
                    Padding = 10
                };
                ImageButton add = new()
                {
                    Source = ImageSource.FromFile("add.png"),
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

                stack.Add(nameEntry);
                stack.Add(numberEntry);
                stack.Add(mileageEntry);

                return (stack,dictionary);
            } 
        }
        public static (View content, Dictionary<Entry, string> fields) DriversDataTemplate
        {
            get
            {
                var dictionary = new Dictionary<Entry, string>();
                var stack = new VerticalStackLayout();

                Entry nameEntry = new();

                dictionary.Add(nameEntry, nameof(DriverEntity.Name));
                dictionary.Add(nameEntry, nameof(DriverEntity.Experience));

                //stack.Add(name);
                stack.Add(nameEntry);
                return (stack, dictionary);
            }
        }
        
        //public static DataTemplate StorageDataTemplate => new(() =>
        //{
        //    var stack = new VerticalStackLayout();
        //    Label name = new();
        //    //Label adress = new();
        //    //name.SetBinding(Label.TextProperty, nameof(StorageEntity.Name));
        //    //adress.SetBinding(Label.TextProperty, nameof(StorageEntity.adress));
        //    stack.Add(name);
        //    //stack.Add(adress);
        //    return stack;
        //});
        //public static DataTemplate PickUpPointDataTemplate => StorageDataTemplate;
        //public static DataTemplate ProductsDataTemplate => new(() =>
        //{
        //    var stack = new VerticalStackLayout();
        //    Label name = new();
        //    Label weight = new();
        //    Label description = new();
        //    name.SetBinding(Label.TextProperty, nameof(ProductEntity.Name));
        //    weight.SetBinding(Label.TextProperty, nameof(ProductEntity.Weight));
        //    description.SetBinding(Label.TextProperty, nameof(ProductEntity.Description));
        //    stack.Add(name);
        //    stack.Add(weight);
        //    stack.Add(description);
        //    return stack;
        //});
        //public static DataTemplate RoutesDataTemplate => new(() =>
        //{
        //    var stack = new VerticalStackLayout();
        //    Label id = new();
        //    Label storage = new();
        //    Label weight = new();
        //    id.SetBinding(Label.TextProperty, nameof(RouteEntity.Id));
        //    //storage.SetBinding(Label.TextProperty, nameof(RouteEntity.Storage.Name));
        //    //weight.SetBinding(Label.TextProperty, nameof(RouteEntity.Weight));
        //    stack.Add(id);
        //    stack.Add(weight);
        //    stack.Add(storage);
        //    return stack;
        //});
        //public static DataTemplate TripsDataTemplate => new(() =>
        //{
        //    var stack = new VerticalStackLayout();
        //    Label started = new();
        //    Label ended = new();
        //    Label status = new();
        //    Label auto = new();
        //    Label driver = new();
        //    started.SetBinding(Label.TextProperty, nameof(TripEntity.StartAt));
        //    ended.SetBinding(Label.TextProperty, nameof(TripEntity.EndAt));
        //    status.SetBinding(Label.TextProperty, nameof(TripEntity.Status));
        //    auto.SetBinding(Label.TextProperty, nameof(TripEntity.Car.Number));
        //    driver.SetBinding(Label.TextProperty, nameof(TripEntity.Driver.Name));
        //    stack.Add(started);
        //    stack.Add(ended);
        //    stack.Add(status);
        //    stack.Add(auto);
        //    stack.Add(driver);
        //    return stack;
        //});
        //public static DataTemplate TripRequestsDataTemplate => new(() =>
        //{
        //    var stack = new VerticalStackLayout();
        //    Label id = new();
        //    Label client = new();
        //    Label products = new();
        //    Label date = new();
        //    id.SetBinding(Label.TextProperty, nameof(TripRequestEntity.Id));
        //    client.SetBinding(Label.TextProperty, nameof(TripRequestEntity.Client.Name));
        //    products.SetBinding(Label.TextProperty, nameof(TripRequestEntity.Products.Count));
        //    //date.SetBinding(Label.TextProperty, nameof(TripRequestEntity.CreatedAt));
        //    stack.Add(id);
        //    stack.Add(client);
        //    stack.Add(products);
        //    stack.Add(date);
        //    return stack;
        //});
    }
}
