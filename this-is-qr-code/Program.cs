using QRCoder;
using System.Diagnostics;
using System.Text.RegularExpressions;

string asciiLogo = @" _   _     _       _                                  _      
| |_| |__ (_)___  (_)___    __ _ _ __    ___ ___   __| | ___ 
| __| '_ \| / __| | / __|  / _` | '__|  / __/ _ \ / _` |/ _ \
| |_| | | | \__ \ | \__ \ | (_| | |    | (_| (_) | (_| |  __/
 \__|_| |_|_|___/ |_|___/  \__, |_|     \___\___/ \__,_|\___|
                              |_|                            
";

Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine(asciiLogo);
Console.ForegroundColor = ConsoleColor.White;


string defaultData = "https://pepamraz.cz";
string data;

Console.WriteLine("Select language / Vyberte jazyk:");
Console.WriteLine("1. English");
Console.WriteLine("2. Čeština");
Console.Write("Your choice / Vaše volba (1 or 2): ");
string languageChoice = Console.ReadLine();

string welcomeMessage, optionMessage, defaultUrlMessage, enterUrlMessage, invalidChoiceMessage, enterFileNameMessage, qrCodeCreatedMessage, openFolderMessage;
if (languageChoice == "1")
{
    welcomeMessage = "Welcome to the QR Code Generator!";
    optionMessage = "Select an option:";
    defaultUrlMessage = $"1. Use default URL ({defaultData})";
    enterUrlMessage = "2. Enter your own URL";
    invalidChoiceMessage = "Invalid choice. Using the default URL.";
    enterFileNameMessage = "Enter the file name (or leave blank for automatic name): ";
    qrCodeCreatedMessage = "QR code was successfully created and saved as '{0}'.";
    openFolderMessage = "Would you like to open the destination folder?\n1. Open\n2. Exit program\nChoice: ";
}
else
{
    welcomeMessage = "Vítejte v aplikaci pro generování QR kódů!";
    optionMessage = "Vyberte možnost:";
    defaultUrlMessage = $"1. Použít výchozí URL ({defaultData})";
    enterUrlMessage = "2. Zadat vlastní URL";
    invalidChoiceMessage = "Neplatná volba. Používá se výchozí URL.";
    enterFileNameMessage = "Zadejte název souboru (nebo nechte prázdné pro automatický název): ";
    qrCodeCreatedMessage = "QR kód byl úspěšně vytvořen a uložen jako '{0}'.";
    openFolderMessage = "Přejete si otevřít cílovou složku?\n1. Otevřít\n2. Ukončit program\nVolba: ";
}

Console.WriteLine(welcomeMessage);
Console.WriteLine(optionMessage);
Console.WriteLine(defaultUrlMessage);
Console.WriteLine(enterUrlMessage);
Console.Write("Vaše volba (1 nebo 2): ");

string choice = Console.ReadLine();

if (choice == "1")
{
    data = defaultData;
}
else if (choice == "2")
{
    Console.Write(languageChoice == "1" ? "Enter URL: " : "Zadejte URL: ");
    data = Console.ReadLine();
}
else
{
    Console.WriteLine(invalidChoiceMessage);
    data = defaultData;
}

Console.Write(enterFileNameMessage);
string fileNameInput = Console.ReadLine();

string fileName = string.IsNullOrWhiteSpace(fileNameInput) ? GenerateFileNameFromUrl(data) : fileNameInput;
fileName = $"{fileName}.svg";

QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
SvgQRCode qrCode = new SvgQRCode(qrCodeData);
string qrCodeImage = qrCode.GetGraphic(20, "#000000", "#00000000");

string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "QRCodes");
string fullPath = Path.Combine(folderPath, fileName);

if (!Directory.Exists(folderPath))
{
    Directory.CreateDirectory(folderPath);
}

File.WriteAllText(fullPath, qrCodeImage);

Console.WriteLine(string.Format(qrCodeCreatedMessage, fullPath));

Console.Write(openFolderMessage);
string openFolderChoice = Console.ReadLine();

if (openFolderChoice == "1")
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
