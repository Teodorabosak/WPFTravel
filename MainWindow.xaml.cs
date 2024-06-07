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
		//upiti, select sve iz tabele

		string korisniciSelect = @"select id_korisnik,  ime as Ime, prezime as Prezime, br_tel as Telefon, adresa as Adresa,  email as Email,  password as Password, username as Username, datum_rodj as 'Datum rodjenja' from Korisnik ";

		string komentarSelect = @"SELECT id_kom, k.Username, ko.Ocena, ko.Komentar
								FROM Komentar ko
								INNER JOIN Korisnik k ON ko.Id_korisnik = k.Id_korisnik;
								";

		string prevozSelect = @"select id_prevoz,  vrsta as Vrsta from prevoz";

		string putnoOsiguranjeSelect = @"SELECT id_osig,
                                po.iznos_o as Iznos,
                                po.pocetak as Pocetak,
                                po.kraj as Kraj,
                                po.id_rez as [Broj rezervacije],
                                z.Username as Username
                            FROM Putno_osiguranje po
                            JOIN Zaposleni z ON po.id_z = z.Id_z;";


		string putovanjaSelect = @"SELECT id_putovanja, p.Datum as Polazak, p.Destinacija, p.Cena, p.Opis, tp.Naziv AS Kategorija, pr.Vrsta AS Prevoz
								FROM Putovanje p
								INNER JOIN Tip_putovanja tp ON p.Id_tip = tp.Id_tip
								INNER JOIN Prevoz pr ON p.Id_prevoz = pr.Id_prevoz
								";

		string rezervacijaSelect = @"SELECT id_rez ,
									p.Destinacija as Destinacija,
									r.Datum_r as 'Datum rodjenja',
									k.Username as Username,
									r.Otkaz as Rezervacija,
									r.Broj_aranzmana as 'Broj osoba'
								FROM Rezervacija r
								JOIN Putovanje p ON r.Id_putovanja = p.Id_putovanja
								JOIN Korisnik k ON r.Id_korisnik = k.Id_korisnik;
