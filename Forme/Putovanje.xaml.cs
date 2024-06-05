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

            if (azuriraj && pomocniRed != null)
            {
                if (pomocniRed.DataView.Table.Columns.Contains("Datum"))
                    dpDatum.SelectedDate = (DateTime)pomocniRed["Datum"];
                if (pomocniRed.DataView.Table.Columns.Contains("Destinacija"))
                    txtDestinacija.Text = pomocniRed["Destinacija"].ToString();
                if (pomocniRed.DataView.Table.Columns.Contains("Cena"))
                    txtCena.Text = pomocniRed["Cena"].ToString();
                if (pomocniRed.DataView.Table.Columns.Contains("Opis"))
                    txtOpis.Text = pomocniRed["Opis"].ToString();
                if (pomocniRed.DataView.Table.Columns.Contains("Id_tip"))
                    cbTipPutovanja.SelectedValue = pomocniRed["Id_tip"];
                if (pomocniRed.DataView.Table.Columns.Contains("Id_prevoz"))
                    cbVrstaPrevoza.SelectedValue = pomocniRed["Id_prevoz"];

            }


        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();

                // Fetching data for Tip Putovanja
                string vratiTipPutovanja = @"SELECT Id_tip, Naziv FROM Tip_Putovanja";
                DataTable dtTipPutovanja = new DataTable();
                SqlDataAdapter daTipPutovanja = new SqlDataAdapter(vratiTipPutovanja, konekcija);
                daTipPutovanja.Fill(dtTipPutovanja);

                cbTipPutovanja.ItemsSource = dtTipPutovanja.DefaultView;
                dtTipPutovanja.Dispose();
                daTipPutovanja.Dispose();

                // Fetching data for Vrsta Prevoza
                string vratiVrstaPrevoza = @"SELECT Id_prevoz, Vrsta FROM Prevoz";
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
			try { 
            konekcija.Open();

            DateTime datum = dpDatum.SelectedDate.Value;
            string destinacija = txtDestinacija.Text;
            decimal cena = decimal.Parse(txtCena.Text);
            string opis = txtOpis.Text;
            int idTipPutovanja = Convert.ToInt32(((DataRowView)cbTipPutovanja.SelectedItem)["Id_tip"]);
            int idVrstaPrevoza = Convert.ToInt32(((DataRowView)cbVrstaPrevoza.SelectedItem)["Id_prevoz"]);

                string sacuvajPutovanje;

            if (azuriraj)
            {
                sacuvajPutovanje = "UPDATE Putovanje SET Datum = @Datum, Destinacija = @Destinacija, " +
                        "Cena = @Cena, Opis = @Opis, Id_tip = @Id_tip, Id_prevoz = @Id_prevoz " +
                        "WHERE Id_putovanja = @Id_putovanja";
            }
            else
            {
                sacuvajPutovanje = "INSERT INTO Putovanje (Datum, Destinacija, Cena, Opis, Id_tip, Id_prevoz) " +
                        "VALUES (@Datum, @Destinacija, @Cena, @Opis, @Id_tip, @Id_prevoz)";
            }

            SqlCommand cmd = new SqlCommand(sacuvajPutovanje, konekcija);

            if (azuriraj)
            {
                cmd.Parameters.AddWithValue("@Id_putovanja", pomocniRed["Id_putovanja"]);
            }

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
