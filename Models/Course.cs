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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }

        // Constructor cu parametri
        public Course(string name, string description, string instructor)
        {
            Name = name;
            Description = description;
            Instructor = instructor;
        }

        // Constructor implicit
        public Course() { }
    }

}


