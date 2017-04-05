using Moq;
using Should;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translate.CrossCutting;
using Translate.Domain.Contracts;
using Xunit;

namespace Translate.Business.Tests
{
    public class ExceptionHandlerTests : TestsFor<ExceptionHandler>
    {
        [Fact]
        public void Run_MethodIsNull_ReturnsDefaultType()
        {
            // Arrange
            Func<int> nullFunction = null;

            // Act
            var result = Instance.Run(nullFunction);

            // Assert
            result.ShouldEqual(default(int));
        }



        [Fact]
        public void Run_MethodThrows_ReturnsDefaultType()
        {
            // Arrange
            Func<int> nullFunction = () => throw new Exception("I'm bad");

            // Act
            var result = Instance.Run(nullFunction);

            // Assert
            result.ShouldEqual(default(int));
        }


        [Fact]
        public void Run_MethodThrows_ExceptionIsLogged()
        {
            // Arrange
            var badException = new Exception("I'm bad");
            Func<int> nullFunction = () => throw badException;

            // Act
            var result = Instance.Run(nullFunction);

            // Assert
            GetMockFor<ILogger>().Verify(o => o.LogException(badException), Times.Once());
        }

    }
}
