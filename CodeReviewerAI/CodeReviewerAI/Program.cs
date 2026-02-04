using CodeReviewerAI.Services;
using CodeReviewerAI.Services.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Octokit;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

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
                string codePath = "C:\\Users\\Ritch\\source\\nullashrepos\\CodeReviewerAI\\CodeReviewerAI.Tests\\Test\\ChatClientMain.cs";
                string codePath2 = "C:\\Users\\Ritch\\source\\nullashrepos\\CodeReviewerAI\\CodeReviewerAI.Tests\\Test\\ChatServerMain.cs";


                string readFile = await File.ReadAllTextAsync(path);
                string readFile2 = await File.ReadAllTextAsync(path2);
                string readFile3 = await File.ReadAllTextAsync(path3);
                string readCode = await File.ReadAllTextAsync(codePath);
                string readCode2 = await File.ReadAllTextAsync(codePath2);

                string fullFile = readFile +"/n"+ readFile2 + "/n" + readFile3 + "\n" + readCode;

                
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
