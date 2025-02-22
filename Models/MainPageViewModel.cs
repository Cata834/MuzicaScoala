using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using MuzicaScoala.Models;
using MuzicaScoala.Resources.Services; // Pentru InstructorService

namespace MuzicaScoala.Models
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly InstructorService _instructorService;

        public ObservableCollection<Instructor> Instructors { get; set; } = new ObservableCollection<Instructor>();

        private Instructor _selectedInstructor;
        public Instructor SelectedInstructor
        {
            get => _selectedInstructor;
            set
            {
                _selectedInstructor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedInstructor)));
            }
        }

        public MainPageViewModel(InstructorService instructorService)
        {
            _instructorService = instructorService;
            LoadInstructors();
        }

        private async void LoadInstructors()
        {
            var instructors = await _instructorService.GetInstructorsAsync();
            Instructors.Clear();
            foreach (var instructor in instructors)
            {
                Instructors.Add(instructor);
            }
        }
    }
}
