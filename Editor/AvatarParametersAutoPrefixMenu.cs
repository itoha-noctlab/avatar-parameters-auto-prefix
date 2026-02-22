#if AKATSUKIYA_VRCSDK3_AVATARS

using UnityEditor;
using UnityEngine;

namespace AKATSUKIYA.AvatarParametersAutoPrefix.Editor
{
    internal static class AvatarParametersAutoPrefixMenu
    {
        private const string MenuPath = "CONTEXT/AvatarParametersAutoPrefix/[DANGER] Apply Destructive";

        [MenuItem(MenuPath)]
        private static void ApplyDestructiveToContext(MenuCommand command)
        {
            if (command.context is not AvatarParametersAutoPrefix target)
            {
                return;
            }

            AvatarParametersAutoPrefixDestructiveApplier.ApplyDestructive(target, true);
        }

        [MenuItem(MenuPath, true)]
        private static bool ValidateApplyDestructiveToContext(MenuCommand command)
        {
            return command.context is AvatarParametersAutoPrefix;
        }
    }
}

#endif
