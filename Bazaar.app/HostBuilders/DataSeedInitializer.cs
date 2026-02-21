using Bazaar.Entityframework.DbContext;
using Bazaar.Entityframework.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Bazaar.app.HostBuilders
{
    public static class DataSeedInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "DataSeed.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Seed file not found at: {filePath}");
                return;
            }

            var jsonString = await File.ReadAllTextAsync(filePath);
            using var doc = JsonDocument.Parse(jsonString);
            var root = doc.RootElement;

            if (root.TryGetProperty("Cities", out var citiesJson))
            {
                foreach (var cityJson in citiesJson.EnumerateArray())
                {
                    var enName = cityJson.GetProperty("En").GetString();
                    if (!await context.Cities.AnyAsync(c => c.EnglishName == enName))
                    {
                        context.Cities.Add(new City
                        {
                            ArabicName = cityJson.GetProperty("Ar").GetString(),
                            EnglishName = enName
                        });
                    }
                }
            }

            if (root.TryGetProperty("Manufacturers", out var manufacturersJson))
            {
                foreach (var mJson in manufacturersJson.EnumerateArray())
                {
                    var mName = mJson.GetProperty("Name").GetString();
                    var manufacturer = await context.Manufacturers
                        .FirstOrDefaultAsync(m => m.Name == mName);

                    if (manufacturer == null)
                    {
                        manufacturer = new Manufacturer { Name = mName! };
                        context.Manufacturers.Add(manufacturer);
                        await context.SaveChangesAsync(); 
                    }
                    foreach (var modelJson in mJson.GetProperty("Models").EnumerateArray())
                    {
                        var modelName = modelJson.GetProperty("Name").GetString();
                        if (!await context.vehicleModels.AnyAsync(v => v.Name == modelName && v.ManufacturerId == manufacturer.Id))
                        {
                            context.vehicleModels.Add(new VehicleModel
                            {
                                Name = modelName!,
                                Category = (Category)modelJson.GetProperty("Cat").GetInt32(),
                                ManufacturerId = manufacturer.Id
                            });
                        }
                    }
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
