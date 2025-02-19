using SQLite;
using MuzicaScoala.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MuzicaScoala.Data
{
    public class MusicSchoolDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        // Constructor pentru inițializarea bazei de date
        public MusicSchoolDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Instructor>().Wait();
        }

        // Adaugă un curs nou sau actualizează unul existent
        public async Task<int> AddCourseAsync(Course course)
        {
            try
            {
                if (course.Id != 0)
                {
                    // Cursul există deja și va fi actualizat
                    return await _database.UpdateAsync(course);
                }
                else
                {
                    var existingCourse = await _database.Table<Course>()
                                                         .Where(c => c.Name == course.Name)
                                                         .FirstOrDefaultAsync();
                    if (existingCourse != null)
                    {
                        return -1; // Returnează un cod de eroare pentru duplicat
                    }

                    return await _database.InsertAsync(course);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adăugarea/actualizarea cursului: {ex.Message}");
                return 0;
            }
        }

        // Obține toate cursurile din baza de date
        public async Task<List<Course>> GetCoursesAsync()
        {
            try
            {
                return await _database.Table<Course>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la obținerea cursurilor: {ex.Message}");
                return new List<Course>();
            }
        }

        // Șterge un curs din baza de date
        public async Task<int> DeleteCourseAsync(Course course)
        {
            try
            {
                return await _database.DeleteAsync(course);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la ștergerea cursului: {ex.Message}");
                return 0;
            }
        }

        // Adaugă un instructor nou sau actualizează unul existent
        public async Task<int> AddInstructorAsync(Instructor instructor)
        {
            try
            {
                if (instructor.Id != 0)
                {
                    return await _database.UpdateAsync(instructor);
                }
                else
                {
                    var existingInstructor = await _database.Table<Instructor>()
                                                            .Where(i => i.Email == instructor.Email)
                                                            .FirstOrDefaultAsync();
                    if (existingInstructor != null)
                    {
                        return -1; // Instructorul există deja
                    }

                    return await _database.InsertAsync(instructor);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adăugarea/actualizarea instructorului: {ex.Message}");
                return 0;
            }
        }

        // Obține toți instructorii din baza de date
        public async Task<List<Instructor>> GetInstructorsAsync()
        {
            try
            {
                return await _database.Table<Instructor>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la obținerea instructorilor: {ex.Message}");
                return new List<Instructor>();
            }
        }

        // Șterge un instructor din baza de date
        public async Task<int> DeleteInstructorAsync(Instructor instructor)
        {
            try
            {
                // Log pentru debugging
                Console.WriteLine($"Ștergem instructorul: {instructor.Name}");
                return await _database.DeleteAsync(instructor); // Șterge instructorul din baza de date
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la ștergerea instructorului: {ex.Message}");
                return 0; // Returnează 0 pentru semnalizarea unei erori
            }
        }



        // Șterge instructorii care nu au cursuri asignate
        public async Task DeleteInstructorsWithoutCoursesAsync()
        {
            try
            {
                // Execută o interogare SQL pentru a șterge toți instructorii care nu au cursuri asignate
                var result = await _database.ExecuteAsync("DELETE FROM Instructor WHERE Courses IS NULL OR TRIM(Courses) = ''");

                Console.WriteLine($"Ștergere completă. {result} instructori au fost șterși.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la ștergerea instructorilor fără cursuri: {ex.Message}");
            }
        }


    }
}

