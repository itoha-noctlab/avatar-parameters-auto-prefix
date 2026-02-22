#if AKATSUKIYA_VRCSDK3_AVATARS

using System.Collections.Generic;

namespace AKATSUKIYA.AvatarParametersAutoPrefix.Editor
{
    internal readonly struct AutoPrefixCandidates
    {
        public readonly List<string> AvatarParameters;
        public readonly List<string> CollisionTags;

        public AutoPrefixCandidates(List<string> avatarParameters, List<string> collisionTags)
        {
            AvatarParameters = avatarParameters ?? new List<string>();
            CollisionTags = collisionTags ?? new List<string>();
        }
    }
}

#endif
