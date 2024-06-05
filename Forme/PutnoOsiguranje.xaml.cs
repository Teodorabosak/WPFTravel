using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WPFTravel.Forme
{
    /// <summary>
    /// Interaction logic for PutnoOsiguranje.xaml
    /// </summary>
    public partial class PutnoOsiguranje : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();

        private bool azuriraj;
        private DataRowView pomocniRed;

        public PutnoOsiguranje()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public PutnoOsiguranje(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();

            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;

            if (azuriraj && pomocniRed != null)
            {
                //txtIdOsig.Text = pomocniRed["Id_osig"].ToString();
                if (pomocniRed.DataView.Table.Columns.Contains("Iznos"))
                    txtIznosO.Text = pomocniRed["Iznos"].ToString();
                if (pomocniRed.DataView.Table.Columns.Contains("Pocetak"))
                    dpPocetak.SelectedDate = (DateTime)pomocniRed["Pocetak"];
                if (pomocniRed.DataView.Table.Columns.Contains("Kraj"))
                    dpKraj.SelectedDate = (DateTime)pomocniRed["Kraj"];
                if (pomocniRed.DataView.Table.Columns.Contains("Id_rez"))
                    txtIdRez.SelectedValue = pomocniRed["Id_rez"];
                if (pomocniRed.DataView.Table.Columns.Contains("Id_z"))
                    txtIdZ.SelectedValue = pomocniRed["Id_z"];
            }
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();

                string vratiRezervacije = "SELECT Id_rez, id_korisnik FROM Rezervacija";
                DataTable dtRezervacije = new DataTable();
                SqlDataAdapter daRezervacije = new SqlDataAdapter(vratiRezervacije, konekcija);
                daRezervacije.Fill(dtRezervacije);

                txtIdRez.ItemsSource = dtRezervacije.DefaultView;
                dtRezervacije.Dispose();
                daRezervacije.Dispose();

                string vratiZaposlene = "SELECT Id_z, Username FROM Zaposleni";
                DataTable dtZaposleni = new DataTable();
                SqlDataAdapter daZaposleni = new SqlDataAdapter(vratiZaposlene, konekcija);
                daZaposleni.Fill(dtZaposleni);

                txtIdZ.ItemsSource = dtZaposleni.DefaultView;
                dtZaposleni.Dispose();
                daZaposleni.Dispose();
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

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();

                

                int idRezervacije = Convert.ToInt32(((DataRowView)txtIdRez.SelectedItem)["Id_rez"]);
                int idZaposlenog = Convert.ToInt32(((DataRowView)txtIdZ.SelectedItem)["Id_z"]);

                string sacuvajOsig;

                if (azuriraj)
                {
                    sacuvajOsig = "UPDATE Putno_Osiguranje SET Iznos_O = @IznosOsiguranja, " +
                        "Pocetak = @Pocetak, Kraj = @Kraj, Id_rez = @IdRezervacije, Id_z = @IdZaposlenog " +
                        "WHERE Id_osig= @IdOsiguranja";

                    //komanda.Parameters.AddWithValue("@IdOsiguranja", (int) pomocniRed["Id_osig"]);
                }
                else
                {
                    sacuvajOsig = "INSERT INTO Putno_Osiguranje (Iznos_o, Pocetak, Kraj, Id_rez, Id_z) " +
                        "VALUES (@IznosOsiguranja, @Pocetak, @Kraj, @IdRezervacije, @IdZaposlenog)";
                }
                SqlCommand cmd = new SqlCommand(sacuvajOsig, konekcija);

                if(azuriraj)
                {
                    cmd.Parameters.AddWithValue("@IdOsiguranja", (int)pomocniRed["Id_osig"]);
                }

                cmd.Parameters.AddWithValue("@IznosOsiguranja", txtIznosO.Text);
                cmd.Parameters.AddWithValue("@Pocetak", dpPocetak.SelectedDate);
                cmd.Parameters.AddWithValue("@Kraj", dpKraj.SelectedDate);
                cmd.Parameters.AddWithValue("@IdRezervacije", idRezervacije);
                cmd.Parameters.AddWithValue("@IdZaposlenog", idZaposlenog);

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
