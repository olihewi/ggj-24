using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class NextSceneButton : MonoBehaviour
    {
        public int sceneIdx = 1;
        private void OnMouseDown()
        {
            SceneManager.LoadScene(sceneIdx);
        }
    }
}