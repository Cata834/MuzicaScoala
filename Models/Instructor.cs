using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuzicaScoala.Models
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Salvăm cursurile ca un singur string, separat prin virgulă
        public string Courses { get; set; } = "";

        // Convertim string-ul în List<string> când avem nevoie
        public List<string> GetCoursesList()
        {
            return string.IsNullOrEmpty(Courses) ? new List<string>() : Courses.Split(',').Select(c => c.Trim()).ToList();
        }

        // Setăm lista de cursuri, transformând-o în string
        public void SetCoursesList(List<string> courses)
        {
            Courses = string.Join(",", courses);
        }
        public string CoursesDisplay => !string.IsNullOrWhiteSpace(Courses)
    ? $"Predă: {Courses}"
    : "Nu are cursuri asignate";



    }
}
