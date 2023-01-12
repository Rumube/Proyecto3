using SQLite4Unity3d;

public class SessionDB
{
    [PrimaryKey, AutoIncrement]
    public int idSession { get; set; }
    public string dateSession { get; set; }
    public override string ToString()
    {
        return string.Format("[Session: Id={0}, Date={1}]", idSession, dateSession);
    }
}
