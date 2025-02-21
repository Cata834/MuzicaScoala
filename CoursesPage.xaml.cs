using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Calendar;
using MuzicaScoala.Models;

namespace MuzicaScoala
{
    public partial class CoursesPage : ContentPage
    {
        private int? _instructorId; // Stocăm ID-ul instructorului (dacă venim din InstructorPage)

        public CoursesPage(int? instructorId = null)
        {
            InitializeComponent();
            _instructorId = instructorId;
        }

        private async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(200);

            try
            {
                var courses = await App.Database.GetCoursesAsync();
                var datesWithCourses = courses
                    .Where(c => c.CourseDate != default(DateTime)) // Verificăm dacă CourseDate nu este valoare implicită (default)
                    .Select(c => c.CourseDate)
                    .ToList();

                if (!datesWithCourses.Any())
                {
                    Console.WriteLine("Nu există cursuri programate.");
                }

                MarkDatesOnCalendar(datesWithCourses);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", ex.Message, "OK");
                Console.WriteLine($"EROARE: {ex}");
            }
        }

        private async Task LoadCourses()
        {
            var courses = await App.Database.GetCoursesAsync();
            var instructors = await App.Database.GetInstructorsAsync();

            if (_instructorId.HasValue)
            {
                courses = courses.Where(c => c.InstructorId == _instructorId.Value).ToList();
            }

            foreach (var course in courses)
            {
                var instructor = instructors.FirstOrDefault(i => i.Id == course.InstructorId);
                course.InstructorName = instructor != null ? instructor.Name : "Fără instructor";
            }

            CoursesListView.ItemsSource = courses;
        }

        // Când utilizatorul selectează o dată
        private void OnSelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
        {
            if (e.NewValue is IList<DateTime> selectedDates && selectedDates.Any())
            {
                DateTime selectedDate = selectedDates[0]; // Luăm prima dată selectată
                Console.WriteLine($"Data selectată: {selectedDate:dd/MM/yyyy}");
            }
        }

        // Când utilizatorul apasă pe "Adaugă Curs"
        private async void OnAddCourseClicked(object sender, EventArgs e)
        {
            var selectedDate = calendar.SelectedDates.FirstOrDefault();

            // Verificăm dacă selectedDate este valid
            if (selectedDate != default(DateTime)) // Verificăm că selectedDate nu este valoare implicită
            {
                Console.WriteLine($"Cursul va fi programat pentru: {selectedDate:dd/MM/yyyy}");

                var newCourse = new Course
                {
                    Name = "Numele Cursului", // Adăugați valorile relevante
                    Description = "Descrierea Cursului",
                    InstructorId = 1, // Exemplu de ID pentru instructor
                    CourseDate = selectedDate
                };

                await App.Database.AddCourseAsync(newCourse);
                await DisplayAlert("Succes", "Cursul a fost adăugat cu succes!", "OK");
            }
            else
            {
                await DisplayAlert("Eroare", "Nu a fost selectată nicio dată.", "OK");
            }
        }

        private async Task AddCourseToDatabase(DateTime selectedDate)
        {
            var newCourse = new Course
            {
                Name = "Noul Curs",
                Description = "Descriere curs",
                InstructorId = 1, // ID valid al instructorului
                CourseDate = selectedDate
            };

            await App.Database.AddCourseAsync(newCourse);
            await DisplayAlert("Succes", "Cursul a fost adăugat cu succes!", "OK");
        }

        // Adăugarea de marcaje pe calendar
        private void MarkDatesOnCalendar(List<DateTime> datesWithCourses)
        {
            if (calendar == null)
            {
                Console.WriteLine("EROARE: calendar este null!");
                return;
            }

            // Atribuim o nouă listă la SelectedDates
            calendar.SelectedDates = new System.Collections.ObjectModel.ObservableCollection<DateTime>(datesWithCourses);
        }

        private async void OnCourseTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Course selectedCourse)
            {
                await Navigation.PushAsync(new AddCoursePage(selectedCourse));
            }
        }
    }
}
