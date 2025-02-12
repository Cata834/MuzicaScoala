using MuzicaScoala.Data;
using MuzicaScoala.Models;

namespace MuzicaScoala
{
    public partial class InstructorPage : ContentPage
    {
        public InstructorPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var name = InstructorNameEntry.Text;
            var email = InstructorEmailEntry.Text;
            var phone = InstructorPhoneEntry.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            var instructor = new Instructor
            {
                Name = name,
                Email = email,
                Phone = phone
            };

            // Salvează instructorul în baza de date
            var result = await App.Database.AddInstructorAsync(instructor);

            if (result > 0)
            {
                // Succes
                await DisplayAlert("Success", "Instructor saved successfully!", "OK");
                await Navigation.PopAsync(); // Navighează înapoi la pagina anterioară
            }
            else if (result == -1)
            {
                // Instructorul există deja
                await DisplayAlert("Error", "Instructor already exists.", "OK");
            }
            else
            {
                // Eroare la salvare
                await DisplayAlert("Error", "Failed to save instructor.", "OK");
            }
        }
    }
}
