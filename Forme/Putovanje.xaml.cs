using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WPFTravel.Forme
{
    /// <summary>
    /// Interaction logic for Putovanje.xaml
    /// </summary>
    public partial class Putovanje : Window
    {
        private bool azuriraj;
        private DataRowView pomocniRed;

        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();

        public Putovanje()
        {
            InitializeComponent();
            txtDestinacija.Focus();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public Putovanje(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtDestinacija.Focus();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;

            if (azuriraj)
            {
                txtIdPutovanja.Text = pomocniRed["Id_putovanja"].ToString();
                dpDatum.SelectedDate = Convert.ToDateTime(pomocniRed["Datum"]);
                txtDestinacija.Text = pomocniRed["Destinacija"].ToString();
                txtCena.Text = pomocniRed["Cena"].ToString();
                txtOpis.Text = pomocniRed["Opis"].ToString();
                cbTipPutovanja.SelectedValue = pomocniRed["Id_tip"];
                cbVrstaPrevoza.SelectedValue = pomocniRed["Id_prevoz"];
            }
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();

                // Fetching data for Tip Putovanja
                string vratiTipPutovanja = @"SELECT Id_tip, Naziv FROM TipPutovanja";
                DataTable dtTipPutovanja = new DataTable();
                SqlDataAdapter daTipPutovanja = new SqlDataAdapter(vratiTipPutovanja, konekcija);
                daTipPutovanja.Fill(dtTipPutovanja);

                cbTipPutovanja.ItemsSource = dtTipPutovanja.DefaultView;

                // Fetching data for Vrsta Prevoza
                string vratiVrstaPrevoza = @"SELECT Id_prevoz, Vrsta FROM VrstaPrevoza";
                DataTable dtVrstaPrevoza = new DataTable();
                SqlDataAdapter daVrstaPrevoza = new SqlDataAdapter(vratiVrstaPrevoza, konekcija);
                daVrstaPrevoza.Fill(dtVrstaPrevoza);

                cbVrstaPrevoza.ItemsSource = dtVrstaPrevoza.DefaultView;
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

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();

                int idPutovanja = int.Parse(txtIdPutovanja.Text);
                DateTime datum = dpDatum.SelectedDate.Value;
                string destinacija = txtDestinacija.Text;
                decimal cena = decimal.Parse(txtCena.Text);
                string opis = txtOpis.Text;
                int idTipPutovanja = (int)cbTipPutovanja.SelectedValue;
                int idVrstaPrevoza = (int)cbVrstaPrevoza.SelectedValue;

                string sacuvajPutovanje;

                if (azuriraj)
                {
                    sacuvajPutovanje = "UPDATE Putovanje SET Datum = @Datum, Destinacija = @Destinacija, Cena = @Cena, Opis = @Opis, Id_tip = @Id_tip, Id_prevoz = @Id_prevoz WHERE Id_putovanja = @Id_putovanja";
                }
                else
                {
                    sacuvajPutovanje = "INSERT INTO Putovanje (Id_putovanja, Datum, Destinacija, Cena, Opis, Id_tip, Id_prevoz) VALUES (@Id_putovanja, @Datum, @Destinacija, @Cena, @Opis, @Id_tip, @Id_prevoz)";
                }

                SqlCommand cmd = new SqlCommand(sacuvajPutovanje, konekcija);
                cmd.Parameters.AddWithValue("@Id_putovanja", idPutovanja);
                cmd.Parameters.AddWithValue("@Datum", datum);
                cmd.Parameters.AddWithValue("@Destinacija", destinacija);
                cmd.Parameters.AddWithValue("@Cena", cena);
                cmd.Parameters.AddWithValue("@Opis", opis);
                cmd.Parameters.AddWithValue("@Id_tip", idTipPutovanja);
                cmd.Parameters.AddWithValue("@Id_prevoz", idVrstaPrevoza);
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
