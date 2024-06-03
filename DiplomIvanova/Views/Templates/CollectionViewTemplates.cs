using DiplomIvanova.DataBase.Entities;
using DiplomIvanova.Enums;

namespace DiplomIvanova.Views.Templates
{
    public static class CollectionViewTemplates
    {
        
        public static DataTemplate CarsDataTemplate => new(()=>
        {
            var stack=new Grid()
            {
                Padding = 10
            };
            Image delete = new()
            {
                Source = ImageSource.FromFile("delete.png")
            };
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.33, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.33, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.25, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.08, GridUnitType.Star) });
            BoxView boxView = new()
            {
                HeightRequest = 1,
                Color = Color.FromRgba(0,0,0,255),
            };

            Entry name = new();
            Entry number = new();
            Entry capacity = new();
            name.SetBinding(Entry.TextProperty, nameof(CarEntity.Name), BindingMode.TwoWay);
            number.SetBinding(Entry.TextProperty, nameof(CarEntity.Number),BindingMode.TwoWay);
            capacity.SetBinding(Entry.TextProperty, nameof(CarEntity.Mileage), BindingMode.TwoWay);
            stack.Add(name);
            stack.Add(number,1,0);
            stack.Add(capacity,2,0);
            stack.Add(delete, 3, 0);
            return stack;
        });
        public static Grid GetCarsHeaderTemplate()
        {
            var stack = new Grid()
            {
                Padding = 10
            };
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.33, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.33, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.33, GridUnitType.Star) });

            Label name = new() { Text = "Наименование" };
            Label number = new() { Text = "Номер" };
            Label capacity = new() { Text = "Пробег/км" };
            stack.Add(name);
            stack.Add(number, 1, 0);
            stack.Add(capacity, 2, 0);

            return stack;
        }
        public static DataTemplate DriversDataTemplate => new(() =>
        {
            var stack = new Grid()
            {
                Padding = 10
            };
            Image delete = new()
            {
                Source = ImageSource.FromFile("delete.png")
            };
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.5, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.42, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.08, GridUnitType.Star) });

            Entry name = new();
            Entry exp = new();

            name.SetBinding(Entry.TextProperty, nameof(DriverEntity.Name));
            exp.SetBinding(Entry.TextProperty, nameof(DriverEntity.Experience));
            stack.Add(name);
            stack.Add(exp,1);
            stack.Add(delete,2);

            return stack;
        });
        public static Grid GetDriversHeaderTemplate()
        {
            var stack = new Grid()
            {
                Padding = 10
            };
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.5, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.5, GridUnitType.Star) });

            Label name = new() { Text = "ФИО" };
            Label exp = new() { Text = "Стаж" };
            stack.Add(name);
            stack.Add(exp, 1, 0);
            return stack;
        }
        public static DataTemplate PickUpPointDataTemplate => new(() =>
        {
            var stack = new Grid()
            {
                Padding = 10
            };
            Image delete = new()
            {
                Source = ImageSource.FromFile("delete.png")
            };
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.5, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.42, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.08, GridUnitType.Star) });
            Label name = new();
            Label adress = new();
            name.SetBinding(Label.TextProperty, nameof(PickUpPointEntity.Name));
            adress.SetBinding(Label.TextProperty, nameof(PickUpPointEntity.Adress));
            stack.Add(name);
            stack.Add(adress,1);
            stack.Add(delete,2);
            return stack;
        });
        public static Grid GetPickUpPointHeaderTemplate()
        {
            var stack = new Grid()
            {
                Padding = 10
            };
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.5, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.5, GridUnitType.Star) });

            Label name = new() { Text = "Наименование" };
            Label adress = new() { Text = "Адресс" };
            stack.Add(name);
            stack.Add(adress,1);

            return stack;
        }
        public static DataTemplate TripsDataTemplate => new(() =>
        {
            var stack = new Grid()
            {
                Padding = 10,
                ColumnSpacing=3,
            };
            Image delete = new()
            {
                Source = ImageSource.FromFile("delete.png")
            };
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.2, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.25, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.18, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.29, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.07, GridUnitType.Star) });
            Label start = new();
            Label durr = new();
            Label driver = new();
            Label car = new();
            Picker status = new();
            //var statuses = new List<TripStatus>()
            //{
            //    TripStatus.Запланирован,
            //    TripStatus.В_пути,
            //    TripStatus.Завершен,
            //};
            var statuses = new List<string>()
            {
                "Запланирован",
                "В пути",
                "Завершен",
            };
            status.ItemsSource = statuses;
            status.SelectedIndex = 0;
            //status.SetBinding(Picker.SelectedItemProperty, nameof(TripEntity.Status));
            start.SetBinding(Label.TextProperty, nameof(TripEntity.StartAt));
            driver.SetBinding(Label.TextProperty, nameof(TripEntity.DriverName));
            car.SetBinding(Label.TextProperty, nameof(TripEntity.CarName));
            stack.Add(start);
            stack.Add(driver,1);
            stack.Add(car,2);
            stack.Add(status,3);
            stack.Add(delete, 4);
            //stack.Add(boxView);
            return stack;
        });
        public static Grid GetTripsHeaderTemplate()
        {
            var stack = new Grid()
            {
                Padding = 10
            };
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.2, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.26, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.25, GridUnitType.Star) });
            stack.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(0.28, GridUnitType.Star) });

            Label start = new() { Text = "Дата Начала" };
            Label driver = new() { Text = "Водитель" };
            Label car = new() { Text = "Автомобиль" };
            Label status = new() { Text = "Статус" };
            stack.Add(start);
            stack.Add(driver,1);
            stack.Add(car,2);
            stack.Add(status, 3);

            return stack;
        }
    }
}
