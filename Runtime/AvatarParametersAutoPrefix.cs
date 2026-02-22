using UnityEngine;
using nadena.dev.ndmf;

namespace AKATSUKIYA.AvatarParametersAutoPrefix
{
    [DisallowMultipleComponent]
    [HelpURL("https://noctlab.com/docs/avatar-parameters-auto-prefix")]
    [AddComponentMenu("AKATSUKIYA/Avatar Parameters Auto Prefix")]
    public sealed class AvatarParametersAutoPrefix : MonoBehaviour, INDMFEditorOnly
    {
        public string prefixName = string.Empty;
    }
}
