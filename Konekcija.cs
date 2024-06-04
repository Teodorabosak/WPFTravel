using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTravel
{
	public class Konekcija
	{
		//predstavlja konekciju sa SQL Server bazom
		public SqlConnection KreirajKonekciju()
		{
			SqlConnectionStringBuilder ccnSb = new SqlConnectionStringBuilder
			{
				DataSource = @"DESKTOP-COA51EH",//server
				InitialCatalog = "TravelWeb", //naziv baze
				IntegratedSecurity = true //lokalna baza

			};
			
			string con = ccnSb.ToString();
			SqlConnection konekcija = new SqlConnection(con);

			return konekcija;
		}
	}
}
