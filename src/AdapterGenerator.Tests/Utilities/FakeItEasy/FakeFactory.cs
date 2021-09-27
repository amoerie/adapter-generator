using FakeItEasy;

namespace AdapterGenerator.Tests.Utilities.FakeItEasy {
  public static class FakeFactory {
    public static void Create<T1>(out T1 fake1) {
      fake1 = A.Fake<T1>();
    }

    public static void Create<T1, T2>(out T1 fake1, out T2 fake2) {
      fake1 = A.Fake<T1>();
      fake2 = A.Fake<T2>();
    }

    public static void Create<T1, T2, T3>(out T1 fake1, out T2 fake2, out T3 fake3) {
      fake1 = A.Fake<T1>();
      fake2 = A.Fake<T2>();
      fake3 = A.Fake<T3>();
    }

    public static void Create<T1, T2, T3, T4>(out T1 fake1, out T2 fake2, out T3 fake3, out T4 fake4) {
      fake1 = A.Fake<T1>();
      fake2 = A.Fake<T2>();
      fake3 = A.Fake<T3>();
      fake4 = A.Fake<T4>();
    }

    public static void Create<T1, T2, T3, T4, T5>(out T1 fake1, out T2 fake2, out T3 fake3, out T4 fake4, out T5 fake5) {
      fake1 = A.Fake<T1>();
      fake2 = A.Fake<T2>();
      fake3 = A.Fake<T3>();
      fake4 = A.Fake<T4>();
      fake5 = A.Fake<T5>();
    }

    public static void Create<T1, T2, T3, T4, T5, T6>(out T1 fake1, out T2 fake2, out T3 fake3, out T4 fake4, out T5 fake5, out T6 fake6) {
      fake1 = A.Fake<T1>();
      fake2 = A.Fake<T2>();
      fake3 = A.Fake<T3>();
      fake4 = A.Fake<T4>();
      fake5 = A.Fake<T5>();
      fake6 = A.Fake<T6>();
    }

    public static void Create<T1, T2, T3, T4, T5, T6, T7>(out T1 fake1, out T2 fake2, out T3 fake3, out T4 fake4, out T5 fake5, out T6 fake6, out T7 fake7) {
      fake1 = A.Fake<T1>();
      fake2 = A.Fake<T2>();
      fake3 = A.Fake<T3>();
      fake4 = A.Fake<T4>();
      fake5 = A.Fake<T5>();
      fake6 = A.Fake<T6>();
      fake7 = A.Fake<T7>();
    }
  }
}