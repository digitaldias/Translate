using Moq;
using Should;
using System;
using Translate.Business.Validators;
using Translate.CrossCutting;
using Translate.Domain.Contracts;
using Translate.Domain.Contracts.Validators;
using Translate.Domain.Entities;
using Xunit;

namespace Translate.Business.Tests.Validators
{
    public class TranslationRequestValidatorTests : TestsFor<TranslationRequestValidator>
    {
        [Fact]
        public void Constructor_DependsOnIlogger()
        {
            // Assert
            Assert.Throws(typeof(ArgumentNullException),
                () => new TranslationRequestValidator(null, ValidLanguageValidator));
        }


        [Fact]
        public void Constructor_DependsOnILanguageValidator()
        {
            // Assert
            Assert.Throws(typeof(ArgumentNullException),
                () => new TranslationRequestValidator(ValidLogger, null));
        }


        [Fact]
        public void IsValid_RequestIsNull_ReturnsFalse()
        {
            // Act
            var result = Instance.IsValid(null);

            // Assert
            result.ShouldBeFalse();
        }


        [Fact]
        public void IsValid_ValidTranslationRequest_ReturnsTrue()
        {
            // Arrange
            GetMockFor<ILanguageValidator>()
                .Setup(o => o.IsValid(It.IsAny<Language>()))
                .Returns(true);

            // Act
            var result = Instance.IsValid(ValidTranslationRequest);

            // Assert
            result.ShouldBeTrue();
        }


        [Fact]
        public void IsValid_LangageFromIsInvalid_ReturnsFalse()
        {
            // Arrange
            var request = ValidTranslationRequest;
            GetMockFor<ILanguageValidator>()
                .SetupSequence(o => o.IsValid(It.IsAny<Language>()))
                .Returns(false)
                .Returns(true);                

            // Act
            var result = Instance.IsValid(request);

            // Assert
            result.ShouldBeFalse();
        }


        [Fact]
        public void IsValid_LangageToIsInvalid_ReturnsFalse()
        {
            // Arrange
            var request = ValidTranslationRequest;
            GetMockFor<ILanguageValidator>()
                .SetupSequence(o => o.IsValid(It.IsAny<Language>()))
                .Returns(true)
                .Returns(false);

            // Act
            var result = Instance.IsValid(request);

            // Assert
            result.ShouldBeFalse();
        }



        [Fact]
        public void IsValid_NoTextToTranslate_ReturnsFalse()
        {
            // Arrange
            var request = ValidTranslationRequest;
            request.Text = string.Empty;
            GetMockFor<ILanguageValidator>()
                .Setup(o => o.IsValid(It.IsAny<Language>()))
                .Returns(true);

            // Act
            var result = Instance.IsValid(request);

            // Assert
            result.ShouldBeFalse();
        }


        #region Helpers

        private TranslationRequest ValidTranslationRequest =>
            new TranslationRequest
            {
                From = FromLanguage,
                To = ToLanguage,
                Text = "Call me Ishmael. Some years ago- never mind how long precisely- having little or no money in my purse, and nothing particular to interest me on shore, I thought I would sail about a little and see the watery part of the world"
            };

        private Language FromLanguage => new Language("en");

        private Language ToLanguage => new Language("de");


        private ILogger ValidLogger
        {
            get { return It.IsAny<ILogger>(); }
        }


        private ILanguageValidator ValidLanguageValidator
        {
            get
            {
                var mock = new Mock<ILanguageValidator>();
                mock.Setup(o => o.IsValid(It.IsAny<Language>()))
                    .Returns(true);

                return mock.Object;
            }
        }

        #endregion

    }
}
