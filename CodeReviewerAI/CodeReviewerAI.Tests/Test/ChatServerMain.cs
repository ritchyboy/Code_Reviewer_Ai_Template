using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace CodeReviewerAI.Test
{
    internal class ChatServerMain
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Chat Server is start ... ");


            TcpListener listener = new TcpListener(IPAddress.Any,5000);
            listener.Start();
            Console.WriteLine("Server is listening on port 5000");


            TcpClient client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("A user as appears");

            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytesReads = await stream.ReadAsync(buffer,0,buffer.Length);

                if (bytesReads == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Console.WriteLine($"Received: {message}");
            }
        }
    }
}
