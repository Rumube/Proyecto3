using SQLite4Unity3d;
public class StudentDB
{
	[PrimaryKey, AutoIncrement]
	public int idStudent { get; set; }
	public string name { get; set; }
	public int idClassroom { get; set; }

	public override string ToString()
	{
		return string.Format("[Person: Id={0}, Name={1},  IdClassroom={2}]", idStudent, name, idClassroom);
	}
}
