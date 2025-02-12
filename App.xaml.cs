using MuzicaScoala.Data; // Am schimbat namespace-ul
using System;
using System.IO;

namespace MuzicaScoala
{
    public partial class App : Application
    {
        private static MusicSchoolDatabase _database;

        public static MusicSchoolDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "musicschooldatabase.db");
                    _database = new MusicSchoolDatabase(dbPath);
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();

            // Wrap the MainPage with a NavigationPage to enable navigation.
            MainPage = new NavigationPage(new MainPage());
          //  MainPage = new NavigationPage(new CoursesPage());

        }
    }
}
