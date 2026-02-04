using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace CodeReviewerAI.Test
{
    internal class ChatClientMain
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Connecting to server.....");
            TcpClient client = new TcpClient();
            await client.ConnectAsync("127.0.0.1", 5000);

            Console.WriteLine("Connected! type your message: ");

            NetworkStream stream = client.GetStream();

            while (true)
            {
                string message = Console.ReadLine();

                byte[]data = Encoding.UTF8.GetBytes(message);

                await stream.WriteAsync(data,0,data.Length);
            }

        }
    }
}
