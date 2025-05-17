using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Scripts.UI
{
    // Через этот класс можно управлять экранами и оверлеями
    // Они подтягиваются автоматом
    // Можно открывать, закрывать переключать экраны и оверлеи передавая в метод класс 
    public class UIController : MonoBehaviour
    {
        [SerializeField, ReadOnly] private GameObject m_ScreensRoot;
        [SerializeField, ReadOnly] private GameObject m_OverlaysRoot;
        
        private List<BaseScreen> _screens;
        //private List<BaseScreen> _openedScreens;
        
        private List<BaseOverlay> _overlays;

        private void Start()
        {
            _screens = new List<BaseScreen>( // Конвертация в List чтобы работал Find
                m_ScreensRoot.GetComponentsInChildren<BaseScreen>(true)
                );
            _overlays = new List<BaseOverlay>(
                m_OverlaysRoot.GetComponentsInChildren<BaseOverlay>(true)
            );
        }   
        
        // ---- Экраны ----
        public void ShowScreen<TScreen>() where TScreen : BaseScreen
        {
            _screens
                .Find(screen => screen is TScreen)
                .Show();
            
        }

        public void HideScreen<TScreen>() where TScreen : BaseScreen
        {
            _screens
                .Find(screen => screen is TScreen)
                .Hide();
        }

        public void ToggleScreen<TScreen>() where TScreen : BaseScreen
        {
            _screens
                .Find(screen => screen is TScreen)
                .Toggle();
        }

        public TScreen GetScreen<TScreen>() where TScreen : BaseScreen
        {
            return _screens.Find(screen => screen is TScreen) as TScreen;
        }
        
        // ---- Оверлеи ----
        public void ShowOverlay<TOverlay>() where TOverlay : BaseOverlay
        {
            _overlays
                .Find(screen => screen is TOverlay)
                .Show();
            
        }

        public void HideOverlay<TOverlay>() where TOverlay : BaseOverlay
        {
            _overlays
                .Find(overlay => overlay is TOverlay)
                .Hide();
        }

        public TOverlay GetOverlay<TOverlay>() where TOverlay : BaseOverlay
        {
            return _overlays.Find(overlay => overlay is TOverlay) as TOverlay;
        }
    }

}