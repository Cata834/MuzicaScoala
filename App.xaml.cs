using MuzicaScoala.Data;
using Syncfusion.Licensing;
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
                    // Setăm calea pentru fișierul de baze de date SQLite
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "musicschooldatabase.db");
                    _database = new MusicSchoolDatabase(dbPath); // Creăm o instanță a bazei de date
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF1cXmhKYVF3WmFZfVtgcF9EaVZVQWY/P1ZhSXxWdkdiUH5Ycn1RQWBaUUU=");
            MainPage = new NavigationPage(new MainPage());
        }
    }
}
