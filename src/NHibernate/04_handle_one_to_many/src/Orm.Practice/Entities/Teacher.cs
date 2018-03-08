using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class Teacher
    {
        public Teacher()
        {
            Students = new List<Student>();
        }
        public virtual bool IsForQuery { get; set; }
        public virtual string Name { get; set; }
        public virtual Guid TeacherId { get; set; }
        public virtual IList<Student> Students { get; set; }
    }

    public class TeacherMap : ClassMap<Teacher>
    {
        public TeacherMap()
        {
            Table("teacher");
            Id(c => c.TeacherId).Column("TID"); ;
            Map(c => c.Name).Column("Name");
            Map(c => c.IsForQuery).Column("IsForQuery");
            HasManyToMany(c => c.Students)
                .ParentKeyColumn("TeacherID")
                .ChildKeyColumn("StudentID")
                .Table("student_teacher")
                .Cascade.All();
        }
    }
}