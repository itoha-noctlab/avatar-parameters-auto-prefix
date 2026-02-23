# Avatar Parameters Auto Prefix

![image1](https://raw.githubusercontent.com/itoha-noctlab/avatar-parameters-auto-prefix/refs/heads/main/img/image1.zh.png)

## 【概要】

- 该工具会在 VRChat 头像构建时，以非破坏方式自动将 `prefixName` 添加到参数名称前。
- 当一个头像中放置多个相同机关，或多个机关的参数名称发生冲突时，可用于避免参数名冲突。
- 以 VPM 包形式发布。可在 VCC 中添加 `https://vpm.noctlab.com/vpm.json` 进行安装。

## 【支持语言 / Supported Languages】

- [ja (日本語)](https://noctlab.com/docs/avatar-parameters-auto-prefix/ja)
- [en (English)](https://noctlab.com/docs/avatar-parameters-auto-prefix/en)
- [ko (한국어)](https://noctlab.com/docs/avatar-parameters-auto-prefix/ko)
- [zh (简体中文)](https://noctlab.com/docs/avatar-parameters-auto-prefix/zh)

## 【规格】

- 以 Modular Avatar 为前提。
- 处理时机为 build-time。
- 因为是非破坏处理，移除此组件后可恢复到引入前状态。
- 不会修改 VRChat 默认参数名（例如 `GestureLeft`、`GestureRight`、`IsLocal`）。

## 【必需依赖】

- `com.vrchat.avatars` `>=3.5.0`
- `nadena.dev.modular-avatar` `>=1.11.0`

## 【使用方法】

1. 在希望重命名参数的头像机关根对象上添加 `Avatar Parameters Auto Prefix` 组件，并设置 `prefixName`。
2. 按平常方式构建头像。

## 【功能细节】

以下名称会转换为 `prefixName + 原名称`。

- `ModularAvatarParameters` 中的参数名
- `ModularAvatarMergeAnimator` 上设置的 `AnimatorController`
    - `AnimatorController.parameters` 中的参数名
    - `VRCAvatarParameterDriver` 的 `parameters[].name`
    - `VRCAnimatorPlayAudio` 的 `parameter`
    - `AnimatorState` 过渡条件中使用的参数名
    - `BlendTree` 内部使用的参数名
- `VRCPhysBone.parameter` 及其派生参数（`_IsGrabbed`, `_IsPosed`, `_Angle`, `_Stretch`, `_Squish`）
- `VRCContactReceiver` 中设置的 `parameter`
- `VRCContactSender` / `VRCContactReceiver` 的 `collisionTags`

冲突验证

- 若重命名后的名称与头像范围内已有名称冲突，将显示错误。

非破坏处理

- 构建处理在运行时复制的资源上执行，不会直接改写原始资源。

也可以在任意时机执行破坏性应用，将修改直接写入原始资源（**务必先备份**）。

## 【排除参数/标签】

以下头像参数不会被重命名。

- `IsLocal`
- `PreviewMode`
- `Viseme`
- `Voice`
- `GestureLeft`
- `GestureRight`
- `GestureLeftWeight`
- `GestureRightWeight`
- `AngularY`
- `VelocityX`
- `VelocityY`
- `VelocityZ`
- `VelocityMagnitude`
- `Upright`
- `Grounded`
- `Seated`
- `AFK`
- `TrackingType`
- `VRMode`
- `MuteSelf`
- `InStation`
- `Earmuffs`
- `IsOnFriendsList`
- `AvatarVersion`
- `IsAnimatorEnabled`
- `ScaleModified`
- `ScaleFactor`
- `ScaleFactorInverse`
- `EyeHeightAsMeters`
- `EyeHeightAsPercent`

以下 CollisionTag（内置标签）不会被重命名。

- `Head`
- `Torso`
- `Hand`
- `HandL`
- `HandR`
- `Foot`
- `FootL`
- `FootR`
- `Finger`
- `FingerL`
- `FingerR`
- `FingerIndex`
- `FingerMiddle`
- `FingerRing`
- `FingerLittle`
- `FingerIndexL`
- `FingerMiddleL`
- `FingerRingL`
- `FingerLittleL`
- `FingerIndexR`
- `FingerMiddleR`
- `FingerRingR`
- `FingerLittleR`
- `HandLeft`
- `HandRight`
- `FingerIndexLeft`
- `FingerIndexRight`
- `FootLeft`
- `FootRight`
- `Hot`
- `Cold`
- `Fire`
- `Freezer`
- `Wet`
- `Water`
- `Wind`
- `Weapon`
- `Shield`
- `Damage`
- `DamageBlunt`
- `DamageSharp`
- `Ammunition`
- `Projectile`
- `Consumable`
- `ConsumableFood`
- `ConsumableDrink`
- `Brush`
- `Dye`

## 【破坏性应用】

- 在组件右键菜单中选择 `[DANGER] Apply Destructive`，可将上述重命名结果直接写入原始资源。
- 应用后无法撤销，请务必提前备份。

## 【许可证】

- 本包基于 MIT License 发布。详情请参阅 [LICENSE](https://github.com/itoha-noctlab/avatar-parameters-auto-prefix/blob/main/LICENSE.txt)。

## 【联系方式】

- 如有问题，请通过 BOOTH 消息或 https://noctlab.com/contact?t=tool&tool=Avatar+Parameters+Auto+Prefix 联系。
