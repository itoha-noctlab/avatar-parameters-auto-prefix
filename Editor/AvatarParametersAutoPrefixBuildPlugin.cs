#if AKATSUKIYA_VRCSDK3_AVATARS

using nadena.dev.ndmf;
using UnityEngine;

[assembly: ExportsPlugin(typeof(AKATSUKIYA.AvatarParametersAutoPrefix.Editor.AvatarParametersAutoPrefixBuildPlugin))]

namespace AKATSUKIYA.AvatarParametersAutoPrefix.Editor
{
    [RunsOnPlatforms(WellKnownPlatforms.VRChatAvatar30)]
    internal sealed class AvatarParametersAutoPrefixBuildPlugin : Plugin<AvatarParametersAutoPrefixBuildPlugin>
    {
        public override string QualifiedName => "noctlab.com.avatar-parameters-auto-prefix";
        public override string DisplayName => "Avatar Parameters Auto Prefix";

        protected override void Configure()
        {
            InPhase(BuildPhase.Transforming)
                .BeforePlugin("nadena.dev.modular-avatar")
                .Run(ApplyPrefixPass.Instance);
        }
    }

    [RunsOnPlatforms(WellKnownPlatforms.VRChatAvatar30)]
    internal sealed class ApplyPrefixPass : Pass<ApplyPrefixPass>
    {
        protected override void Execute(BuildContext context)
        {
            var avatarRoot = context.AvatarRootTransform;
            if (avatarRoot == null) return;

            foreach (var autoPrefix in avatarRoot.GetComponentsInChildren<AvatarParametersAutoPrefix>(true))
            {
                if (autoPrefix == null) continue;

                var trimmedPrefix = autoPrefix.prefixName?.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(trimmedPrefix))
                {
                    Debug.LogWarning(
                        Localized.UI.BuildIgnoredEmptyPrefix.Format(GetHierarchyPath(autoPrefix.transform)),
                        autoPrefix
                    );
                    continue;
                }

                if (AutoPrefixProcessor.TryGetValidationError(autoPrefix.transform, trimmedPrefix, out var validationError))
                {
                    Debug.LogWarning(
                        Localized.UI.BuildIgnoredValidation.Format(validationError),
                        autoPrefix
                    );
                    continue;
                }

                AutoPrefixProcessor.ApplyUnder(autoPrefix.transform, trimmedPrefix, true, null);
            }
        }

        private static string GetHierarchyPath(Transform transform)
        {
            if (transform == null) return "<null>";

            var path = transform.name;
            var current = transform.parent;
            while (current != null)
            {
                path = current.name + "/" + path;
                current = current.parent;
            }

            return path;
        }
    }
}

#endif
