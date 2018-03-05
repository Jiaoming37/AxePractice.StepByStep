using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class Student
    {
        public Student()
        {
            Teachers = new List<Teacher>();
        }
        public virtual string Name { get; set; }
        public virtual Guid StudentId { get; set; }
        public virtual bool IsForQuery { get; set; }
        public virtual IList<Teacher> Teachers { get; set; }
    }

    public class StudentMap : ClassMap<Student>
    {
        public StudentMap()
        {
            Table("student");
            Id(c => c.StudentId).Column("StudentID"); ;
            Map(c => c.Name).Column("Name");
            Map(c => c.IsForQuery).Column("IsForQuery");
            HasManyToMany(c => c.Teachers).ParentKeyColumn("StudentID").ChildKeyColumn("TeacherID")
                .Table("student_teacher").Cascade.AllDeleteOrphan();
        }
    }
}