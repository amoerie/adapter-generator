using AdapterGenerator.Core.Parsing;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public interface IAdapterTestsPropertyTestMethodGenerator {
    MethodDeclarationSyntax Generate(IClassAdapterTestsGenerationContextWithClass context, IProperty targetProperty);
  }
}