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

        // �nc�rc�m cursurile din baza de date c�nd pagina este ap�sat�
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var courses = await App.Database.GetCoursesAsync();
            CoursesListView.ItemsSource = courses;
        }

        // C�nd utilizatorul selecteaz� un curs pentru a-l edita
        private async void OnCourseTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var selectedCourse = (Course)e.Item;
                await Navigation.PushAsync(new AddCoursePage(selectedCourse)); // Navig�m la pagina de editare a cursului
            }
        }

        // C�nd utilizatorul apas� pe "Add New Course"
        private async void OnAddCourseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCoursePage()); // Navig�m la pagina de ad�ugare curs
        }
    }
}
