using Oversett.Domain.Contracts;
using Oversett.IoC;
using StructureMap;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Oversett
{
    class Program
    {
        private static Container _container = new Container(new RuntimeRegistry());
        private static ITranslationService _translationService;

        static Program()
        {
            _translationService = _container.GetInstance<ITranslationService>();
        }

        static void Main(string[] args)
        {
            if (args.Length == 0) {
                ShowSyntax();
                return;
            }

            Console.WriteLine("Passing empty parameters: " + _translationService.TranslateSingle("en", "no", ""));
            Console.WriteLine($"Testing translation of Illegal languages: " + _translationService.TranslateSingle("en", "mø", "hound"));

            string untranslated = "Det var den beste av tider, det var den værste av tider";
            var translated = XDocument.Parse(_translationService.TranslateSingle("no", "en", untranslated));

            Console.WriteLine("Translating");
            Console.WriteLine(untranslated + "\n==>\n" + translated.Root.Value);

            Console.WriteLine("Done");
        }



        private static void ShowSyntax()
        {
            Console.WriteLine("Oversett <TEXT>\nVirkelig.. det ER ikke værre altså!");
        }
    }
}
