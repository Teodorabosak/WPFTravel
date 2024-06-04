using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WPFTravel.Forme
{
    /// <summary>
    /// Interaction logic for TipPutovanja.xaml
    /// </summary>
    public partial class TipPutovanja : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();

        private bool azuriraj;
        private DataRowView pomocniRed;

        public TipPutovanja()
        {
            InitializeComponent();
            txtNaziv.Focus();
        }

        public TipPutovanja(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtNaziv.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;

            konekcija = kon.KreirajKonekciju();

            if (azuriraj)
            {
                txtNaziv.Text = pomocniRed["Naziv"].ToString();
            }
        }

        private void BtnSacuvaj(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();

                string naziv = txtNaziv.Text;
                string sacuvajTipPutovanja;

                if (azuriraj)
                {
                    sacuvajTipPutovanja = "UPDATE TipPutovanja SET Naziv = @Naziv WHERE Id = @Id";
                }
                else
                {
                    sacuvajTipPutovanja = "INSERT INTO TipPutovanja (Naziv) VALUES (@Naziv)";
                }

                SqlCommand cmd = new SqlCommand(sacuvajTipPutovanja, konekcija);
                cmd.Parameters.AddWithValue("@Naziv", naziv);

                if (azuriraj)
                {
                    cmd.Parameters.AddWithValue("@Id", pomocniRed["Id"]);
                }

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
