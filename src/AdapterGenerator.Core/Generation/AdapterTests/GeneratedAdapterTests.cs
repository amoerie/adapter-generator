using System;
using AdapterGenerator.Core.Generation.Adapters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests {
  public class GeneratedAdapterTests : IGeneratedAdapterTests {
    public string TestClassName { get; }
    public IGeneratedAdapter Adapter { get; }
    public CompilationUnitSyntax CompilationUnitSyntax { get; }

    public GeneratedAdapterTests(string testClassName, IGeneratedAdapter adapter,
      CompilationUnitSyntax compilationUnitSyntax) {
      if (testClassName == null) throw new ArgumentNullException(nameof(testClassName));
      if (adapter == null) throw new ArgumentNullException(nameof(adapter));
      if (compilationUnitSyntax == null) throw new ArgumentNullException(nameof(compilationUnitSyntax));
      Adapter = adapter;
      CompilationUnitSyntax = compilationUnitSyntax;
      TestClassName = testClassName;
    }
  }
}