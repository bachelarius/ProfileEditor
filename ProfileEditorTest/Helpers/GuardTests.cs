using ProfileEditor.Helpers;

namespace ProfileEditorTests.Helpers {
    public class GuardTests
    {
        [Fact]
        public void NotNull_ReferenceType_WithValidValue_ReturnsValue()
        {
            // Arrange
            string testValue = "test";

            // Act
            var result = Guard.NotNull(testValue, nameof(testValue));

            // Assert
            Assert.Equal(testValue, result);
        }

        [Fact]
        public void NotNull_ReferenceType_WithNullValue_ThrowsArgumentNullException()
        {
            // Arrange
            string? testValue = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => Guard.NotNull(testValue, nameof(testValue))
            );
            Assert.Equal(nameof(testValue), exception.ParamName);
        }

        [Fact]
        public void NotNull_ValueType_WithValidValue_ReturnsValue()
        {
            // Arrange
            int? testValue = 42;

            // Act
            var result = Guard.NotNull(testValue, nameof(testValue));

            // Assert
            Assert.Equal(testValue.Value, result);
        }

        [Fact]
        public void NotNull_ValueType_WithNullValue_ThrowsArgumentNullException()
        {
            // Arrange
            int? testValue = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => Guard.NotNull(testValue, nameof(testValue))
            );
            Assert.Equal(nameof(testValue), exception.ParamName);
        }
    }
}
