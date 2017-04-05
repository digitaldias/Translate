# Translate
A wrapper library to use the [Microsoft Cognitive Translation](https://www.microsoft.com/cognitive-services/en-us/translator-api) API. It automatically requests an authentication token every 9 minutes, and performs the API calls masking away the details of each REST invocation.

## Quickstart / Running the code 
* Follow [this guide](http://docs.microsofttranslator.com/text-translate.html) to create a cognitive services client in Azure
* Create the file **Translate-Secrets.json** in your **My Documents** folder and paste in the Json structure at the bottom of this document. 
* Replace the aaa's with the values from your azure account.


## Using it
It should be pretty straight-forward to use this, following the example from the Console Application in the presentation Layer. For brevity, here are the available methods: 

>**TranslationService()**<br />
>Blank constructor, it will start a timer internally to refresh the client token, which resets every 10 minutes<br />
>
>**NOTE**<br />
>The constructor initiates a timer that immediately requests an authentication token from your Azure Cognitive Service 
> account. 


>**.TranslateSingle(from, to, text)**<br/>
>Translate a single chunk of text from one language to another
>```csharp
>var from = "en";
>var to   = "no";
>var text = "yes we have no bananas";
>
> var translatedText = translationService.TranslateSingle(from, to, text);
>```

>**.SupportedLanguages**<br />
>Property access to all supported languages. It will return an **IEnumerable&lt;Language&gt;** that has the following structure:
>```csharp
>namespace Translate.Domain.Entities
>{
>   public class Language
>   {
>        public string Code { get; set; }
>    }
>}
>```
>**NOTE**<br />
>The supported languages are kept in-memory in every future call, so feel free to use this collection
>in your application logic

## Contributions welcome!
I'm happy to accept any contributions to make this wrapper as useful as possible! The more, the merrier!
A couple of notes: 
- The project was written in [Visual Studio 2017](https://www.visualstudio.com). In order to work on this code, you only need the Community Edition.
- Please write [useful commit messages](https://chris.beams.io/posts/git-commit/)
- I am a strong believer in Unit-Testing and TDD. Please ensure adequate test-coverage for your logic
    - [XUnit.Net](https://xunit.github.io/) is used as the test framwork
- Follow the code-style used. It is based on proven patterns and practices :)


## A note on secrets
You need to provide the Account Name and key to the framework, and of course, saving that into source-code is not consider great practice. Thus, I implemented a settings-reader that will grab the values from an explicitly defined Json file. <br />
 this is implemented as follows:

The framework defines an interface named **ISettingsReader**:

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

## Resources

Contents of Translate-Secrets.json:
```Json
{
    "Translator:AccountKey"  : "aaaaaaaaaaaaaaaaaaaaaaaaaa",
    "Translator:AccountName" : "aaaaaaaaaaaaaaaaaaaaaaaaaa"
}
```

(Replace the values with the actual AccountKey and AccountName from your Azure Cognitive Services Account )


