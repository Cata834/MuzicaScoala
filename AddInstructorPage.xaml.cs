using MuzicaScoala.Models;
using MuzicaScoala.Data;
using System;
using System.Collections.Generic;

namespace MuzicaScoala
{
    public partial class AddInstructorPage : ContentPage
    {
        private List<string> _coursesList = new List<string> { "Pian", "Chitară", "Vioară", "Tobe", "Canto" }; // Lista de cursuri disponibile

        public AddInstructorPage()
        {
            InitializeComponent();
            LoadCourses(); // Încărcăm lista de cursuri în Picker
        }

        private void LoadCourses()
        {
            CoursesPicker.ItemsSource = _coursesList;
            CoursesPicker.ItemDisplayBinding = new Binding("."); // Afișează valoarea direct
        }

        private async void OnSaveInstructorClicked(object sender, EventArgs e)
        {
            string name = NameEntry.Text;
            string email = EmailEntry.Text;
            string phone = PhoneEntry.Text;
            string selectedCourse = CoursesPicker.SelectedItem?.ToString(); // Obținem cursul selectat

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) || selectedCourse == null)
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            Instructor newInstructor = new Instructor
            {
                Name = name,
                Email = email,
                Phone = phone,
                Courses = selectedCourse // Stocăm cursul ca string
            };

            await App.Database.AddInstructorAsync(newInstructor);
            await Navigation.PopAsync();
        }
    }
}
