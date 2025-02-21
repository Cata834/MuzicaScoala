using MuzicaScoala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MuzicaScoala
{
    public partial class InstructorPage : ContentPage
    {
        private Instructor _instructor;
        public InstructorPage()
        {
            InitializeComponent();
        }

        public InstructorPage(Instructor instructor)
        {
            InitializeComponent();
            _instructor = instructor;
            DisplayInstructorDetails();
        }

        private void DisplayInstructorDetails()
        {
            Title = _instructor.Name;
            DisplayAlert("Instructor Found", $"Instructor: {_instructor.Name}", "OK");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadInstructors(); // Încărcăm instructorii la afișarea paginii
        }

        private async Task LoadInstructors()
        {
            var instructors = await App.Database.GetInstructorsAsync();
            InstructorListView.ItemsSource = instructors;
        }

        private async void OnAddInstructorClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddInstructorPage()); // Navigăm la pagina de adăugare instructor
        }

        private async void OnInstructorTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Instructor selectedInstructor)
            {
                // Navigăm către CoursesPage și trimitem ID-ul instructorului
                await Navigation.PushAsync(new CoursesPage(selectedInstructor.Id));
            }
        }

        private async void OnDeleteInstructorsWithoutCoursesClicked(object sender, EventArgs e)
        {
            var instructors = await App.Database.GetInstructorsAsync();
            var courses = await App.Database.GetCoursesAsync();

            // Filtrăm instructorii fără cursuri asociate
            var instructorsWithoutCourses = instructors
                .Where(i => !courses.Any(c => c.InstructorId == i.Id))
                .ToList();

            if (instructorsWithoutCourses.Any())
            {
                foreach (var instructor in instructorsWithoutCourses)
                {
                    Console.WriteLine($"Instructorul {instructor.Name} va fi șters.");
                    await App.Database.DeleteInstructorAsync(instructor);
                }

                await LoadInstructors();
                await DisplayAlert("Succes", "Instructorii fără cursuri au fost șterși.", "OK");
            }
            else
            {
                await DisplayAlert("Info", "Nu sunt instructori fără cursuri.", "OK");
            }
        }
    }
}
