using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFTravel.Forme
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	
	public partial class Korisnik : Window
	{
		SqlConnection konekcija = new SqlConnection();
		Konekcija kon = new Konekcija();

		private bool azuriraj;
		private DataRowView pomocniRed;


		public Korisnik()
		{
			InitializeComponent();
			txtIme.Focus();
			konekcija = kon.KreirajKonekciju();
		}

		public Korisnik( bool azuriraj, DataRowView pomocniRed)
		{

			InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtBrTel.Focus();
			this.azuriraj = azuriraj;
			this.pomocniRed = pomocniRed;

			if (azuriraj && pomocniRed != null)
                
			{
  
                    // Check if the expected columns are present
                    if (pomocniRed.DataView.Table.Columns.Contains("Br_Tel"))
                        txtBrTel.Text = pomocniRed["Br_Tel"].ToString();
                    if (pomocniRed.DataView.Table.Columns.Contains("Adresa"))
                        txtAdresa.Text = pomocniRed["Adresa"].ToString();
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
                    if (pomocniRed.DataView.Table.Columns.Contains("Datum_rodj"))
                        dpDatumRodj.SelectedDate = Convert.ToDateTime(pomocniRed["Datum_rodj"]);
                
            }

			

		}

		private void BtnSacuvaj(object sender, RoutedEventArgs e) //sacuvaj
		{

            if (string.IsNullOrEmpty(txtBrTel.Text) ||
                string.IsNullOrEmpty(txtAdresa.Text) ||
                string.IsNullOrEmpty(txtIme.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtPrezime.Text) ||
                string.IsNullOrEmpty(txtPassword.Password) ||
                string.IsNullOrEmpty(txtUsername.Text) ||
                dpDatumRodj.SelectedDate == null)
            {
                MessageBox.Show("Sva polja moraju biti popunjena!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                konekcija.Open();
                SqlCommand komanda = new SqlCommand();
                komanda.Connection = konekcija;

                if (azuriraj)
                {
                    komanda.CommandText = "UPDATE Korisnik SET Br_tel = @BrTel, Adresa = @Adresa, Ime = @Ime, Email = @Email, Prezime = @Prezime, Password = @Password, Username = @Username, Datum_rodj = @DatumRodj WHERE Id_korisnik = @IdKorisnik";
                    komanda.Parameters.AddWithValue("@IdKorisnik", (int)pomocniRed["Id_korisnik"]);
                }
                else
                {
                    komanda.CommandText = "INSERT INTO Korisnik (Br_tel, Adresa, Ime, Email, Prezime, Password, Username, Datum_rodj) VALUES (@BrTel, @Adresa, @Ime, @Email, @Prezime, @Password, @Username, @DatumRodj)";
                }

                komanda.Parameters.AddWithValue("@BrTel", txtBrTel.Text);
                komanda.Parameters.AddWithValue("@Adresa", txtAdresa.Text);
                komanda.Parameters.AddWithValue("@Ime", txtIme.Text);
                komanda.Parameters.AddWithValue("@Email", txtEmail.Text);
                komanda.Parameters.AddWithValue("@Prezime", txtPrezime.Text);
                komanda.Parameters.AddWithValue("@Password", txtPassword.Password);
                komanda.Parameters.AddWithValue("@Username", txtUsername.Text);
                komanda.Parameters.AddWithValue("@DatumRodj", dpDatumRodj.SelectedDate.Value);

                komanda.ExecuteNonQuery();
                MessageBox.Show("Podaci su uspešno sačuvani!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo je do greške: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                konekcija.Close();
            }
        }

		private void BtnOtkazi(object sender, RoutedEventArgs e) //otkazi
		{
			this.Close();
		}
	}
}
