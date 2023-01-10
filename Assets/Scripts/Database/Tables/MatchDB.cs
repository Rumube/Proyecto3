using SQLite4Unity3d;

public class MatchDB
{
    [PrimaryKey, AutoIncrement]
    public int idMatch { get; set; }
    public int idStudent { get; set; }
    public int idSession { get; set; }
    public int idGame { get; set; }
    public int team { get; set; }
    public int level { get; set; }
    public int averageSuccess { get; set; }
    public int averageErrors { get; set; }
    public int averagePoints { get; set; }
    public float averageTime { get; set; }
}
