using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WPFTravel.Forme
{
    /// <summary>
    /// Interaction logic for Rezervacija.xaml
    /// </summary>
    public partial class Rezervacija : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();

        private bool azuriraj;
        private DataRowView pomocniRed;

        public Rezervacija()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            txtIdRez.Focus();
        }

        public Rezervacija(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            txtIdRez.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;

            if (azuriraj)
            {
                txtIdRez.Text = pomocniRed["Id_rez"].ToString();
                cbPutovanje.SelectedValue = pomocniRed["Id_putovanja"];
                dpDatumR.SelectedDate = (DateTime)pomocniRed["Datum_r"];
                cbKorisnik.SelectedValue = pomocniRed["Id_korisnik"];
                chkOtkaz.IsChecked = pomocniRed["Otkaz"].ToString() == "otkazana";
                txtBrojAranzmana.Text = pomocniRed["Broj_aranzmana"].ToString();
                txtIdRez.IsEnabled = false;
            }
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();

                string vratiPutovanja = @"SELECT Id_putovanja, Destinacija FROM Putovanje";
                DataTable dtPutovanja = new DataTable();
                SqlDataAdapter daPutovanja = new SqlDataAdapter(vratiPutovanja, konekcija);
                daPutovanja.Fill(dtPutovanja);

                cbPutovanje.ItemsSource = dtPutovanja.DefaultView;
                dtPutovanja.Dispose();
                daPutovanja.Dispose();

                string vratiKor = @"SELECT Id_korisnik FROM Korisnik";
                DataTable dtKor = new DataTable();
                SqlDataAdapter daKor = new SqlDataAdapter(vratiKor, konekcija);
                daKor.Fill(dtKor);

                cbKorisnik.ItemsSource = dtKor.DefaultView;
                dtKor.Dispose();
                daKor.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Padajuće liste nisu popunjene! " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        private void BtnSacuvaj(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();

                int id_Rez = int.Parse(txtIdRez.Text);
                DateTime datum = dpDatumR.SelectedDate.Value;

                string otkaz = chkOtkaz.IsChecked ?? false ? "otkazana" : "aktivna";
                int idPutovanja = Convert.ToInt32(((DataRowView)cbPutovanje.SelectedItem)["Id_putovanja"]);
                int idKorisnika = Convert.ToInt32(((DataRowView)cbKorisnik.SelectedItem)["Id_korisnik"]);
                int brojAranzmana = int.Parse(txtBrojAranzmana.Text);

                string sacuvajRezervaciju;

                if (azuriraj)
                {
                    sacuvajRezervaciju = "UPDATE Rezervacija SET Datum_r = @DatumRezervacije, Id_putovanja = @IdPutovanja, Id_korisnik = @IdKorisnika, Otkaz = @Otkaz, Broj_aranzmana = @BrojAranzmana " +
                                         "WHERE Id_rez = @IdRezervacije";
                }
                else
                {
                    sacuvajRezervaciju = "INSERT INTO Rezervacija (Id_rez, Datum_r, Id_putovanja, Id_korisnik, Otkaz, Broj_aranzmana) " +
                                         "VALUES (@IdRezervacije, @DatumRezervacije, @IdPutovanja, @IdKorisnika, @Otkaz, @BrojAranzmana)";
                }

                SqlCommand cmd = new SqlCommand(sacuvajRezervaciju, konekcija);
                cmd.Parameters.AddWithValue("@IdRezervacije", id_Rez);
                cmd.Parameters.AddWithValue("@DatumRezervacije", datum);
                cmd.Parameters.AddWithValue("@IdPutovanja", idPutovanja);
                cmd.Parameters.AddWithValue("@IdKorisnika", idKorisnika);
                cmd.Parameters.AddWithValue("@Otkaz", otkaz);
                cmd.Parameters.AddWithValue("@BrojAranzmana", brojAranzmana);
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

        private void BtnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
