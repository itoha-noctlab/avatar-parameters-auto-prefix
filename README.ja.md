# Avatar Parameters Auto Prefix

![image1](https://raw.githubusercontent.com/itoha-noctlab/avatar-parameters-auto-prefix/refs/heads/main/img/image1.ja.png)

## 【概要】

- VRChatアバーターのビルド時に、パラメータ名に指定した`prefixName`を**非破壊**で自動付与するツールです。
- 同じギミックを1アバター内に複数配置したり、ギミック同士で競合する名称のパラメータがある場合などに、パラメータ名の衝突を回避するために利用できます。
- VPM package として公開しています。VCC で `https://vpm.noctlab.com/vpm.json` を追加することで導入できます。

## 【対応言語 / Supported Languages】

- [ja (日本語)](https://noctlab.com/docs/avatar-parameters-auto-prefix/ja)
- [en (English)](https://noctlab.com/docs/avatar-parameters-auto-prefix/en)
- [ko (한국어)](https://noctlab.com/docs/avatar-parameters-auto-prefix/ko)
- [zh (简体中文)](https://noctlab.com/docs/avatar-parameters-auto-prefix/zh)

## 【仕様】

- Modular Avatar を前提とします。
- 処理タイミングは build-time です。
- 非破壊のためコンポーネントを外すことで導入前の状態に戻せます。
- VRChat標準パラメータ名（例: `GestureLeft`, `GestureRight`, `IsLocal`...）は変更しません。

## 【必須依存】

- `com.vrchat.avatars` `>=3.5.0`
- `nadena.dev.modular-avatar` `>=1.11.0`

## 【使用方法】

1. パラメータ名を変更したいアバターギミックのルート階層に `Avatar Parameters Auto Prefix` コンポーネントを追加し `prefixName` を設定します。
2. 通常どおりアバターをビルドします。

## 【機能詳細】

以下の名称を対象に、`prefixName + 元の名称` へ変換します。

- `ModularAvatarParameters` に含まれるパラメータ名
- `ModularAvatarMergeAnimator` に設定されている `AnimatorController`
- `AnimatorController.parameters` に設定されているパラメータ名
- `VRCAvatarParameterDriver` の `parameters[].name`
- `VRCAnimatorPlayAudio` の `parameter`
- `AnimatorState` の遷移条件で使用されているパラメータ名
    - `BlendTree` 内で使用されているパラメータ名
- `VRCPhysBone.parameter` とその派生パラメータ（`_IsGrabbed`, `_IsPosed`, `_Angle`, `_Stretch`, `_Squish`）
- `VRCContactReceiver` に設定されている `parameter`
- `VRCContactSender` / `VRCContactReceiver` の `collisionTags`

衝突検証

- 変更後名称がアバター全体の既存名称と衝突する場合、エラーとして表示します。

非破壊処理

- ビルド処理はランタイム複製したアセットに対して行うため、元アセットを直接書き換えません。

任意のタイミングで元アセットに直接書き込む破壊的適用も可能です（**事前にバックアップ必須**）。

## 【除外パラメータ/タグ】

以下のアバターパラメータは名称を変更しません。

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

以下のCollisionTag（組み込みタグ）は名称を変更しません。

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

## 【破壊的適用】

- コンポーネントの右クリックメニューから `[DANGER] Apply Destructive` を選択すると、上記の名称変更を元アセットに直接書き込むことができます。
- 適用後は元に戻せないため、必ず事前にバックアップを取ってください。

## 【ライセンス】

- このパッケージは MIT License で提供されます。詳細は [LICENSE](https://github.com/itoha-noctlab/avatar-parameters-auto-prefix/blob/main/LICENSE.txt) を参照してください。

## 【お問い合わせ】

- 不具合などありましたら、BOOTHメッセージまたは https://noctlab.com/contact?t=tool&tool=Avatar+Parameters+Auto+Prefix にてご連絡ください。
