using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LaunchMenu
{
    [RequireComponent(typeof(Button))]
    public class LevelSelectButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _levelText = null;
        private TextMeshProUGUI LevelText => _levelText;
        
        public void Init(int level, Action<int> clickAction)
        {
            LevelText.text = level.ToString();
            GetComponent<Button>().onClick.AddListener(() => clickAction(level));
        }
    }
}