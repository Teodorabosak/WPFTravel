using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WPFTravel.Forme
{
    /// <summary>
    /// Interaction logic for RezervacijaPutovanja.xaml
    /// </summary>
    public partial class Rezervacija : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();

        public Rezervacija()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            txtIdRez.Focus();
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
            }
            catch (SqlException)
            {
                MessageBox.Show("Padajuće liste nisu popunjene!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            // Implement your save logic here
            MessageBox.Show("Podaci su uspešno sačuvani.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void BtnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
