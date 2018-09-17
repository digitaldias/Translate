using Should;
using System;
using Translate.Business.Validators;
using Translate.CrossCutting;
using Translate.Domain.Entities;
using Xunit;

namespace Translate.Business.Tests.Validators
{
    public class LanguageValidatorTests: TestsFor<LanguageValidator>
    {

        [Fact]
        public void Constructor_LoggerIsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws(typeof(ArgumentNullException), () => new LanguageValidator(null));
        }


        [Fact]
        public void IsValid_LanguageIsNull_ReturnsFalse()
        {
            // Act
            var result = Instance.IsValid(null);

            // Assert
            result.ShouldBeFalse();
        }


        [Fact]
        public void IsValid_LanguageIsValid_ReturnsTrue()
        {
            // Act
            var result = Instance.IsValid(new Language("en"));

            // Assert
            result.ShouldBeTrue();
        }


        [Fact]
        public void IsValid_LanguageCodeIsNull_ReturnsFalse()
        {
            // Arrange
            var badLanguage = new Language("");

            // Act
            var result = Instance.IsValid(badLanguage);

            // Assert
            result.ShouldBeFalse();
        }


        [Fact]
        public void IsValid_LanguageCodeIs1InLength_ReturnsFalse()
        {
            // Arrange
            var badLanguage = new Language("a");

            // Act
            var result = Instance.IsValid(badLanguage);

            // Assert
            result.ShouldBeFalse();
        }


        [Fact]
        public void IsValid_LanguageIs5InLength_ReturnsFalse()
        {
            // Arrange
            var badLanguage = new Language("abcde");

            // Act
            var result = Instance.IsValid(badLanguage);

            // Assert
            result.ShouldBeFalse();
        }
    }
}
