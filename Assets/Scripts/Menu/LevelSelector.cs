using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace levelSelection
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        public Scene selectedLevel;

        public void StartGame()
        {
            SceneManager.LoadScene(selectedLevel.handle);
        }

        public void SelectLevel(Level level)
        {
            selectedLevel = level.level;
            startButton.interactable = true;
        }
    }
}