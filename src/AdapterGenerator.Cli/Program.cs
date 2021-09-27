using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using AdapterGenerator.Core;
using AdapterGenerator.Core.Composition;
using Autofac;

namespace AdapterGenerator.Cli
{
    /// <summary>
    /// Program to generate C# adapter code
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Generates an adapter and according unit tests for every pair of matching classes it can find in the source files and target files
        /// and writes this adapter and its unit tests to the specified output directory.
        /// </summary>
        /// <param name="sourceFiles">One or more files to use as input for the adapter</param>
        /// <param name="targetFiles">One or more files to use as output for the adapter</param>
        /// <param name="outputDirectory">The directory in which to put the generated code</param>
        /// <returns></returns>
        static void Main(string sourceFiles, string targetFiles, string outputDirectory)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<CoreModule>();
            var container = containerBuilder.Build();
            var adapterGeneratorService = container.Resolve<IAdapterGeneratorService>();
            var outputDirectoryInfo = new DirectoryInfo(outputDirectory);

            adapterGeneratorService.GenerateAdapters(ToFileInfos(sourceFiles), ToFileInfos(targetFiles), outputDirectoryInfo);
        }

        private static ImmutableList<FileInfo> ToFileInfos(string pattern)
        {
            var directory = Path.GetDirectoryName(pattern);
            var file = Path.GetFileName(pattern);

            return Directory.GetFiles(directory, file, new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                }).Select(f => new FileInfo(f))
                .ToImmutableList();
        }
    }
}