# Translate
A wrapper library to use the [Microsoft Cognitive Translation](https://www.microsoft.com/cognitive-services/en-us/translator-api) API. It automatically requests an authentication token every 9 minutes, and performs the API calls masking away the details of each REST invocation.

## Using it
It should be pretty straight-forward to use this, following the example from the Console Application in the presentation Layer. For brevity, here are the available methods: 

>**TranslationService()**<br />
>Blank constructor, it will start a timer internally to refresh the client token, which resets every 10 minutes<br />
>
>**NOTE**<br />
>The constructor initiates a timer that immediately requests an authentication token from your Azure Cognitive Service 
> account. 



>**TranslationService.TranslateSingle(from, to, text)**<br/>
>Translate a single chunk of text from one language to another
>```csharp
>var from = "en";
>var to   = "no";
>var text = "yes we have no bananas";
>
> var translatedText = translationService.TranslateSingle(from, to, text);
>```


>**TranslationService.SupportedLanguages**<br />
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


## Getting Started
In order to use this library, you'll need to create a (free) Cognitive Services Account in the Azure portal.

[There is a good guide for doing this here](http://docs.microsofttranslator.com/text-translate.html)

Next, you need to provide the Account Name and key to the framework. Right now, this is implemented as follows:

The framework defines an interface named **ISettingsReader** that is defined as such:

```csharp
namespace Translate.Domain.Contracts
{
    public interface ISettingsReader
    {
        string this[string name] { get; }
    }
}
```

You can implement your own *SettingsReader* to obtain the AccountName and AccountKey, or you can opt-in on the already
provided implementation, which looks for a determined file in the *My Documents* folder: 

It's name is<br />
     ***&lt;Your Application Name&gt;-Secrets.Json***

In the default Project, the console application is named **Translate.exe** so there is a corresponding<br />
***Translate-Secrets.Json*** in my **My Documents** folder

The contents of the Json file are: 
```Json
{
    "Translator:AccountKey"  : "aaaaaaaaaaaaaaaaaaaaaaaaaa",
    "Translator:AccountName" : "aaaaaaaaaaaaaaaaaaaaaaaaaa"
}
```
(Replace the values with the actual AccountKey and AccountName from your Azure Cognitive Services Account )


