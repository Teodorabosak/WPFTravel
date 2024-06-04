using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WPFTravel.Forme
{
    /// <summary>
    /// Interaction logic for Zaposleni.xaml
    /// </summary>
    public partial class Zaposleni : Window
    {
        private bool azuriraj;
        private DataRowView pomocniRed;

        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();

        public Zaposleni()
        {
            InitializeComponent();
            txtIme.Focus();
            konekcija = kon.KreirajKonekciju();
        }

        public Zaposleni(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtIme.Focus();
            konekcija = kon.KreirajKonekciju();

            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;

            if (azuriraj)
            {
                txtIdZaposlenog.Text = pomocniRed["Id_z"].ToString();
                txtIme.Text = pomocniRed["Ime_z"].ToString();
                txtPrezime.Text = pomocniRed["Prez_z"].ToString();
                txtEmail.Text = pomocniRed["Email"].ToString();
                txtPassword.Password = pomocniRed["Password"].ToString();
                txtUsername.Text = pomocniRed["Username"].ToString();
            }
        }

        private void BtnSacuvaj(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();

                int idZaposlenog = int.Parse(txtIdZaposlenog.Text);
                string ime = txtIme.Text;
                string prezime = txtPrezime.Text;
                string email = txtEmail.Text;
                string password = txtPassword.Password; // Use PasswordBox for security
                string username = txtUsername.Text;

                string sacuvajZaposlenog;

                if (azuriraj)
                {
                    sacuvajZaposlenog = "UPDATE Zaposleni SET Ime_z = @Ime, Prez_z = @Prezime, Email = @Email, Password = @Password, Username = @Username WHERE Id_z = @IdZaposlenog";
                }
                else
                {
                    sacuvajZaposlenog = "INSERT INTO Zaposleni (Id_z, Ime_z, Prez_z, Email, Password, Username) VALUES (@IdZaposlenog, @Ime, @Prezime, @Email, @Password, @Username)";
                }

                SqlCommand cmd = new SqlCommand(sacuvajZaposlenog, konekcija);
                cmd.Parameters.AddWithValue("@IdZaposlenog", idZaposlenog);
                cmd.Parameters.AddWithValue("@Ime", ime);
                cmd.Parameters.AddWithValue("@Prezime", prezime);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Podaci su uspešno sačuvani.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo je do greške prilikom čuvanja podataka: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija.State == ConnectionState.Open)
                {
                    konekcija.Close();
                }
            }
        }

        private void BtnOtkazi(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
