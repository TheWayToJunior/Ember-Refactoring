using Ember.Shared;
using System.Linq;
using Xunit;

namespace Ember.Tests
{
    public class OperationResultBuilderTests
    {
        [Fact]
        public void OperationResultBuilder_ResultHasValueAndError_Error()
        {
            var builder = OperationResult<string>.CreateBuilder();

            /// Act
            var result = builder.SetValue("Test").AppendError("Test").BuildResult();

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Test", result.Value);
            Assert.Equal("Test", result.Errors.First());
        }

        [Fact]
        public void OperationResultBuilder_ResultHasErrorAndValue_Error()
        {
            var builder = OperationResult<string>.CreateBuilder();

            /// Act
            var result = builder.AppendError("Test").SetValue("Test").BuildResult();

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Test", result.Value);
            Assert.Equal("Test", result.Errors.First());
        }

        [Fact]
        public void OperationResultBuilder_ResultHasError_Error()
        {
            var builder = OperationResult.CreateBuilder();

            /// Act
            var result = builder.AppendError("Test").BuildResult();

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Test", result.Errors.First());
        }

        [Fact]
        public void OperationResultBuilder_ResultHasNoError_Success()
        {
            var builder = OperationResult.CreateBuilder();

            /// Act
            var result = builder.BuildResult();

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Errors);
        }
    }
}
