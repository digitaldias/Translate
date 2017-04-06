using Moq;
using Translate.CrossCutting;
using Translate.Domain.Contracts;
using Xunit;
using Should;
using Translate.Domain.Entities;
using FizzWare.NBuilder;
using System;

namespace Translate.Business.Tests
{
    public class TranslationServiceTest : TestsFor<TranslationService>
    {
        private IExceptionHandler _exceptionHandler;

        public TranslationServiceTest()
        {
        }


        public override void OverrideMocks()
        {
            var mockedLogger = GetMockFor<ILogger>().Object;
            _exceptionHandler = new ExceptionHandler(mockedLogger);
            Inject(_exceptionHandler);
        }


        [Fact]
        public void SupportedLanguages_WhenCalled_AquiresLanguages()
        {
            // Act
            var result = Instance.SupportedLanguages;

            // Assert
            GetMockFor<ITranslationClient>().Verify(o => o.GetLanguageCodes(), Times.Once());                
        }


        [Fact]
        public void SupportedLanguages_WhenCalledTwice_AquiresLanguagesOnlyOnce()
        {
            // Act
            var result = Instance.SupportedLanguages;
            var result2 = Instance.SupportedLanguages;

            // Assert
            GetMockFor<ITranslationClient>().Verify(o => o.GetLanguageCodes(), Times.Once());
        }


        [Fact]
        public void TranslateSingle_FromIsEmpty_ReturnsEmptyString()
        {
            // Act
            var result = Instance.TranslateSingle(null, "en", "yes we have no bananas");

            // Assert
            result.ShouldBeEmpty();
        }


        [Fact]
        public void TranslateSingle_ToIsEmpty_ReturnsEmptyString()
        {
            // Act
            var result = Instance.TranslateSingle("en", null, "yes we have no bananas");

            // Assert
            result.ShouldBeEmpty();
        }


        [Fact]
        public void TranslateSingle_TextIsEmpty_ReturnsEmptyString()
        {
            // Act
            var result = Instance.TranslateSingle("en", "no", null);

            // Assert
            result.ShouldBeEmpty();
        }


        [Fact]
        public void TranslateSingle_FromIsInvalidLanguage_ReturnsEmptyString()
        {
            // Arrange
            SupportEnglishAndNorwegian();
            var invalidCode = "Invalid";

            // Act
            var result = Instance.TranslateSingle(invalidCode, "en", "yes we have no bananas");

            // Assert
            result.ShouldBeEmpty();
        }


