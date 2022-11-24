namespace vareAPI.Models;

public class VareDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string VareCollectionName { get; set; } = null!;
}