using DiplomIvanova.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DiplomIvanova.Views.Templates
{
    public static class CollectionViewTemplates
    {
        public static DataTemplate CarsDataTemplate => new(()=>
        {
            var stack=new VerticalStackLayout()
            {
                Padding = 10
            };
            Label name = new();
            Label number = new();
            Label date = new();
            Label mileage = new();
            Label capacity = new();
            name.SetBinding(Label.TextProperty, nameof(CarEntity.Name));
            number.SetBinding(Label.TextProperty, nameof(CarEntity.Number));
            date.SetBinding(Label.TextProperty, nameof(CarEntity.DateOfCommissioning));
            mileage.SetBinding(Label.TextProperty, nameof(CarEntity.Mileage));
            capacity.SetBinding(Label.TextProperty, nameof(CarEntity.Сapacity));
            stack.Add(name);
            stack.Add(number);
            stack.Add(date);
            stack.Add(mileage);
            stack.Add(capacity);
            return stack;
        });
        public static DataTemplate ClientsDataTemplate => new(() =>
        {
            var stack = new VerticalStackLayout()
            {
                Padding = 10
            }; 
            Label name = new();
            Label number = new();
            Label adress = new();
            name.SetBinding(Label.TextProperty, nameof(ClientEntity.Name));
            number.SetBinding(Label.TextProperty, nameof(ClientEntity.Phone));
            adress.SetBinding(Label.TextProperty, nameof(ClientEntity.Adress));
            stack.Add(name);
            stack.Add(number);
            stack.Add(adress);
            return stack;
        });
        public static DataTemplate DriversDataTemplate => new(() =>
        {
            var stack = new VerticalStackLayout()
            {
                Padding = 10
            };
            Label name = new();
            name.SetBinding(Label.TextProperty, nameof(DriverEntity.Name));
            stack.Add(name);
            return stack;
        });
        public static DataTemplate StorageDataTemplate => new(() =>
        {
            var stack = new VerticalStackLayout()
            {
                Padding = 10
            };
            Label name = new();
            //Label adress = new();
            //name.SetBinding(Label.TextProperty, nameof(StorageEntity.Name));
            //adress.SetBinding(Label.TextProperty, nameof(StorageEntity.adress));
            stack.Add(name);
            //stack.Add(adress);
            return stack;
        });
        public static DataTemplate PickUpPointDataTemplate => StorageDataTemplate;
        public static DataTemplate ProductsDataTemplate => new(() =>
        {
            var stack = new VerticalStackLayout()
            {
                Padding = 10
            };
            Label name = new();
            Label weight = new();
            Label description = new();
            name.SetBinding(Label.TextProperty, nameof(ProductEntity.Name));
            weight.SetBinding(Label.TextProperty, nameof(ProductEntity.Weight));
            description.SetBinding(Label.TextProperty, nameof(ProductEntity.Description));
            stack.Add(name);
            stack.Add(weight);
            stack.Add(description);
            return stack;
        });
        public static DataTemplate RoutesDataTemplate => new(() =>
        {
            var stack = new VerticalStackLayout()
            {
                Padding = 10
            };
            Label id = new();
            Label storage = new();
            Label weight = new();
            id.SetBinding(Label.TextProperty, nameof(RouteEntity.Id));
            //storage.SetBinding(Label.TextProperty, nameof(RouteEntity.Storage.Name));
            //weight.SetBinding(Label.TextProperty, nameof(RouteEntity.Weight));
            stack.Add(id);
            stack.Add(weight);
            stack.Add(storage);
            return stack;
        });
        public static DataTemplate TripsDataTemplate => new(() =>
        {
            var stack = new VerticalStackLayout()
            {
                Padding = 10
            };
            Label started = new();
            Label ended = new();
            Label status = new();
            Label auto = new();
            Label driver = new();
            started.SetBinding(Label.TextProperty, nameof(TripEntity.StartAt));
            ended.SetBinding(Label.TextProperty, nameof(TripEntity.EndAt));
            status.SetBinding(Label.TextProperty, nameof(TripEntity.Status));
            auto.SetBinding(Label.TextProperty, nameof(TripEntity.Car.Number));
            driver.SetBinding(Label.TextProperty, nameof(TripEntity.Driver.Name));
            stack.Add(started);
            stack.Add(ended);
            stack.Add(status);
            stack.Add(auto);
            stack.Add(driver);
            return stack;
        });
        public static DataTemplate TripRequestsDataTemplate => new(() =>
        {
            var stack = new VerticalStackLayout()
            {
                Padding = 10
            };
            Label id = new();
            Label client = new();
            Label products = new();
            Label date = new();
            id.SetBinding(Label.TextProperty, nameof(TripRequestEntity.Id));
            client.SetBinding(Label.TextProperty, nameof(TripRequestEntity.Client.Name));
            products.SetBinding(Label.TextProperty, nameof(TripRequestEntity.Products.Count));
            //date.SetBinding(Label.TextProperty, nameof(TripRequestEntity.CreatedAt));
            stack.Add(id);
            stack.Add(client);
            stack.Add(products);
            stack.Add(date);
            return stack;
        });
    }
}
