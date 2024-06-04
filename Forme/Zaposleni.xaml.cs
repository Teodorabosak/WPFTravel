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
	/// Interaction logic for Zaposleni.xaml
	/// </summary>
	public partial class Zaposleni : Window
	{
		SqlConnection konekcija = new SqlConnection();
		Konekcija kon = new Konekcija();


		public Zaposleni()
		{
		InitializeComponent();
		txtIme.Focus();

		konekcija = kon.KreirajKonekciju();
		}

		public Zaposleni(bool azuriraj, DataRowView pomocniRed)
		{

			InitializeComponent();
			txtIdZaposlenog.Focus();
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
