using System.Collections.Generic;
using System.Linq;
using Orm.Practice.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public class ManyToManyQueryFacts: OrmFactBase
    {
        public ManyToManyQueryFacts(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void should_get_all_students()
        {
            List<Teacher> teachers = Session.Query<Teacher>()
                .Where(p => p.IsForQuery)
                .OrderBy(p => p.Name)
                .ToList();

            Assert.Equal(
                new [] {"student 1", "student 2"},
                teachers.First().Students.Select(s => s.Name).OrderBy(n => n).ToArray());

            Assert.Equal(
                new[] { "student 1", "student 2" },
                teachers.Last().Students.Select(s => s.Name).OrderBy(n => n).ToArray());
        }

        [Fact]
        public void should_get_all_teachers()
        {
            List<Student> students = Session.Query<Student>()
                .Where(p => p.IsForQuery)
                .OrderBy(p => p.Name)
                .ToList();

            Assert.Equal(
                new[] { "teacher 1", "teacher 2" },
                students.First().Teachers.Select(s => s.Name).OrderBy(n => n).ToArray());

            Assert.Equal(
                new[] { "teacher 1", "teacher 2" },
                students.Last().Teachers.Select(s => s.Name).OrderBy(n => n).ToArray());
        }
    }
}