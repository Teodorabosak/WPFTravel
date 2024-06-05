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
                if (pomocniRed != null)
                {
                    txtVrstaPrevoza.Text = pomocniRed["Vrsta"].ToString();
                }
                else
                {
                    MessageBox.Show("Podaci nisu dostupni za ažuriranje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();

                //int idPrevoz = int.Parse(txtIdPrevoz.Text);
                string vrstaPrevoza = txtVrstaPrevoza.Text;

                string sacuvajPrevoz;

                if (azuriraj)
                {
                    sacuvajPrevoz = "UPDATE Prevoz SET Vrsta = @VrstaPrevoza WHERE Id_prevoz = @IdPrevoz";
                    SqlCommand cmd = new SqlCommand(sacuvajPrevoz, konekcija);
                    cmd.Parameters.AddWithValue("@IdPrevoz", (int)pomocniRed["Id_prevoz"]);
                    cmd.Parameters.AddWithValue("@VrstaPrevoza", vrstaPrevoza);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    sacuvajPrevoz = "INSERT INTO Prevoz (Vrsta) VALUES (@VrstaPrevoza)";
                    SqlCommand cmd = new SqlCommand(sacuvajPrevoz, konekcija);
                    cmd.Parameters.AddWithValue("@VrstaPrevoza", vrstaPrevoza);
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

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
