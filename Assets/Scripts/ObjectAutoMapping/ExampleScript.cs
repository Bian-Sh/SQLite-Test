using UnityEngine;
using System.IO;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Linq;

[ExecuteAlways]
public class ExampleScript : MonoBehaviour
{
    private SQLiteConnection _connection;

    private void OnEnable()
    {
        // Open a connection to the database
        string dbPath = Path.Combine(Application.streamingAssetsPath, "database.db");
        if (File.Exists(dbPath))
        {
            File.Delete(dbPath);
        }
        _connection = new SQLiteConnection(dbPath);

        // Create tables if they don't exist
        _connection.CreateTable<A>();
        _connection.CreateTable<B>();

        // Create some fake data
        B b1 = new B { name = "B1", a_list = new List<A>() };
        B b2 = new B { name = "B2", a_list = new List<A>() };

        A a1 = new A { b = b1, name = "A1" };
        A a2 = new A { b = b1, name = "A2" };
        A a3 = new A { b = b2, name = "A3" };
        A a4 = new A { b = b2, name = "A4" };

        b1.a_list.Add(a1);
        b1.a_list.Add(a2);
        b2.a_list.Add(a3);
        b2.a_list.Add(a4);

        _connection.InsertWithChildren(b1);
        _connection.InsertWithChildren(b2);

        // Query for A object and load its related B object
        var results = _connection.GetAllWithChildren<A>(recursive: true);

        foreach (var a in results)
        {
            Debug.Log($"A is: {a.name}, B is: {a.b.name}, B.a_list Count {a.b.a_list.Count}");

            foreach (var relatedA in a.b.a_list)
            {
                Debug.Log($"\tB : {a.b.name}, Related A : {relatedA.name}");
            }
        }

        Debug.Log($"{nameof(ExampleScript)}:  Query with filter ");
        // Query for A object and load its related B object by filter x=>x.name==B1;
        // 不要试图访问 B的实例作为 filter 来查询，因为B还未实例化： var result = _connection.GetAllWithChildren<A>(a => a.b.name == "B1", recursive: true); 
        // 可以使用 a 实例中的 ForeignKey 做查询，缺点是 需要知道过滤的 id 的常数值。 var results2 = _connection.GetAllWithChildren<A>(a => a.b_id == 1, recursive: true);
        var results2 = _connection.GetAllWithChildren<A>(a => a.b_id == 1, recursive: true);
        // 这个虽然可以，但是效率低下，因为创建了所有的 A 实例。 var results2 = _connection.GetAllWithChildren<A>(recursive: true).Where(a => a.b.name == "B1");

        foreach (var a in results2)
        {
            Debug.Log($"A is: {a.name}, B is: {a.b.name}, B.a_list Count {a.b.a_list.Count}");

            foreach (var relatedA in a.b.a_list)
            {
                Debug.Log($"\tB : {a.b.name}, Related A : {relatedA.name}");
            }
        }


        //  Close the connection
        _connection.Close();
    }
    public class A
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [Column("name")]
        public string name { get; set; }
        [ForeignKey(typeof(B))]
        public int b_id { get; set; }

        [ManyToOne]
        public B b { get; set; }
    }

    public class B
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<A> a_list { get; set; }
    }

}
