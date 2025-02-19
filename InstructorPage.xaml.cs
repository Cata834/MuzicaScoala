using MuzicaScoala.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MuzicaScoala
{
    public partial class InstructorPage : ContentPage
    {
        public InstructorPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadInstructors(); // Când pagina apare, încărcăm instructorii
        }

        private async Task LoadInstructors()
        {
            var instructors = await App.Database.GetInstructorsAsync();
            InstructorListView.ItemsSource = instructors; // Afișează instructorii
        }

        private async void OnAddInstructorClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddInstructorPage());
        }
    }
}
