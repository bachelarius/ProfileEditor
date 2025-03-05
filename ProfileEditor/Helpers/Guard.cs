namespace ProfileEditor.Helpers {
    public static class Guard {
        public static T NotNull<T>(T? value, string parameterName) where T : class {
            if (value == null) {
                throw new ArgumentNullException(parameterName);
            } else {
                return value!;
            }
        }

        public static T NotNull<T>(T? value, string parameterName) where T : struct {
            if (!value.HasValue) {
                throw new ArgumentNullException(parameterName);
            } else {
                return value.Value;
            }
        }
    }
}
