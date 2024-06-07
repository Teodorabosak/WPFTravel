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
            
        }

        public Rezervacija(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
           
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;

            if (azuriraj && pomocniRed != null)
            {
                if (pomocniRed.DataView.Table.Columns.Contains("Id_putovanja"))
                    cbPutovanje.SelectedValue = pomocniRed["Id_putovanja"];
                if (pomocniRed.DataView.Table.Columns.Contains("Datum_r"))
                    dpDatumR.SelectedDate = (DateTime)pomocniRed["Datum_r"];
                if (pomocniRed.DataView.Table.Columns.Contains("Id_korisnik"))
                    cbKorisnik.SelectedValue = pomocniRed["Id_korisnik"];
                if (pomocniRed.DataView.Table.Columns.Contains("Otkaz"))
                    chkOtkaz.IsChecked = pomocniRed["Otkaz"].ToString() == "Otkazana";
                if (pomocniRed.DataView.Table.Columns.Contains("Broj_aranzmana"))
                    txtBrojAranzmana.Text = pomocniRed["Broj_aranzmana"].ToString();
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

                string vratiKor = @"SELECT Id_korisnik, Username FROM Korisnik";
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

                //int id_Rez = int.Parse(txtIdRez.Text);
                DateTime datum = dpDatumR.SelectedDate.Value;

                string otkaz = chkOtkaz.IsChecked ?? false ? "Otkazana" : "Aktivna";
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
                    sacuvajRezervaciju = "INSERT INTO Rezervacija (Datum_r, Id_putovanja, Id_korisnik, Otkaz, Broj_aranzmana) " +
                                         "VALUES ( @DatumRezervacije, @IdPutovanja, @IdKorisnika, @Otkaz, @BrojAranzmana)";
                }
                SqlCommand cmd = new SqlCommand(sacuvajRezervaciju, konekcija);
                if (azuriraj)
                {
                    cmd.Parameters.AddWithValue("@IdRezervacije", pomocniRed["Id_rez"]);
                }
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
            this.Close();        }
    }
}
