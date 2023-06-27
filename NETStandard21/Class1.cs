using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NETStandard21
{
    public class Class1
    {
        public static Assembly CompileCode( string code )
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText( code );

            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(Class1).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                // Add more assemblies here that you will need
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions( OutputKind.DynamicallyLinkedLibrary ) );

            using( var ms = new MemoryStream() )
            {
                EmitResult result = compilation.Emit( ms );

                if( !result.Success )
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where( diagnostic =>
                         diagnostic.IsWarningAsError ||
                         diagnostic.Severity == DiagnosticSeverity.Error );

                    foreach( Diagnostic diagnostic in failures )
                    {
                        var lineSpan = diagnostic.Location.GetLineSpan(); // Get the linespan of the error
                        Console.Error.WriteLine( "Error at Line {0}, Column {1}: {2}", lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character, diagnostic.GetMessage( CultureInfo.InvariantCulture ) );
                    }

                    return null;
                }
                else
                {
                    ms.Seek( 0, SeekOrigin.Begin );
                    return Assembly.Load( ms.ToArray() );
                }
            }
        }

    }
}
