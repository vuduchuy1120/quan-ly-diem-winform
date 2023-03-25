using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Student
    {
        public Student()
        {
            Grades = new HashSet<Grade>();
        }

        public string RollNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public int? MajorId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime? Dob { get; set; }

        public virtual Major Major { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }

        public override string ToString()
        {
            return Major.Description; 
        }
    }
}
