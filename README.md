# Avatar Parameters Auto Prefix

![image1](https://raw.githubusercontent.com/itoha-noctlab/avatar-parameters-auto-prefix/refs/heads/main/img/image1.en.png)

## 【Overview】

- This tool automatically and non-destructively prepends `prefixName` to parameter names during VRChat avatar build.
- It helps avoid parameter name collisions, for example when placing the same gimmick multiple times in one avatar or when multiple gimmicks use overlapping parameter names.
- It is distributed as a VPM package. Add `https://vpm.noctlab.com/vpm.json` to VCC to install it.

## 【Supported Languages】

- [ja (日本語)](https://noctlab.com/avatar-parameters-auto-prefix/ja)
- [en (English)](https://noctlab.com/avatar-parameters-auto-prefix/en)
- [ko (한국어)](https://noctlab.com/avatar-parameters-auto-prefix/ko)
- [zh (简体中文)](https://noctlab.com/avatar-parameters-auto-prefix/zh)

## 【Specifications】

- Designed for use with Modular Avatar.
- Processing timing is build-time.
- Because processing is non-destructive, removing this component restores behavior to the pre-introduction state.
- VRChat default parameter names (for example `GestureLeft`, `GestureRight`, `IsLocal`) are never renamed.

## 【Required Dependencies】

- `com.vrchat.avatars` `>=3.5.0`
- `nadena.dev.modular-avatar` `>=1.11.0`

## 【Usage】

1. Add the `Avatar Parameters Auto Prefix` component to the root object of the avatar gimmick you want to rename, then set `prefixName`.
2. Build the avatar as usual.

## 【Details】

The following names are converted to `prefixName + originalName`.

- Parameter names in `ModularAvatarParameters`
- `AnimatorController` assigned to `ModularAvatarMergeAnimator`
- Parameter names in `AnimatorController.parameters`
- `parameters[].name` in `VRCAvatarParameterDriver`
- `parameter` in `VRCAnimatorPlayAudio`
- Parameter names used in `AnimatorState` transition conditions
- Parameter names used inside `BlendTree`
- `VRCPhysBone.parameter` and derived parameters (`_IsGrabbed`, `_IsPosed`, `_Angle`, `_Stretch`, `_Squish`)
- `parameter` configured in `VRCContactReceiver`
- `collisionTags` in `VRCContactSender` / `VRCContactReceiver`

Collision validation

- If a renamed result conflicts with an existing name in the avatar scope, the tool reports an error.

Non-destructive processing

- Build processing runs on runtime-cloned assets and does not modify original assets directly.

You can also apply the rename destructively to original assets at any time (**always back up first**).

## 【Excluded Parameters and Tags】

The following avatar parameters are never renamed.

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

The following CollisionTags (built-in tags) are never renamed.

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

## 【Destructive Apply】

- From the component context menu, choose `[DANGER] Apply Destructive` to write the same rename results directly into original assets.
- This operation cannot be undone. Always make a backup beforehand.

## 【License】

- This package is provided under the MIT License. See [LICENSE](https://github.com/itoha-noctlab/avatar-parameters-auto-prefix/blob/main/LICENSE.txt) for details.

## 【Contact】

- For bug reports or questions, please contact via BOOTH message or https://noctlab.com/contact?t=tool&tool=Avatar+Parameters+Auto+Prefix.
