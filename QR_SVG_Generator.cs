using QRCoder;
using System.IO;
namespace QR_URL_GENERATOR_SVG;
internal class QR_SVG_Generator
{
    private static QR_SVG_Generator _instance;

    public static QR_SVG_Generator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new QR_SVG_Generator();
            }
            return _instance;
        }
    }
    public string generate_QR(string url)
    {
        string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileNameWithoutExtension = "QRCode";
        string fileExtension = ".svg";
        string fullPath = string.Empty;

        int fileNumber = 1;

        // Encuentra el siguiente nombre de archivo disponible
        do
        {
            fullPath = Path.Combine(directoryPath, $"{fileNameWithoutExtension}_{fileNumber}{fileExtension}");
            fileNumber++;
        }
        while (File.Exists(fullPath));

        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            SvgQRCode svgQRCode = new (qrCodeData);
            string qrCodeAsSvg = svgQRCode.GetGraphic(20);
            // Guardar el SVG en un archivo
            File.WriteAllText(fullPath, qrCodeAsSvg);
        }

        return fullPath;
    }
}
