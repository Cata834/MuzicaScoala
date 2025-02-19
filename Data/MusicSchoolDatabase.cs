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
                // Verificăm dacă cursul există deja în baza de date (pe baza unui ID sau nume)
                if (course.Id != 0)
                {
                    // Cursul există deja și va fi actualizat
                    return await _database.UpdateAsync(course);
                }
                else
                {
                    // Verifică dacă există deja un curs cu același nume
                    var existingCourse = await _database.Table<Course>()
                                                         .Where(c => c.Name == course.Name)
                                                         .FirstOrDefaultAsync();
                    if (existingCourse != null)
                    {
                        // Dacă există un curs cu același nume, returnează un cod de eroare
                        return -1; // Returnează un cod de eroare pentru duplicat
                    }

                    // Cursul nu există și va fi inserat
                    return await _database.InsertAsync(course);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adăugarea/actualizarea cursului: {ex.Message}");
                return 0; // Returnează 0 pentru semnalizarea unei erori
            }
        }

        // Obține toate cursurile din baza de date
        public async Task<List<Course>> GetCoursesAsync()
        {
            try
            {
                return await _database.Table<Course>().ToListAsync(); // Obține lista tuturor cursurilor
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la obținerea cursurilor: {ex.Message}");
                return new List<Course>(); // Returnează o listă goală în caz de eroare
            }
        }

        // Șterge un curs din baza de date
        public async Task<int> DeleteCourseAsync(Course course)
        {
            try
            {
                return await _database.DeleteAsync(course); // Șterge cursul din baza de date
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la ștergerea cursului: {ex.Message}");
                return 0; // Returnează 0 pentru semnalizarea unei erori
            }
        }

        // Adaugă un instructor nou sau actualizează unul existent
        public async Task<int> AddInstructorAsync(Instructor instructor)
        {
            try
            {
                if (instructor.Id != 0)
                {
                    // Instructorul există deja și va fi actualizat
                    return await _database.UpdateAsync(instructor);
                }
                else
                {
                    // Verifică dacă există deja un instructor cu același Email
                    var existingInstructor = await _database.Table<Instructor>()
                                                            .Where(i => i.Email == instructor.Email)
                                                            .FirstOrDefaultAsync();
                    if (existingInstructor != null)
                    {
                        return -1; // Instructorul există deja
                    }

                    // Instructorul nu există și va fi inserat
                    return await _database.InsertAsync(instructor);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adăugarea/actualizarea instructorului: {ex.Message}");
                return 0; // Returnează 0 pentru semnalizarea unei erori
            }
        }


        // Obține toți instructorii din baza de date
        public async Task<List<Instructor>> GetInstructorsAsync()
        {
            try
            {
                return await _database.Table<Instructor>().ToListAsync(); // Obține lista tuturor instructorilor
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la obținerea instructorilor: {ex.Message}");
                return new List<Instructor>(); // Returnează o listă goală în caz de eroare
            }
        }

        // Șterge un instructor din baza de date
        public async Task<int> DeleteInstructorAsync(Instructor instructor)
        {
            try
            {
                return await _database.DeleteAsync(instructor); // Șterge instructorul din baza de date
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la ștergerea instructorului: {ex.Message}");
                return 0; // Returnează 0 pentru semnalizarea unei erori
            }
        }
        
        }
    }

