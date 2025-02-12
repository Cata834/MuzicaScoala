using MuzicaScoala.Models;

namespace MuzicaScoala
{
    public partial class InstructorDetailPage : ContentPage
    {
        public InstructorDetailPage(Instructor instructor)
        {
            InitializeComponent();

            // Setãm valorile din instructor în etichetele corespunzãtoare
            InstructorNameLabel.Text = instructor.Name;
            InstructorEmailLabel.Text = instructor.Email;
            InstructorPhoneLabel.Text = instructor.Phone;
        }
    }
}
