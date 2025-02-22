using MuzicaScoala.Models;
using MuzicaScoala.Data;
using System;
using System.Collections.Generic;

namespace MuzicaScoala
{
    public partial class AddInstructorPage : ContentPage
    {
        private Instructor _currentInstructor;
        private List<string> _coursesList = new List<string> { "Pian", "Chitară", "Vioară", "Tobe", "Canto" }; // Lista de cursuri disponibile

        public AddInstructorPage(Instructor instructor = null)
        {
            InitializeComponent();
            _currentInstructor = instructor;
            LoadCourses(); // Încărcăm lista de cursuri în Picker
            if (_currentInstructor != null)
            {
                // Pre-completăm câmpurile cu datele instructorului existent
                NameEntry.Text = _currentInstructor.Name;
                EmailEntry.Text = _currentInstructor.Email;
                PhoneEntry.Text = _currentInstructor.Phone;
                CoursesPicker.SelectedItem = _currentInstructor.Courses;
            }
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

            Instructor instructor;

            if (_currentInstructor != null)
            {
                // Actualizăm instructorul existent
                _currentInstructor.Name = name;
                _currentInstructor.Email = email;
                _currentInstructor.Phone = phone;
                _currentInstructor.Courses = selectedCourse;

                instructor = _currentInstructor; // Folosim instructorul existent
            }
            else
            {
                // Adăugăm un instructor nou
                instructor = new Instructor
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    Courses = selectedCourse
                };
            }

            // Salvăm instructorul în baza de date
            await App.Database.SaveInstructorAsync(instructor);

            // Ne întoarcem la pagina anterioară
            await Navigation.PopAsync();
        }
    }
   }

