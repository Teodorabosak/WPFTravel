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
	/// Interaction logic for Komentar.xaml
	/// </summary>
	public partial class Komentar : Window
	{
		SqlConnection konekcija = new SqlConnection();
		Konekcija kon = new Konekcija();

        private bool azuriraj;
        private DataRowView pomocniRed;

        public Komentar()
		{
			InitializeComponent();
			
			konekcija = kon.KreirajKonekciju();
			PopuniPadajuceListe();
		}
        public Komentar(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            this.azuriraj = azuriraj; //azuriraj je kao ozn da li je u pitanju azuriranje ili insert
            this.pomocniRed = pomocniRed;
            //Ako je pomocniRed null, to znači da se kreira novi zapis,
            //a ako nije null, znači da se ažurira postojeći zapis.

            if (azuriraj && pomocniRed != null)
            {
                
               
                if (pomocniRed.DataView.Table.Columns.Contains("Id_korisnik"))
                    cbKorisnik.SelectedValue = pomocniRed["Id_korisnik"];
                if (pomocniRed.DataView.Table.Columns.Contains("Komentar"))
                    txtKomentar.Text = pomocniRed["Komentar"].ToString();
                if (pomocniRed.DataView.Table.Columns.Contains("Ocena"))
                    txtOcena.Text = pomocniRed["Ocena"].ToString();
            }
            //Ovo omogućava da se forma popuni sa podacima iz postojećeg zapisa koji se ažurira.

        }
        private void PopuniPadajuceListe()
        {
            try
            {
                //gde ima combobox
                konekcija.Open();

                string vratiKor = @"select Id_korisnik, Username from Korisnik"; //sta treba da nam vraca
                DataTable dtKorisnik = new DataTable(); 
                SqlDataAdapter daKorisnik = new SqlDataAdapter(vratiKor, konekcija);
                daKorisnik.Fill(dtKorisnik);

                cbKorisnik.ItemsSource = dtKorisnik.DefaultView; 
                //će prikazati podatke iz DataTable-a dtKorisnik u svom padajućem meniju,
                //omogućavajući korisnicima da izaberu jednu od opcija koje su prikazane.
                dtKorisnik.Dispose(); //oslobadja resurse
                daKorisnik.Dispose();

            }
            catch (SqlException)
            {
                MessageBox.Show("Padajuće liste nisu popunjene!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

		private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
		{
            if (cbKorisnik.SelectedValue == null ||
        string.IsNullOrEmpty(txtOcena.Text) ||
        string.IsNullOrEmpty(txtKomentar.Text))
            {
                MessageBox.Show("Sva polja moraju biti popunjena!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(txtOcena.Text, out decimal ocena) || ocena < 1 || ocena > 5)
            {
                MessageBox.Show("Ocena mora biti broj između 1 i 5!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                konekcija.Open();
                SqlCommand komanda = new SqlCommand();
                komanda.Connection = konekcija;

                int idKorisnika = Convert.ToInt32(((DataRowView)cbKorisnik.SelectedItem)["Id_korisnik"]);

                if (azuriraj)
                {
                    komanda.CommandText = "UPDATE Komentar SET Id_korisnik = @IdKorisnik, Ocena = @Ocena, Komentar = @Komentar WHERE Id_kom = @IdKom";
                    komanda.Parameters.AddWithValue("@IdKom", (int)pomocniRed["Id_kom"]);
                }
                else
                {
                    komanda.CommandText = "INSERT INTO Komentar (Id_korisnik, Ocena, Komentar) VALUES (@IdKorisnik, @Ocena, @Komentar)";
                }

                komanda.Parameters.AddWithValue("@IdKorisnik", idKorisnika);
                komanda.Parameters.AddWithValue("@Ocena", ocena);
                komanda.Parameters.AddWithValue("@Komentar", txtKomentar.Text);

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

		private void btnOtkazi_Click(object sender, RoutedEventArgs e)
		{
            this.Close();
        }

		
	}
}
