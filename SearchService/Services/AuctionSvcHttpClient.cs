using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

public class AuctionSvcHttpClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;


    public AuctionSvcHttpClient(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _config = configuration;
    }

    public async Task<List<Item>> GetItemsForSearchDB()
    {
        // we want the date of the auction that;s been updated. the latest in our database
        var lastUpdated = DB.Find<Item, string>()
        .Sort(a => a.Descending(a => a.UpdatedAt))
        .Project(a => a.UpdatedAt.ToString())
        .ExecuteFirstAsync();

        return await _http.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"]
             + "/api/auctions?date=" + lastUpdated);
    }
}
