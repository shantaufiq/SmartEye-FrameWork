using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Seville
{
    public class EnvNavigation : MonoBehaviour
    {
        public int targetNextSceneOrArea;
        public void OnClickChangeEnvirontment()
        {
            EnvironmentManager.Instance.StartAreaByIndex(targetNextSceneOrArea);
        }

        public void ChangeScene()
        {
            SceneManager.LoadScene(targetNextSceneOrArea);
        }
    }
}