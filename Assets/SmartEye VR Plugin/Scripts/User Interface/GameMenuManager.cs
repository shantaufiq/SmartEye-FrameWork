using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Tproject.AudioManager;

namespace SmartEye.Framework
{
    public class GameMenuManager : MonoBehaviour
    {
        public Transform playerHead;
        public float spawnDistance = 2;
        public GameObject gameMenuCanvas;
        public InputActionProperty showButton;
        public Slider musicSlider, sfxSlider;

        private void Update()
        {
            if (showButton.action.WasPressedThisFrame())
            {
                gameMenuCanvas.SetActive(!gameMenuCanvas.activeSelf);

                gameMenuCanvas.transform.position = playerHead.position + new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized * spawnDistance;
                SetUpVolume();
            }

            gameMenuCanvas.transform.LookAt(new Vector3(playerHead.position.x, gameMenuCanvas.transform.position.y, playerHead.position.z));
            gameMenuCanvas.transform.forward *= -1;
        }

        private void SetUpVolume()
        {
            musicSlider.value = AudioManager.Instance.GetMusicVolume();
            sfxSlider.value = AudioManager.Instance.GetSfxVolume();
        }

        public void OnDragChangeMusicVolum()
        {
            AudioManager.Instance.ChangeMusicVolume(musicSlider.value);
        }

        public void OnDragChangeSfxVolum()
        {
            AudioManager.Instance.ChangeSfxVolume(sfxSlider.value);
        }
    }
}