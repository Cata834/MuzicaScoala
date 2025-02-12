using MuzicaScoala.Models;

namespace MuzicaScoala
{
    public partial class InstructorDetailPage : ContentPage
    {
        public InstructorDetailPage(Instructor instructor)
        {
            InitializeComponent();

            // Set�m valorile din instructor �n etichetele corespunz�toare
            InstructorNameLabel.Text = instructor.Name;
            InstructorEmailLabel.Text = instructor.Email;
            InstructorPhoneLabel.Text = instructor.Phone;
        }
    }
}
