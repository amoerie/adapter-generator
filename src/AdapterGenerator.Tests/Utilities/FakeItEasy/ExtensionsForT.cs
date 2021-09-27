using FakeItEasy;

namespace AdapterGenerator.Tests.Utilities.FakeItEasy
{
    public static class ExtensionsForT
    {
        public static T Fake<T>(this T reference) where T : class
        {
            return A.Fake<T>();
        }

        public static T Dummy<T>(this T reference)
        {
            return A.Dummy<T>();
        }
    }
}