﻿using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace StructuredLogger.BinaryLogger
{
    internal class BuildSubmissionStartedEvent()
        : BuildStatusEventArgs(message: "", helpKeyword: null, senderName: null, eventTimestamp: DateTime.UtcNow)
    {
        public IDictionary<string, string?> GlobalProperties { get; set; }

        public IEnumerable<string> EntryProjectsFullPath { get; set; }

        public IEnumerable<string> TargetNames { get; set; }

        public BuildRequestDataFlags Flags { get; set; }

        public int SubmissionId { get; set; }
    }

    public enum BuildRequestDataFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,

        /// <summary>
        /// When this flag is present, the existing ProjectInstance in the build will be replaced by this one.
        /// </summary>
        ReplaceExistingProjectInstance = 1 << 0,

        /// <summary>
        /// When this flag is present, the "BuildResult" issued in response to this request will
        /// include "BuildResult.ProjectStateAfterBuild".
        /// </summary>
        ProvideProjectStateAfterBuild = 1 << 1,

        /// <summary>
        /// When this flag is present and the project has previously been built on a node whose affinity is
        /// incompatible with the affinity this request requires, we will ignore the project state (but not
        /// target results) that were previously generated.
        /// </summary>
        /// <remarks>
        /// This usually is not desired behavior.  It is only provided for those cases where the client
        /// knows that the new build request does not depend on project state generated by a previous request.  Setting
        /// this flag can provide a performance boost in the case of incompatible node affinities, as MSBuild would
        /// otherwise have to serialize the project state from one node to another, which may be
        /// expensive depending on how much data the project previously generated.
        ///
        /// This flag has no effect on target results, so if a previous request already built a target, the new
        /// request will not re-build that target (nor will any of the project state mutations which previously
        /// occurred as a consequence of building that target be re-applied.)
        /// </remarks>
        IgnoreExistingProjectState = 1 << 2,

        /// <summary>
        /// When this flag is present, caches including the "ProjectRootElementCacheBase" will be cleared
        /// after the build request completes.  This is used when the build request is known to modify a lot of
        /// state such as restoring packages or generating parts of the import graph.
        /// </summary>
        ClearCachesAfterBuild = 1 << 3,

        /// <summary>
        /// When this flag is present, the top level target(s) in the build request will be skipped if those targets
        /// are not defined in the Project to build. This only applies to this build request (if another target calls
        /// the "missing target" at any other point this will still result in an error).
        /// </summary>
        SkipNonexistentTargets = 1 << 4,

        /// <summary>
        /// When this flag is present, the "BuildResult" issued in response to this request will
        /// include a "BuildResult.ProjectStateAfterBuild" that includes ONLY the
        /// explicitly-requested properties, items, and metadata.
        /// </summary>
        ProvideSubsetOfStateAfterBuild = 1 << 5,

        /// <summary>
        /// When this flag is present, projects loaded during build will ignore missing imports ("ProjectLoadSettings.IgnoreMissingImports" and "ProjectLoadSettings.IgnoreInvalidImports").
        /// This is especially useful during a restore since some imports might come from packages that haven't been restored yet.
        /// </summary>
        IgnoreMissingEmptyAndInvalidImports = 1 << 6,

        /// <summary>
        /// When this flag is present, an unresolved MSBuild project SDK will fail the build.  This flag is used to
        /// change the "IgnoreMissingEmptyAndInvalidImports" behavior to still fail when an SDK is missing
        /// because those are more fatal.
        /// </summary>
        FailOnUnresolvedSdk = 1 << 7,
    }
}
