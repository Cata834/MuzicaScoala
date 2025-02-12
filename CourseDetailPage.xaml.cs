using MuzicaScoala.Models;

namespace MuzicaScoala
{
    public partial class CourseDetailPage : ContentPage
    {
        private readonly Course _course;

        public CourseDetailPage(Course course)
        {
            InitializeComponent();
            _course = course;

            // Afișăm detaliile cursului pe pagină
            CourseNameLabel.Text = _course.Name;
            CourseDescriptionLabel.Text = _course.Description;
            InstructorLabel.Text = _course.Instructor;
        }
    }
}
