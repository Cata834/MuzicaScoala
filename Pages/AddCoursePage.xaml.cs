using MuzicaScoala.Data;
using MuzicaScoala.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuzicaScoala
{
    public partial class AddCoursePage : ContentPage
    {
        private Course _course;
        private List<Instructor> _instructors;  // Stocăm instructorii pentru Picker

        public AddCoursePage(Course course = null)
        {
            InitializeComponent();
            LoadInstructorsAsync(course);

            _course = course ?? new Course();

            if (course != null)
            {
                CourseNameEntry.Text = course.Name;
                CourseDescriptionEditor.Text = course.Description;
                SaveButton.Text = "Update Course";
            }
            else
            {
                SaveButton.Text = "Add Course";
            }
        }

        // Încărcăm instructorii și îi setăm în Picker
        private async void LoadInstructorsAsync(Course course)
        {
            _instructors = await App.Database.GetInstructorsAsync();

            InstructorPicker.ItemsSource = _instructors;  // Setăm direct lista de instructori
            InstructorPicker.ItemDisplayBinding = new Binding("Name");  // Afișăm doar numele

            if (course != null)
            {
                var selectedInstructor = _instructors.FirstOrDefault(i => i.Id == course.InstructorId);
                if (selectedInstructor != null)
                {
                    InstructorPicker.SelectedItem = selectedInstructor;
                }
            }
        }

        private async void OnSaveCourseClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CourseNameEntry.Text) ||
                string.IsNullOrWhiteSpace(CourseDescriptionEditor.Text) ||
                InstructorPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            // Selectăm instructorul real
            var selectedInstructor = (Instructor)InstructorPicker.SelectedItem;

            _course.Name = CourseNameEntry.Text;
            _course.Description = CourseDescriptionEditor.Text;
            _course.InstructorId = selectedInstructor.Id;
            _course.InstructorName = selectedInstructor.Name;

            int result = await App.Database.AddCourseAsync(_course);

            if (result > 0)
            {
                await DisplayAlert("Success", "Course saved successfully!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "An error occurred while saving the course.", "OK");
            }
        }
    }
}
