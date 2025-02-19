using MuzicaScoala.Models; // Asigură-te că ai referință la modelele tale
using MuzicaScoala.Data;
using System;

namespace MuzicaScoala
{
    public partial class AddInstructorPage : ContentPage
    {
        public AddInstructorPage()
        {
            InitializeComponent();
        }

        // Logica pentru a salva instructorul în baza de date
        private async void OnSaveInstructorClicked(object sender, EventArgs e)
        {
            // Obține informațiile din Entry-uri
            string name = NameEntry.Text;
            string email = EmailEntry.Text;
            string phone = PhoneEntry.Text;

            // Verifică dacă toate câmpurile sunt completate
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            // Creează un nou obiect Instructor
            Instructor newInstructor = new Instructor
            {
                Name = name,
                Email = email,
                Phone = phone
            };

            // Adaugă instructorul în baza de date
            await App.Database.AddInstructorAsync(newInstructor);


            // Navighează înapoi la pagina principală (sau o altă pagină dorită)
            await Navigation.PopAsync();
        }
    }
}
