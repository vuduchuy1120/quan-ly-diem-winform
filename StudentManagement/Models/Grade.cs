using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Grade
    {
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public double? Grade1 { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}
