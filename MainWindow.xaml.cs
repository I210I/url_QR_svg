using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
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
        string url = tb_url.Text;
        if (!string.IsNullOrEmpty(url) && valid_url(url))
        {
            generar.generate_QR(url);
            tb_url.Text = "";
        }
    }
    private void encriptar()
    {
        try
        {
            string keyString = "mi_clavemi_clave"; // Debe ser de 16, 24, o 32 caracteres
            string url = tb_url.Text;

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("La URL no puede estar vacía.");
                return;
            }

            var key = Encoding.UTF8.GetBytes(keyString);
            string encryptedBase64;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.Mode = CipherMode.ECB; // Modo ECB no requiere IV
                aesAlg.Padding = PaddingMode.PKCS7;
                MemoryStream msEncrypt;
                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, null))
                using ( msEncrypt = new System.IO.MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(url);
                }

                encryptedBase64 = Convert.ToBase64String(msEncrypt.ToArray());
            }

            tb_urlcodificada.Text = encryptedBase64;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ocurrió un error: " + ex.Message);
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

    private void event_encriptar(object sender, RoutedEventArgs e)
    {
        encriptar();
    }
}