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



		public TipPutovanja(bool azuriraj, DataRowView pomocniRed)
		{

			InitializeComponent();
			txtIdTip.Focus();
			//this.azuriraj = azuriraj;
			//this.pomocniRed = pomocniRed;

			konekcija = kon.KreirajKonekciju();

		}

		private void BtnSacuvaj(object sender, RoutedEventArgs e) //sacuvaj
		{

		}

		private void BtnOtkazi(object sender, RoutedEventArgs e) //otkazi
		{
			this.Close();
		}
	}
}
