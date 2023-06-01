using SQLite;

[Table("Courses")]
public class Course
{
    [PrimaryKey,AutoIncrement]
    [Column("id")]
    public int id { get; set; }

    [Column("name")]
    public string name { get; set; }

    [Column("description")]
    public string description { get; set; }

    [Indexed]
    [Column("teacherId")]
    public int teacherId { get; set; }
}
