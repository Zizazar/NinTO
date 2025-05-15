using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Components
{
    [RequireComponent(typeof(Button), typeof(RectTransform))]
    public class CharacterButton : MonoBehaviour
    {
        // TODO: Вместо индекса использовать референс на префаб или хз
        [SerializeField] private int npcIndex;
        [SerializeField] private float yOffset;
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;

        public int NpcIndex => npcIndex;
        public Button.ButtonClickedEvent OnClick => GetComponent<Button>().onClick; // Небольшой костыль
        
        private RectTransform _rect;
        private Button _button;
        private Vector2 _initialPosition;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _button = GetComponent<Button>();
            _initialPosition = _rect.anchoredPosition;
        }

        public void Select()
        {
            _rect.DOAnchorPos(_initialPosition + new Vector2(0, yOffset), duration)
                .SetEase(ease);
        }

        public void Deselect()
        {
            _rect.DOAnchorPos(_initialPosition, duration);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}