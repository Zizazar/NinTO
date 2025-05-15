using UnityEngine;

namespace _Game.Scripts.UI
{
    // Базовый класс для всех экранов 
    // Подтягивается в контроллер автоматически
    // Класс должен быть только на одном обьекте
    public abstract class BaseScreen : MonoBehaviour
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

        // ReSharper disable Unity.PerformanceAnalysis
        public virtual void Toggle()
        {
            //gameObject.SetActive(!IsActive);
            // Чтобы работали перезаписаные методы
            if (gameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
}