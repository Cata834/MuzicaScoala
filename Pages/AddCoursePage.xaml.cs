using MuzicaScoala.Data;
using MuzicaScoala.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MuzicaScoala
{
    public partial class AddCoursePage : ContentPage
    {
        private Course _course;

        public AddCoursePage(Course course = null)
        {
            InitializeComponent();
            LoadInstructorsAsync(course);  // Încărcăm instructorii asincron

            if (course != null)
            {
                _course = course;
                CourseNameEntry.Text = course.Name;
                CourseDescriptionEditor.Text = course.Description;

                SaveButton.Text = "Update Course";  // Schimbă textul butonului în "Update"
            }
            else
            {
                _course = new Course();  // Cursul este gol pentru adăugare
                SaveButton.Text = "Add Course";  // Butonul va spune "Add Course"
            }
        }

        // Metodă asincronă pentru a încărca instructorii
        private async void LoadInstructorsAsync(Course course)
        {
            var instructors = await App.Database.GetInstructorsAsync();  // Obținem lista de instructori din DB
            InstructorPicker.ItemsSource = instructors;  // Setăm lista de instructori în Picker

            if (course != null)
            {
                // Selectează instructorul existent dacă este deja salvat
                var selectedInstructor = instructors.FirstOrDefault(i => i.Id == course.InstructorId);

                if (selectedInstructor != null)
                {
                    InstructorPicker.SelectedItem = selectedInstructor;
                }
            }
        }

        // Salvarea cursului
        private async void OnSaveCourseClicked(object sender, EventArgs e)
        {
            // Verificăm dacă toate câmpurile sunt completate
            if (string.IsNullOrWhiteSpace(CourseNameEntry.Text) ||
                string.IsNullOrWhiteSpace(CourseDescriptionEditor.Text) ||
                InstructorPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            var selectedInstructor = (Instructor)InstructorPicker.SelectedItem;

            _course.Name = CourseNameEntry.Text;
            _course.Description = CourseDescriptionEditor.Text;
            _course.InstructorId = selectedInstructor.Id;
            _course.InstructorName = selectedInstructor.Name;

            // Adăugăm cursul în baza de date
            int result = await App.Database.AddCourseAsync(_course);

            if (result > 0)
            {
                await DisplayAlert("Success", "Course saved successfully!", "OK");
                await Navigation.PopAsync();  // Navigăm înapoi la pagina principală
            }
            else
            {
                await DisplayAlert("Error", "An error occurred while saving the course.", "OK");
            }
        }
    }
}
