using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;


namespace MuzicaScoala.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // ID-ul cursului
        public string Name { get; set; } // Numele cursului
        public string Description { get; set; } // Descrierea cursului
        public int InstructorId { get; set; } // ID-ul instructorului care predă cursul (relatie cu Instructor)
        public string InstructorName { get; set; }

        public DateTime CourseDate { get; set; }

    }
}


