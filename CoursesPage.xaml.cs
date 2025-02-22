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
            MessagingCenter.Subscribe<AddCoursePage, DateTime>(this, "CourseAdded", (sender, newDate) =>
            {
                MarkDatesOnCalendar(new List<DateTime> { newDate });
            });
        }

        // Acest eveniment este apelat atunci când pagina apare pe ecran
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.Delay(200); // Mică întârziere pentru UI

            try
            {
                var courses = await App.Database.GetCoursesAsync();
                var datesWithCourses = courses
                    .Where(c => c.CourseDate != default(DateTime))
                    .Select(c => c.CourseDate.Date) // Asigură-te că e doar data, fără ore
                    .Distinct() // Evită duplicatele
                    .ToList();

                MarkDatesOnCalendar(datesWithCourses);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", ex.Message, "OK");
                Console.WriteLine($"EROARE: {ex}");
            }
        }




        // Funcția care încarcă cursurile din baza de date
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
        private async void OnSelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
        {
            if (e.NewValue is IList<DateTime> selectedDates && selectedDates.Any())
            {
                DateTime selectedDate = selectedDates[0]; // Obținem prima dată selectată

                // Verificăm dacă există cursuri pentru acea dată
                var coursesForDate = await App.Database.GetCoursesByDateAsync(selectedDate);
                if (coursesForDate.Any())
                {
                    // Dacă există cursuri, afișăm un mesaj cu datele cursurilor
                    string courseNames = string.Join(", ", coursesForDate.Select(c => c.Name));
                    await DisplayAlert("Cursuri Programate", $"Pentru această dată sunt programate cursurile: {courseNames}", "OK");
                }
                else
                {
                    // Dacă nu există cursuri, afișăm un mesaj informativ
                    await DisplayAlert("Informație", "Nu sunt cursuri programate pentru această dată.", "OK");
                }
            }
        }





        // Când utilizatorul apasă pe "Adaugă Curs"
        private async void OnAddCourseClicked(object sender, EventArgs e)
        {
            var selectedDate = calendar.SelectedDate;

            if (selectedDate.HasValue)
            {
                DateTime selectedDateValue = selectedDate.Value; // Extragem valoarea

                // Debug: Afișează data selectată
                Console.WriteLine($"Data selectată: {selectedDateValue:dd/MM/yyyy}");

                // Continuăm cu logica de adăugare a cursului
                var newCourse = new Course
                {
                    Name = "Numele Cursului", // Adăugați valorile relevante
                    Description = "Descrierea Cursului",
                    InstructorId = 1, // Exemplu de ID pentru instructor
                    CourseDate = selectedDateValue // Folosim selectedDateValue
                };

                await App.Database.AddCourseAsync(newCourse);
                await DisplayAlert("Succes", "Cursul a fost adăugat cu succes!", "OK");
            }
            else
            {
                await DisplayAlert("Eroare", "Nu a fost selectată nicio dată.", "OK");
            }
        }




        // Adăugarea de marcaje pe calendar
        private void MarkDatesOnCalendar(List<DateTime> datesWithCourses)
        {
            if (calendar == null)
            {
                Console.WriteLine("EROARE: calendar este null!");
                return;
            }

            // Clear selecțiile anterioare
            calendar.SelectedDates.Clear();

            foreach (var date in datesWithCourses)
            {
                if (!calendar.SelectedDates.Contains(date))
                {
                    calendar.SelectedDates.Add(date); // Marcam data pe calendar
                }
            }

            Console.WriteLine("Datele au fost marcate pe calendar.");
        }




        private async void OnCourseTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var selectedCourse = (Course)e.Item;

            // Afișăm un mesaj cu informațiile cursului
            await DisplayAlert("Curs Selectat", $"Ai selectat: {selectedCourse.Name} de la {selectedCourse.InstructorName}", "OK");

            // Debifează elementul selectat din listă pentru UI
            ((ListView)sender).SelectedItem = null;
        }


    }
}
