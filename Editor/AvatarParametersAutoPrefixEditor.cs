#if AKATSUKIYA_VRCSDK3_AVATARS

using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AKATSUKIYA.AvatarParametersAutoPrefix.Editor
{
    [CustomEditor(typeof(AvatarParametersAutoPrefix))]
    internal sealed class AvatarParametersAutoPrefixEditor : UnityEditor.Editor
    {
        private static readonly Color ErrorBorderColor = new(0.85f, 0.2f, 0.2f, 1f);
        private SerializedProperty _prefixName;

        private void OnEnable()
        {
            _prefixName = serializedObject.FindProperty("prefixName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPrefixNameField(_prefixName);
            TrimPrefixName(_prefixName);

            var hasPrefixError = HasPrefixNameError(_prefixName);
            var hasValidationError = false;
            if (hasPrefixError)
            {
                EditorGUILayout.HelpBox(Localized.UI.PrefixNameRequired, MessageType.Error);
            }
            else if (target is AvatarParametersAutoPrefix component)
            {
                var prefix = _prefixName?.stringValue ?? string.Empty;
                hasValidationError = AutoPrefixProcessor.TryGetValidationError(component.transform, prefix, out var validationError);
                if (hasValidationError)
                {
                    EditorGUILayout.HelpBox(Localized.UI.RenameConflictInInspector.Format(validationError), MessageType.Error);
                }
            }

            if (!hasPrefixError && !hasValidationError)
            {
                DrawRenameAllInformationBox();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawRenameAllInformationBox()
        {
            var component = target as AvatarParametersAutoPrefix;
            if (component == null)
            {
                return;
            }

            var prefix = _prefixName?.stringValue ?? string.Empty;
            var candidates = AutoPrefixProcessor.CollectRenameCandidates(component.transform, prefix);

            var sb = new StringBuilder();
            sb.AppendLine(Localized.UI.RenameSummaryHeader);
            sb.AppendLine(Localized.UI.AvatarParametersSection);

            if (candidates.AvatarParameters.Count == 0)
            {
                sb.AppendLine(Localized.UI.NoTargets);
            }
            else
            {
                foreach (var name in candidates.AvatarParameters)
                {
                    sb.AppendLine($"- {name} -> {prefix}{name}");
                }
            }

            sb.AppendLine(Localized.UI.CollisionTagsSection);
            if (candidates.CollisionTags.Count == 0)
            {
                sb.AppendLine(Localized.UI.NoTargets);
            }
            else
            {
                foreach (var tag in candidates.CollisionTags)
                {
                    sb.AppendLine($"- {tag} -> {prefix}{tag}");
                }
            }

            EditorGUILayout.HelpBox(sb.ToString().TrimEnd(), MessageType.Info);
        }

        private static void DrawPrefixNameField(SerializedProperty prefixName)
        {
            if (prefixName == null) return;

            var rect = EditorGUILayout.GetControlRect();
            EditorGUI.PropertyField(rect, prefixName);

            if (!string.IsNullOrWhiteSpace(prefixName.stringValue)) return;

            DrawBorder(rect, ErrorBorderColor, 1f);
        }

        private static void DrawBorder(Rect rect, Color color, float thickness)
        {
            var bottom = new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness);
            EditorGUI.DrawRect(bottom, color);
        }

        private static void TrimPrefixName(SerializedProperty prefixName)
        {
            if (prefixName == null) return;

            var value = prefixName.stringValue;
            if (value == null) return;

            var trimmed = value.Trim();
            if (string.Equals(value, trimmed, System.StringComparison.Ordinal)) return;

            prefixName.stringValue = trimmed;
        }

        private static bool HasPrefixNameError(SerializedProperty prefixName)
        {
            if (prefixName == null) return true;
            return string.IsNullOrWhiteSpace(prefixName.stringValue);
        }
    }
}

#endif
