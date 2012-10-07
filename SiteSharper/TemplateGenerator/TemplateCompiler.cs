using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using Toolbox;

namespace ProductSite.TemplateGenerator
{
	static class TemplateCompiler
	{
		public static CompiledTemplate compile(this ParsedTemplate parsedTemplate)
		{
			var assembly = compileToAssembly(parsedTemplate.CompileUnit);
			return new CompiledTemplate(parsedTemplate.FullyQualifiedClassName, assembly);
		}

		static Assembly compileToAssembly(CodeCompileUnit compileUnit)
		{
			var provider = new CSharpCodeProvider();
			var baseAssembly = typeof(RazorHtmlTemplate<>).Assembly;

			// todo: autodetect @using header lines and add them.

			var parameters = new CompilerParameters(
				// path does not work
				// csharp.dll is for the use of dynamic
				new[] { Path.GetFileName(baseAssembly.CodeBase), "System.Core.dll", "Microsoft.CSharp.dll"},
				string.Empty,
				false);

			dumpGeneratedCode(compileUnit, provider);

			var compRes = provider.CompileAssemblyFromDom(parameters, new[] { compileUnit });

			if (compRes.Output.Count != 0)
			{
#if DEBUG
				Log.D("Compilation results:");
				foreach (var str in compRes.Output)
					Log.D(str);
#endif

				if (compRes.Errors.Count != 0)
					throw new Exception(compRes.Errors[0].ToString());
			}
			var assembly = compRes.CompiledAssembly;
			return assembly;
		}


		static void dumpGeneratedCode(CodeCompileUnit compileUnit, CSharpCodeProvider provider)
		{
			using (var generatedCode = new StringWriter())
			{
				var options = new CodeGeneratorOptions();
				provider.GenerateCodeFromCompileUnit(compileUnit, generatedCode, options);

#if DEBUG
				var generated = generatedCode.ToString();
				Log.D("Generated code:");
				using (var reader = new StringReader(generated))
				{
					var lineNumber = 1;
					while (reader.Peek() >= 0)
					{
						Log.D(lineNumber + ": " + reader.ReadLine());
						++lineNumber;
					}
				}
#endif
			}
		}

	}
}
