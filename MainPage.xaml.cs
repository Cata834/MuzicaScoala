using MuzicaScoala.Models;
using MuzicaScoala.Data;
using System.Collections.Generic;
using System.Linq;

namespace MuzicaScoala
{
    public partial class MainPage : ContentPage
    {
        private List<Instructor> _allInstructors = new List<Instructor>();

        public MainPage()
        {
            InitializeComponent();
            LoadInstructors();
        }

        private async void LoadInstructors()
        {
            _allInstructors = await App.Database.GetInstructorsAsync();
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = e.NewTextValue?.ToLower() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return; // Nu face nimic dacă bara e goală
            }

            var foundInstructor = _allInstructors
                .FirstOrDefault(i => i.Name.ToLower().Contains(searchTerm));

            if (foundInstructor != null)
            {
                await Navigation.PushAsync(new InstructorPage(foundInstructor));
            }
        }

        private async void OnCoursesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CoursesPage());
        }

        private async void OnInstructorClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InstructorPage());
        }

        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }

        private async void OnAddCourseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCoursePage());
        }

        private async void OnAddInstructorClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddInstructorPage());
        }
    }
}
