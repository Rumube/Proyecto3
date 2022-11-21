using SQLite4Unity3d;

public class MatchDB
{
    [PrimaryKey, AutoIncrement]
    public int idMatch { get; set; }
    public string idStudent { get; set; }
    public string idSession { get; set; }
    public string idGame { get; set; }
    public string team { get; set; }
    public string level { get; set; }
    public string averageSuccess { get; set; }
    public string averageErrors { get; set; }
    public string averagePoints { get; set; }
    public string averageTime { get; set; }
}
