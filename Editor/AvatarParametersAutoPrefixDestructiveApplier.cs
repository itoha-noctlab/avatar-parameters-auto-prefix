#if AKATSUKIYA_VRCSDK3_AVATARS

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using nadena.dev.modular_avatar.core;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AKATSUKIYA.AvatarParametersAutoPrefix.Editor
{
    public sealed class DestructiveApplyResult
    {
        public IReadOnlyList<string> DuplicatedControllerPaths => _duplicatedControllerPaths;
        public IReadOnlyList<string> DuplicatedClipPaths => _duplicatedClipPaths;

        internal readonly List<string> _duplicatedControllerPaths = new();
        internal readonly List<string> _duplicatedClipPaths = new();

        public bool HasAnyDuplication => _duplicatedControllerPaths.Count > 0 || _duplicatedClipPaths.Count > 0;
    }

    public static class AvatarParametersAutoPrefixDestructiveApplier
    {
        private sealed class DuplicationReport
        {
            public readonly List<string> DuplicatedControllerPaths = new();
            public readonly List<string> DuplicatedClipPaths = new();

            public bool HasAnyDuplication => DuplicatedControllerPaths.Count > 0 || DuplicatedClipPaths.Count > 0;
        }

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

            if (!ValidateNoDuplicationConflicts(component.transform, prefix, component))
            {
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

            if (!DuplicateAnimatorControllers(component.transform, prefix, out var duplicationReport))
            {
                return false;
            }

            var ownerGameObject = component.gameObject;
            var ownerScene = ownerGameObject.scene;

            AutoPrefixProcessor.ApplyUnder(component.transform, prefix, false, null);
            UnityEngine.Object.DestroyImmediate(component);

            EditorUtility.SetDirty(ownerGameObject);
            if (ownerScene.IsValid())
            {
                EditorSceneManager.MarkSceneDirty(ownerScene);
            }

            if (duplicationReport.HasAnyDuplication)
            {
                ShowDuplicationReport(duplicationReport, ownerGameObject);
            }

            return true;
        }

        public static bool ApplyDestructive(
            AvatarParametersAutoPrefix component,
            bool showConfirmationDialog,
            bool showDuplicationReport,
            out DestructiveApplyResult result
        )
        {
            result = null;

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

            if (!ValidateNoDuplicationConflicts(component.transform, prefix, component))
            {
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

            if (!DuplicateAnimatorControllers(component.transform, prefix, out var duplicationReport))
            {
                return false;
            }

            var ownerGameObject = component.gameObject;
            var ownerScene = ownerGameObject.scene;

            AutoPrefixProcessor.ApplyUnder(component.transform, prefix, false, null);
            UnityEngine.Object.DestroyImmediate(component);

            EditorUtility.SetDirty(ownerGameObject);
            if (ownerScene.IsValid())
            {
                EditorSceneManager.MarkSceneDirty(ownerScene);
            }

            result = new DestructiveApplyResult();
            result._duplicatedControllerPaths.AddRange(duplicationReport.DuplicatedControllerPaths);
            result._duplicatedClipPaths.AddRange(duplicationReport.DuplicatedClipPaths);

            if (showDuplicationReport && duplicationReport.HasAnyDuplication)
            {
                ShowDuplicationReport(duplicationReport, ownerGameObject);
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

        private static bool DuplicateAnimatorControllers(Transform root, string prefix, out DuplicationReport duplicationReport)
        {
            duplicationReport = new DuplicationReport();

            if (root == null)
            {
                return true;
            }

            var duplicatedBySource = new Dictionary<RuntimeAnimatorController, RuntimeAnimatorController>();
            var duplicatedClipBySource = new Dictionary<AnimationClip, AnimationClip>();
            var hasCopiedAnyAsset = false;

            foreach (var mergeAnimator in root.GetComponentsInChildren<ModularAvatarMergeAnimator>(true))
            {
                if (mergeAnimator?.animator is not AnimatorController sourceController)
                {
                    continue;
                }

                if (!TryGetOrCreateDuplicatedController(
                        sourceController,
                        prefix,
                        duplicatedBySource,
                        duplicatedClipBySource,
                        duplicationReport,
                        out var duplicatedController
                    ))
                {
                    return false;
                }

                if (duplicatedController == null || ReferenceEquals(mergeAnimator.animator, duplicatedController))
                {
                    continue;
                }

                Undo.RecordObject(mergeAnimator, Localized.UI.DestructiveApplyUndoLabel);
                mergeAnimator.animator = duplicatedController;
                EditorUtility.SetDirty(mergeAnimator);
                hasCopiedAnyAsset = true;
            }

            if (hasCopiedAnyAsset)
            {
                AssetDatabase.SaveAssets();
            }

            return true;
        }

        private static bool ValidateNoDuplicationConflicts(Transform root, string prefix, UnityEngine.Object contextObject)
        {
            if (root == null)
            {
                return true;
            }

            var uniqueControllers = new HashSet<AnimatorController>();
            foreach (var mergeAnimator in root.GetComponentsInChildren<ModularAvatarMergeAnimator>(true))
            {
                if (mergeAnimator?.animator is AnimatorController controller)
                {
                    uniqueControllers.Add(controller);
                }
            }

            var plannedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var controller in uniqueControllers)
            {
                if (!TryBuildControllerDuplicatePath(controller, prefix, out var controllerPath, out var errorMessage))
                {
                    Debug.LogError(errorMessage, contextObject);
                    return false;
                }

                if (!ValidatePlannedPath(
                        controllerPath,
                        plannedPaths,
                        Localized.Message.AssetTypeAnimatorController,
                        controller.name,
                        contextObject
                    ))
                {
                    return false;
                }

                foreach (var clip in controller.animationClips)
                {
                    if (clip == null)
                    {
                        continue;
                    }

                    if (!TryBuildClipDuplicatePath(clip, prefix, out var clipPath, out errorMessage))
                    {
                        Debug.LogError(errorMessage, contextObject);
                        return false;
                    }

                    if (!ValidatePlannedPath(
                            clipPath,
                            plannedPaths,
                            Localized.Message.AssetTypeAnimationClip,
                            clip.name,
                            contextObject
                        ))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool ValidatePlannedPath(
            string path,
            HashSet<string> plannedPaths,
            string assetType,
            string sourceName,
            UnityEngine.Object contextObject
        )
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError(Localized.Message.DuplicationTargetPathInvalid.Format(assetType, sourceName), contextObject);
                return false;
            }

            if (!plannedPaths.Add(path))
            {
                Debug.LogError(Localized.Message.DuplicationTargetPathConflict.Format(assetType, path), contextObject);
                return false;
            }

            if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) != null)
            {
                Debug.LogError(Localized.Message.DuplicationTargetAlreadyExists.Format(assetType, path), contextObject);
                return false;
            }

            return true;
        }

        private static bool TryGetOrCreateDuplicatedController(
            AnimatorController sourceController,
            string prefix,
            Dictionary<RuntimeAnimatorController, RuntimeAnimatorController> duplicatedBySource,
            Dictionary<AnimationClip, AnimationClip> duplicatedClipBySource,
            DuplicationReport duplicationReport,
            out RuntimeAnimatorController duplicatedController
        )
        {
            if (duplicatedBySource.TryGetValue(sourceController, out duplicatedController))
            {
                return true;
            }

            if (!TryBuildControllerDuplicatePath(sourceController, prefix, out var duplicatedPath, out var pathError))
            {
                Debug.LogError(pathError, sourceController);
                duplicatedController = null;
                return false;
            }

            var sourcePath = AssetDatabase.GetAssetPath(sourceController);
            if (!AssetDatabase.CopyAsset(sourcePath, duplicatedPath))
            {
                Debug.LogError(Localized.Message.FailedToDuplicateAnimatorController.Format(sourcePath, duplicatedPath), sourceController);
                duplicatedController = null;
                return false;
            }

            duplicatedController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(duplicatedPath);
            if (duplicatedController == null)
            {
                Debug.LogError(Localized.Message.FailedToLoadDuplicatedAnimatorController.Format(duplicatedPath), sourceController);
                return false;
            }

            if (duplicatedController is not AnimatorController duplicatedAnimatorController)
            {
                Debug.LogError(Localized.Message.DuplicatedControllerTypeInvalid.Format(duplicatedPath), sourceController);
                duplicatedController = null;
                return false;
            }

            if (!DuplicateAnimationClipsInController(duplicatedAnimatorController, prefix, duplicatedClipBySource, duplicationReport))
            {
                duplicatedController = null;
                return false;
            }

            duplicatedBySource[sourceController] = duplicatedController;
            duplicationReport.DuplicatedControllerPaths.Add(duplicatedPath);
            return true;
        }

        private static void RemapStateMachineClips(
            AnimatorStateMachine stateMachine,
            Dictionary<AnimationClip, AnimationClip> duplicatedClipBySource
        )
        {
            if (stateMachine == null)
            {
                return;
            }

            var states = stateMachine.states;
            for (var i = 0; i < states.Length; i++)
            {
                var state = states[i].state;
                if (state == null)
                {
                    continue;
                }

                state.motion = RemapMotionClips(state.motion, duplicatedClipBySource);
            }

            foreach (var childStateMachine in stateMachine.stateMachines)
            {
                RemapStateMachineClips(childStateMachine.stateMachine, duplicatedClipBySource);
            }
        }

        private static Motion RemapMotionClips(Motion motion, Dictionary<AnimationClip, AnimationClip> duplicatedClipBySource)
        {
            if (motion == null)
            {
                return null;
            }

            if (motion is AnimationClip sourceClip)
            {
                if (duplicatedClipBySource.TryGetValue(sourceClip, out var duplicatedClip))
                {
                    return duplicatedClip;
                }

                return sourceClip;
            }

            if (motion is BlendTree blendTree)
            {
                var children = blendTree.children;
                var changed = false;

                for (var i = 0; i < children.Length; i++)
                {
                    var remappedMotion = RemapMotionClips(children[i].motion, duplicatedClipBySource);
                    if (ReferenceEquals(children[i].motion, remappedMotion))
                    {
                        continue;
                    }

                    var child = children[i];
                    child.motion = remappedMotion;
                    children[i] = child;
                    changed = true;
                }

                if (changed)
                {
                    blendTree.children = children;
                    EditorUtility.SetDirty(blendTree);
                }
            }

            return motion;
        }

        private static bool DuplicateAnimationClipsInController(
            AnimatorController controller,
            string prefix,
            Dictionary<AnimationClip, AnimationClip> duplicatedClipBySource,
            DuplicationReport duplicationReport
        )
        {
            if (controller == null)
            {
                return true;
            }

            foreach (var sourceClip in controller.animationClips)
            {
                if (sourceClip == null)
                {
                    continue;
                }

                if (!TryGetOrCreateDuplicatedClip(sourceClip, prefix, duplicatedClipBySource, duplicationReport, out _))
                {
                    return false;
                }
            }

            foreach (var layer in controller.layers)
            {
                RemapStateMachineClips(layer.stateMachine, duplicatedClipBySource);
            }

            ReplaceClipReferencesInControllerSerialized(controller, duplicatedClipBySource);

            EditorUtility.SetDirty(controller);
            return true;
        }

        private static void ReplaceClipReferencesInControllerSerialized(
            AnimatorController controller,
            Dictionary<AnimationClip, AnimationClip> duplicatedClipBySource
        )
        {
            if (controller == null || duplicatedClipBySource.Count == 0)
            {
                return;
            }

            var controllerPath = AssetDatabase.GetAssetPath(controller);
            if (string.IsNullOrEmpty(controllerPath))
            {
                return;
            }

            var assets = AssetDatabase.LoadAllAssetsAtPath(controllerPath);
            foreach (var asset in assets)
            {
                if (asset == null || asset is AnimationClip)
                {
                    continue;
                }

                var serializedObject = new SerializedObject(asset);
                var property = serializedObject.GetIterator();
                var changed = false;
                var enterChildren = true;

                while (property.NextVisible(enterChildren))
                {
                    enterChildren = false;
                    if (property.propertyType != SerializedPropertyType.ObjectReference)
                    {
                        continue;
                    }

                    if (property.objectReferenceValue is not AnimationClip sourceClip)
                    {
                        continue;
                    }

                    if (!duplicatedClipBySource.TryGetValue(sourceClip, out var duplicatedClip))
                    {
                        continue;
                    }

                    if (ReferenceEquals(sourceClip, duplicatedClip))
                    {
                        continue;
                    }

                    property.objectReferenceValue = duplicatedClip;
                    changed = true;
                }

                if (!changed)
                {
                    continue;
                }

                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(asset);
            }
        }

        private static bool TryGetOrCreateDuplicatedClip(
            AnimationClip sourceClip,
            string prefix,
            Dictionary<AnimationClip, AnimationClip> duplicatedClipBySource,
            DuplicationReport duplicationReport,
            out AnimationClip duplicatedClip
        )
        {
            if (duplicatedClipBySource.TryGetValue(sourceClip, out duplicatedClip))
            {
                return true;
            }

            if (!TryBuildClipDuplicatePath(sourceClip, prefix, out var duplicatedPath, out var pathError))
            {
                Debug.LogError(pathError, sourceClip);
                duplicatedClip = null;
                return false;
            }

            var clipCopy = UnityEngine.Object.Instantiate(sourceClip);
            if (clipCopy == null)
            {
                Debug.LogError(Localized.Message.FailedToInstantiateAnimationClip.Format(sourceClip.name), sourceClip);
                duplicatedClip = null;
                return false;
            }

            clipCopy.name = Path.GetFileNameWithoutExtension(duplicatedPath);
            AssetDatabase.CreateAsset(clipCopy, duplicatedPath);

            duplicatedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(duplicatedPath);
            if (duplicatedClip == null)
            {
                Debug.LogError(Localized.Message.FailedToLoadDuplicatedAnimationClip.Format(duplicatedPath), sourceClip);
                return false;
            }

            duplicatedClipBySource[sourceClip] = duplicatedClip;
            duplicationReport.DuplicatedClipPaths.Add(duplicatedPath);
            return true;
        }

        private static bool TryBuildControllerDuplicatePath(
            AnimatorController sourceController,
            string prefix,
            out string duplicatedPath,
            out string errorMessage
        )
        {
            duplicatedPath = null;
            errorMessage = null;

            var sourcePath = AssetDatabase.GetAssetPath(sourceController);
            if (string.IsNullOrEmpty(sourcePath))
            {
                errorMessage = Localized.Message.AnimatorControllerAssetPathInvalid.Format(sourceController.name);
                return false;
            }

            var directoryPath = Path.GetDirectoryName(sourcePath);
            if (string.IsNullOrEmpty(directoryPath))
            {
                errorMessage = Localized.Message.AnimatorControllerDirectoryPathInvalid.Format(sourcePath);
                return false;
            }

            var extension = Path.GetExtension(sourcePath);
            if (string.IsNullOrEmpty(extension))
            {
                extension = ".controller";
            }

            var duplicatedName = SanitizeAssetName(prefix + sourceController.name);
            if (string.IsNullOrEmpty(duplicatedName))
            {
                duplicatedName = sourceController.name;
            }

            duplicatedPath = Path.Combine(directoryPath, duplicatedName + extension).Replace("\\", "/");
            return true;
        }

        private static bool TryBuildClipDuplicatePath(
            AnimationClip sourceClip,
            string prefix,
            out string duplicatedPath,
            out string errorMessage
        )
        {
            duplicatedPath = null;
            errorMessage = null;

            var sourcePath = AssetDatabase.GetAssetPath(sourceClip);
            if (string.IsNullOrEmpty(sourcePath))
            {
                errorMessage = Localized.Message.AnimationClipAssetPathInvalid.Format(sourceClip.name);
                return false;
            }

            var directoryPath = Path.GetDirectoryName(sourcePath);
            if (string.IsNullOrEmpty(directoryPath))
            {
                errorMessage = Localized.Message.AnimationClipDirectoryPathInvalid.Format(sourcePath);
                return false;
            }

            var duplicatedName = SanitizeAssetName(prefix + sourceClip.name);
            if (string.IsNullOrEmpty(duplicatedName))
            {
                duplicatedName = sourceClip.name;
            }

            duplicatedPath = Path.Combine(directoryPath, duplicatedName + ".anim").Replace("\\", "/");
            return true;
        }

        private static void ShowDuplicationReport(DuplicationReport report, UnityEngine.Object contextObject)
        {
            var builder = new StringBuilder();
            builder.AppendLine(Localized.UI.DestructiveApplyReportHeader);

            if (report.DuplicatedControllerPaths.Count > 0)
            {
                builder.AppendLine(Localized.UI.DestructiveApplyReportAnimatorControllers);
                foreach (var path in report.DuplicatedControllerPaths)
                {
                    builder.Append(" - ").AppendLine(path);
                }
            }

            if (report.DuplicatedClipPaths.Count > 0)
            {
                builder.AppendLine(Localized.UI.DestructiveApplyReportAnimationClips);
                foreach (var path in report.DuplicatedClipPaths)
                {
                    builder.Append(" - ").AppendLine(path);
                }
            }

            var message = builder.ToString().TrimEnd();
            Debug.Log(message, contextObject);
            EditorUtility.DisplayDialog(Localized.UI.DestructiveApplyReportDialogTitle, message, Localized.UI.Ok);
        }

        private static string SanitizeAssetName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var invalidChars = Path.GetInvalidFileNameChars();
            var chars = value.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                if (Array.IndexOf(invalidChars, chars[i]) >= 0)
                {
                    chars[i] = '_';
                }
            }

            return new string(chars).Trim();
        }
    }
}

#endif
