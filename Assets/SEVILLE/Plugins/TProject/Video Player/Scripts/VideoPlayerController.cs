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
        [SerializeField] private GameObject panelThumbnail;

        [Space(3f)]
        [SerializeField] private CanvasGroup controllerGroup;
        [SerializeField] private Slider sliderProgress;
        [SerializeField] private GameObject activeControllerGroup;
        [SerializeField] private GameObject buttonPlay;
        [SerializeField] private GameObject buttonPause;
        [SerializeField] private GameObject buttonReverse;
        [SerializeField] private GameObject buttonForward;
        [SerializeField] private GameObject buttonReplay;

        private AudioManager m_audioManager;
        private LTDescr currentTween; // Reference to the current LeanTween animation

        private static List<VideoPlayerController> controllers = new List<VideoPlayerController>();

        private void Awake() =>
            controllers.Add(this);


        private void OnDestroy() =>
            controllers.Remove(this);


        private void Start()
        {
            videoplayer.loopPointReached += CheckerOnVideoEnd;

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

            SetupButtonFuction();
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

        private void SetupButtonFuction()
        {
            buttonPlay.GetComponent<Button>().onClick.AddListener(() => TogglePlayPause());
            buttonPause.GetComponent<Button>().onClick.AddListener(() => TogglePlayPause());
            buttonReverse.GetComponent<Button>().onClick.AddListener(() => OnClickReverseTime());
            buttonForward.GetComponent<Button>().onClick.AddListener(() => OnClickForwardTime());
            buttonReplay.GetComponent<Button>().onClick.AddListener(() => OnClickReplay());
        }

        private void PlayPauseVisibility()
        {
            buttonPause.SetActive(videoplayer.isPlaying);
            buttonPlay.SetActive(!videoplayer.isPlaying);
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

        private void UpdateUI(bool isPlaying)
        {
            buttonPause.SetActive(isPlaying);
            buttonPlay.SetActive(!isPlaying);
            panelThumbnail.SetActive(!isPlaying);
        }

        private void CheckerOnVideoEnd(VideoPlayer vp)
        {
            OnVideoFinished?.Invoke();

            panelThumbnail.SetActive(true);
            buttonReplay.SetActive(true);
            activeControllerGroup.SetActive(false);
        }

        #region Button-Function
        public void TogglePlayPause()
        {
            SetVideoPlayState(!videoplayer.isPlaying);
        }

        public void OnClickForwardTime() =>
            videoplayer.frame += 450;

        public void OnClickReverseTime() =>
            videoplayer.frame -= 450;

        public void OnClickReplay()
        {
            if (videoplayer != null)
            {
                videoplayer.Stop();
                videoplayer.time = 0;
                videoplayer.Play();

                activeControllerGroup.SetActive(true);
                UpdateUI(videoplayer.isPlaying);

                buttonReplay.SetActive(false);
            }
        }

        #endregion Button-Function

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetControllerVisibilty(true);
            PlayPauseVisibility();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (videoplayer.isPlaying)
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
