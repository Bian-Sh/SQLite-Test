using SQLite;

[Table("Teacher")]
public class Teacher : BaseEntity
{
    [Column("name")]
    public string name { get; set; }

    [Column("email")]
    public string email { get; set; }

    [Column("password")]
    public string password { get; set; }
}
