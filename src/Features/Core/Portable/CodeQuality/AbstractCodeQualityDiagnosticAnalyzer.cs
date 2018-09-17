﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.PooledObjects;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CodeQuality
{
    internal abstract class AbstractCodeQualityDiagnosticAnalyzer : DiagnosticAnalyzer, IBuiltInAnalyzer
    {
        private readonly GeneratedCodeAnalysisFlags _generatedCodeAnalysisFlags;

        protected AbstractCodeQualityDiagnosticAnalyzer(
            ImmutableArray<DiagnosticDescriptor> descriptors,
            GeneratedCodeAnalysisFlags generatedCodeAnalysisFlags)
        {
            SupportedDiagnostics = descriptors;
            _generatedCodeAnalysisFlags = generatedCodeAnalysisFlags;
        }

        public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }

        public sealed override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(_generatedCodeAnalysisFlags);
            context.EnableConcurrentExecution();

            InitializeWorker(context);
        }

        protected abstract void InitializeWorker(AnalysisContext context);

        public abstract DiagnosticAnalyzerCategory GetAnalyzerCategory();
        public abstract bool OpenFileOnly(Workspace workspace);

        protected static DiagnosticDescriptor CreateDescriptor(
            string id,
            LocalizableString title,
            LocalizableString messageFormat,
            bool isUnneccessary,
            bool isEnabledByDefault = true,
            bool isConfigurable = true,
            params string[] customTags)
        {
            var customTagsBuilder = ArrayBuilder<string>.GetInstance();
            customTagsBuilder.AddRange(customTags.Concat(WellKnownDiagnosticTags.Telemetry));

            if (!isConfigurable)
            {
                customTagsBuilder.Add(WellKnownDiagnosticTags.NotConfigurable);
            }

            if (isUnneccessary)
            {
                customTagsBuilder.Add(WellKnownDiagnosticTags.Unnecessary);
            }

            return new DiagnosticDescriptor(
                id, title, messageFormat,
                DiagnosticCategory.CodeQuality,
                DiagnosticSeverity.Info,
                isEnabledByDefault,
                customTags: customTagsBuilder.ToArrayAndFree());
        }
    }
}
