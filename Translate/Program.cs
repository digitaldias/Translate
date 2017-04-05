using Translate.Domain.Contracts;
using Translate.IoC;
using StructureMap;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Translate
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

            var textToTranslate = string.Join(" ", args);

            Console.WriteLine(textToTranslate);
            Console.WriteLine(new string('-', 20));

            var translatedResult = XElement.Parse(_translationService.TranslateSingle("en", "no", textToTranslate));

            var decoded = System.Web.HttpUtility.UrlDecode(translatedResult.Value);
            Console.WriteLine(decoded);

            Console.WriteLine();


            Console.WriteLine("Done");
        }



        private static void ShowSyntax()
        {
            Console.WriteLine("Oversett <TEXT>\nVirkelig.. det ER ikke værre altså!");
        }
    }
}
