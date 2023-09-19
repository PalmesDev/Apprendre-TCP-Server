﻿using System.Net;
using System.Net.Sockets;
using System.Text;

IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
IPAddress ipAddress = ipHostInfo.AddressList[0].MapToIPv4();

IPEndPoint iPEndPoint = new IPEndPoint(ipAddress, 8080); 
// Definit une adresse IP serveur ainsi que son port de connexion.

using Socket client = new(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
// Definit un socket avec comme parametre l'ip, le type de socket (ici stream pour une communication send/recive), et le type de protocol (ici TCP) permettant la connexion au serveur distant.

await client.ConnectAsync(iPEndPoint);
// L'opérateur await permet d'attendre la connexion asyncrone(?) du client vers le serveur. Lorsque le client est connecter, await n'attend plus la connexion.

while (true)
{


    // Envoi du message au serveur.


    var message = "ConnexionHelloMessage";
    // Definition du message. 
    var messageBytes = Encoding.UTF8.GetBytes(message);
    // Encodage du message en UTF8 (Binaire).
    await client.SendAsync(messageBytes, SocketFlags.None);
    // Envoie le message et attend l'accusé de reception. 
    Console.WriteLine($"Le client envoie le message : \"{message}\"");
    // Affichage du message envoyer par le serveur dans la console.


    // Reception de l'accusé de reception.


    var buffer = new byte[1024];
    // Initialisation d'une memoire tampon de 1024 bytes afin de stocker l'accusé de reception.
    var received = await client.ReceiveAsync(buffer, SocketFlags.None);
    // Attend et recoit le message dans le buffer 
    var reponse = Encoding.UTF8.GetString(buffer, 0, received);
    // Décode le message dans le buffer 
    if(reponse == "Message reçu par le serveur <Fin de message>")
    {
        Console.WriteLine($"Le client a reçu l'accusé de reception : \"{reponse}\"");

    }

    client.Shutdown(SocketShutdown.Both);

}