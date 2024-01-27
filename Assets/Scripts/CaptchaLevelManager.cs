using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptchaGame
{
    public class CaptchaLevelManager : MonoBehaviour
    {
        public static CaptchaLevelManager Instance { get; private set; }
        public CaptchaLevelBase[] levels;
        public int currentLevelIndex = 0;
        public CaptchaLevelBase CurrentLevel => levels[currentLevelIndex];

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (levels.Length == 0) Debug.LogError("Levels have not been set up in the CaptchaLevelManager");
            foreach (var level in levels)
            {
                level.gameObject.SetActive(false);
            }
            StartCoroutine(CaptchaGameRoutine());
        }

        private IEnumerator CaptchaGameRoutine()
        {
            for (int i = currentLevelIndex; i < levels.Length; i++)
            {
                var level = levels[i];
                if (level == null)
                {
                    Debug.LogError($"Captcha level {i} is null!");
                    continue;
                }
                Debug.Log($"Playing captcha level {level.gameObject.name}");
                level.gameObject.SetActive(true);
                yield return level.LevelRoutine();
                level.gameObject.SetActive(false);
            }
        }
        
    }
}
