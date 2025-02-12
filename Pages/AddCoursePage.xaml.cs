using MuzicaScoala.Data;
using MuzicaScoala.Models;

namespace MuzicaScoala
{
    public partial class AddCoursePage : ContentPage
    {
        private Course _course; // Obiectul de curs care va fi adăugat sau actualizat

        // Constructorul paginii, care poate primi un curs existent pentru actualizare
        public AddCoursePage(Course course = null)
        {
            InitializeComponent();

            // Dacă avem un curs existent, îl folosim pentru actualizare
            if (course != null)
            {
                _course = course;
                CourseNameEntry.Text = course.Name;
                CourseDescriptionEditor.Text = course.Description;
               // InstructorEntry.Text = course.Instructor;
                SaveButton.Text = "Update Course"; // Schimbă textul butonului în "Update"
            }
            else
            {
                _course = new Course(); // Dacă nu avem curs, creăm un obiect gol
                SaveButton.Text = "Add Course"; // Butonul va spune "Add Course"
            }
        }

        // Când utilizatorul apasă pe "Save Course" sau "Update Course"
        private async void OnSaveCourseClicked(object sender, EventArgs e)
        {
            // Verificăm dacă toate câmpurile sunt completate
            if (string.IsNullOrWhiteSpace(CourseNameEntry.Text) ||
                string.IsNullOrWhiteSpace(CourseDescriptionEditor.Text) ||
                string.IsNullOrWhiteSpace(InstructorEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            // Actualizăm obiectul Course cu datele din formular
            _course.Name = CourseNameEntry.Text;
            _course.Description = CourseDescriptionEditor.Text;
           // _course.Instructor = InstructorEntry.Text;

            // Adăugăm sau actualizăm cursul în baza de date
            int result = await App.Database.AddCourseAsync(_course);

            if (result > 0)
            {
                // Cursul a fost adăugat sau actualizat cu succes
                await DisplayAlert("Success", "Course saved successfully!", "OK");
            }
            else
            {
                // A apărut o eroare la salvarea cursului
                await DisplayAlert("Error", "An error occurred while saving the course.", "OK");
            }

            // Navigăm înapoi la pagina principală
            await Navigation.PopAsync();
        }
    }
}
