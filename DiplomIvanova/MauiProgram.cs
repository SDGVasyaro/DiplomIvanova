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
                .UseMauiCommunityToolkitMaps("u7EGInfKGuIz5RygcVVH~Z56wGuow2DopibKxwXC6Mg~AgA16K0pMysGzoPNo4co02s9iYElmAQ7727E5EMHsmcw4jVnfkSqIXGZENZEbWk_")
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
            using (var db = new AppDbContext())
            {
                if (await db.Database.EnsureCreatedAsync())
                {
                    var cancellationToken = new CancellationToken();
                    var carInfo = new CarEntity()
                    {
                        Name = "BMW",
                        DateOfCommissioning = DateTime.Now,
                        Mileage = 100,
                        Number = "a666aa76",
                        Сapacity = 10d,
                    };
                    var carInfo1 = new CarEntity()
                    {
                        Name = "Mercedes",
                        DateOfCommissioning = DateTime.Now,
                        Mileage = 70,
                        Number = "a656aa76",
                        Сapacity = 9d,
                    };
                    var carInfo2 = new CarEntity()
                    {
                        Name = "Volvo",
                        DateOfCommissioning = DateTime.Now,
                        Mileage = 140,
                        Number = "a623aa88",
                        Сapacity = 10d,
                    };
                    var carInfo3 = new CarEntity()
                    {
                        Name = "Volkswagen",
                        DateOfCommissioning = DateTime.Now,
                        Mileage = 80,
                        Number = "м666aa76",
                        Сapacity = 10d,
                    };
                    var carInfo4 = new CarEntity()
                    {
                        Name = "Chevrolet",
                        DateOfCommissioning = DateTime.Now,
                        Mileage = 99,
                        Number = "c666aa76",
                        Сapacity = 5d,
                    };
                    var carInfo5 = new CarEntity()
                    {
                        Name = "Lada",
                        DateOfCommissioning = DateTime.Now,
                        Mileage = 100,
                        Number = "к666aa76",
                        Сapacity = 10d,
                    };
                    var carInfo6 = new CarEntity()
                    {
                        Name = "Audi",
                        DateOfCommissioning = DateTime.Now,
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
            }

        }
    }
}
