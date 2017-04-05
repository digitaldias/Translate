using StructureMap;
using System;
using System.Diagnostics;
using System.Linq;
using Translate.Domain.Contracts;
using Translate.IoC;

namespace Translate
{
    class Program
    {
        private static Container _container = new Container(new RuntimeRegistry());
        private static ITranslationService _translationService;
        private static Stopwatch _stopwatch;

        static Program()
        {
            _stopwatch = Stopwatch.StartNew();
            _translationService = _container.GetInstance<ITranslationService>();
            WriteAndResetStopwatch("Service Initialized and Authorization token acquired");
        }


        static void Main(string[] args)
        {
            if (args.Length == 0) {
                ShowSyntax();
                return;
            }

            // Output all supported languages
            var supportedLanguages = _translationService.SupportedLanguages;
            Console.WriteLine($"Supported Languages: {string.Join(", ", supportedLanguages.Select(l => l.Code))}");
            WriteAndResetStopwatch("Supported Languages retrieved");


            // Get text to translate from Project Debug arguments. 
            // The text is obtained from Samuel L. Ipsum generator
            var textToTranslate = string.Join(" ", args);
            Console.WriteLine(textToTranslate);


            // Translate from english to norwegian
            var translatedResult = _translationService.TranslateSingle("en", "no", textToTranslate);
            Console.WriteLine(translatedResult);
            WriteAndResetStopwatch("Text translation complete");

            Console.WriteLine("Done");
        }


        private static void WriteAndResetStopwatch(string message)
        {
            Console.WriteLine($"[{_stopwatch.ElapsedMilliseconds} ms] {message}");

            // Add some air
            Console.WriteLine(new string('-', 20));

            _stopwatch.Restart();
        }


        private static void ShowSyntax()
        {
            Console.WriteLine("Translate <text>\nIt's THAT easy!");
        }
    }
}
