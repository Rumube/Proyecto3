using SQLite4Unity3d;

public class ClassroomDB
{
	[PrimaryKey, AutoIncrement]
	public int idClassroom { get; set; }
	public string name { get; set; }

	public override string ToString()
	{
		return string.Format("[Classroom: Id={0}, Name={1}]", idClassroom, name);
	}

	public int GetIDClassroom()
    {
		return idClassroom;
    }
}
