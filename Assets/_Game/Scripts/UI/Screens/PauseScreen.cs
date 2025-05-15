using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Screens
{
    public class PauseScreen : BaseScreen
    {
        [SerializeField] private Button returnButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button menuButton;

        private void Awake()
        {
            returnButton.onClick.AddListener(Hide);
            exitButton.onClick.AddListener(Application.Quit);
            menuButton.onClick.AddListener(OnMenuButtonClicked);
        }

        private void OnMenuButtonClicked()
        {
            SceneManager.LoadScene(0);
        }

        private void OnDestroy()
        {
            returnButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
            menuButton.onClick.RemoveAllListeners();
        }
    }
}