using System.Linq;
using NHibernate;
using Orm.Practice.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public class ManyToManyModifyFacts: OrmFactBase
    {
        public ManyToManyModifyFacts(ITestOutputHelper output) : base(output)
        {
            ExecuteNonQuery("DELETE FROM [dbo].[student_teacher] where StudentID in (SELECT StudentID FROM [dbo].[student] WHERE IsForQuery=0)");
            ExecuteNonQuery("DELETE FROM [dbo].[teacher] WHERE IsForQuery=0");
            ExecuteNonQuery("DELETE FROM [dbo].[student] WHERE IsForQuery=0");
        }

        [Fact]
        public void should_insert_student_and_teacher_by_student()
        {
            var student = new Student {IsForQuery = false, Name = "student 3"};
            var teacher = new Teacher {IsForQuery = false, Name = "teacher 3"};
            student.Teachers.Add(teacher);

            Session.Save(student);
            Session.Flush();

            Session.Clear();

            Student studentResult = Session.Query<Student>().FirstOrDefault(s => s.Name == "student 3");
            Teacher teacherResult = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 3");
            Assert.NotNull(studentResult);
            Assert.NotNull(teacherResult);
            Assert.Equal(new [] {"teacher 3"}, studentResult.Teachers.Select(t => t.Name).ToArray());
            Assert.Equal(new [] {"student 3"}, teacherResult.Students.Select(t => t.Name).ToArray());
        }

        [Fact]
        public void should_insert_student_and_teacher_by_teacher()
        {
            var student = new Student { IsForQuery = false, Name = "student 3" };
            var teacher = new Teacher { IsForQuery = false, Name = "teacher 3" };
            teacher.Students.Add(student);

            Session.Save(teacher);
            Session.Flush();

            Session.Clear();

            Student studentResult = Session.Query<Student>().FirstOrDefault(s => s.Name == "student 3");
            Teacher teacherResult = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 3");
            Assert.NotNull(studentResult);
            Assert.NotNull(teacherResult);
            Assert.Equal(new[] { "teacher 3" }, studentResult.Teachers.Select(t => t.Name).ToArray());
            Assert.Equal(new[] { "student 3" }, teacherResult.Students.Select(t => t.Name).ToArray());
        }

        [Fact]
        public void should_delete_student_and_teacher_by_student()
        {
            var student = new Student { IsForQuery = false, Name = "student 3" };
            var teacher = new Teacher { IsForQuery = false, Name = "teacher 3" };
            student.Teachers.Add(teacher);

            Session.Save(student);
            Session.Flush();

            Session.Delete(Session.Query<Student>().First());
            Session.Flush();

            Session.Clear();

            Student studentResult = Session.Query<Student>().FirstOrDefault(s => s.Name == "student 3");
            Teacher teacherResult = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 3");

            Assert.Null(studentResult);
            Assert.Null(teacherResult);
        }

        [Fact]
        public void should_delete_student_and_teacher_by_teacher()
        {
            var student = new Student { IsForQuery = false, Name = "student 3" };
            var teacher = new Teacher { IsForQuery = false, Name = "teacher 3" };
            teacher.Students.Add(student);

            Session.Save(teacher);
            Session.Flush();

            Session.Delete(Session.Query<Teacher>().First());
            Session.Flush();

            Session.Clear();

            Student studentResult = Session.Query<Student>().FirstOrDefault(s => s.Name == "student 3");
            Teacher teacherResult = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 3");

            Assert.Null(studentResult);
            Assert.Null(teacherResult);
        }


        void ExecuteNonQuery(string sql)
        {
            ISQLQuery query = StatelessSession.CreateSQLQuery(sql);
            query.ExecuteUpdate();
        }
    }
}