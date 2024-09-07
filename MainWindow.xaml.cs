using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
namespace QR_URL_GENERATOR_SVG;



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
        string url = tb_urlbase + tb_url.Text;
        if (!string.IsNullOrEmpty(url) && valid_url(url))
        {
            generar.generate_QR(url);
            tb_url.Text = "";
        }
    }
    private string encriptar(string url)
    {
        tb_urlcodificada.Text = "";
        try
        {
            string keyString = "pmYyzUrq7U6oCZnHrqaEa2Yz1s8A3ydQ"; // Debe ser de 16, 24, o 32 caracteres


            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("La URL no puede estar vacía.");
                return "";
            }

            var key = Encoding.UTF8.GetBytes(keyString);
            string encryptedBase64 = "";

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.Mode = CipherMode.ECB; // Modo ECB no requiere IV
                aesAlg.Padding = PaddingMode.PKCS7;
                MemoryStream msEncrypt;
                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, null))
                using (msEncrypt = new System.IO.MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(url);
                }

                encryptedBase64 = Convert.ToBase64String(msEncrypt.ToArray());
            }
            return encryptedBase64;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ocurrió un error: " + ex.Message);
        }
        return "";
    }
    private void event_generar(object sender, RoutedEventArgs e)
    {
        generar();
    }



    private void event_encriptar(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(tb_vw.Text) || string.IsNullOrEmpty(tb_dl.Text))
            return;
        string json = "{_vw:\"" + encriptar(tb_vw.Text) + "\",_dl:\"" + encriptar(tb_dl.Text) + "\",_t:\"\"}";

        tb_vw.Text = "";
        tb_dl.Text = "";
        tb_urlcodificada.Text = json;

    }

    private void limpiar(object sender, RoutedEventArgs e)
    {
        tb_urlcodificada.Text = "";
    }
}