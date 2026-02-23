# Avatar Parameters Auto Prefix

![image1](https://raw.githubusercontent.com/itoha-noctlab/avatar-parameters-auto-prefix/refs/heads/main/img/image1.ko.png)

## 【개요】

- 이 도구는 VRChat 아바타 빌드 시 파라미터 이름에 `prefixName`을 비파괴 방식으로 자동 접두합니다.
- 하나의 아바타에 같은 기믹을 여러 개 배치하거나, 여러 기믹 간 파라미터 이름이 충돌할 때 이름 충돌을 피하는 데 유용합니다.
- VPM 패키지로 배포됩니다. VCC에 `https://vpm.noctlab.com/vpm.json` 을 추가하여 설치할 수 있습니다。

## 【지원 언어 / Supported Languages】

- [ja (日本語)](https://noctlab.com/docs/avatar-parameters-auto-prefix/ja)
- [en (English)](https://noctlab.com/docs/avatar-parameters-auto-prefix/en)
- [ko (한국어)](https://noctlab.com/docs/avatar-parameters-auto-prefix/ko)
- [zh (简体中文)](https://noctlab.com/docs/avatar-parameters-auto-prefix/zh)

## 【사양】

- Modular Avatar 사용을 전제로 합니다.
- 처리 시점은 build-time 입니다.
- 비파괴 처리이므로 컴포넌트를 제거하면 도입 전 상태로 되돌릴 수 있습니다.
- VRChat 기본 파라미터 이름(예: `GestureLeft`, `GestureRight`, `IsLocal`)은 변경하지 않습니다.

## 【필수 의존성】

- `com.vrchat.avatars` `>=3.5.0`
- `nadena.dev.modular-avatar` `>=1.11.0`

## 【사용 방법】

1. 이름을 변경하고 싶은 아바타 기믹의 루트 오브젝트에 `Avatar Parameters Auto Prefix` 컴포넌트를 추가하고 `prefixName`을 설정합니다.
2. 평소처럼 아바타를 빌드합니다.

## 【상세 동작】

다음 항목의 이름을 `prefixName + 원래 이름` 으로 변환합니다.

- `ModularAvatarParameters` 에 포함된 파라미터 이름
- `ModularAvatarMergeAnimator` 에 설정된 `AnimatorController`
- `AnimatorController.parameters` 의 파라미터 이름
- `VRCAvatarParameterDriver` 의 `parameters[].name`
- `VRCAnimatorPlayAudio` 의 `parameter`
- `AnimatorState` 전이 조건에서 사용되는 파라미터 이름
- `BlendTree` 내부에서 사용되는 파라미터 이름
- `VRCPhysBone.parameter` 및 파생 파라미터(`_IsGrabbed`, `_IsPosed`, `_Angle`, `_Stretch`, `_Squish`)
- `VRCContactReceiver` 에 설정된 `parameter`
- `VRCContactSender` / `VRCContactReceiver` 의 `collisionTags`

충돌 검증

- 변경 후 이름이 아바타 범위 내 기존 이름과 충돌하면 오류를 표시합니다.

비파괴 처리

- 빌드 처리는 런타임 복제된 에셋에서 수행되며, 원본 에셋을 직접 수정하지 않습니다.

원본 에셋에 직접 반영하는 파괴적 적용도 언제든 실행할 수 있습니다(**반드시 사전 백업 필요**).

## 【제외 파라미터/태그】

다음 아바타 파라미터는 이름을 변경하지 않습니다.

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

다음 CollisionTag(기본 내장 태그)는 이름을 변경하지 않습니다.

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

## 【파괴적 적용】

- 컴포넌트 컨텍스트 메뉴에서 `[DANGER] Apply Destructive` 를 선택하면, 위와 같은 이름 변경을 원본 에셋에 직접 기록합니다.
- 이 작업은 되돌릴 수 없습니다. 반드시 미리 백업해 주세요.

## 【라이선스】

- 이 패키지는 MIT License로 제공됩니다. 자세한 내용은 [LICENSE](https://github.com/itoha-noctlab/avatar-parameters-auto-prefix/blob/main/LICENSE.txt) 를 확인하세요.

## 【문의】

- 버그 제보나 문의는 BOOTH 메시지 또는 https://noctlab.com/contact?t=tool&tool=Avatar+Parameters+Auto+Prefix 로 연락해 주세요.
