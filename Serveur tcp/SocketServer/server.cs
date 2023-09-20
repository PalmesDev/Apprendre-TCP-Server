using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Serveur starting...");

IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(Dns.GetHostName());
IPAddress ipAddress = ipHostInfo.AddressList[0];
IPEndPoint iPEndPoint = new IPEndPoint(ipAddress, 54265);

using Socket listener = new(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
// Défini un socket server avec comme paramètres l'ip, le type de socket ainsi que le protocol.

listener.Bind(iPEndPoint);
// Liaison du socket server à l'ip d'écoute. 
listener.Listen(100);
// Met le socket server en ecoute 
Console.WriteLine("Serveur démarré, en attente du client...");

var handler = await listener.AcceptAsync();
// Attend la connexion avec le client

while (true) // si la connexion est établie
{


    // Reception du message du client 


    var buffer = new byte[1024];
    // Initialisation d'une memoire tampon de 1024 bytes afin de stocker l'accusé de reception.
    var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
    // Encodage du message en UTF8 (Binaire).
    var reponse = Encoding.UTF8.GetString(buffer, 0, received);
    // Décodage du buffer en string

        Console.WriteLine($"Client : \"{ipAddress.MapToIPv4()}\" sur le port : \"{iPEndPoint.Port}\" connecté au serveur.");
        Console.WriteLine($"Le serveur a reçu le message : \"{reponse}\"");

        var adrMessage = "Hello Client !";
        // Definition de l'accusé de reception
        var echoBytes = Encoding.UTF8.GetBytes(adrMessage);
        // Encodage du message en UTF8 (Binaire).
        await handler.SendAsync(echoBytes, 0);
        // Envoi de l'accusé de reception au client

        Console.WriteLine($"Le serveur a envoyer l'accusé de reception au client : \"{adrMessage}\"");
        Console.ReadLine();
        break;
    
}