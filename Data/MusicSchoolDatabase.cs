using SQLite;
using MuzicaScoala.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MuzicaScoala.Data
{
    public class MusicSchoolDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public MusicSchoolDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Course>().ConfigureAwait(false);
        }

        // Adaugă un curs nou sau actualizează unul existent
        public async Task<int> AddCourseAsync(Course course)
        {
            try
            {
                // Verificăm dacă cursul există deja în bază (pe baza unui nume sau ID)
                if (course.Id != 0)
                {
                    // Cursul există deja în bază și va fi actualizat
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
                        return -1; // Sau o altă valoare pentru a semnala că deja există
                    }

                    // Cursul nu există și va fi inserat ca un nou curs
                    return await _database.InsertAsync(course);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adăugarea/actualizarea cursului: {ex.Message}");
                return 0; // Returnează 0 pentru a semnala o eroare
            }
        }

        // Obține toate cursurile
        public async Task<List<Course>> GetCoursesAsync()
        {
            try
            {
                return await _database.Table<Course>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la obținerea cursurilor: {ex.Message}");
                return new List<Course>(); // Returnează o listă goală în caz de eroare
            }
        }

        // Șterge un curs
        public async Task<int> DeleteCourseAsync(Course course)
        {
            try
            {
                return await _database.DeleteAsync(course);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la ștergerea cursului: {ex.Message}");
                return 0; // Returnează 0 pentru eșec
            }
        }
    }
}