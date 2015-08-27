// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.UnitTests.Diagnostics
{
    public partial class UnusedSuppressMessageTests
    {
        #region UnusedSuppressionBasic

        [Fact]
        public void UnusedSuppressionBasicOnType()
        {
            VerifyCSharp(@"
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(""Test"", ""CS0168"", Scope=""Type"", Target =""B"")]
class C1
        {
            public void A()
            {
                int i;
            }
        }
",
                Diagnostic("RS9000",new LinePosition(1,11)));
        }

        public void UnusedSuppressionBasicOnType_NoDiag()
        {
            VerifyCSharp(@"
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(""Test"", ""CS0168"", Scope=""Type"", Target =""C1"")]
class C1
        {
            public void A()
            {
                int i;
            }
        }
",
                DiagnosticDescription.None);
        }

        public void UnusedGlobalSuppression_NoError()
        {
            VerifyCSharp(@"
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(""Test"", ""CS0168"")]
class C1
        {
            public void A()
            {
                int i;
            }
        }
",
                DiagnosticDescription.None);
        }

        public void UnusedGlobalSuppression()
        {
            VerifyCSharp(@"
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(""Test"", ""CS0"")]
class C1
        {
            public void A()
            {
                int i;
            }
        }
",
                Diagnostic("RS9000", new LinePosition(1, 11)));
        }

        public void UnusedGlobalSuppression()
        {
            VerifyCSharp(@"
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(""Test"", ""CS0"")]
class C1
        {
            public void A()
            {
                int i;
            }
        }
",
                Diagnostic("RS9000", new LinePosition(1, 11)));
        }

        public void UnusedGlobalSuppression_Module()
        {
            VerifyCSharp(@"
[module: System.Diagnostics.CodeAnalysis.SuppressMessage(""Test"", ""CS0"")]
class C1
        {
            public void A()
            {
                int i;
            }
        }
",
                Diagnostic("RS9000", new LinePosition(1, 9)));
        }

        public void UnusedGlobalSuppression_TwoMessages()
        {
            VerifyCSharp(@"
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(""Test"", ""CS0"")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(""Test"", ""CS0168"")]
class C1
        {
            public void A()
            {
                int i;
            }
        }
",
                Diagnostic("RS9000", new LinePosition(1, 11)),
                Diagnostic("RS9000", new LinePosition(2, 11)));
        }

        public void UnusedSuppressionOnType_MissingScope()
        {
            VerifyCSharp(@"
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(""Test"", ""CS0"",  Target =""C1"")]

class C1
        {
            public void A()
            {
                int i;
            }
        }
",
                Diagnostic("RS9000", new LinePosition(1, 11)));
        }

        #endregion

        VerifyCSharp(string Source, params DiagnosticDescription[] Expected)
        {
            var compilation = CreateCompilation(Source, LanguageNames.CSharp, null);
            var UnusedDiagnostics = AnalyzerDriver.CheckForUnusedGlobalSuppressions(compilation);
            UnusedDiagnostics.Verify(expected);
        }

        private static Compilation CreateCompilation(string source, string language, string rootNamespace)
        {
            string fileName = language == LanguageNames.CSharp ? "Test.cs" : "Test.vb";
            string projectName = "TestProject";

            var syntaxTree = language == LanguageNames.CSharp ?
                CSharpSyntaxTree.ParseText(source, path: fileName) :
                VisualBasicSyntaxTree.ParseText(source, path: fileName);

            if (language == LanguageNames.CSharp)
            {
                return CSharpCompilation.Create(
                    projectName,
                    syntaxTrees: new[] { syntaxTree },
                    references: new[] { TestBase.MscorlibRef });
            }
            else
            {
                return VisualBasicCompilation.Create(
                    projectName,
                    syntaxTrees: new[] { syntaxTree },
                    references: new[] { TestBase.MscorlibRef },
                    options: new VisualBasicCompilationOptions(
                        OutputKind.DynamicallyLinkedLibrary,
                        rootNamespace: rootNamespace));
            }
        }

        protected DiagnosticDescription Diagnostic(string id, LinePosition linePosition)
        {
            return new DiagnosticDescription(id, false, null,null, linePosition, null, false);
        }

    }
}
