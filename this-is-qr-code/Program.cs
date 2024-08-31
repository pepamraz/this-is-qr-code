using QRCoder;
using System.Diagnostics;
using System.Text.RegularExpressions;

string defaultData = "https://g.page/r/CfenZnuKjDCfEBM/review";
string data;

Console.WriteLine("Vítejte v aplikaci pro generování QR kódů!");
Console.WriteLine("Vyberte možnost:");
Console.WriteLine("1. Použít výchozí URL (https://g.page/r/CfenZnuKjDCfEBM/review)");
Console.WriteLine("2. Zadat vlastní URL");
Console.Write("Vaše volba (1 nebo 2): ");

string choice = Console.ReadLine();

if (choice == "1")
{
    data = defaultData;
}
else if (choice == "2")
{
    Console.Write("Zadejte URL: ");
    data = Console.ReadLine();
}
else
{
    Console.WriteLine("Neplatná volba. Používá se výchozí URL.");
    data = defaultData;
}

Console.Write("Zadejte název souboru (nebo nechte prázdné pro automatický název): ");
string fileNameInput = Console.ReadLine();

string fileName = string.IsNullOrWhiteSpace(fileNameInput) ? GenerateFileNameFromUrl(data) : fileNameInput;
fileName = $"{fileName}.svg";

QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
SvgQRCode qrCode = new SvgQRCode(qrCodeData);
string qrCodeImage = qrCode.GetGraphic(20, "#000000", "#00000000");

string metadata = $"<metadata>\n  <rdf:RDF>\n    <rdf:Description rdf:about=\"\">\n      <dc:title>QR Code Metadata</dc:title>\n      <dc:description>{data}</dc:description>\n    </rdf:Description>\n  </rdf:RDF>\n</metadata>";

qrCodeImage = qrCodeImage.Replace("<svg", $"<svg>\n{metadata}\n");

string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "QRCodes");
string fullPath = Path.Combine(folderPath, fileName);

if (!Directory.Exists(folderPath))
{
    Directory.CreateDirectory(folderPath);
}

File.WriteAllText(fullPath, qrCodeImage);

Console.WriteLine($"QR kód byl úspěšně vytvořen a uložen jako '{fullPath}'.");

Console.Write("Přejete si otevřít cílovou složku?\n1. Otevřít\n2. Ukončit program\nVolba: ");
string openFolderChoice = Console.ReadLine();

if (openFolderChoice?.ToLower() == "1")
{
    Process.Start("explorer.exe", folderPath);
}

static string GenerateFileNameFromUrl(string url)
{
    Uri uri = new Uri(url);
    string host = uri.Host;
    string fileName = Regex.Replace(host, @"[^a-zA-Z0-9\-]", "-");
    return fileName;
}
