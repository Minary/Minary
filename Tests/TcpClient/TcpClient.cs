using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpClient
{
  public class TcpClient
  {

    public string SendRequest(string host, int port, string data)
    {
      System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient(host, port);
      byte[] realData = System.Text.Encoding.ASCII.GetBytes(data);
      System.Net.Sockets.NetworkStream stream = client.GetStream();

      // Send the message to the connected TcpServer. 
      stream.Write(realData, 0, realData.Length);


      // Receive server response data
      StringBuilder response = new StringBuilder();
      byte[] responseData = new byte[65554];
      int bytesReceived = -1;

      while ((bytesReceived = stream.Read(responseData, 0, responseData.Length)) > 0)
      {
        response.Append(System.Text.Encoding.ASCII.GetString(responseData, 0, bytesReceived));
      }

      return response.ToString();
    }
  }
}
