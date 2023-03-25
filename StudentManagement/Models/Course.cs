﻿using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Course
    {
        public Course()
        {
            Grades = new HashSet<Grade>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
    }
}
