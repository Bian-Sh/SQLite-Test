using SQLite;

[Table("Teachers")]
public class Teacher
{
    [PrimaryKey,AutoIncrement]
    [Column("teacher_id")]
    public int teacher_id { get; set; }

    [Column("name")]
    public string name { get; set; }

    [Column("email")]
    public string email { get; set; }

    [Column("password")]
    public string password { get; set; }
}
