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
            

            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;


            if (azuriraj)
            {
                if (pomocniRed != null)
                {
                    // Provera da li su kolone prisutne i popunjavanje TextBox-ova
                    
                    if (pomocniRed.DataView.Table.Columns.Contains("Email"))
                        txtEmail.Text = pomocniRed["Email"].ToString();
                    if (pomocniRed.DataView.Table.Columns.Contains("Ime"))
                        txtIme.Text = pomocniRed["Ime"].ToString();
                    if (pomocniRed.DataView.Table.Columns.Contains("Prezime"))
                        txtPrezime.Text = pomocniRed["Prezime"].ToString();
                    if (pomocniRed.DataView.Table.Columns.Contains("Username"))
                        txtUsername.Text = pomocniRed["Username"].ToString();
                    if (pomocniRed.DataView.Table.Columns.Contains("Password"))
                        txtPassword.Password = pomocniRed["Password"].ToString();
                    
                }
                else
                {
                    MessageBox.Show("Podaci nisu dostupni za ažuriranje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            konekcija = kon.KreirajKonekciju();
        }

        private void BtnSacuvaj(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand komanda = new SqlCommand();
                komanda.Connection = konekcija;



                if (azuriraj)
                {
                    komanda.CommandText = "UPDATE Zaposleni SET Ime_z = @Ime, Prez_z = @Prezime, Email = @Email, Password = @Password, Username = @Username WHERE Id_z = @IdZaposlenog";

                    komanda.Parameters.AddWithValue("@IdZaposlenog", (int)pomocniRed["Id_z"]);
                }
                else
                {
                    komanda.CommandText = "INSERT INTO Zaposleni (Ime_z, Prez_z, Email, Password, Username) VALUES (@Ime, @Prezime, @Email, @Password, @Username)";


                }



                komanda.Parameters.AddWithValue("@Ime", txtIme.Text);
                komanda.Parameters.AddWithValue("@Prezime", txtPrezime.Text);
                komanda.Parameters.AddWithValue("@Email", txtEmail.Text);
                komanda.Parameters.AddWithValue("@Password", txtPassword.Password);
                komanda.Parameters.AddWithValue("@Username", txtUsername.Text);

                komanda.ExecuteNonQuery();

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
