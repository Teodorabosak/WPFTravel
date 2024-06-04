using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFTravel.Forme
{
	/// <summary>
	/// Interaction logic for Putovanje.xaml
	/// </summary>
	public partial class Putovanje : Window
	{

		Konekcija kon = new Konekcija();
		SqlConnection konekcija = new SqlConnection();

		public Putovanje()
		{
			InitializeComponent();
			txtDestinacija.Focus();
			konekcija = kon.KreirajKonekciju();

		}
	}
}
