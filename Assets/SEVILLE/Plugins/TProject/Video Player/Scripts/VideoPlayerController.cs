using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Tproject.AudioManager;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

namespace TProject
{
    public class VideoPlayerController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Video Config")]
        public VideoClip videoClip;

        [Range(.1f, 1f)]
        [SerializeField] private float hideScreenControlTime = .9f;
        [SerializeField] private bool playOnStart = false;

        [Header("Video Event")]
        [Space(5f)]
        public UnityEvent OnVideoFinished;

        [Header("Component Dependencies")]
        [SerializeField] private VideoPlayer videoplayer;
        [SerializeField] private Slider sliderProgress;
        [SerializeField] private CanvasGroup controllerGroup;
        [SerializeField] private GameObject playButton;
        [SerializeField] private GameObject pauseButton;
        [SerializeField] private GameObject panelThumbnail;

        private AudioManager m_audioManager;
        private LTDescr currentTween; // Reference to the current LeanTween animation

        private static List<VideoPlayerController> controllers = new List<VideoPlayerController>();

        private void Awake() =>
            controllers.Add(this);


        private void OnDestroy() =>
            controllers.Remove(this);


        private void Start()
        {
            videoplayer.loopPointReached += CheckEnd;

            if (AudioManager.Instance != null) m_audioManager = AudioManager.Instance;
            else Debug.LogWarning("please add Audio Manager for the audio video output");

            videoplayer.clip = videoClip;

            StartCoroutine(GetAudioSourceCoroutine(() =>
            {
                if (playOnStart)
                {
                    Invoke("PlayVideo", .1f);
                };
            }));
        }

        private IEnumerator GetAudioSourceCoroutine(Action OnGetSource)
        {
            // Wait until AudioManager and videoSource are assigned
            while (AudioManager.Instance == null || AudioManager.Instance.videoSource == null)
            {
                Debug.Log("Waiting for AudioSource to be available...");
                yield return null; // Check again in the next frame
            }

            AudioSource videoAudioSource = AudioManager.Instance.videoSource;
            videoplayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoplayer.SetTargetAudioSource(0, videoAudioSource);
            Debug.Log("AudioSource successfully assigned to VideoPlayer.");

            OnGetSource.Invoke();
        }

        private void PlayPauseVisibility()
        {
            pauseButton.SetActive(videoplayer.isPlaying);
            playButton.SetActive(!videoplayer.isPlaying);
        }

        public void SkipTime(long frameStep)
        {
            videoplayer.frame += frameStep;
        }

        private void SetVideoPlayState(bool isPlaying)
        {
            foreach (var controller in controllers)
            {
                if (controller != this && controller.videoplayer.isPlaying)
                {
                    controller.videoplayer.Pause();
                    controller.UpdateUI(false);
                }
            }

            if (isPlaying)
            {
                videoplayer.Play();
                m_audioManager.musicSource.mute = true;
            }
            else
            {
                videoplayer.Pause();
                m_audioManager.musicSource.mute = false;
            }

            UpdateUI(isPlaying);
        }

        public void TogglePlayPause()
        {
            SetVideoPlayState(!videoplayer.isPlaying);
        }

        private void UpdateUI(bool isPlaying)
        {
            pauseButton.SetActive(isPlaying);
            playButton.SetActive(!isPlaying);
            panelThumbnail.SetActive(!isPlaying);
        }

        private void CheckEnd(VideoPlayer vp)
        {
            OnVideoFinished?.Invoke();
        }

        public void OnClickForwardTime() =>
            videoplayer.frame += 450;

        public void OnClickReverseTime() =>
            videoplayer.frame -= 450;

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetControllerVisibilty(true);
            PlayPauseVisibility();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetControllerVisibilty(false);
        }

        public void SetControllerVisibilty(bool visibilityState)
        {
            if (currentTween != null)
            {
                LeanTween.cancel(currentTween.uniqueId);
            }

            controllerGroup.interactable = visibilityState;
            controllerGroup.blocksRaycasts = visibilityState;

            if (visibilityState)
            {
                currentTween = LeanTween.alphaCanvas(controllerGroup, 1, 0.1f)
                                        .setEase(LeanTweenType.easeInOutQuad);
            }
            else
            {
                currentTween = LeanTween.alphaCanvas(controllerGroup, 0, hideScreenControlTime)
                                        .setEase(LeanTweenType.easeInOutQuad);
            }
        }
    }
}
