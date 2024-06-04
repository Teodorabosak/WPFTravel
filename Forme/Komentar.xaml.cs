using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFTravel.Forme
{
	/// <summary>
	/// Interaction logic for Komentar.xaml
	/// </summary>
	public partial class Komentar : Window
	{
		SqlConnection konekcija = new SqlConnection();
		Konekcija kon = new Konekcija();

		public Komentar()
		{
			InitializeComponent();
			txtIdKomentara.Focus();
			konekcija = kon.KreirajKonekciju();
			PopuniPadajuceListe();
		}
        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();

                string vratiKor = @"select id_korisnik, Ime + ' '+ prezime from korisnik";
                DataTable dtKorisnik = new DataTable();
                SqlDataAdapter daKorisnik = new SqlDataAdapter(vratiKor, konekcija);
                daKorisnik.Fill(dtKorisnik);

                cbKorisnik.ItemsSource = dtKorisnik.DefaultView;
                dtKorisnik.Dispose();
                daKorisnik.Dispose();

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
    }
}
