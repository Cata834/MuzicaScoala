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
        private int? _instructorId;

        public CoursesPage(int? instructorId = null)
        {
            InitializeComponent();
            _instructorId = instructorId;


            MessagingCenter.Unsubscribe<AddCoursePage, DateTime>(this, "CourseAdded");
            MessagingCenter.Subscribe<AddCoursePage, DateTime>(this, "CourseAdded", (sender, newDate) =>
            {
                Console.WriteLine($"[MessagingCenter] Curs nou adăugat pe data: {newDate:dd/MM/yyyy}");
                MarkDatesOnCalendar(new List<DateTime> { newDate });
            });
            MessagingCenter.Subscribe<AddCoursePage, DateTime>(this, "CourseAdded", async (sender, newDate) =>
            {
                await LoadCourses(); // 🔹 Actualizăm lista cursurilor
            });

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<AddCoursePage, DateTime>(this, "CourseAdded");
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(200);

            try
            {
                var courses = await App.Database.GetCoursesAsync();
                var datesWithCourses = courses
                    .Where(c => c.CourseDate > DateTime.MinValue)
                    .Select(c => c.CourseDate.Date)
                    .Distinct()
                    .ToList();

                MarkDatesOnCalendar(datesWithCourses);
                await LoadCourses(); // 🔹 Reîncărcăm lista cursurilor
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", ex.Message, "OK");
                Console.WriteLine($"[EROARE] {ex}");
            }
        }


        private async Task LoadCourses()
        {
            var courses = await App.Database.GetCoursesAsync();
            var instructors = await App.Database.GetInstructorsAsync();

            foreach (var course in courses)
            {
                var instructor = instructors.FirstOrDefault(i => i.Id == course.InstructorId);
                course.InstructorName = instructor?.Name ?? "Fără instructor";
            }

            CoursesListView.ItemsSource = courses;
        }
   


        private bool isProcessingSelection = false;

        private async void OnSelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
        {
            if (isProcessingSelection) return; // Evită apelurile multiple

            isProcessingSelection = true; // Blocăm execuția multiplă

            try
            {
                if (e.NewValue is IList<DateTime> selectedDates && selectedDates.Any())
                {
                    DateTime selectedDate = selectedDates[0];

                    Console.WriteLine($"[DEBUG] Utilizatorul a selectat data: {selectedDate:dd/MM/yyyy}");

                    var coursesForDate = await App.Database.GetCoursesByDateAsync(selectedDate);
                    if (coursesForDate.Any())
                    {
                        string courseNames = string.Join(", ", coursesForDate.Select(c => c.Name));

                        await DisplayAlert("Cursuri Programate", $"Pentru această dată sunt programate cursurile: {courseNames}", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Informație", "Nu sunt cursuri programate pentru această dată.", "OK");
                    }
                }
            }
            finally
            {
                await Task.Delay(300); // Mic delay pentru a preveni reapelările rapide
                isProcessingSelection = false; // Resetăm flag-ul
            }
        }


        private async void OnAddCourseClicked(object sender, EventArgs e)
        {
            var selectedDate = calendar.SelectedDate;

            if (selectedDate.HasValue)
            {
                DateTime selectedDateValue = selectedDate.Value;

                // Verificăm dacă există deja un curs în acea dată
                var existingCourses = await App.Database.GetCoursesByDateAsync(selectedDateValue);
                if (existingCourses.Any(c => c.Name == "Numele Cursului"))
                {
                    await DisplayAlert("Eroare", "Cursul pentru această dată există deja!", "OK");
                    return;
                }

                var newCourse = new Course
                {
                    Name = "Numele Cursului",
                    Description = "Descrierea Cursului",
                    InstructorId = 1,
                    CourseDate = selectedDateValue
                };

                await App.Database.AddCourseAsync(newCourse);
                await DisplayAlert("Succes", "Cursul a fost adăugat cu succes!", "OK");

                MessagingCenter.Send(this, "CourseAdded", selectedDateValue);
            }
            else
            {
                await DisplayAlert("Eroare", "Nu a fost selectată nicio dată.", "OK");
            }
        }


        private void MarkDatesOnCalendar(List<DateTime> datesWithCourses)
        {
            if (calendar == null)
            {
                Console.WriteLine("[EROARE] calendar este null!");
                return;
            }

            var existingDates = new HashSet<DateTime>(calendar.SelectedDates);
            foreach (var date in datesWithCourses)
            {
                if (!existingDates.Contains(date)) // Evităm adăugarea duplicată
                {
                    calendar.SelectedDates.Add(date);
                }
            }

            Console.WriteLine($"[DEBUG] Datele marcate pe calendar: {string.Join(", ", calendar.SelectedDates)}");
        }


        private async void OnCourseTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var selectedCourse = (Course)e.Item;

            await DisplayAlert("Detalii Curs",
                $"Nume: {selectedCourse.Name}\nInstructor: {selectedCourse.InstructorName}\nData: {selectedCourse.CourseDate:dd/MM/yyyy}\nDescriere: {selectedCourse.Description}",
                "OK");

            ((ListView)sender).SelectedItem = null;
        }

    }
}
