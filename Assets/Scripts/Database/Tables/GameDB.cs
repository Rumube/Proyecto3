using SQLite4Unity3d;

public class GameDB
{
    [PrimaryKey, AutoIncrement]
    public int idGame { get; set; }
    public string name { get; set; }
    public override string ToString()
    {
        return string.Format("[Game: Id={0}, Name={1}]", idGame, name);
    }
}
