#if AKATSUKIYA_VRCSDK3_AVATARS

using System;
using System.Collections.Generic;
using nadena.dev.modular_avatar.core;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;
using VRC.Dynamics;
using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Dynamics.Contact.Components;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace AKATSUKIYA.AvatarParametersAutoPrefix.Editor
{
    internal sealed class AutoPrefixPlaySessionState
    {
        private readonly Dictionary<string, RuntimeAnimatorController> _mergeAnimatorOriginalController = new();
        private readonly Dictionary<string, AnimatorController> _createdRuntimeControllers = new();
        private readonly Dictionary<string, List<string>> _maParameterOriginalNames = new();
        private readonly Dictionary<string, string> _menuMainParameterOriginalNames = new();
        private readonly Dictionary<string, List<string>> _menuSubParameterOriginalNames = new();
        private readonly Dictionary<string, string> _contactReceiverOriginalParameterNames = new();
        private readonly Dictionary<string, string> _physBoneOriginalParameterNames = new();
        private readonly Dictionary<string, List<string>> _contactCollisionTagsOriginalNames = new();

        public void CaptureMergeAnimatorOriginal(ModularAvatarMergeAnimator mergeAnimator)
        {
            var key = GetGlobalObjectIdString(mergeAnimator);
            if (key == null || _mergeAnimatorOriginalController.ContainsKey(key)) return;
            _mergeAnimatorOriginalController[key] = mergeAnimator.animator;
        }

        public void TrackCreatedController(ModularAvatarMergeAnimator mergeAnimator, AnimatorController runtimeController)
        {
            var key = GetGlobalObjectIdString(mergeAnimator);
            if (key == null || _createdRuntimeControllers.ContainsKey(key)) return;
            _createdRuntimeControllers[key] = runtimeController;
        }

        public void CaptureMAParameterOriginal(ModularAvatarParameters parameters)
        {
            var key = GetGlobalObjectIdString(parameters);
            if (key == null || _maParameterOriginalNames.ContainsKey(key)) return;

            var names = new List<string>(parameters.parameters.Count);
            foreach (var param in parameters.parameters)
            {
                names.Add(param.nameOrPrefix);
            }

            _maParameterOriginalNames[key] = names;
        }

        public void CaptureMenuOriginal(ModularAvatarMenuItem menuItem)
        {
            if (menuItem?.Control == null) return;

            var key = GetGlobalObjectIdString(menuItem);
            if (key == null || _menuMainParameterOriginalNames.ContainsKey(key)) return;
            _menuMainParameterOriginalNames[key] = menuItem.Control.parameter?.name;

            var subNames = new List<string>();
            var subParameters = menuItem.Control.subParameters;
            if (subParameters != null)
            {
                foreach (var subParameter in subParameters)
                {
                    subNames.Add(subParameter?.name);
                }
            }

            _menuSubParameterOriginalNames[key] = subNames;
        }

        public void CaptureContactReceiverOriginal(VRCContactReceiver receiver)
        {
            var key = GetGlobalObjectIdString(receiver);
            if (key == null || _contactReceiverOriginalParameterNames.ContainsKey(key)) return;
            _contactReceiverOriginalParameterNames[key] = receiver.parameter;
        }

        public void CaptureContactCollisionTagsOriginal(ContactBase contact)
        {
            var key = GetGlobalObjectIdString(contact);
            if (key == null || _contactCollisionTagsOriginalNames.ContainsKey(key)) return;

            var original = contact.collisionTags;
            _contactCollisionTagsOriginalNames[key] = original == null ? new List<string>() : new List<string>(original);
        }

        public void CapturePhysBoneOriginal(VRCPhysBone physBone)
        {
            var key = GetGlobalObjectIdString(physBone);
            if (key == null || _physBoneOriginalParameterNames.ContainsKey(key)) return;
            _physBoneOriginalParameterNames[key] = physBone.parameter;
        }

        public void RestoreAndDispose()
        {
            foreach (var kv in _mergeAnimatorOriginalController)
            {
                var component = ResolveByGlobalObjectId<ModularAvatarMergeAnimator>(kv.Key);
                if (component == null) continue;
                component.animator = kv.Value;
            }

            foreach (var kv in _maParameterOriginalNames)
            {
                var component = ResolveByGlobalObjectId<ModularAvatarParameters>(kv.Key);
                if (component == null) continue;

                var current = component.parameters;
                var restoreCount = Math.Min(current.Count, kv.Value.Count);
                for (var i = 0; i < restoreCount; i++)
                {
                    var config = current[i];
                    config.nameOrPrefix = kv.Value[i];
                    current[i] = config;
                }
            }

            foreach (var kv in _menuMainParameterOriginalNames)
            {
                var component = ResolveByGlobalObjectId<ModularAvatarMenuItem>(kv.Key);
                if (component?.Control == null) continue;

                if (component.Control.parameter != null)
                {
                    component.Control.parameter.name = kv.Value;
                }

                if (_menuSubParameterOriginalNames.TryGetValue(kv.Key, out var subNames))
                {
                    var subParameters = component.Control.subParameters;
                    if (subParameters != null)
                    {
                        var restoreCount = Math.Min(subParameters.Length, subNames.Count);
                        for (var i = 0; i < restoreCount; i++)
                        {
                            if (subParameters[i] == null) continue;
                            subParameters[i].name = subNames[i];
                        }
                    }
                }
            }

            foreach (var kv in _contactReceiverOriginalParameterNames)
            {
                var component = ResolveByGlobalObjectId<VRCContactReceiver>(kv.Key);
                if (component == null) continue;
                component.parameter = kv.Value;
            }

            foreach (var kv in _physBoneOriginalParameterNames)
            {
                var component = ResolveByGlobalObjectId<VRCPhysBone>(kv.Key);
                if (component == null) continue;
                component.parameter = kv.Value;
            }

            foreach (var kv in _contactCollisionTagsOriginalNames)
            {
                var component = ResolveByGlobalObjectId<ContactBase>(kv.Key);
                if (component == null) continue;
                component.collisionTags = new List<string>(kv.Value);
            }

            foreach (var controller in _createdRuntimeControllers.Values)
            {
                if (controller == null) continue;
                if (EditorUtility.IsPersistent(controller)) continue;
                Object.DestroyImmediate(controller);
            }

            _mergeAnimatorOriginalController.Clear();
            _createdRuntimeControllers.Clear();
            _maParameterOriginalNames.Clear();
            _menuMainParameterOriginalNames.Clear();
            _menuSubParameterOriginalNames.Clear();
            _contactReceiverOriginalParameterNames.Clear();
            _physBoneOriginalParameterNames.Clear();
            _contactCollisionTagsOriginalNames.Clear();
        }

        private static string GetGlobalObjectIdString(Object obj)
        {
            if (obj == null) return null;
            var id = GlobalObjectId.GetGlobalObjectIdSlow(obj);
            return id.ToString();
        }

        private static T ResolveByGlobalObjectId<T>(string idString) where T : Object
        {
            if (!GlobalObjectId.TryParse(idString, out var globalId)) return null;
            return GlobalObjectId.GlobalObjectIdentifierToObjectSlow(globalId) as T;
        }
    }

    internal static class AutoPrefixProcessor
    {
        private static readonly HashSet<string> BuiltInContactCollisionTags = CreateBuiltInContactCollisionTags();

        private static readonly string[] PhysBoneDerivedSuffixes =
        {
            "_IsGrabbed",
            "_IsPosed",
            "_Angle",
            "_Stretch",
            "_Squish"
        };

        // VRChat default parameters should never be renamed.
        private static readonly HashSet<string> ReservedVRChatParameters = new(StringComparer.Ordinal)
        {
            "IsLocal",
            "PreviewMode",
            "Viseme",
            "Voice",
            "GestureLeft",
            "GestureRight",
            "GestureLeftWeight",
            "GestureRightWeight",
            "AngularY",
            "VelocityX",
            "VelocityY",
            "VelocityZ",
            "VelocityMagnitude",
            "Upright",
            "Grounded",
            "Seated",
            "AFK",
            "TrackingType",
            "VRMode",
            "MuteSelf",
            "InStation",
            "Earmuffs",
            "IsOnFriendsList",
            "AvatarVersion",
            "IsAnimatorEnabled",
            "ScaleModified",
            "ScaleFactor",
            "ScaleFactorInverse",
            "EyeHeightAsMeters",
            "EyeHeightAsPercent"
        };

        public static AutoPrefixCandidates CollectRenameCandidates(Transform root, string prefix)
        {
            var avatarParameters = new HashSet<string>(StringComparer.Ordinal);
            var collisionTags = new HashSet<string>(StringComparer.Ordinal);

            if (root == null || string.IsNullOrEmpty(prefix))
            {
                return new AutoPrefixCandidates(new List<string>(), new List<string>());
            }

            foreach (var parameters in root.GetComponentsInChildren<ModularAvatarParameters>(true))
            {
                if (parameters == null) continue;

                foreach (var config in parameters.parameters)
                {
                    var sourceName = config.nameOrPrefix;
                    if (string.IsNullOrEmpty(sourceName)) continue;
                    if (string.Equals(sourceName, PrefixIfNotEmpty(prefix, sourceName), StringComparison.Ordinal)) continue;
                    avatarParameters.Add(sourceName);
                }
            }

            foreach (var mergeAnimator in root.GetComponentsInChildren<ModularAvatarMergeAnimator>(true))
            {
                if (mergeAnimator?.animator is not AnimatorController controller) continue;

                foreach (var animatorParameter in controller.parameters)
                {
                    var sourceName = animatorParameter.name;
                    if (string.IsNullOrEmpty(sourceName)) continue;
                    if (string.Equals(sourceName, PrefixIfNotEmpty(prefix, sourceName), StringComparison.Ordinal)) continue;
                    avatarParameters.Add(sourceName);
                }
            }

            foreach (var physBone in root.GetComponentsInChildren<VRCPhysBone>(true))
            {
                if (physBone == null || string.IsNullOrEmpty(physBone.parameter)) continue;

                var sourceBase = physBone.parameter;
                if (!string.Equals(sourceBase, PrefixIfNotEmpty(prefix, sourceBase), StringComparison.Ordinal))
                {
                    avatarParameters.Add(sourceBase);
                }

                foreach (var suffix in PhysBoneDerivedSuffixes)
                {
                    var sourceDerived = sourceBase + suffix;
                    if (string.Equals(sourceDerived, PrefixIfNotEmpty(prefix, sourceDerived), StringComparison.Ordinal)) continue;
                    avatarParameters.Add(sourceDerived);
                }
            }

            foreach (var contactReceiver in root.GetComponentsInChildren<VRCContactReceiver>(true))
            {
                if (contactReceiver == null || string.IsNullOrEmpty(contactReceiver.parameter)) continue;

                var sourceName = contactReceiver.parameter;
                if (string.Equals(sourceName, PrefixIfNotEmpty(prefix, sourceName), StringComparison.Ordinal)) continue;
                avatarParameters.Add(sourceName);
            }

            foreach (var contact in root.GetComponentsInChildren<ContactBase>(true))
            {
                if (contact?.collisionTags == null) continue;

                foreach (var tag in contact.collisionTags)
                {
                    if (!ShouldPrefixContactCollisionTag(tag)) continue;
                    collisionTags.Add(tag);
                }
            }

            var avatarList = new List<string>(avatarParameters);
            avatarList.Sort(StringComparer.Ordinal);
            var collisionList = new List<string>(collisionTags);
            collisionList.Sort(StringComparer.Ordinal);
            return new AutoPrefixCandidates(avatarList, collisionList);
        }

        public static bool TryGetValidationError(Transform root, string prefix, out string error)
        {
            error = null;

            if (root == null || string.IsNullOrEmpty(prefix))
            {
                return false;
            }

            try
            {
                BuildParameterRemap(root, prefix);
                BuildCollisionTagRemap(root, prefix);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                error = ex.Message;
                return true;
            }
        }

        public static void ApplyUnder(
            Transform root,
            string prefix,
            bool cloneAnimatorController,
            AutoPrefixPlaySessionState playState
        )
        {
            if (root == null || string.IsNullOrEmpty(prefix)) return;

            var parameterRemap = BuildParameterRemap(root, prefix);
            var collisionTagRemap = BuildCollisionTagRemap(root, prefix);

            foreach (var mergeAnimator in root.GetComponentsInChildren<ModularAvatarMergeAnimator>(true))
            {
                ApplyToMergeAnimator(mergeAnimator, parameterRemap, cloneAnimatorController, playState);
            }

            foreach (var parameters in root.GetComponentsInChildren<ModularAvatarParameters>(true))
            {
                ApplyToParameters(parameters, parameterRemap, playState);
            }

            foreach (var menuItem in root.GetComponentsInChildren<ModularAvatarMenuItem>(true))
            {
                ApplyToMenuItem(menuItem, parameterRemap, playState);
            }

            foreach (var contactReceiver in root.GetComponentsInChildren<VRCContactReceiver>(true))
            {
                ApplyToContactReceiver(contactReceiver, parameterRemap, playState);
            }

            foreach (var physBone in root.GetComponentsInChildren<VRCPhysBone>(true))
            {
                ApplyToPhysBone(physBone, parameterRemap, playState);
            }

            foreach (var contact in root.GetComponentsInChildren<ContactBase>(true))
            {
                ApplyToContactCollisionTags(contact, collisionTagRemap, playState);
            }
        }

        private static Dictionary<string, string> BuildParameterRemap(Transform root, string prefix)
        {
            var remap = new Dictionary<string, string>(StringComparer.Ordinal);
            var existingNames = CollectExistingParameterNames(ResolveConflictScopeRoot(root));

            foreach (var parameters in root.GetComponentsInChildren<ModularAvatarParameters>(true))
            {
                if (parameters == null) continue;

                foreach (var config in parameters.parameters)
                {
                    var sourceName = config.nameOrPrefix;
                    if (string.IsNullOrEmpty(sourceName)) continue;

                    var mappedName = PrefixIfNotEmpty(prefix, sourceName);
                    remap[sourceName] = mappedName;
                }
            }

            foreach (var mergeAnimator in root.GetComponentsInChildren<ModularAvatarMergeAnimator>(true))
            {
                if (mergeAnimator?.animator is not AnimatorController controller) continue;

                foreach (var animatorParameter in controller.parameters)
                {
                    var sourceName = animatorParameter.name;
                    if (string.IsNullOrEmpty(sourceName)) continue;

                    remap[sourceName] = PrefixIfNotEmpty(prefix, sourceName);
                }
            }

            foreach (var physBone in root.GetComponentsInChildren<VRCPhysBone>(true))
            {
                if (physBone == null || string.IsNullOrEmpty(physBone.parameter)) continue;

                var sourceBase = physBone.parameter;
                var mappedBase = PrefixIfNotEmpty(prefix, sourceBase);
                remap[sourceBase] = mappedBase;

                foreach (var suffix in PhysBoneDerivedSuffixes)
                {
                    var sourceDerived = sourceBase + suffix;
                    var mappedDerived = mappedBase + suffix;

                    remap[sourceDerived] = mappedDerived;
                }
            }

            foreach (var contactReceiver in root.GetComponentsInChildren<VRCContactReceiver>(true))
            {
                if (contactReceiver == null || string.IsNullOrEmpty(contactReceiver.parameter)) continue;

                var sourceName = contactReceiver.parameter;
                remap[sourceName] = PrefixIfNotEmpty(prefix, sourceName);
            }

            foreach (var pair in remap)
            {
                if (string.Equals(pair.Key, pair.Value, StringComparison.Ordinal)) continue;
                if (!existingNames.Contains(pair.Value)) continue;

                throw new InvalidOperationException(
                    Localized.Message.ParameterNameConflict.Format(pair.Value)
                );
            }

            return remap;
        }

        private static Dictionary<string, string> BuildCollisionTagRemap(Transform root, string prefix)
        {
            var remap = new Dictionary<string, string>(StringComparer.Ordinal);
            var existingTags = CollectExistingCollisionTags(ResolveConflictScopeRoot(root));

            foreach (var contact in root.GetComponentsInChildren<ContactBase>(true))
            {
                if (contact?.collisionTags == null) continue;

                foreach (var tag in contact.collisionTags)
                {
                    if (!ShouldPrefixContactCollisionTag(tag)) continue;

                    remap[tag] = prefix + tag;
                }
            }

            foreach (var pair in remap)
            {
                if (string.Equals(pair.Key, pair.Value, StringComparison.Ordinal)) continue;
                if (!existingTags.Contains(pair.Value)) continue;

                throw new InvalidOperationException(
                    Localized.Message.CollisionTagNameConflict.Format(pair.Value)
                );
            }

            return remap;
        }

        private static void ApplyToMergeAnimator(
            ModularAvatarMergeAnimator mergeAnimator,
            Dictionary<string, string> parameterRemap,
            bool cloneAnimatorController,
            AutoPrefixPlaySessionState playState
        )
        {
            if (mergeAnimator == null || mergeAnimator.animator == null) return;
            if (mergeAnimator.animator is not AnimatorController originalController) return;

            var targetController = originalController;
            if (cloneAnimatorController)
            {
                var runtimeCopy = Object.Instantiate(originalController);
                runtimeCopy.name = originalController.name + "__AutoPrefixRuntime";
                playState?.CaptureMergeAnimatorOriginal(mergeAnimator);
                playState?.TrackCreatedController(mergeAnimator, runtimeCopy);
                mergeAnimator.animator = runtimeCopy;
                targetController = runtimeCopy;
            }

            RenameAnimatorController(targetController, parameterRemap);
        }

        private static void ApplyToParameters(
            ModularAvatarParameters parameters,
            Dictionary<string, string> parameterRemap,
            AutoPrefixPlaySessionState playState
        )
        {
            if (parameters == null) return;
            playState?.CaptureMAParameterOriginal(parameters);

            var list = parameters.parameters;
            for (var i = 0; i < list.Count; i++)
            {
                var config = list[i];
                config.nameOrPrefix = RemapAnimatorParameter(config.nameOrPrefix, parameterRemap);
                list[i] = config;
            }
        }

        private static void ApplyToMenuItem(
            ModularAvatarMenuItem menuItem,
            Dictionary<string, string> parameterRemap,
            AutoPrefixPlaySessionState playState
        )
        {
            if (menuItem?.Control == null) return;

            playState?.CaptureMenuOriginal(menuItem);
            if (menuItem.Control.parameter != null)
            {
                menuItem.Control.parameter.name = RemapAnimatorParameter(menuItem.Control.parameter.name, parameterRemap);
            }

            var subParameters = menuItem.Control.subParameters;
            if (subParameters == null) return;

            foreach (var subParameter in subParameters)
            {
                if (subParameter == null) continue;
                subParameter.name = RemapAnimatorParameter(subParameter.name, parameterRemap);
            }
        }

        private static void ApplyToContactReceiver(
            VRCContactReceiver receiver,
            Dictionary<string, string> parameterRemap,
            AutoPrefixPlaySessionState playState
        )
        {
            if (receiver == null) return;

            playState?.CaptureContactReceiverOriginal(receiver);
            receiver.parameter = RemapAnimatorParameter(receiver.parameter, parameterRemap);
        }

        private static void ApplyToPhysBone(
            VRCPhysBone physBone,
            Dictionary<string, string> parameterRemap,
            AutoPrefixPlaySessionState playState
        )
        {
            if (physBone == null) return;

            playState?.CapturePhysBoneOriginal(physBone);
            physBone.parameter = RemapAnimatorParameter(physBone.parameter, parameterRemap);
        }

        private static void ApplyToContactCollisionTags(
            ContactBase contact,
            Dictionary<string, string> collisionTagRemap,
            AutoPrefixPlaySessionState playState
        )
        {
            if (contact == null || collisionTagRemap == null || collisionTagRemap.Count == 0) return;
            if (contact.collisionTags == null || contact.collisionTags.Count == 0) return;

            playState?.CaptureContactCollisionTagsOriginal(contact);

            var tags = contact.collisionTags;
            for (var i = 0; i < tags.Count; i++)
            {
                var tag = tags[i];
                if (string.IsNullOrEmpty(tag)) continue;
                if (!collisionTagRemap.TryGetValue(tag, out var mapped)) continue;
                tags[i] = mapped;
            }
        }

        private static string PrefixIfNotEmpty(string prefix, string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (ReservedVRChatParameters.Contains(value)) return value;
            return prefix + value;
        }

        private static bool ShouldPrefixContactCollisionTag(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return false;
            return !BuiltInContactCollisionTags.Contains(tag);
        }

        private static Transform ResolveConflictScopeRoot(Transform from)
        {
            if (from == null) return null;

            var avatar = from.GetComponentInParent<VRCAvatarDescriptor>(true);
            if (avatar != null) return avatar.transform;

            return from.root;
        }

        private static HashSet<string> CollectExistingParameterNames(Transform scopeRoot)
        {
            var names = new HashSet<string>(StringComparer.Ordinal);
            if (scopeRoot == null) return names;

            foreach (var parameters in scopeRoot.GetComponentsInChildren<ModularAvatarParameters>(true))
            {
                if (parameters == null) continue;

                foreach (var config in parameters.parameters)
                {
                    if (string.IsNullOrEmpty(config.nameOrPrefix)) continue;
                    names.Add(config.nameOrPrefix);
                }
            }

            foreach (var mergeAnimator in scopeRoot.GetComponentsInChildren<ModularAvatarMergeAnimator>(true))
            {
                if (mergeAnimator?.animator is not AnimatorController controller) continue;

                foreach (var parameter in controller.parameters)
                {
                    if (string.IsNullOrEmpty(parameter.name)) continue;
                    names.Add(parameter.name);
                }
            }

            foreach (var physBone in scopeRoot.GetComponentsInChildren<VRCPhysBone>(true))
            {
                if (physBone == null || string.IsNullOrEmpty(physBone.parameter)) continue;

                names.Add(physBone.parameter);
                foreach (var suffix in PhysBoneDerivedSuffixes)
                {
                    names.Add(physBone.parameter + suffix);
                }
            }

            foreach (var contactReceiver in scopeRoot.GetComponentsInChildren<VRCContactReceiver>(true))
            {
                if (contactReceiver == null || string.IsNullOrEmpty(contactReceiver.parameter)) continue;
                names.Add(contactReceiver.parameter);
            }

            return names;
        }

        private static HashSet<string> CollectExistingCollisionTags(Transform scopeRoot)
        {
            var tags = new HashSet<string>(StringComparer.Ordinal);
            if (scopeRoot == null) return tags;

            foreach (var contact in scopeRoot.GetComponentsInChildren<ContactBase>(true))
            {
                if (contact?.collisionTags == null) continue;

                foreach (var tag in contact.collisionTags)
                {
                    if (string.IsNullOrEmpty(tag)) continue;
                    tags.Add(tag);
                }
            }

            return tags;
        }

        private static HashSet<string> CreateBuiltInContactCollisionTags()
        {
            var tags = new HashSet<string>(StringComparer.Ordinal)
            {
                "Head",
                "Torso",
                "Hand",
                "HandL",
                "HandR",
                "Foot",
                "FootL",
                "FootR",
                "Finger",
                "FingerL",
                "FingerR",
                "FingerIndex",
                "FingerMiddle",
                "FingerRing",
                "FingerLittle",
                "FingerIndexL",
                "FingerMiddleL",
                "FingerRingL",
                "FingerLittleL",
                "FingerIndexR",
                "FingerMiddleR",
                "FingerRingR",
                "FingerLittleR",
                "HandLeft",
                "HandRight",
                "FingerIndexLeft",
                "FingerIndexRight",
                "FootLeft",
                "FootRight",
                "Hot",
                "Cold",
                "Fire",
                "Freezer",
                "Wet",
                "Water",
                "Wind",
                "Weapon",
                "Shield",
                "Damage",
                "DamageBlunt",
                "DamageSharp",
                "Ammunition",
                "Projectile",
                "Consumable",
                "ConsumableFood",
                "ConsumableDrink",
                "Brush",
                "Dye"
            };
            return tags;
        }

        private static void RenameAnimatorController(AnimatorController controller, Dictionary<string, string> parameterRemap)
        {
            if (controller == null) return;

            var srcParameters = controller.parameters;
            var dstParameters = new AnimatorControllerParameter[srcParameters.Length];

            for (var i = 0; i < srcParameters.Length; i++)
            {
                var src = srcParameters[i];
                var newName = RemapAnimatorParameter(src.name, parameterRemap);

                dstParameters[i] = new AnimatorControllerParameter
                {
                    name = newName,
                    type = src.type,
                    defaultBool = src.defaultBool,
                    defaultFloat = src.defaultFloat,
                    defaultInt = src.defaultInt
                };
            }

            controller.parameters = dstParameters;

            var layers = controller.layers;
            for (var i = 0; i < layers.Length; i++)
            {
                var layer = layers[i];
                ProcessStateMachine(layer.stateMachine, parameterRemap);
                layers[i] = layer;
            }

            controller.layers = layers;
        }

        private static void ProcessStateMachine(
            AnimatorStateMachine stateMachine,
            Dictionary<string, string> remap
        )
        {
            if (stateMachine == null) return;

            foreach (var transition in stateMachine.anyStateTransitions)
            {
                ProcessTransition(transition, remap);
            }

            foreach (var transition in stateMachine.entryTransitions)
            {
                ProcessTransition(transition, remap);
            }

            foreach (var childState in stateMachine.states)
            {
                ProcessState(childState.state, remap);
            }

            foreach (var childStateMachine in stateMachine.stateMachines)
            {
                var nested = childStateMachine.stateMachine;
                ProcessStateMachine(nested, remap);

                foreach (var transition in stateMachine.GetStateMachineTransitions(nested))
                {
                    ProcessTransition(transition, remap);
                }
            }
        }

        private static void ProcessState(AnimatorState state, Dictionary<string, string> remap)
        {
            if (state == null) return;

            state.mirrorParameter = RemapAnimatorParameter(state.mirrorParameter, remap);
            state.timeParameter = RemapAnimatorParameter(state.timeParameter, remap);
            state.speedParameter = RemapAnimatorParameter(state.speedParameter, remap);
            state.cycleOffsetParameter = RemapAnimatorParameter(state.cycleOffsetParameter, remap);

            foreach (var transition in state.transitions)
            {
                ProcessTransition(transition, remap);
            }

            state.motion = ProcessMotion(state.motion, remap);

            foreach (var behaviour in state.behaviours)
            {
                if (behaviour is VRCAvatarParameterDriver driver)
                {
                    ProcessDriver(driver, remap);
                }

                if (behaviour is VRCAnimatorPlayAudio playAudio)
                {
                    playAudio.ParameterName = RemapAnimatorParameter(playAudio.ParameterName, remap);
                }
            }
        }

        private static Motion ProcessMotion(Motion motion, Dictionary<string, string> remap)
        {
            if (motion is not BlendTree blendTree) return motion;

            // BlendTree may point to an external asset. Clone it before mutation to keep processing non-destructive.
            if (EditorUtility.IsPersistent(blendTree))
            {
                var cloned = Object.Instantiate(blendTree);
                cloned.name = blendTree.name + "__AutoPrefixRuntime";
                blendTree = cloned;
            }

            blendTree.blendParameter = RemapAnimatorParameter(blendTree.blendParameter, remap);
            blendTree.blendParameterY = RemapAnimatorParameter(blendTree.blendParameterY, remap);

            var children = blendTree.children;
            for (var i = 0; i < children.Length; i++)
            {
                var child = children[i];
                child.directBlendParameter = RemapAnimatorParameter(child.directBlendParameter, remap);
                child.motion = ProcessMotion(child.motion, remap);
                children[i] = child;
            }

            blendTree.children = children;
            return blendTree;
        }

        private static void ProcessTransition(AnimatorTransitionBase transition, Dictionary<string, string> remap)
        {
            if (transition == null) return;

            var conditions = transition.conditions;
            for (var i = 0; i < conditions.Length; i++)
            {
                var condition = conditions[i];
                condition.parameter = RemapAnimatorParameter(condition.parameter, remap);
                conditions[i] = condition;
            }

            transition.conditions = conditions;
        }

        private static void ProcessDriver(VRCAvatarParameterDriver driver, Dictionary<string, string> remap)
        {
            if (driver == null || driver.parameters == null) return;

            var parameters = driver.parameters;
            for (var i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                parameter.name = RemapAnimatorParameter(parameter.name, remap);
                parameter.source = RemapAnimatorParameter(parameter.source, remap);
                parameter.destParam = RemapAnimatorParameter(parameter.destParam as string, remap);
                parameter.sourceParam = RemapAnimatorParameter(parameter.sourceParam as string, remap);
            }
        }

        private static string GetHierarchyPath(Transform transform)
        {
            if (transform == null) return "<null>";

            var path = transform.name;
            var current = transform.parent;
            while (current != null)
            {
                path = current.name + "/" + path;
                current = current.parent;
            }

            return path;
        }

        private static string RemapAnimatorParameter(string name, Dictionary<string, string> remap)
        {
            if (string.IsNullOrEmpty(name)) return name;
            return remap.TryGetValue(name, out var replaced) ? replaced : name;
        }
    }
}

#endif
