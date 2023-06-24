using SQLite;

[Table("Course")]
public class Course:BaseEntity
{
    [Column("name")]
    public string name { get; set; }

    [Column("description")]
    public string description { get; set; }

    [Indexed]
    [Column("teacherId")]
    public int teacherId { get; set; }
}
