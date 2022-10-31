using vareAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace vareAPI.Services;

public class VareService
{
    private readonly IMongoCollection<Vare> _vareCollection;

    public VareService(
        IOptions<VareDatabaseSettings> vareDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            vareDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            vareDatabaseSettings.Value.DatabaseName);

        _vareCollection = mongoDatabase.GetCollection<Vare>(
            vareDatabaseSettings.Value.VareCollectionName);
    }

    public async Task<List<Vare>> GetAsync() =>
        await _vareCollection.Find(_ => true).ToListAsync();

    public async Task<Vare?> GetAsync(string id) =>
        await _vareCollection.Find(x => x.ProductId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Vare newVare) =>
        await _vareCollection.InsertOneAsync(newVare);

    public async Task UpdateAsync(string id, Vare updatedVare) =>
        await _vareCollection.ReplaceOneAsync(x => x.ProductId == id, updatedVare);

    public async Task RemoveAsync(string id) =>
        await _vareCollection.DeleteOneAsync(x => x.ProductId == id);
}