";

		string tipPutovanjaSelect = @"SELECT id_tip ,
									Naziv
									FROM Tip_putovanja
									";

		string zaposleniSelect = @"SELECT  id_z ,
								  Ime_z as Ime,
								  Prez_z as Prezime,
								  Email,
								  Password,
								  Username
								  FROM Zaposleni
								  ";
		
		#endregion

		#region Select upiti sa uslovom 
		//update konkretan red, menjamo

		string UpdateKorisnik = @"select * from korisnik where id_korisnik= @Id";
		string UpdateKomentar = @"select * from komentar where id_kom=  @Id";
		string UpdatePrevoz = @"select * from prevoz where id_prevoz= @Id";
		string UpdatePutnoOsiguranje = @"select * from putno_osiguranje where id_osig= @Id";
		string UpdatePutovanje = @"select * from putovanje where id_putovanja= @Id";
		string UpdateRezervacija = @"select * from rezervacija where id_rez= @Id";
		string UpdateTipPutovanja = @"select * from tip_putovanja where id_tip= @Id";
		string UpdateZaposleni = @"select * from zaposleni  where id_z= @Id";

		#endregion

		#region Delete upiti
		//brisanje reda
		string korisniciDelete = @" delete from korisnik where id_korisnik= @Id ";
		string komentarDelete = @" delete  from komentar where id_kom=  @Id ";
		string prevozDelete = @" delete from prevoz where id_prevoz=  @Id";
		string deletePO = @" delete from putno_osiguranje where id_osig= @Id";
		string putovanjeDelete = @" delete  from putovanje where id_putovanja= @Id";
		string deleteRez = @" delete from rezervacija where id_rez= @Id";
		string deleteTP = @" delete from tip_putovanja where id_tip= @Id";
		string deleteZap = @" delete from zaposleni  where id_z= @Id ";
		#endregion

		public MainWindow() //glavni prozor, konstruktor
		{
			InitializeComponent();
			konekcija = kon.KreirajKonekciju();
			UcitajPodatke(dataGridCentralni, putovanjaSelect); //metoda koju pozivamo da bi nam se popunili podaci u centralnom delu tabee, default

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
		//dugmici sta vracaju

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
			Window prozor; //promenljliva prozor tipa prozor

			if (ucitanaTabela.Equals(korisniciSelect)) //ako je tabela koja je ucitana korisnik 
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

		void PopuniFormu(DataGrid grid, string selectUslov)
		{
			if (grid.SelectedItems.Count == 0) //br selektovanih stavki jednak nuli
			{
				MessageBox.Show("Niste selektovali red!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				//otvara konekcija
				konekcija.Open();
				azuriraj = true; //u toku je azuriranje
				DataRowView red = (DataRowView)grid.SelectedItems[0]; //uzima taj red
				pomocniRed = red; //cuva taj red u promenljicoj



				// Dobiti naziv kolone koja sadrži ID, provera 
				string idColumnName = GetIdColumnName();
				


				SqlCommand komanda = new SqlCommand(selectUslov, konekcija);
				komanda.Parameters.AddWithValue("@id", red[idColumnName]); // Dodati vrednost ID parametra u SQL komandu
				SqlDataReader citac = komanda.ExecuteReader(); // Izvršiti komandu i dobiti SqlDataReader za čitanje podataka






				while (citac.Read())
					{
						if (ucitanaTabela.Equals(korisniciSelect, StringComparison.Ordinal))
						// Kreirati novi prozor za korisnika i popuniti podatke iz citaca
					{
							Korisnik prozorKor = new Korisnik(azuriraj, pomocniRed);
							prozorKor.txtBrTel.Text = citac["br_tel"].ToString();
							prozorKor.txtAdresa.Text = citac["adresa"].ToString();
							prozorKor.txtIme.Text = citac["ime"].ToString();
							prozorKor.txtPrezime.Text = citac["prezime"].ToString();
							prozorKor.txtUsername.Text = citac["username"].ToString();
							prozorKor.txtPassword.Password = citac["password"].ToString();
							prozorKor.dpDatumRodj.Text = citac["datum_rodj"].ToString();

						prozorKor.ShowDialog();
					}
						else if (ucitanaTabela.Equals(rezervacijaSelect, StringComparison.Ordinal))
						{
							Rezervacija prozorRez = new Rezervacija(azuriraj, pomocniRed);
							//prozorRez.txtIdRez.Text = citac["Id_rez"].ToString();
							prozorRez.cbPutovanje.SelectedValue = citac["Id_putovanja"];
							prozorRez.dpDatumR.SelectedDate = Convert.ToDateTime(citac["Datum_r"]);
							prozorRez.cbKorisnik.SelectedValue = citac["Id_korisnik"];

						if(citac["otkaz"] != DBNull.Value)
{
							// Proveriti da li je vrednost polja 'otkaz' jednaka "Otkazana"
							if (citac["otkaz"].ToString() == "Otkazana")
							{
								// Postaviti čekboks na true ako je 'otkaz' jednaka "Otkazana"
								prozorRez.chkOtkaz.IsChecked = true;
							}
							else
							{
								// Ako nije "Otkazana", postaviti čekboks na false
								prozorRez.chkOtkaz.IsChecked = false;
							}
						}
						else
						{
							// Ako polje 'otkaz' nema vrednost, postaviti čekboks na false, ovo se nece desiti a za svaki sluc
							prozorRez.chkOtkaz.IsChecked = false;
						}


						prozorRez.txtBrojAranzmana.Text = citac["Broj_aranzmana"].ToString();

						prozorRez.ShowDialog();
					}
						else if (ucitanaTabela.Equals(prevozSelect, StringComparison.Ordinal))
						{
							Prevoz prozorPR = new Prevoz(azuriraj, pomocniRed);
							prozorPR.txtVrstaPrevoza.Text = citac["vrsta"].ToString();

						prozorPR.ShowDialog();
						}
						else if (ucitanaTabela.Equals(tipPutovanjaSelect, StringComparison.Ordinal))
						{
							TipPutovanja prozorTP = new TipPutovanja(azuriraj, pomocniRed);
						prozorTP.txtNaziv.Text = citac["naziv"].ToString();
						
						prozorTP.ShowDialog();

						}
						else if (ucitanaTabela.Equals(zaposleniSelect, StringComparison.Ordinal))
						{
							Zaposleni prozorZap = new Zaposleni(azuriraj, pomocniRed);

							prozorZap.txtEmail.Text = citac["Email"].ToString(); //naziv iz sql
							prozorZap.txtIme.Text = citac["ime_z"].ToString();
							prozorZap.txtPrezime.Text = citac["prez_z"].ToString();
							prozorZap.txtUsername.Text = citac["username"].ToString();
							prozorZap.txtPassword.Password = citac["password"].ToString();


						prozorZap.ShowDialog();

					}
						else if (ucitanaTabela.Equals(komentarSelect, StringComparison.Ordinal))
						{
							Komentar prozorKom = new Komentar(azuriraj, pomocniRed);

						prozorKom.txtKomentar.Text = citac["komentar"].ToString();
						prozorKom.txtOcena.Text = citac["ocena"].ToString();
						prozorKom.cbKorisnik.SelectedValue = citac["Id_korisnik"].ToString();


						prozorKom.ShowDialog();
					}
						else if (ucitanaTabela.Equals(putnoOsiguranjeSelect, StringComparison.Ordinal))
						{
							PutnoOsiguranje prozorPO = new PutnoOsiguranje(azuriraj, pomocniRed);

						prozorPO.txtIznosO.Text = citac["Iznos_O"].ToString();
						prozorPO.dpPocetak.SelectedDate = Convert.ToDateTime(citac["Pocetak"]);
						prozorPO.dpKraj.SelectedDate = Convert.ToDateTime(citac["Kraj"]);
						prozorPO.txtIdRez.SelectedValue = citac["Id_rez"].ToString();
						prozorPO.txtIdZ.SelectedValue = citac["Id_z"].ToString();

						prozorPO.ShowDialog();
					}
				
						else if (ucitanaTabela.Equals(putovanjaSelect, StringComparison.Ordinal))
						{
							Putovanje prozorPut = new Putovanje(azuriraj, pomocniRed);

						prozorPut.dpDatum.SelectedDate = Convert.ToDateTime(citac["Datum"]);
						prozorPut.txtDestinacija.Text= citac["Destinacija"].ToString();
						prozorPut.txtOpis.Text = citac["Opis"].ToString();
						prozorPut.txtCena.Text = citac["Cena"].ToString();
						prozorPut.cbTipPutovanja.SelectedValue = citac["Id_tip"].ToString();
						prozorPut.cbVrstaPrevoza.SelectedValue = citac["Id_prevoz"].ToString();

						prozorPut.ShowDialog();
						
						}
				}
					

				citac.Close();
			}
			catch (ArgumentOutOfRangeException)
			{
				MessageBox.Show("Niste selektovali red!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			
			finally
			{
				if (konekcija != null)
				{
					konekcija.Close();
				}

				azuriraj = false;
			}

		}



		// DELETE
		void ObrisiZapis(DataGrid grid, string deleteUpit, string idColumnName) //brisanje zapisa iz baze podataka na osnovu selektovanog reda u DataGrid kontroli. 
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
					//Dodajemo parametar @id u SQL komandu i dodeljujemo mu vrednost
					//identifikatora izabranog reda. idColumnName je naziv kolone koja sadrži identifikator.
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

		string GetIdColumnName()
		{
			if (ucitanaTabela.Equals(korisniciSelect, StringComparison.Ordinal)) return "id_korisnik"; // trenutno učitana tabela korisniciSelect, metod će vratiti naziv kolone "id_korisnik".
			if (ucitanaTabela.Equals(rezervacijaSelect, StringComparison.Ordinal)) return "Id_rez";
			if (ucitanaTabela.Equals(prevozSelect, StringComparison.Ordinal)) return "id_prevoz";
			if (ucitanaTabela.Equals(tipPutovanjaSelect, StringComparison.Ordinal)) return "id_tip";
			if (ucitanaTabela.Equals(zaposleniSelect, StringComparison.Ordinal)) return "id_z";
			if (ucitanaTabela.Equals(komentarSelect, StringComparison.Ordinal)) return "id_kom";
			if (ucitanaTabela.Equals(putnoOsiguranjeSelect, StringComparison.Ordinal)) return "id_osig";
			if (ucitanaTabela.Equals(putovanjaSelect, StringComparison.Ordinal)) return "id_putovanja";
			return "ID"; 
		}

		private void btnIzmeni_Click(object sender, RoutedEventArgs e)
		{
			if (ucitanaTabela.Equals(korisniciSelect))
			{
				PopuniFormu(dataGridCentralni, UpdateKorisnik);
				UcitajPodatke(dataGridCentralni, korisniciSelect);

			}
			else if (ucitanaTabela.Equals(rezervacijaSelect))
			{
				PopuniFormu(dataGridCentralni, UpdateRezervacija);
				UcitajPodatke(dataGridCentralni, rezervacijaSelect);
			}
			else if (ucitanaTabela.Equals(putovanjaSelect))
			{
				PopuniFormu(dataGridCentralni, UpdatePutovanje);
				UcitajPodatke(dataGridCentralni, putovanjaSelect);
			}
			else if (ucitanaTabela.Equals(komentarSelect))
			{
				PopuniFormu(dataGridCentralni, UpdateKomentar);
				UcitajPodatke(dataGridCentralni, komentarSelect);
			}
			else if (ucitanaTabela.Equals(zaposleniSelect))
			{
				PopuniFormu(dataGridCentralni, UpdateZaposleni);
				UcitajPodatke(dataGridCentralni, zaposleniSelect);
			}
			else if (ucitanaTabela.Equals(tipPutovanjaSelect))
			{
				PopuniFormu(dataGridCentralni, UpdateTipPutovanja);
				UcitajPodatke(dataGridCentralni, tipPutovanjaSelect);
			}
			else if (ucitanaTabela.Equals(prevozSelect))
			{
				PopuniFormu(dataGridCentralni, UpdatePrevoz);
				UcitajPodatke(dataGridCentralni, prevozSelect);
			}
			else if (ucitanaTabela.Equals(putnoOsiguranjeSelect))
			{
				PopuniFormu(dataGridCentralni, UpdatePutnoOsiguranje);
				UcitajPodatke(dataGridCentralni, putnoOsiguranjeSelect);

			}
		}

		
	}

}