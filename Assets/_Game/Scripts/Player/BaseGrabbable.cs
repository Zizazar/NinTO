using UnityEngine;

namespace _Game.Scripts.Player
{
    public class BaseGrabbable : MonoBehaviour, IGrabbable
    {
        public void OnGrab()
        {
            AudioManager.Instance.PlaySoundRandomPitchOnPos("grab", transform.position, 0.9f, 1.2f);
        }

        public void OnRelease()
        {
            AudioManager.Instance.PlaySoundRandomPitchOnPos("grab", transform.position, 0.5f, 0.8f);
        }
    }
}