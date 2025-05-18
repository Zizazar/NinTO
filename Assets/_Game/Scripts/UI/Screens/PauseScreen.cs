using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Screens
{
    public class PauseScreen : BaseScreen
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button menuButton;

        private void Awake()
        {
            resumeButton.onClick.AddListener(OnResumeButtonClicked);
            exitButton.onClick.AddListener(Application.Quit);
            menuButton.onClick.AddListener(OnMenuButtonClicked);
        }

        private void OnResumeButtonClicked()
        {
            G.main.onResume.Invoke();
            Hide();
        }

        private void OnMenuButtonClicked()
        {
            SceneManager.LoadScene(0);
        }

        private void OnDestroy()
        {
            resumeButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
            menuButton.onClick.RemoveAllListeners();
        }
    }
}