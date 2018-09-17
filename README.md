[![BCH compliance](https://bettercodehub.com/edge/badge/digitaldias/Translate?branch=master)](https://bettercodehub.com/)

# Translate
This repository defines  the **TranslationService** and includes a sample console application to translate text between all supported languages of the [Microsoft Cognitive Translation API](https://www.microsoft.com/cognitive-services/en-us/translator-api)

**TranslationService** automatically requests a new authentication token every 9 minutes (Translate API provides a 10-minute token), and performs the Microsoft Translation API calls masking away the details of each REST invocation.<br />
<br />
The list of API methods can be found [here](http://docs.microsofttranslator.com/text-translate.html#!/default).

## TranslationService Details
### Methods and properties
```csharp
// Translate text from one language to another
string TranslateSingle(Language from, Language to, string text)

// Dictionary of supported languages, populated during startup
Dictionary<string,Language> SupportedLanguages { get; }

// Detect the language used in a chunk of text
Language DetectLanguage(string text)

// Break a text up into sentences and then return the character length for each sentence
IEnumerable<int> BreakSentences(Language language, string text)

// Add a known translation into the memory of the translation service
bool AddTranslation(Language from, Language to, string originalText, string translatedText);

// Provides a way to give the Translation API a better translation.  The userName is required 
// to track the originator of the translation
bool AddTranslation(Language from, Language to, string originalText, string translatedText, string userName);
```

### Entities
```csharp
   public class Language
   {
        public string Name { get; set; }
        public string Code { get; set; }
   }
```


## Quickstart / Running the code 
* Follow [this guide](http://docs.microsofttranslator.com/text-translate.html) to create a cognitive services client in Azure
* Create the file **Translate-Secrets.json** in your **My Documents** folder and paste in the Json structure at the bottom of this document. 


## Contributions welcome!
I'm happy to accept any contributions to make this wrapper as useful as possible! The more, the merrier!
A couple of notes: 
- The project was written in [Visual Studio 2017](https://www.visualstudio.com). In order to work on this code, you only need the Community Edition.
- Please write [useful commit messages](https://chris.beams.io/posts/git-commit/)
- I am a strong believer in Unit-Testing and TDD. Please ensure adequate test-coverage for your logic
    - [XUnit.Net](https://xunit.github.io/) is used as the test framwork
- Follow the code-style used. It is based on proven patterns and practices :)


## A note on secrets
You need to provide the Account Name and key to the framework, and of course, saving that into source-code is not considered great practice.<br />

The framework therefore exposes the following interface named **ISettingsReader**:

```csharp
namespace Translate.Domain.Contracts
{
    public interface ISettingsReader
    {
        string this[string name] { get; }
    }
}
```
The implementation of this interface can be found in the **Translate.Data.File** project, where I'm simply reading the settings from a Json file stored in **My Documents**. 


You can implement your own *SettingsReader* to obtain the AccountName and AccountKey, or you can opt-in on the already
provided implementation

In the project, the console application is named **Translate.exe** so there is a corresponding<br />
***Translate-Secrets.Json*** in my **My Documents** folder

## Attributions
The following external nuGet packages are part of the solution:

| Nuget Package                                                                                 | Purpose                           |
| -------------                                                                                 | -------                           |
| [StructureMap](http://www.nuget.org/packages/StructureMap/)                                   | IoC Container                     | 
| [StructureMap.AutoMocking.Moq](http://www.nuget.org/packages/structuremap.automocking.moq/)   | For baseclass in unit-testing     | 
| [Moq](http://www.nuget.org/packages/Moq/)                                                     | My favourite mocking framework    |
| [Should](http://www.nuget.org/packages/Should/)                                               | For writing fluent assertions     |
| [NewtonSoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)                             | For working with Json             |
| [xUnit](http://www.nuget.org/packages/xunit/)                                                 | Unit testing framework            |
| [RestSharp](http://www.nuget.org/packages/RestSharp/)                                         | for performing restful operations |
| [nBuilder](https://code.google.com/archive/p/nbuilder/)                                       | For rapid testdata creation       |


## Resources

Contents of Translate-Secrets.json:
```Json
{
    "Translator:AccountName" : "[Your Account Name]",
    "Translator:AccountKey"  : "[Your Account Key]"
}
```
Save this file in **My Documents** folder



