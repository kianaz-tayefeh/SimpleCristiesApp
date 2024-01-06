using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDB", MongoClientSettings
      .FromConnectionString(app.Configuration.GetConnectionString("MongoDBConnection")));

        await DB.Index<Item>()
        .Key(a => a.Make, KeyType.Text)
        .Key(a => a.Model, KeyType.Text)
        .Key(a => a.Color, KeyType.Text)
        .CreateAsync();

        using var scope = app.Services.CreateScope();
        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

        var items = await httpClient.GetItemsForSearchDB();
        Console.WriteLine("items" + items);

        Console.WriteLine(items.Count + "returned from the auction service");

        if (items.Count > 0) await DB.SaveAsync(items);
    }
}
