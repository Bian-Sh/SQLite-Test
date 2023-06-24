using SQLite;

[Table("Teachers")]
public class BaseEntity
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int id { get; set; }
}