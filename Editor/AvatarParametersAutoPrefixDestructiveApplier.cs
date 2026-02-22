#if AKATSUKIYA_VRCSDK3_AVATARS

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AKATSUKIYA.AvatarParametersAutoPrefix.Editor
{
    public static class AvatarParametersAutoPrefixDestructiveApplier
    {
        public static bool ApplyDestructive(
            AvatarParametersAutoPrefix component,
            bool showConfirmationDialog = true
        )
        {
            if (component == null)
            {
                return false;
            }

            var prefix = component.prefixName?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(prefix))
            {
                Debug.LogWarning(Localized.Message.DestructiveApplyIgnoredEmptyPrefix, component);
                return false;
            }

            if (AutoPrefixProcessor.TryGetValidationError(component.transform, prefix, out var validationError))
            {
                Debug.LogError(validationError, component);
                return false;
            }

            if (showConfirmationDialog)
            {
                var accepted = EditorUtility.DisplayDialog(
                    Localized.UI.DestructiveApplyDialogTitle,
                    Localized.UI.DestructiveApplyDialogMessage,
                    Localized.UI.DestructiveApplyDialogOk,
                    Localized.UI.DestructiveApplyDialogCancel
                );
                if (!accepted)
                {
                    return false;
                }
            }

            AutoPrefixProcessor.ApplyUnder(component.transform, prefix, false, null);
            Object.DestroyImmediate(component);

            EditorUtility.SetDirty(component.gameObject);
            if (component.gameObject.scene.IsValid())
            {
                EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
            }

            return true;
        }

        public static bool ApplyDestructiveFromScript(
            this AvatarParametersAutoPrefix component,
            bool showConfirmationDialog = true
        )
        {
            return ApplyDestructive(component, showConfirmationDialog);
        }
    }
}

#endif
