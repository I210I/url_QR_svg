using System.Windows;
using System.Windows.Input;
namespace QR_URL_GENERATOR_SVG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public bool valid_url(string url) => Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult);

        private void generar()
        {
            var generar = QR_SVG_Generator.Instance;
            string url = tb_url.Text;
            if (!string.IsNullOrEmpty(url) && valid_url(url))
            {
                generar.generate_QR(url);
                tb_url.Text = "";
            }
        }
        private void event_generar(object sender, RoutedEventArgs e)
        {
            generar();
        }

        private void event_generar(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                generar();
            }
        }
    }
}