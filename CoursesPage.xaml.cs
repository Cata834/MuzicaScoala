using MuzicaScoala.Data;
using MuzicaScoala.Models;

namespace MuzicaScoala
{
    public partial class CoursesPage : ContentPage
    {
        public CoursesPage()
        {
            InitializeComponent();
        }

        // Încãrcãm cursurile din baza de date când pagina este apãsatã
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var courses = await App.Database.GetCoursesAsync();
            CoursesListView.ItemsSource = courses;
        }

        // Când utilizatorul selecteazã un curs pentru a-l edita
        private async void OnCourseTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var selectedCourse = (Course)e.Item;
                await Navigation.PushAsync(new AddCoursePage(selectedCourse)); // Navigãm la pagina de editare a cursului
            }
        }

        // Când utilizatorul apasã pe "Add New Course"
        private async void OnAddCourseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCoursePage()); // Navigãm la pagina de adãugare curs
        }
    }
}
