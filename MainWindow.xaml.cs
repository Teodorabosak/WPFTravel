using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFTravel.Forme;

namespace WPFTravel
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		string ucitanaTabela;
		bool azuriraj;
		DataRowView pomocniRed; //cuva info o indeksu reda koji je korisnik ozn


		Konekcija kon = new Konekcija();
		SqlConnection konekcija = new SqlConnection();

		#region Select Upiti

		string korisniciSelect = @"select br_tel, adresa, ime, email, prezime, password, username, datum_rodj from Korisnik ";

		string komentarSelect = @"select id_kom, id_korisnik, ocena, komentar from komentar";

		string prevozSelect = @"select id_prevoz, vrsta from prevoz";

		string putnoOsiguranjeSelect = @"select id_osig, iznos_o, pocetak, kraj, id_rez, id_z from Putno_osiguranje ";

		string putovanjaSelect = @"select id_putovanja, datum, destinacija, cena, opis, id_tip, id_prevoz from putovanje";

		string rezervacijaSelect = @"SELECT Id_rez
								  ,Id_putovanja
								  ,Datum_r
								  ,Id_korisnik
								  ,Otkaz
								  ,Broj_aranzmana
								   FROM Rezervacija";

		string tipPutovanjaSelect = @"SELECT Id_tip
									,Naziv
									FROM Tip_putovanja
									";

		string zaposleniSelect = @"SELECT  Id_z,
								  Ime_z,
								  Prez_z,
								  Email,
								  Password,
								  Username
								  FROM Zaposleni
								  ";
		//string rezJoin = @""
		#endregion

		#region Select upiti sa uslovom

		string UpdateKorisnik = @"select * from korisnik where id_korisnik=";
		string UpdateKomentar = @"select * from komentar where id_kom=";
		string UpdatePrevoz = @"select * from prevoz where id_prevoz=";
		string UpdatePutnoOsiguranje = @"select * from putno_osiguranje where id_osig=";
		string UpdatePutovanje = @"select * from putovanje where id_putovanja=";
		string UpdateRezervacija = @"select * from rezervacija where id_rez=";
		string UpdateTipPutovanja = @"select * from tip_putovanja where id_tip=";
		string UpdateZaposleni = @"select * from zaposleni  where id_z=";

		#endregion

		#region Delete upiti
		string korisniciDelete = @" delete from korisnik where id_korisnik= ";
		string komentarDelete = @" delete  from komentar where id_kom= ";
		string prevozDelete = @" delete from prevoz where id_prevoz= ";
		string deletePO = @" delete from putno_osiguranje where id_osig=";
		string putovanjeDelete = @" delete  from putovanje where id_putovanja=";
		string deleteRez = @" delete from rezervacija where id_rez=";
		string deleteTP = @" delete from tip_putovanja where id_tip=";
		string deleteZap = @" delete from zaposleni  where id_z= ";
		#endregion

		public MainWindow()
		{
			InitializeComponent();
			konekcija = kon.KreirajKonekciju();
			UcitajPodatke(dataGridCentralni, putovanjaSelect);

		}
		public void UcitajPodatke(DataGrid grid, string selectUpit)
		{
			try
			{
				konekcija.Open();
				SqlDataAdapter dataAdapter = new SqlDataAdapter(selectUpit, konekcija);
				DataTable dt = new DataTable();
				dataAdapter.Fill(dt);



				if (grid != null)
				{
					grid.ItemsSource = dt.DefaultView;
				}
				ucitanaTabela = selectUpit;
				dt.Dispose();
				dataAdapter.Dispose();
			}
			catch (SqlException)
			{
				MessageBox.Show("Neuspešno učitani podaci!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				if (konekcija != null)
				{
					konekcija.Close();
				}
			}
		}


		private void BtnKorisnici(object sender, RoutedEventArgs e)
		{
			UcitajPodatke(dataGridCentralni, korisniciSelect);
		}

		private void BtnPutovanja(object sender, RoutedEventArgs e)
		{
			UcitajPodatke(dataGridCentralni, putovanjaSelect);
		}
		private void BtnTipoviPutovanja(object sender, RoutedEventArgs e)
		{
			UcitajPodatke(dataGridCentralni, tipPutovanjaSelect);
		}
		private void BtnPutnaOsiguranja(object sender, RoutedEventArgs e)
		{
			UcitajPodatke(dataGridCentralni, putnoOsiguranjeSelect);
		}
		private void BtnRezervacije(object sender, RoutedEventArgs e)
		{
			UcitajPodatke(dataGridCentralni, rezervacijaSelect);
		}
		private void BtnPrevoz(object sender, RoutedEventArgs e)
		{
			UcitajPodatke(dataGridCentralni, prevozSelect);
		}
		private void BtnZaposleni(object sender, RoutedEventArgs e)
		{
			UcitajPodatke(dataGridCentralni, zaposleniSelect);
		}
		private void BtnKomentar(object sender, RoutedEventArgs e)
		{
			UcitajPodatke(dataGridCentralni, komentarSelect);
		}

		// CREATE
		private void BtnDodaj(object sender, RoutedEventArgs e)
		{
			Window prozor;

			if (ucitanaTabela.Equals(korisniciSelect))
			{
				prozor = new Korisnik();
				prozor.ShowDialog();
				UcitajPodatke(dataGridCentralni, korisniciSelect);
			}
			else if (ucitanaTabela.Equals(rezervacijaSelect))
			{
				prozor = new Rezervacija();
				prozor.ShowDialog();
				UcitajPodatke(dataGridCentralni, rezervacijaSelect);
			}
			else if (ucitanaTabela.Equals(tipPutovanjaSelect))
			{
				prozor = new TipPutovanja();
				prozor.ShowDialog();
				UcitajPodatke(dataGridCentralni, tipPutovanjaSelect);

			}
			else if (ucitanaTabela.Equals(putovanjaSelect))
			{
				prozor = new Putovanje();
				prozor.ShowDialog();
				UcitajPodatke(dataGridCentralni, putovanjaSelect);

			}
			else if (ucitanaTabela.Equals(putnoOsiguranjeSelect))
			{
				prozor = new PutnoOsiguranje();
				prozor.ShowDialog();
				UcitajPodatke(dataGridCentralni, putnoOsiguranjeSelect);
			}
			else if (ucitanaTabela.Equals(komentarSelect))
			{
				prozor = new Komentar();
				prozor.ShowDialog();
				UcitajPodatke(dataGridCentralni, komentarSelect);
			}
			else if (ucitanaTabela.Equals(zaposleniSelect))
			{
				prozor = new Zaposleni();
				prozor.ShowDialog();
				UcitajPodatke(dataGridCentralni, zaposleniSelect);
			}
			else if (ucitanaTabela.Equals(prevozSelect))
			{
				prozor = new Prevoz();
				prozor.ShowDialog();
				UcitajPodatke(dataGridCentralni, prevozSelect);
			}

		}
		// UPDATE

		void PopuniFormu(DataGrid grid, string selectUslov, string idColumnName)
		{
			if (grid.SelectedItems.Count == 0)
			{
				MessageBox.Show("Niste selektovali red!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				konekcija.Open();
				DataRowView red = (DataRowView)grid.SelectedItems[0];
				int id = Convert.ToInt32(red[idColumnName]);

				using (SqlCommand komanda = new SqlCommand(selectUslov, konekcija))
				{
					komanda.Parameters.Add("@Id", SqlDbType.Int).Value = id;
					using (SqlDataReader citac = komanda.ExecuteReader())
					{
						if (citac.Read())
						{
							if (ucitanaTabela.Equals(korisniciSelect, StringComparison.Ordinal))
							{
								Korisnik prozorKor = new Korisnik(azuriraj, pomocniRed);
								prozorKor.txtBrTel.Text = citac["br_tel"].ToString();
								prozorKor.txtAdresa.Text = citac["adresa"].ToString();
								prozorKor.txtIme.Text = citac["ime"].ToString();
								prozorKor.txtPrezime.Text = citac["prezime"].ToString();
								prozorKor.txtUsername.Text = citac["username"].ToString();
								prozorKor.txtPassword.Password = citac["password"].ToString();
								prozorKor.dpDatumRodj.Text = citac["datum_rodj"].ToString();
							}
							else if (ucitanaTabela.Equals(rezervacijaSelect, StringComparison.Ordinal))
							{
								Rezervacija prozorRez = new Rezervacija(azuriraj, pomocniRed);
								prozorRez.txtIdRez.Text = citac["Id_rez"].ToString();
								prozorRez.cbPutovanje.SelectedValue = citac["Id_putovanja"];
								prozorRez.dpDatumR.SelectedDate = Convert.ToDateTime(citac["Datum_r"]);
								prozorRez.cbKorisnik.SelectedValue = citac["Id_korisnik"];
								prozorRez.chkOtkaz.IsChecked = (bool)citac["Otkaz"];
								prozorRez.txtBrojAranzmana.Text = citac["Broj_aranzmana"].ToString();
							}
							else if (ucitanaTabela.Equals(prevozSelect, StringComparison.Ordinal))
							{
								Prevoz prozorPR = new Prevoz(azuriraj, pomocniRed);
								// Popunite potrebne vrednosti za prevoz
							}
							else if (ucitanaTabela.Equals(tipPutovanjaSelect, StringComparison.Ordinal))
							{
								TipPutovanja prozorTP = new TipPutovanja(azuriraj, pomocniRed);
								// Popunite potrebne vrednosti za tip putovanja
							}
							else if (ucitanaTabela.Equals(zaposleniSelect, StringComparison.Ordinal))
							{
								Zaposleni prozorZap = new Zaposleni(azuriraj, pomocniRed);
								// Popunite potrebne vrednosti za zaposleni
							}
							else if (ucitanaTabela.Equals(komentarSelect, StringComparison.Ordinal))
							{
								Komentar prozorKom = new Komentar(azuriraj, pomocniRed);
								// Popunite potrebne vrednosti za komentar
							}
							else if (ucitanaTabela.Equals(putnoOsiguranjeSelect, StringComparison.Ordinal))
							{
								PutnoOsiguranje prozorPO = new PutnoOsiguranje(azuriraj, pomocniRed);
								// Popunite potrebne vrednosti za putno osiguranje
							}
							else if (ucitanaTabela.Equals(putovanjaSelect, StringComparison.Ordinal))
							{
								Putovanje prozorPut = new Putovanje(azuriraj, pomocniRed);
								// Popunite potrebne vrednosti za putovanje
							}
						}
					}
				}
			}
			catch (ArgumentOutOfRangeException)
			{
				MessageBox.Show("Niste selektovali red!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (SqlException ex)
			{
				MessageBox.Show($"SQL greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				if (konekcija != null)
				{
					konekcija.Close();
				}
			}
		}




		// DELETE
		void ObrisiZapis(DataGrid grid, string deleteUpit, string idColumnName)
		{
			try
			{
				konekcija.Open();

				DataRowView red = (DataRowView)grid.SelectedItems[0];
				MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni?",
				"Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (rezultat == MessageBoxResult.Yes)
				{
					SqlCommand komanda = new SqlCommand(deleteUpit, konekcija);
					komanda.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(red[idColumnName]);
					komanda.ExecuteNonQuery();
					komanda.Dispose();
				}

			}
			catch (ArgumentOutOfRangeException)
			{
				MessageBox.Show("Niste selektovali red",
				"Obavestenje",
				MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (SqlException)
			{
				MessageBox.Show("Postoje povezani podaci u drugim tabelama",
				"Obavestenje!",
				MessageBoxButton.OK, MessageBoxImage.Error);

			}
			finally
			{
				if (konekcija != null)
				{
					konekcija.Close();
				}
			}
		}
		private void BtnObrisi(object sender, RoutedEventArgs e)
		{
			if (ucitanaTabela.Equals(korisniciSelect))
			{
				ObrisiZapis(dataGridCentralni, korisniciDelete, "Id_korisnik");
				UcitajPodatke(dataGridCentralni, korisniciSelect);

			}
			else if (ucitanaTabela.Equals(komentarSelect))
			{
				ObrisiZapis(dataGridCentralni, komentarDelete, "id_kom");
				UcitajPodatke(dataGridCentralni, komentarSelect);

			}
			else if (ucitanaTabela.Equals(prevozSelect))
			{
				ObrisiZapis(dataGridCentralni, prevozDelete, "id_prevoz");
				UcitajPodatke(dataGridCentralni, prevozSelect);

			}
			else if (ucitanaTabela.Equals(putovanjaSelect))
			{
				ObrisiZapis(dataGridCentralni, putovanjeDelete, "id_putovanja");
				UcitajPodatke(dataGridCentralni, putovanjaSelect);

			}
			else if (ucitanaTabela.Equals(rezervacijaSelect))
			{
				ObrisiZapis(dataGridCentralni, deleteRez, "Id_rez");
				UcitajPodatke(dataGridCentralni, rezervacijaSelect);

			}
			else if (ucitanaTabela.Equals(zaposleniSelect))
			{
				ObrisiZapis(dataGridCentralni, deleteZap, "Id_z");
				UcitajPodatke(dataGridCentralni, zaposleniSelect);

			}
			else if (ucitanaTabela.Equals(tipPutovanjaSelect))
			{
				ObrisiZapis(dataGridCentralni, deleteTP, "Id_tip");
				UcitajPodatke(dataGridCentralni, tipPutovanjaSelect);

			}
			else if (ucitanaTabela.Equals(putnoOsiguranjeSelect))
			{
				ObrisiZapis(dataGridCentralni, deletePO, "id_osig");
				UcitajPodatke(dataGridCentralni, putnoOsiguranjeSelect);

			}
		}

		private void btnIzmeni_Click(object sender, RoutedEventArgs e)
		{
			if (ucitanaTabela.Equals(korisniciSelect))
			{
				PopuniFormu(dataGridCentralni, UpdateKorisnik, "id_korisnik");
				UcitajPodatke(dataGridCentralni, korisniciSelect);

			}
			else if (ucitanaTabela.Equals(rezervacijaSelect))
			{
				PopuniFormu(dataGridCentralni, UpdateRezervacija, "id_rez");
				UcitajPodatke(dataGridCentralni, rezervacijaSelect);
			}
			else if (ucitanaTabela.Equals(putovanjaSelect))
			{
				PopuniFormu(dataGridCentralni, UpdatePutovanje, "id_putovanje");
				UcitajPodatke(dataGridCentralni, putovanjaSelect);
			}
			else if (ucitanaTabela.Equals(komentarSelect))
			{
				PopuniFormu(dataGridCentralni, UpdateKomentar, "id_kom");
				UcitajPodatke(dataGridCentralni, komentarSelect);
			}
			else if (ucitanaTabela.Equals(zaposleniSelect))
			{
				PopuniFormu(dataGridCentralni, UpdateZaposleni, "id_z");
				UcitajPodatke(dataGridCentralni, zaposleniSelect);
			}
			else if (ucitanaTabela.Equals(tipPutovanjaSelect))
			{
				PopuniFormu(dataGridCentralni, UpdateTipPutovanja, "id_tip");
				UcitajPodatke(dataGridCentralni, tipPutovanjaSelect);
			}
			else if (ucitanaTabela.Equals(prevozSelect))
			{
				PopuniFormu(dataGridCentralni, UpdatePrevoz, "id_prevoz");
				UcitajPodatke(dataGridCentralni, prevozSelect);
			}
			else if (ucitanaTabela.Equals(putnoOsiguranjeSelect))
			{
				PopuniFormu(dataGridCentralni, UpdatePutnoOsiguranje,"id_osig");
				UcitajPodatke(dataGridCentralni, putnoOsiguranjeSelect);

			}
		}

		
	}

}