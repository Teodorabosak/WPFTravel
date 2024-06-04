using System.Windows;

namespace WPFTravel.Forme
{
    /// <summary>
    /// Interaction logic for PutnoOsiguranje.xaml
    /// </summary>
    public partial class PutnoOsiguranje : Window
    {
        public PutnoOsiguranje()
        {
            InitializeComponent();
        }

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            // Implementacija logike za čuvanje podataka o putnom osiguranju
        }

        private void BtnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
