namespace MuzicaScoala
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        // Navighează la CoursePage
        private async void OnViewCoursesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CoursesPage());
        }

        // Navighează la InstructorPage
        private async void OnViewInstructorsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InstructorPage());
        }
    }
}
