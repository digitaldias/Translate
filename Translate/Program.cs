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
            WriteAndResetStopwatch($"Supported Languages:\n{string.Join(", ", supportedLanguages.Values.Select(l => l.Code))}");            

            // Get text to translate from Project Debug arguments. 
            // The text is obtained from Samuel L. Ipsum generator
            var textToTranslate = string.Join(" ", args);
            WriteAndResetStopwatch(textToTranslate);

            // Translate from english to norwegian
            var english          = supportedLanguages["en"];
            var norwegian        = supportedLanguages["no"];
            var translatedResult = _translationService.TranslateSingle(from: english, to: norwegian, text: textToTranslate);            
            WriteAndResetStopwatch(translatedResult);

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
