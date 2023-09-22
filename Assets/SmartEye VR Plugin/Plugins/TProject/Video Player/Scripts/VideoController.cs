using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using SmartEye.Framework;

namespace TProject
{
    public class VideoController : MonoBehaviour
    {
        public CanvasRayChecker checker;
        public VideoPlayer videoPlayer;
        public Slider sliderProgress;
        public GameObject controllerGroup;
        public GameObject playButton;
        public GameObject pauseButton;

        [Tooltip("Time to auto hide the controller. 0 will not hide the controller.")]
        [SerializeField] public int hideScreenControlTime = 0;
        private float hideScreenTime = 0;

        private void Update()
        {
            if (videoPlayer.frameCount > 0)
            {
                float progress = (float)videoPlayer.frame / (float)videoPlayer.frameCount;

                UpdateTimeBarValue(progress);
            }

            if (UserInteract())
            {
                hideScreenTime = 0;

                if (controllerGroup != null)
                    controllerGroup.SetActive(true);

                if (Input.GetKeyDown(KeyCode.RightArrow))
                    OnClickForwardTime();

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    OnClickReverseTime();

                if (Input.GetKeyDown(KeyCode.Space))
                    OnClickPlayPause();

                CheckPlayPauseButton();
            }
            else
            {
                hideScreenTime += Time.deltaTime;
                if (!(hideScreenTime >= hideScreenControlTime)) return;
                hideScreenTime = hideScreenControlTime;

                controllerGroup.SetActive(false);
            }
        }

        void CheckPlayPauseButton()
        {
            if (videoPlayer.isPlaying)
            {
                pauseButton.SetActive(true);
                playButton.SetActive(false);
            }
            else
            {
                playButton.SetActive(true);
                pauseButton.SetActive(false);
            }
        }

        public void OnClickForwardTime()
        {
            videoPlayer.frame += 450;
        }

        public void OnClickReverseTime()
        {
            videoPlayer.frame -= 450;
        }

        public void OnClickPlayPause()
        {
            CheckState();
        }

        void CheckState()
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
        }

        public void UpdateTimeBarValue(float val)
        {
            sliderProgress.value = val;
            // Debug.Log($"current time: {val}");
        }

        bool UserInteract()
        {
            if (Application.isMobilePlatform)
            {
                if (Input.touches.Length >= 1)
                    return true;
                else
                    return false;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                    return true;
                return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0 || checker.isPlayerHoverCanvas);
            }

        }
    }
}


