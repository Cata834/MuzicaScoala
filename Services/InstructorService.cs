using MuzicaScoala.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using MuzicaScoala.Models;

namespace MuzicaScoala.Resources.Services // Spatiu de nume specificat
{
    public class InstructorService
    {
        private readonly MusicSchoolDatabase _database;

        public InstructorService(MusicSchoolDatabase database)
        {
            _database = database;
        }

        // Metodă pentru a obține toți instructorii
        public Task<List<Instructor>> GetInstructorsAsync()
        {
            return _database.GetInstructorsAsync();
        }

        // Metodă pentru a salva sau actualiza un instructor
        public Task<int> SaveInstructorAsync(Instructor instructor)
        {
            return _database.SaveInstructorAsync(instructor);
        }
    }

}
