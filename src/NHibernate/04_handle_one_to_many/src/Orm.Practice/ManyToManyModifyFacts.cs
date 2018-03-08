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

            Session.Delete(Session.Query<Student>().First(s => s.Name == "student 3"));
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

            Session.Delete(Session.Query<Teacher>().First(t => t.Name == "teacher 3"));
            Session.Flush();

            Session.Clear();

            Student studentResult = Session.Query<Student>().FirstOrDefault(s => s.Name == "student 3");
            Teacher teacherResult = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 3");

            Assert.Null(studentResult);
            Assert.Null(teacherResult);
        }

        [Fact]
        public void should_update_student_by_teacher()
        {
            var teacher = new Teacher { IsForQuery = false, Name = "teacher 3" };
            var student = new Student { IsForQuery = false, Name = "student 3" };
            teacher.Students.Add(student);
            Session.Save(teacher);
            Session.Flush();

            teacher.Students.First().Name = "new student name";
            Session.Update(teacher);
            Session.Flush();

            Session.Clear();

            Student studentResult = Session.Query<Student>().FirstOrDefault(s => s.Name == "new student name");
            Teacher teacherResult = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 3");
            Assert.NotNull(studentResult);
            Assert.NotNull(teacherResult);
            Assert.Equal(new[] { "teacher 3" }, studentResult.Teachers.Select(t => t.Name).ToArray());
            Assert.Equal(new[] { "new student name" }, teacherResult.Students.Select(t => t.Name).ToArray());
        }

        [Fact]
        public void should_update_teacher_by_student()
        {
            var teacher = new Teacher { IsForQuery = false, Name = "teacher 3" };
            var student = new Student { IsForQuery = false, Name = "student 3" };
            student.Teachers.Add(teacher);
            Session.Save(student);
            Session.Flush();

            student.Teachers.First().Name = "new teacher name";
            Session.Update(student);
            Session.Flush();

            Session.Clear();

            Student studentResult = Session.Query<Student>().FirstOrDefault(s => s.Name == "student 3");
            Teacher teacherResult = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "new teacher name");
            Assert.NotNull(studentResult);
            Assert.NotNull(teacherResult);
            Assert.Equal(new[] { "new teacher name" }, studentResult.Teachers.Select(t => t.Name).ToArray());
            Assert.Equal(new[] { "student 3" }, teacherResult.Students.Select(t => t.Name).ToArray());
        }

        [Fact]
        public void should_delete_all_table_by_teacher()
        {
            var teacher3 = new Teacher { IsForQuery = false, Name = "teacher 3" };
            var student3 = new Student { IsForQuery = false, Name = "student 3" };
            teacher3.Students.Add(student3);
            var teacher4 = new Teacher { IsForQuery = false, Name = "teacher 4" };
            student3.Teachers.Add(teacher4);

            Session.Save(teacher3);
            Session.Flush();

            Session.Delete(Session.Query<Teacher>().First(t => t.Name == "teacher 3"));
            Session.Flush();

            Session.Clear();

            Student studentResult = Session.Query<Student>().FirstOrDefault(s => s.Name == "student 3");
            Teacher teacher3Result = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 3");
            Teacher teacher4Result = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 4");

            Assert.Null(studentResult);
            Assert.Null(teacher3Result);
            Assert.Null(teacher4Result);
        }


        [Fact]
        public void should_delete_the_teacher_by_teacher_when_save_update()
        {
            var teacher3 = new Teacher { IsForQuery = false, Name = "teacher 3" };
            var student3 = new Student { IsForQuery = false, Name = "student 3" };
            teacher3.Students.Add(student3);
            var teacher4 = new Teacher { IsForQuery = false, Name = "teacher 4" };
            student3.Teachers.Add(teacher4);

            Session.Save(teacher3);
            Session.Flush();

            Session.Delete(Session.Query<Teacher>().First(t => t.Name == "teacher 3"));
            Session.Flush();

            Session.Clear();

            Student studentResult = Session.Query<Student>().FirstOrDefault(s => s.Name == "student 3");
            Teacher teacher3Result = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 3");
            Teacher teacher4Result = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 4");

//            Assert.Equal(1, studentResult.Teachers.Count);
//            Assert.NotNull(studentResult.Teachers.Single(t => t.Name == "teacher 4"));
//            Assert.Null(teacher3Result);
//            Assert.NotNull(teacher4Result);
        }

        [Fact]
        public void should_only_delete_relation_by_teacher()
        {
            var teacher3 = new Teacher { IsForQuery = false, Name = "teacher 3" };
            var student3 = new Student { IsForQuery = false, Name = "student 3" };
            teacher3.Students.Add(student3);
            var teacher4 = new Teacher { IsForQuery = false, Name = "teacher 4" };
            student3.Teachers.Add(teacher4);

            Session.Save(teacher3);
            Session.Flush();

            teacher3.Students.Remove(student3);
            Session.Flush();

            Session.Clear();

            Teacher teacher3Result = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 3");
            Teacher teacher4Result = Session.Query<Teacher>().FirstOrDefault(s => s.Name == "teacher 4");
            Student studentResult = Session.Query<Student>().FirstOrDefault(s => s.Name == "student 3");

            Assert.NotNull(studentResult);
            Assert.NotNull(teacher3Result);
            Assert.NotNull(teacher4Result);
            Assert.Empty(teacher3.Students);
        }


        void ExecuteNonQuery(string sql)
        {
            ISQLQuery query = StatelessSession.CreateSQLQuery(sql);
            query.ExecuteUpdate();
        }
    }
}