using UnityEngine;

namespace _Game.Scripts.UI
{
    public abstract class BaseOverlay : MonoBehaviour
    {
        public bool IsActive => gameObject.activeSelf;

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        } 
    }
}