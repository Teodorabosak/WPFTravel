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
	/// Interaction logic for Window1.xaml
	/// </summary>
	
	public partial class Korisnik : Window
	{
		SqlConnection konekcija = new SqlConnection();
		Konekcija kon = new Konekcija();

		private bool azuriraj;
		private DataRowView pomocniRed;


		public Korisnik()
		{
			InitializeComponent();
			txtIme.Focus();
			konekcija = kon.KreirajKonekciju();
		}

		public Korisnik( bool azuriraj, DataRowView pomocniRed)
		{

			InitializeComponent();
			txtBrTel.Focus();
			this.azuriraj = azuriraj;
			this.pomocniRed = pomocniRed;

			if (azuriraj)
			{
				txtBrTel.Text = pomocniRed["Br_Tel"].ToString();
				txtAdresa.Text = pomocniRed["Adresa"].ToString();
				txtIme.Text = pomocniRed["Ime"].ToString();
				txtPrezime.Text = pomocniRed["Prezime"].ToString();
				txtUsername.Text = pomocniRed["Username"].ToString();
				txtPassword.Password = pomocniRed["Password"].ToString();
				dpDatumRodj.SelectedDate = Convert.ToDateTime(pomocniRed["Datum_rodj"]);
			}

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
