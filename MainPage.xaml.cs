using MuzicaScoala.Models; // Asigură-te că ai referință la modelele tale
using MuzicaScoala.Data;

namespace MuzicaScoala
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Navigăm către pagina cu lista de cursuri
        private async void OnCoursesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CoursesPage());
        }

        // Navigăm către pagina cu instructorii
        private async void OnInstructorClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InstructorPage());
        }

        // Navigăm către pagina "About"
        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }

        // Navigăm către pagina AddCoursePage pentru a adăuga un curs nou
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
