using MongoDB.Driver;
using SearchService.Models;

namespace SearchService;

public class SearchDbContext
{
    private readonly IMongoCollection<Item> _itemCollection;

    public SearchDbContext()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        _itemCollection = client.GetDatabase("SearchDB").GetCollection<Item>("Item");
    }
}
