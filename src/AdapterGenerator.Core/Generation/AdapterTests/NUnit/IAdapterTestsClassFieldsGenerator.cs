using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.AdapterTests.NUnit {
  public interface IAdapterTestsClassFieldsGenerator {
    IImmutableList<FieldDeclarationSyntax> Generate(IClassAdapterTestsGenerationContextWithClass context);
    IImmutableList<FieldDeclarationSyntax> Generate(IEnumAdapterTestsGenerationContextWithClass context);
  }
}