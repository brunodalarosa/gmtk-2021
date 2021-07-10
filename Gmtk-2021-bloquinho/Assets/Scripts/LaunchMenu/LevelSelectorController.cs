using Manager;
using UnityEngine;

namespace LaunchMenu
{
    public class LevelSelectorController : MonoBehaviour
    {
        [SerializeField]
        private MainMenuController _mainMenuController = null;
        private MainMenuController MainMenuController => _mainMenuController;
        
        [SerializeField]
        private LevelSelectButton _levelSelectButtonPrefab = null;
        private LevelSelectButton LevelSelectButtonPrefab => _levelSelectButtonPrefab;

        private void Start()
        {
            for (var index = 0; index <= AdventureModeManager.Finallevel; index++)
            {
                var levelButton = Instantiate(LevelSelectButtonPrefab, transform);
                levelButton.Init(index, StartFromLevel);
            }
        }

        private void StartFromLevel(int levelIndex)
        {
            MainMenuController.StartAdventureMode(levelIndex);
        }
    }

   
}