using CommunityToolkit.Maui.Maps;
using DiplomIvanova.DataBase.Context;
using DiplomIvanova.DataBase.Entities;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace DiplomIvanova
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            InitDataBase();
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .UseArcGISRuntime(x=>x.UseApiKey("AAPK283232b584dd43af8134a1904f5c4e93NNfvzDYJJXH2yLEInk5GQSfbVJX3T-QiOnGlcO1PuMN3qSRgR-phBwnw15yUZOPh"))
                //.UseMauiCommunityToolkitMaps("u7EGInfKGuIz5RygcVVH~Z56wGuow2DopibKxwXC6Mg~AgA16K0pMysGzoPNo4co02s9iYElmAQ7727E5EMHsmcw4jVnfkSqIXGZENZEbWk_")
                .ConfigureEssentials()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        


        private static async void InitDataBase()
        {
            var cancellationToken = new CancellationToken();
            try
            {
                using (var db = new AppDbContext())
                {
                    if (db.Database.EnsureCreated())
                    {
                        await AddCarsAsync(db, cancellationToken);
                        await AddDriversAsync(db, cancellationToken);
                        await AddPointsAsync(db, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private static async Task AddCarsAsync(AppDbContext db, CancellationToken cancellationToken)
        {
            var carInfo = new CarEntity()
            {
                Name = "BMW",
                Mileage = 100,
                Number = "a666aa76",
                Сapacity = 10d,
            };
            var carInfo1 = new CarEntity()
            {
                Name = "Mercedes",
                Mileage = 70,
                Number = "a656aa76",
                Сapacity = 9d,
            };
            var carInfo2 = new CarEntity()
            {
                Name = "Volvo",
                Mileage = 140,
                Number = "a623aa88",
                Сapacity = 10d,
            };
            var carInfo3 = new CarEntity()
            {
                Name = "Volkswagen",
                Mileage = 80,
                Number = "м666aa76",
                Сapacity = 10d,
            };
            var carInfo4 = new CarEntity()
            {
                Name = "Chevrolet",
                Mileage = 99,
                Number = "c666aa76",
                Сapacity = 5d,
            };
            var carInfo5 = new CarEntity()
            {
                Name = "Lada",
                Mileage = 100,
                Number = "к666aa76",
                Сapacity = 10d,
            };
            var carInfo6 = new CarEntity()
            {
                Name = "Audi",
                Mileage = 100,
                Number = "в666aa76",
                Сapacity = 10d,
            };
            await db.Cars.AddAsync(carInfo, cancellationToken);
            await db.Cars.AddAsync(carInfo1, cancellationToken);
            await db.Cars.AddAsync(carInfo2, cancellationToken);
            await db.Cars.AddAsync(carInfo3, cancellationToken);
            await db.Cars.AddAsync(carInfo4, cancellationToken);
            await db.Cars.AddAsync(carInfo5, cancellationToken);
            await db.Cars.AddAsync(carInfo6, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        private static async Task AddDriversAsync(AppDbContext db, CancellationToken cancellationToken)
        {
            var carInfo = new DriverEntity()
            {
                Name = "Рождественский В.С.",
                Experience = 17,
            };
            var carInfo1 = new DriverEntity()
            {
                Name = "Антонов А.А.",
                Experience = 10,
            };
            var carInfo2 = new DriverEntity()
            {
                Name = "Пименов В.С.",
                Experience = 8,
            };
            var carInfo3 = new DriverEntity()
            {
                Name = "Игнатов Н.С.",
                Experience = 5,
            };
            var carInfo4 = new DriverEntity()
            {
                Name = "Соболев С.С.",
                Experience = 12,
            };
            var carInfo5 = new DriverEntity()
            {
                Name = "Григорьев Ш.П.",
                Experience = 14,
            };
            var carInfo6 = new DriverEntity()
            {
                Name = "Колунов И.А.",
                Experience = 13,
            };

            await db.Drivers.AddAsync(carInfo, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            await db.Drivers.AddAsync(carInfo1, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            await db.Drivers.AddAsync(carInfo2, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            await db.Drivers.AddAsync(carInfo3, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            await db.Drivers.AddAsync(carInfo4, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            await db.Drivers.AddAsync(carInfo5, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            await db.Drivers.AddAsync(carInfo6, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            //var carInfo7 = new DriverEntity()
            //{
            //    Name = "Стрельбовский К.А.",
            //    Experience = 21,
            //};
            //await db.Drivers.AddAsync(carInfo7, cancellationToken);
            //await db.SaveChangesAsync(cancellationToken);
        }
        private static async Task AddPointsAsync(AppDbContext db, CancellationToken cancellationToken)
        {
            var carInfo = new PickUpPointEntity()
            {
                Adress= "г.Москва, Звёздный бульвар, 19с1",
                Latitude = 55.812054,
                Longitude= 37.625981,
                Name="Пункт 1"
            };
            var carInfo2 = new PickUpPointEntity()
            {
                Adress = "г. Москва, Проспект Мира, 79",
                Latitude = 55.790980,
                Longitude = 37.634182,
                Name = "Пункт 2"
            };
            var carInfo3 = new PickUpPointEntity()
            {
                Adress = "улица Земляной Вал, 38-40/15с9",
                Latitude = 55.755404,
                Longitude = 37.656747,
                Name = "Пункт 3"
            };
            var carInfo4 = new PickUpPointEntity()
            {
                Adress = "г. Москва, 5-й Котельнический переулок, 11",
                Latitude = 55.741754,
                Longitude = 37.649256,
                Name = "Пункт 4"
            };
            var carInfo5 = new PickUpPointEntity()
            {
                Adress = "г.Москва, Малая Грузинская улица, 38",
                Latitude = 55.768099,
                Longitude = 37.572667,
                Name = "Пункт 5"
            };
            var carInfo6 = new PickUpPointEntity()
            {
                Adress = "г. Москва, Хорошёвское шоссе, 80",
                Latitude = 55.778346,
                Longitude = 37.522755,
                Name = "Пункт 6"
            };
            await db.PickUpPoints.AddAsync(carInfo, cancellationToken);
            await db.PickUpPoints.AddAsync(carInfo2, cancellationToken);
            await db.PickUpPoints.AddAsync(carInfo3, cancellationToken);
            await db.PickUpPoints.AddAsync(carInfo4, cancellationToken);
            await db.PickUpPoints.AddAsync(carInfo5, cancellationToken);
            await db.PickUpPoints.AddAsync(carInfo6, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
