using System.Net;
using System.Net.Sockets;
using System.Text;

IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
IPAddress ipAddress = ipHostInfo.AddressList[0].MapToIPv4();

IPEndPoint iPEndPoint = new IPEndPoint(ipAddress, 8080);

using Socket listener = new(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

listener.Bind(iPEndPoint);
listener.Listen(100);

var handler = await listener.AcceptAsync();
while (true)
{


    // Reception du message du client 


    var buffer = new byte[1024];
    var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
    var reponse = Encoding.UTF8.GetString(buffer, 0, received);
    var fdm = "<Fin de message>";

    if (reponse.IndexOf(fdm) > -1)
    {
        Console.WriteLine($"Le serveur a reçu le message : \"{reponse.Replace(fdm, "")}\"");

        var adrMessage = "Message reçu par le serveur <Fin de message>";
        var echoBytes = Encoding.UTF8.GetBytes(adrMessage);
        await handler.SendAsync(echoBytes, 0);
        Console.WriteLine($"Le serveur a envoyer l'accusé de reception au client : \"{adrMessage}\"");

        break;


    }
}