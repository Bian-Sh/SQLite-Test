using SQLite;
using System.Linq;
using UnityEngine;
using EasyButtons;

/*
 本代码演示了如下 SQLite 数据库操作：
1. 插入数据
2. 查询数据
3. 更新数据
4. 删除数据
5. 联合查询

本代码使用了 EasyButtons 插件，方便 Inspector 上直接点击运行测试逻辑。
编辑器点击 Play ，然后再 此脚本的编辑器面板是点击对应的按钮测试。
 */

public class SQLHowTo : MonoBehaviour
{
    private SQLiteConnection db;

    private void Awake()
    {
        var dbPath = $"{Application.streamingAssetsPath}/test.db";
        db = new SQLiteConnection(dbPath);
    }
    private void OnDestroy()
    {
        db.Close();
    }

    [Button]
    public void InsertCourse() //插入课程
    {
        var newCourse = new Course()
        {
            name = "新课程",
            description = "这是一门新课程",
            teacherId = 1,
        };
        db.Insert(newCourse);
        Debug.Log($"{nameof(SQLHowTo)}: 请通过 SqliteBrowser 查看新增的课程 {name}");
    }

    [Button]

    public void QueryCourses()//查询所有课程
    {
        var courses = db.Table<Course>().ToList();
        // 可以在这里对 courses 进行处理，此处只是简单的打印
        foreach (var course in courses)
        {
            Debug.Log(course.name);
        }
    }

    [Button]

    public void UpdateCourse() //更新课程
    {
        var course = db.Table<Course>().Where(c => c.id == 1).FirstOrDefault();
        Debug.Log($"{nameof(SQLHowTo)}: 请通过 SqliteBrowser 查看课程的更新，before  {course.name}");
        if (course != null)
        {
            course.name = "更新后的课程名称";
            db.Update(course);
        }
        Debug.Log($"{nameof(SQLHowTo)}: 请通过 SqliteBrowser 查看课程的更新，after  {course.name}");
    }

    [Button]
    public void DeleteCourse() //删除课程
    {
        var course = db.Table<Course>().Where(c => c.id == 1).FirstOrDefault();
        if (course != null)
        {
            db.Delete(course);
        }
        Debug.Log($"{nameof(SQLHowTo)}: 请通过 SqliteBrowser 查看课程的删除， {course.name} 将消失");
    }

    [Button]
    public void QueryCoursesWithTeachers() // 对课程和关联教师进行联合查询
    {
        var query = db.Table<Course>().Select(c => new
        {
            CourseName = c.name,
            TeacherName = db.Table<Teacher>().Where(t => t.teacher_id == c.teacherId).FirstOrDefault().name
        });
        // 可以在这里对 query 进行处理，此处只是简单的打印
        foreach (var item in query)
        {
            Debug.Log($"{nameof(SQLHowTo)}: 课程名称：{item.CourseName}，教师名称：{item.TeacherName}");
        }
    }
}
