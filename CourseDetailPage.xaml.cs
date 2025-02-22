using MuzicaScoala.Models;

namespace MuzicaScoala
{
    public partial class CourseDetailPage : ContentPage
    {
        private DateTime _selectedDate;

        public CourseDetailPage(DateTime selectedDate)
        {
            InitializeComponent();
            _selectedDate = selectedDate;
            LoadCourses();
        }

        private async void LoadCourses()
        {
            var courses = await App.Database.GetCoursesByDateAsync(_selectedDate);
            CoursesListView.ItemsSource = courses;
        }
    }

}
