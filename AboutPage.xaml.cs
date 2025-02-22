namespace MuzicaScoala
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        // Navigheazã la CoursePage
        private async void OnViewCoursesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CoursesPage());
        }

        // Navigheazã la InstructorPage
        private async void OnViewInstructorsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InstructorPage());
        }
    }
}
