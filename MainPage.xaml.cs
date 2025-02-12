using MuzicaScoala.Data;
using MuzicaScoala.Models;

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
            // Poți naviga către o pagină care afișează cursurile, de exemplu CoursesPage.xaml
            await Navigation.PushAsync(new CoursesPage());
        }

        // Navigăm către pagina cu instructorii
        private async void OnInstructorsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InstructorsPage());
        }

        // Navigăm către pagina "About"
        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }

        // Navigăm către pagina AddCoursePage pentru a adăuga un curs nou
        private async void OnAddCourseClicked(object sender, EventArgs e)
        {
            // Navigăm către pagina AddCoursePage pentru a adăuga un curs nou
            await Navigation.PushAsync(new AddCoursePage());
        }
    }
}
