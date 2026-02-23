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

            public static readonly LocalizedString DestructiveApplyUndoLabel = new(
                en: "Apply Avatar Parameters Auto Prefix (Destructive)",
                ja: "Avatar Parameters Auto Prefix を破壊的適用",
                ko: "Avatar Parameters Auto Prefix 파괴적 적용",
                zh: "破坏性应用 Avatar Parameters Auto Prefix"
            );

            public static readonly LocalizedString DestructiveApplyReportDialogTitle = new(
                en: "Avatar Parameters Auto Prefix",
                ja: "Avatar Parameters Auto Prefix",
                ko: "Avatar Parameters Auto Prefix",
                zh: "Avatar Parameters Auto Prefix"
            );

            public static readonly LocalizedString DestructiveApplyReportHeader = new(
                en: "Destructive apply is completed. The following assets have been duplicated:",
                ja: "破壊的適用が完了し、以下のアセットが複製されました。:",
                ko: "파괴적 적용이 완료되었으며, 다음 에셋이 복제되었습니다.:",
                zh: "破坏性应用已完成，以下资产已被复制。:"
            );

            public static readonly LocalizedString DestructiveApplyReportAnimatorControllers = new(
                en: "Animator Controllers:",
                ja: "Animator Controller:",
                ko: "Animator Controller:",
                zh: "Animator Controller:"
            );

            public static readonly LocalizedString DestructiveApplyReportAnimationClips = new(
                en: "Animation Clips:",
                ja: "Animation Clip:",
                ko: "Animation Clip:",
                zh: "Animation Clip:"
            );

            public static readonly LocalizedString Ok = new(
                en: "OK",
                ja: "OK",
                ko: "확인",
                zh: "确定"
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

            public static readonly LocalizedString DuplicationTargetPathInvalid = new(
                en: "Target path is invalid for {0}: {1}",
                ja: "{0} の出力先パスが不正です: {1}",
                ko: "{0}의 대상 경로가 잘못되었습니다: {1}",
                zh: "{0} 的目标路径无效: {1}"
            );

            public static readonly LocalizedString DuplicationTargetPathConflict = new(
                en: "Duplicate target {0} path detected: {1}",
                ja: "{0} の出力先パスが重複しています: {1}",
                ko: "{0} 대상 경로가 중복되었습니다: {1}",
                zh: "检测到重复的 {0} 目标路径: {1}"
            );

            public static readonly LocalizedString DuplicationTargetAlreadyExists = new(
                en: "Target {0} already exists: {1}",
                ja: "出力先に {0} が既に存在します: {1}",
                ko: "대상 {0}이(가) 이미 존재합니다: {1}",
                zh: "目标 {0} 已存在: {1}"
            );

            public static readonly LocalizedString AssetTypeAnimatorController = new(
                en: "AnimatorController",
                ja: "AnimatorController",
                ko: "AnimatorController",
                zh: "AnimatorController"
            );

            public static readonly LocalizedString AssetTypeAnimationClip = new(
                en: "AnimationClip",
                ja: "AnimationClip",
                ko: "AnimationClip",
                zh: "AnimationClip"
            );

            public static readonly LocalizedString AnimatorControllerAssetPathInvalid = new(
                en: "AnimatorController asset path is invalid: {0}",
                ja: "AnimatorController のアセットパスが不正です: {0}",
                ko: "AnimatorController 에셋 경로가 잘못되었습니다: {0}",
                zh: "AnimatorController 资源路径无效: {0}"
            );

            public static readonly LocalizedString AnimatorControllerDirectoryPathInvalid = new(
                en: "AnimatorController directory path is invalid: {0}",
                ja: "AnimatorController のディレクトリパスが不正です: {0}",
                ko: "AnimatorController 디렉터리 경로가 잘못되었습니다: {0}",
                zh: "AnimatorController 目录路径无效: {0}"
            );

            public static readonly LocalizedString FailedToDuplicateAnimatorController = new(
                en: "Failed to duplicate AnimatorController: {0} -> {1}",
                ja: "AnimatorController の複製に失敗しました: {0} -> {1}",
                ko: "AnimatorController 복제에 실패했습니다: {0} -> {1}",
                zh: "复制 AnimatorController 失败: {0} -> {1}"
            );

            public static readonly LocalizedString FailedToLoadDuplicatedAnimatorController = new(
                en: "Failed to load duplicated AnimatorController: {0}",
                ja: "複製された AnimatorController の読み込みに失敗しました: {0}",
                ko: "복제된 AnimatorController 로드에 실패했습니다: {0}",
                zh: "加载复制后的 AnimatorController 失败: {0}"
            );

            public static readonly LocalizedString DuplicatedControllerTypeInvalid = new(
                en: "Duplicated controller is not AnimatorController: {0}",
                ja: "複製されたコントローラーが AnimatorController ではありません: {0}",
                ko: "복제된 컨트롤러가 AnimatorController가 아닙니다: {0}",
                zh: "复制后的控制器不是 AnimatorController: {0}"
            );

            public static readonly LocalizedString AnimationClipAssetPathInvalid = new(
                en: "AnimationClip asset path is invalid: {0}",
                ja: "AnimationClip のアセットパスが不正です: {0}",
                ko: "AnimationClip 에셋 경로가 잘못되었습니다: {0}",
                zh: "AnimationClip 资源路径无效: {0}"
            );

            public static readonly LocalizedString AnimationClipDirectoryPathInvalid = new(
                en: "AnimationClip directory path is invalid: {0}",
                ja: "AnimationClip のディレクトリパスが不正です: {0}",
                ko: "AnimationClip 디렉터리 경로가 잘못되었습니다: {0}",
                zh: "AnimationClip 目录路径无效: {0}"
            );

            public static readonly LocalizedString FailedToInstantiateAnimationClip = new(
                en: "Failed to instantiate AnimationClip: {0}",
                ja: "AnimationClip の生成に失敗しました: {0}",
                ko: "AnimationClip 인스턴스 생성에 실패했습니다: {0}",
                zh: "实例化 AnimationClip 失败: {0}"
            );

            public static readonly LocalizedString FailedToLoadDuplicatedAnimationClip = new(
                en: "Failed to load duplicated AnimationClip: {0}",
                ja: "複製された AnimationClip の読み込みに失敗しました: {0}",
                ko: "복제된 AnimationClip 로드에 실패했습니다: {0}",
                zh: "加载复制后的 AnimationClip 失败: {0}"
            );
        }
    }
}