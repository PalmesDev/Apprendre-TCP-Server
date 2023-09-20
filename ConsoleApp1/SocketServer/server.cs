using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Serveur starting...");

IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(Dns.GetHostName());
IPAddress ipAddress = ipHostInfo.AddressList[0];

IPEndPoint iPEndPoint = new IPEndPoint(ipAddress, 54265);

using Socket listener = new(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

listener.Bind(iPEndPoint);
listener.Listen(100);
Console.WriteLine("Serveur démarré, en attente du client...");
var handler = await listener.AcceptAsync();


while (true)
{


    // Reception du message du client 


    var buffer = new byte[1024];
    var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
    var reponse = Encoding.UTF8.GetString(buffer, 0, received);

        Console.WriteLine($"Client : \"{ipAddress.MapToIPv4()}\" sur le port : \"{iPEndPoint.Port}\" connecté au serveur.");
        Console.WriteLine($"Le serveur a reçu le message : \"{reponse}\"");

        var adrMessage = "Hello Client !";
        var echoBytes = Encoding.UTF8.GetBytes(adrMessage);
        await handler.SendAsync(echoBytes, 0);
        Console.WriteLine($"Le serveur a envoyer l'accusé de reception au client : \"{adrMessage}\"");
        Console.ReadLine();
        break;
    
}