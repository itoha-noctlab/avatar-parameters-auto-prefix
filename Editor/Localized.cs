using AKATSUKIYA.AvatarParametersAutoPrefix.Editor.Localization;

namespace AKATSUKIYA.AvatarParametersAutoPrefix.Editor
{
    
    internal static class Localized
    {
        public static class UI
        {
            public static readonly LocalizedString PrefixNameRequired = new(
                en: "PrefixName is required. If empty, this component is ignored during build.",
                ja: "PrefixName は必須です。空欄の場合、ビルド時にこのコンポーネントは無視されます。",
                ko: "PrefixName은 필수입니다. 비어 있으면 빌드 시 이 컴포넌트는 무시됩니다.",
                zh: "PrefixName 为必填项。留空时，此组件在构建时会被忽略。"
            );

            public static readonly LocalizedString RenameConflictInInspector = new(
                en: "Name conflicts prevent renaming with this configuration. This component is ignored during build.\n{0}",
                ja: "名称の衝突があるため、この設定ではリネームできません。ビルド時にこのコンポーネントは無視されます。\n{0}",
                ko: "이 설정에서는 이름 충돌로 인해 리네임할 수 없습니다. 빌드 시 이 컴포넌트는 무시됩니다.\n{0}",
                zh: "由于名称冲突，无法使用当前设置重命名。此组件在构建时会被忽略。\n{0}"
            );

            public static readonly LocalizedString RenameSummaryHeader = new(
                en: "During build, AvatarParameters and CollisionTags under this component will be renamed non-destructively as follows.",
                ja: "ビルド時、このコンポーネント配下の AvatarParameter と CollisionTag が非破壊で一括変更されます。",
                ko: "빌드 시 이 컴포넌트 하위의 AvatarParameter 및 CollisionTag가 비파괴 방식으로 일괄 변경됩니다.",
                zh: "构建时，此组件下的 AvatarParameter 和 CollisionTag 将按如下方式以非破坏方式批量重命名。"
            );

            public static readonly LocalizedString AvatarParametersSection = new(
                en: "(AvatarParameters)",
                ja: "(AvatarParameters)",
                ko: "(AvatarParameters)",
                zh: "(AvatarParameters)"
            );

            public static readonly LocalizedString CollisionTagsSection = new(
                en: "(CollisionTags)",
                ja: "(CollisionTags)",
                ko: "(CollisionTags)",
                zh: "(CollisionTags)"
            );

            public static readonly LocalizedString NoTargets = new(
                en: "- (none)",
                ja: "- (対象なし)",
                ko: "- (대상 없음)",
                zh: "- (无目标)"
            );

            public static readonly LocalizedString BuildIgnoredEmptyPrefix = new(
                en: "'{0}' has an empty prefixName. This component is ignored.",
                ja: "'{0}' の prefixName が空欄のため、このコンポーネントを無視します。",
                ko: "'{0}'의 prefixName이 비어 있어 이 컴포넌트를 무시합니다.",
                zh: "'{0}' 的 prefixName 为空，因此将忽略此组件。"
            );

            public static readonly LocalizedString BuildIgnoredValidation = new(
                en: "{0} This component is ignored.",
                ja: "{0} このコンポーネントを無視します。",
                ko: "{0} 이 컴포넌트를 무시합니다.",
                zh: "{0} 将忽略此组件。"
            );

            public static readonly LocalizedString DestructiveApplyDialogTitle = new(
                en: "Avatar Parameters Auto Prefix",
                ja: "Avatar Parameters Auto Prefix",
                ko: "Avatar Parameters Auto Prefix",
                zh: "Avatar Parameters Auto Prefix"
            );

            public static readonly LocalizedString DestructiveApplyDialogMessage = new(
                en: "This will apply prefixName destructively to all parameters under this component.\nAfter applying, this component will be removed.\nThis cannot be undone. Please make sure to back up beforehand.\nDo you want to continue?",
                ja: "prefixName をコンポーネント配下のすべてのパラメータに対して破壊的に適用します。\n適用後、このコンポーネントは削除されます。\n適用後は元に戻せません。必ず事前にバックアップを取ってください。\n続行しますか？",
                ko: "prefixName을 컴포넌트 하위의 모든 파라미터에 대해 파괴적으로 적용합니다.\n적용 후 이 컴포넌트는 제거됩니다.\n적용 후에는 되돌릴 수 없습니다. 반드시 사전에 백업을 해주세요.\n계속하시겠습니까?",
                zh: "这将把 prefixName 破坏性地应用于此组件下的所有参数。\n应用后，此组件将被删除。\n应用后无法撤销。请务必事先备份。\n您要继续吗？"
            );

            public static readonly LocalizedString DestructiveApplyDialogOk = new(
                en: "Continue",
                ja: "続行",
                ko: "계속",
                zh: "继续"
            );

            public static readonly LocalizedString DestructiveApplyDialogCancel = new(
                en: "Cancel",
                ja: "キャンセル",
                ko: "취소",
                zh: "取消"
            );

        }

        public static class Message
        {
            public static readonly LocalizedString ParameterNameConflict = new(
                en: "'{0}' already exists as a parameter name.",
                ja: "'{0}' は既に存在するパラメータ名です。",
                ko: "'{0}' 는 이미 존재하는 파라미터 이름입니다.",
                zh: "'{0}' 已是已存在的参数名称。"
            );

            public static readonly LocalizedString CollisionTagNameConflict = new(
                en: "'{0}' already exists as a CollisionTag name.",
                ja: "'{0}' は既に存在するCollisionTag名です。",
                ko: "'{0}' 는 이미 존재하는 CollisionTag 이름입니다.",
                zh: "'{0}' 已是已存在的 CollisionTag 名称。"
            );

            public static readonly LocalizedString DestructiveApplyIgnoredEmptyPrefix = new(
                en: "Destructive apply was canceled because prefixName is empty.",
                ja: "prefixName が空欄のため、破壊的適用を中止しました。",
                ko: "prefixName이 비어 있어 파괴적 적용을 중단했습니다.",
                zh: "由于 prefixName 为空，已取消破坏性应用。"
            );
        }
    }
}