        [Fact]
        public void TranslateSingle_FromIsInvalidLanguage_LoggerIsInvoked()
        {
            // Arrange
            SupportEnglishAndNorwegian();
            var invalidCode = "Invalid";

            // Act
            var result = Instance.TranslateSingle(invalidCode, "en", "yes we have no bananas");

            // Assert
            GetMockFor<ILogger>().Verify(l => l.LogError(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void TranslateSingle_ToIsInvalidLanguage_LoggerIsInvoked()
        {
            // Arrange
            SupportEnglishAndNorwegian();
            var invalidCode = "Invalid";

            // Act
            var result = Instance.TranslateSingle("en", invalidCode, "yes we have no bananas");

            // Assert
            GetMockFor<ILogger>().Verify(l => l.LogError(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void TranslateSingle_ToIsInvalidLanguage_ReturnsEmptyString()
        {
            // Arrange
            SupportEnglishAndNorwegian();
            var invalidCode = "Invalid";

            // Act
            var result = Instance.TranslateSingle("en", invalidCode, "yes we have no bananas");

            // Assert
            result.ShouldBeEmpty();
        }


        [Fact]
        public void TranslateSingle_AllParamatersValid_ResultReturnedFromTranslationClient()
        {
            // Arrange
            SupportEnglishAndNorwegian();
            var from = new Language { Code = "en" };
            var to = new Language { Code = "no" };
            var text = "yes we have no bananas";
            var translated = "<result>ja vi mangler bananer</result>";
            GetMockFor<ITranslationClient>().Setup(c => c.TranslateSingle(from.Code, to.Code, text)).Returns(translated);

            // Act
            var result = Instance.TranslateSingle(from, to, text);

            // Assert
            result.ShouldEqual("ja vi mangler bananer");
        }


        [Fact]
        public void DetectLanguage_TextIsNull_ReturnsNull()
        {
            // Act
            var result = Instance.DetectLanguage(null);

            // Assert
            result.ShouldBeNull();
        }


        [Fact]
        public void DetectLanguage_EnglishPhrase_ReturnsEnglishLanguage()
        {
            // Act
            Instance.DetectLanguage(EnglishPhrase);

            // Assert
            GetMockFor<ITranslationClient>().Verify(c => c.DetectLanguage(EnglishPhrase), Times.Once());
        }


        [Fact]
        public void DetectLanguage_TextIsNull_DoesNotInvokeRestClient()
        {
            // Arrange
            string nullString = null;

            // Act
            var result = Instance.DetectLanguage(nullString);

            // Assert
            GetMockFor<ITranslationClient>().Verify(c => c.DetectLanguage(nullString), Times.Never());
        }


        [Fact]
        public void DetectLanguage_ClientThrowsException_IsHandledByExceptionHandler()
        {
            // Arrange            
            var badException = new Exception("I'm bad");
            GetMockFor<ITranslationClient>().Setup(c => c.DetectLanguage(EnglishPhrase)).Throws(badException);

            // Act           
            var result = Instance.DetectLanguage(EnglishPhrase);

            // Assert
            // By asserting that the logger is called, we KNOW that the exceptionHandler is running this 
            GetMockFor<ILogger>().Verify(l => l.LogException(badException), Times.Once());
        }


        [Fact]
        public void DetectLanguage_TextIsOver100000Characters_TextIsTrimmedTo100000Characters()
        {
            // Arrange
            var random = new RandomGenerator();
            var longText = random.Phrase(120_000);

            // Act
            var result = Instance.DetectLanguage(longText);

            // Assert
            GetMockFor<ITranslationClient>().Verify(c => c.DetectLanguage(It.Is<string>(text => text.Length == 100_000)));
        }


        [Fact]
        public void BreakSentences_LanguageIsNull_ReturnsNull()
        {
            // Act           
            var result = Instance.BreakSentences(null, EnglishPhrase);

            // Assert
            result.ShouldBeNull();
        }



        [Fact]
        public void BreakSentences_ValidParameters_InvokesTranslationClient()
        {
            var englishLanguage = EnglishLanguage;
            // Act           
            var result = Instance.BreakSentences(englishLanguage, EnglishPhrase);

            // Assert
            GetMockFor<ITranslationClient>().Verify(c => c.BreakSentences(englishLanguage, EnglishPhrase), Times.Once());
        }


        [Fact]
        public void BreakSentences_TextIsNull_ReturnsNull()
        {
            // Act           
            var result = Instance.BreakSentences(EnglishLanguage, null);

            // Assert
            result.ShouldBeNull();
        }
        

        [Fact]
        public void AddTranslation_LanguageFromIsNull_ReturnsFalse()
        {
            // Arrange            

            // Act           
            var result = Instance.AddTranslation(null, EnglishLanguage, EnglishPhrase, NorwegianPhrase, Someone);

            // Assert
            result.ShouldBeFalse();
        }



        [Fact]
        public void AddTranslation_AllParametersValid_ResultIsTrue()
        {
            // Arrange            
            AddTranslationClientWillReturnTrue();            

            // Act           
            var result = Instance.AddTranslation(EnglishLanguage, NorwegianLanguage, EnglishPhrase, NorwegianPhrase, Someone);

            // Assert
            result.ShouldBeTrue();
        }


        [Fact]
        public void AddTranslation_AllParametersValid_InvokesTranslationClient()
        {
            // Arrange            
            var english              = EnglishLanguage;
            var norsk                = NorwegianLanguage;
            var englishText          = EnglishPhrase;
            var norwegianTranslation = NorwegianPhrase;
            var user                 = Someone;

            // Act           
            var result = Instance.AddTranslation(english, norsk, englishText, norwegianTranslation, user);

            // Assert
            GetMockFor<ITranslationClient>().Verify(c => c.AddTranslation(english, norsk, englishText, norwegianTranslation, user), Times.Once());
        }


        [Fact]
        public void AddTranslation_OriginalTextTooLong_ReturnsFalse()
        {
            // Arrange  
            AddTranslationClientWillReturnTrue();

            var random = new RandomGenerator();
            var english = EnglishLanguage;
            var norsk = NorwegianLanguage;
            var englishText = random.Phrase(1010);
            var norwegianTranslation = NorwegianPhrase;

            // Act           
            var result = Instance.AddTranslation(english, norsk, englishText, norwegianTranslation, Someone);

            // Assert
            result.ShouldBeFalse();
        }


        [Fact]
        public void AddTranslation_TranslatedTextTooLong_ReturnsFalse()
        {
            // Arrange  
            AddTranslationClientWillReturnTrue();

            var random = new RandomGenerator();
            var english = EnglishLanguage;
            var norsk = NorwegianLanguage;
            var englishText = EnglishPhrase;
            var norwegianTranslation = random.Phrase(2020);

            // Act           
            var result = Instance.AddTranslation(english, norsk, englishText, norwegianTranslation, Someone);

            // Assert
            result.ShouldBeFalse();
        }



        #region HELPER METHODS AND PROPERTIES 

        private string Someone => "someone@somewhere.com";


        private void AddTranslationClientWillReturnTrue()
        {
            GetMockFor<ITranslationClient>().Setup(c => c.AddTranslation(It.IsAny<Language>(), It.IsAny<Language>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        }


        private string EnglishPhrase => "A Tale of Two Cities. It was the best of times, it was the worst of times, it was the age of wisdom, it was the age of foolishness, it was the epoch of belief, it was the epoch of incredulity, it was the season of Light, it was the season of Darkness, it was the spring of hope, it was the winter of despair.";


        private string NorwegianPhrase => "En historie om to byer. Det var den beste av tider, det var den værste av tider, det var en alder av visdom, det var en alder for dumhet, det var en epoke med vantro, det var lysets sesong, det var mørkets sesong, det var håpets vår, det var vinteren for desperasjon.";


        private Language EnglishLanguage => new Language{ Code = "en" };


        private Language NorwegianLanguage => new Language{ Code = "no" };


        private void SupportEnglishAndNorwegian()
        {
            var supportedLanguages = new Language[] { new Language { Code = "no"}, new Language { Code = "en"} };

            GetMockFor<ITranslationClient>().Setup(o => o.GetLanguageCodes()).Returns(supportedLanguages);
        }

        #endregion
    }
}
