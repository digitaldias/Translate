using Oversett.Domain.Contracts;
using Oversett.IoC;
using StructureMap;
using System;

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
            Console.WriteLine($"Testing translation of 'hound' to Norwegian: " + translationService.TranslateSingle("hound", "en", "no"));
        }



        private static void ShowSyntax()
        {
            Console.WriteLine("Oversett <TEXT>\nVirkelig.. det ER ikke værre altså!");
        }
    }
}
