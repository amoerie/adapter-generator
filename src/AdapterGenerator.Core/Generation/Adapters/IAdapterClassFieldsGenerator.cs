using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdapterGenerator.Core.Generation.Adapters {
  public interface IAdapterClassFieldsGenerator {
    IImmutableList<FieldDeclarationSyntax> Generate(IClassAdapterGenerationContextWithClass context);
  }
}