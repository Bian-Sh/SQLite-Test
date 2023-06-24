using System.IO;
using System.Linq;
using NUnit.Framework;
using SQLite;

namespace zFramework.Tests
{
    public class SQLiteTests
    {
        private string dbPath;
        private SQLiteConnection db;

        [SetUp]
        public void SetUp() //搭建测试环境    
        {
            dbPath = Path.GetTempFileName();
            db = new SQLiteConnection(dbPath);
            db.CreateTable<Course>();
            db.CreateTable<Teacher>();
        }

        [TearDown]
        public void TearDown() //清理测试环境
        {
            db.Close();
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
        }

        [Test]
        public void TestInsertCourse() // 新增课程
        {
            var newCourse = new Course()
            {
                name = "新课程",
                description = "这是一门新课程",
                teacherId = 1,
            };
            db.Insert(newCourse);

            var courses = db.Table<Course>().ToList();
            Assert.AreEqual(1, courses.Count);
            Assert.AreEqual(newCourse.name, courses[0].name);
        }

        [Test]
        public void TestQueryCourses() //查询课程
        {
            var newCourse = new Course()
            {
                name = "新课程",
                description = "这是一门新课程",
                teacherId = 1,
            };
            db.Insert(newCourse);

            var courses = db.Table<Course>().ToList();
            Assert.AreEqual(1, courses.Count);
            Assert.AreEqual(newCourse.name, courses[0].name);
        }

        [Test]
        public void TestUpdateCourse() // 更新课程
        {
            var newCourse = new Course()
            {
                name = "新课程",
                description = "这是一门新课程",
                teacherId = 1,
            };
            db.Insert(newCourse);

            var course = db.Table<Course>().Where(c => c.id == 1).FirstOrDefault();
            Assert.IsNotNull(course);

            course.name = "更新后的课程名称";
            db.Update(course);

            var updatedCourse = db.Table<Course>().Where(c => c.id == 1).FirstOrDefault();
            Assert.IsNotNull(updatedCourse);
            Assert.AreEqual(course.name, updatedCourse.name);
        }


        [Test]
        public void TestDeleteCourse()//删除课程
        {
            var newCourse = new Course()
            {
                name = "新课程",
                description = "这是一门新课程",
                teacherId = 1,
            };
            db.Insert(newCourse);

            var course = db.Table<Course>().Where(c => c.id == 1).FirstOrDefault();
            Assert.IsNotNull(course);

            db.Delete(course);

            var deletedCourse = db.Table<Course>().Where(c => c.id == 1).FirstOrDefault();
            Assert.IsNull(deletedCourse);
        }
        [Test]
        public void TestQueryTeacherWithCourse() // 查询老师和课程
        {
            var newCourse = new Course() { name = "新课程", description = "这是一门新课程", teacherId = 1 };
            db.Insert(newCourse);

            var newTeacher = new Teacher() { name = "新老师" };
            db.Insert(newTeacher);

            var query = db.Table<Teacher>().Select(v => new
            {
                TeacherId = v.id,
                CourseName = db.Table<Course>().Where(c => c.teacherId == v.id).FirstOrDefault().name
            }).ToList();

            Assert.AreEqual(1, query.Count);
            Assert.AreEqual(newTeacher.id, query[0].TeacherId);
            Assert.AreEqual(newCourse.name, query[0].CourseName);
        }
    }
}
