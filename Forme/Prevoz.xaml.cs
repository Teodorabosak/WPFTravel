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
using System.Windows.Shapes;

namespace WPFTravel.Forme
{
	/// <summary>
	/// Interaction logic for Prevoz.xaml
	/// </summary>
	public partial class Prevoz : Window
	{
		Konekcija kon = new Konekcija();
		SqlConnection konekcija = new SqlConnection();

		public Prevoz()
		{
			InitializeComponent();
			txtVrstaPrevoza.Focus();
			konekcija = kon.KreirajKonekciju();
		}
	}
}
