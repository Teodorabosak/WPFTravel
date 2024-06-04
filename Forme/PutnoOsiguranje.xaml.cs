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

            if (azuriraj)
            {
                txtIdOsig.Text = pomocniRed["IdOsiguranja"].ToString();
                txtIznosO.Text = pomocniRed["IznosOsiguranja"].ToString();
                dpPocetak.SelectedDate = Convert.ToDateTime(pomocniRed["Pocetak"]);
                dpKraj.SelectedDate = Convert.ToDateTime(pomocniRed["Kraj"]);
                txtIdRez.SelectedValue = pomocniRed["IdRezervacije"];
                txtIdZ.SelectedValue = pomocniRed["IdZaposlenog"];
            }
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();

                string vratiRezervacije = "SELECT Id_putovanja, Destinacija FROM Putovanje";
                DataTable dtRezervacije = new DataTable();
                SqlDataAdapter daRezervacije = new SqlDataAdapter(vratiRezervacije, konekcija);
                daRezervacije.Fill(dtRezervacije);

                txtIdRez.ItemsSource = dtRezervacije.DefaultView;
                dtRezervacije.Dispose();
                daRezervacije.Dispose();

                string vratiZaposlene = "SELECT Id_korisnik FROM Korisnik";
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

                int idOsiguranja = int.Parse(txtIdOsig.Text);
                decimal iznosOsiguranja = decimal.Parse(txtIznosO.Text);
                DateTime pocetak = dpPocetak.SelectedDate.Value;
                DateTime kraj = dpKraj.SelectedDate.Value;
                int idRezervacije = Convert.ToInt32(((DataRowView)txtIdRez.SelectedItem)["Id_putovanja"]);
                int idZaposlenog = Convert.ToInt32(((DataRowView)txtIdZ.SelectedItem)["Id_korisnik"]);

                string sacuvajOsiguranje;

                if (azuriraj)
                {
                    sacuvajOsiguranje = "UPDATE PutnoOsiguranje SET IznosOsiguranja = @IznosOsiguranja, Pocetak = @Pocetak, Kraj = @Kraj, IdRezervacije = @IdRezervacije, IdZaposlenog = @IdZaposlenog WHERE IdOsiguranja = @IdOsiguranja";
                }
                else
                {
                    sacuvajOsiguranje = "INSERT INTO PutnoOsiguranje (IdOsiguranja, IznosOsiguranja, Pocetak, Kraj, IdRezervacije, IdZaposlenog) VALUES (@IdOsiguranja, @IznosOsiguranja, @Pocetak, @Kraj, @IdRezervacije, @IdZaposlenog)";
                }

                SqlCommand cmd = new SqlCommand(sacuvajOsiguranje, konekcija);
                cmd.Parameters.AddWithValue("@IdOsiguranja", idOsiguranja);
                cmd.Parameters.AddWithValue("@IznosOsiguranja", iznosOsiguranja);
                cmd.Parameters.AddWithValue("@Pocetak", pocetak);
                cmd.Parameters.AddWithValue("@Kraj", kraj);
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
