using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis
{
   internal class UnusedSuppresionDiagnostic
    {
        private static readonly LocalizableString s_localizableMessage = new LocalizableResourceString(nameof(CodeAnalysisResources.UnnecessarySuppressionMessage), CodeAnalysisResources.ResourceManager, typeof(CodeAnalysisResources));

        private static readonly LocalizableString s_localizableTitleUnnecessarySuppression = new LocalizableResourceString(nameof(CodeAnalysisResources.UnnecessarySuppressionTitle), CodeAnalysisResources.ResourceManager, typeof(CodeAnalysisResources));
        private static readonly LocalizableString s_localizableCategory = new LocalizableString(nameof(CodeAnalysisResources.UnnecessarySuppressionCategory), CodeAnalysisResources.ResourceManager, typeof(CodeAnalysisResources));
        private static readonly DiagnosticDescriptor s_descriptorUnnecessarySuppression = new DiagnosticDescriptor("RS9000",
                                                                    s_localizableTitleUnnecessarySuppression.ToString(),
                                                                    s_localizableMessage.ToString(),
                                                                    s_localizableCategory.ToString(),
                                                                    DiagnosticSeverity.Hidden,
                                                                    isEnabledByDefault: true,
                                                                    customTags: WellKnownDiagnosticTags.Unnecessary);

        static DiagnosticDescriptor descriptor = new DiagnosticDescriptor("RS2001", "Suppression Message unnecessary", "SuppressMessage Attribute is unnesscessary", "Suppression", DiagnosticSeverity.Warning, true);

        private UnusedSuppresionDiagnostic(){}
        internal static Diagnostic Create (Location location)
        {
           return Diagnostic.Create(s_descriptorUnnecessarySuppression, location);
        }
    }
}
