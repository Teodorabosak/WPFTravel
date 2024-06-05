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
            konekcija = kon.KreirajKonekciju();

        }

        public TipPutovanja(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtNaziv.Focus();

            konekcija = kon.KreirajKonekciju();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;

            if (azuriraj)
            {
                if (pomocniRed != null)
                {
                    txtNaziv.Text = pomocniRed["Naziv"].ToString();
                }
                else
                {
                    MessageBox.Show("Podaci nisu dostupni za ažuriranje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                    sacuvajTipPutovanja = "UPDATE Tip_putovanja SET Naziv = @Naziv WHERE Id_tip = @Id_tip";
                    SqlCommand cmd = new SqlCommand(sacuvajTipPutovanja, konekcija);
                    cmd.Parameters.AddWithValue("@Id_tip", (int)pomocniRed["Id_tip"]);
                    cmd.Parameters.AddWithValue("@Naziv", naziv); 
                    cmd.ExecuteNonQuery();
                }

                else
                {
                    sacuvajTipPutovanja = "INSERT INTO Tip_Putovanja (Naziv) VALUES (@Naziv)";
                    SqlCommand cmd = new SqlCommand(sacuvajTipPutovanja, konekcija);
                    cmd.Parameters.AddWithValue("@Naziv", naziv); 

                    cmd.ExecuteNonQuery();
                }
                
                
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
