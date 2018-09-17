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
            var textToTranslate = string.Join(" ", args);


            // Output all supported languages
            var supportedLanguages = _translationService.SupportedLanguages;
            WriteAndResetStopwatch($"Supported Languages:\n{string.Join(", ", supportedLanguages.Values.Select(l => l.Code))}");


            // Detect a different language (Norwegian)
            var norwegianAnthem = "Ja vi elsker dette landet som det stiger frem. Furet værbitt over vannet med de tusen hjem. Elsker, elsker det og tenker på vår far og mor.";
            var detectedNorwegian = _translationService.DetectLanguage(norwegianAnthem);
            WriteAndResetStopwatch($"Detected language to be '{detectedNorwegian.Code}'");


            // Break up the sentences and return their character counts
            var result = _translationService.BreakSentences(detectedNorwegian, norwegianAnthem);
            WriteAndResetStopwatch("Sentences broken:");
            int index = 1;
            foreach(var sentence in result)
            {
                Console.WriteLine($"Sentence {index} contains {sentence} characters");
            }


            // Detect the language of the text to translate from
            var detectedLanguage = _translationService.DetectLanguage(textToTranslate);
            WriteAndResetStopwatch($"Detected language to be '{detectedLanguage.Code}'");


            // Get text to translate from Project Debug arguments. 
            // The text is obtained from Samuel L. Ipsum generator
            WriteAndResetStopwatch(textToTranslate);


            // Translate into norwegian            
            var norwegian        = supportedLanguages["no"];
            var translatedResult = _translationService.TranslateSingle(from: detectedLanguage, to: norwegian, text: textToTranslate);            
            WriteAndResetStopwatch(translatedResult);


            // End of program
            Console.WriteLine("Done. Press any key to finish");
            Console.ReadKey();
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
