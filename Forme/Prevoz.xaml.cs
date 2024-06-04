using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WPFTravel.Forme
{
    /// <summary>
    /// Interaction logic for Prevoz.xaml
    /// </summary>
    public partial class Prevoz : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();

        private bool azuriraj;
        private DataRowView pomocniRed;

        public Prevoz()
        {
            InitializeComponent();
            txtVrstaPrevoza.Focus();
            konekcija = kon.KreirajKonekciju();
        }

        public Prevoz(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtVrstaPrevoza.Focus();
            konekcija = kon.KreirajKonekciju();

            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;

            if (azuriraj)
            {
                txtIdPrevoz.Text = pomocniRed["Id_prevoz"].ToString();
                txtVrstaPrevoza.Text = pomocniRed["Vrsta"].ToString();
            }
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();

                int idPrevoz = int.Parse(txtIdPrevoz.Text);
                string vrstaPrevoza = txtVrstaPrevoza.Text;

                string sacuvajPrevoz;

                if (azuriraj)
                {
                    sacuvajPrevoz = "UPDATE VrstaPrevoza SET Vrsta = @VrstaPrevoza WHERE Id_prevoz = @IdPrevoz";
                }
                else
                {
                    sacuvajPrevoz = "INSERT INTO VrstaPrevoza (Id_prevoz, Vrsta) VALUES (@IdPrevoz, @VrstaPrevoza)";
                }

                SqlCommand cmd = new SqlCommand(sacuvajPrevoz, konekcija);
                cmd.Parameters.AddWithValue("@IdPrevoz", idPrevoz);
                cmd.Parameters.AddWithValue("@VrstaPrevoza", vrstaPrevoza);
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

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
