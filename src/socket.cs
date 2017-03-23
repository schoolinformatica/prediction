using System;
using System.Net.Sockets;

namespace MyClient
 {
class Client_Socket{
    public void Publish(string message){
TcpClient socket = new TcpClient();
socket.Connect("localhost",9999);
NetworkStream network = socket.GetStream();
System.IO.StreamWriter streamWriter= new System.IO.StreamWriter(network); 
streamWriter.Write(message);
streamWriter.Flush();   
network.Close();
   }

}
}