using FizzWare.NBuilder;
using Moq;
using Should;
using System;
using Translate.CrossCutting;
using Translate.Domain.Contracts;
using Translate.Domain.Entities;
using Xunit;

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


        #region HELPER METHODS AND PROPERTIES 

        private string Someone => "someone@somewhere.com";


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
