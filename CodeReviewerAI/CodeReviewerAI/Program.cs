using CodeReviewerAI.Services;
using CodeReviewerAI.Services.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using System.Runtime.CompilerServices;

namespace CodeReviewerAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            
            try
            {
                string path = "C:\\Users\\Ritch\\source\\nullashrepos\\CodeReviewerAI\\CodeReviewerAI\\Config\\Base_Persona.txt";
                string path2 = "C:\\Users\\Ritch\\source\\nullashrepos\\CodeReviewerAI\\CodeReviewerAI\\Config\\Lang_CSharp.txt";
                string path3 = "C:\\Users\\Ritch\\source\\nullashrepos\\CodeReviewerAI\\CodeReviewerAI\\Config\\Output_Schema.txt";

                string readFile = await File.ReadAllTextAsync(path);
                string readFile2 = await File.ReadAllTextAsync(path2);
                string readFile3 = await File.ReadAllTextAsync(path3);

                string fullFile = readFile +"/n"+ readFile2 + "/n" + readFile3;

                
                Console.WriteLine(fullFile);
                Console.ReadLine();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

    }
}
