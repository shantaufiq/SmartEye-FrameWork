using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Tproject.AudioManager;

namespace Seville
{
    public class MenuManager : MonoBehaviour
    {
        public Slider musicSlider, sfxSlider;

        public void SetUpVolume()
        {
            musicSlider.value = AudioManager.Instance.GetMasterVolume();
            sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        }

        public void QuitApplication()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }
}