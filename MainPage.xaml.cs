namespace MuzicaScoala
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCoursesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CoursesPage());
        }

        
        private async void OnInstructorsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InstructorsPage());
        }

        
        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}