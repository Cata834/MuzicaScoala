using Syncfusion.Maui.Calendar;
using Syncfusion.Maui.Core;
using MuzicaScoala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(200); 

            try
            {
                await LoadCourses();
                var courses = await App.Database.GetCoursesAsync();
                var datesWithCourses = courses
                    .Where(c => c.CourseDate != DateTime.MinValue)
                    .Select(c => (DateTime?)c.CourseDate)
                    .ToList();

                if (calendar != null) // Verificăm din nou înainte de a seta SelectedDates
                {
                    MarkDatesOnCalendar(datesWithCourses);
                }
                else
                {
                    Console.WriteLine("EROARE: Calendarul este NULL!");
                }
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
        private void OnSelectionChanged(object sender, Syncfusion.Maui.Calendar.CalendarSelectionChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                DateTime selectedDate = (DateTime)e.NewValue;
                Console.WriteLine($"Data selectată: {selectedDate:dd/MM/yyyy}");
            }
        }

        // Când utilizatorul apasă pe "Adaugă Curs"
        private async void OnAddCourseClicked(object sender, EventArgs e)
        {
            var selectedDate = calendar.SelectedDate;

            if (selectedDate.HasValue)
            {
                Console.WriteLine($"Cursul va fi programat pentru: {selectedDate.Value:dd/MM/yyyy}");

                var newCourse = new Course
                {
                    Name = "Numele Cursului",
                    Description = "Descrierea cursului",
                    InstructorId = 1,
                    CourseDate = selectedDate
                };

                await Navigation.PushAsync(new AddCoursePage(newCourse));
            }
        }

        // Adăugarea de marcaje pe calendar
        private void MarkDatesOnCalendar(List<DateTime?> datesWithCourses)
        {
            if (calendar == null)
            {
                Console.WriteLine("EROARE: calendar este null!");
                return;
            }

            var selectedDates = datesWithCourses
                .Where(date => date.HasValue)
                .Select(date => date.Value)
                .ToList();

            // Creăm o ObservableCollection din lista selectedDates
            var observableSelectedDates = new System.Collections.ObjectModel.ObservableCollection<DateTime>(selectedDates);

            // Acum atribuim ObservableCollection calendarului
            calendar.SelectedDates = observableSelectedDates;
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
