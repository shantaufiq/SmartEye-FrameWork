using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

namespace SmartEye.Framework
{
    public class EnvironmentManager : MonoBehaviour
    {
        public static EnvironmentManager Instance;
        EnvAreaHandler currentArea;
        public List<EnvAreaHandler> EnvAreaHandlers;

        [Header("Sphere Area Settings")]
        // public Shader formatShader;
        private Material formatMaterial;
        public GameObject targetSphereArea;

        bool isChangingProcess = false;

        void OnApplicationQuit()
        {
            Debug.Log("Application ending after " + Time.time + " seconds");
            SetInt("areaIndex", 0);
            formatMaterial.color = new Color(1, 1, 1, 0);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // DontDestroyOnLoad(this);
            }
            // else if (Instance != null && !isChangingProcess)
            // {
            //     Destroy(this);
            // }

#if UNITY_EDITOR
            Material material = AssetDatabase.LoadAssetAtPath<Material>("Assets/SmartEye VR Plugin/Package Resources/Materials/TargetEnvironment360.mat");
            formatMaterial = material;
#endif
        }

        private void Start()
        {
            ChangeAreaByIndex(Getint("areaIndex"));
        }

        public void ChangeAreaByIndex(int index)
        {
            if (index > EnvAreaHandlers.Count)
            {
                Debug.LogWarning($"Index area {index} Doesn't available in EnvAreaHandlers List");
                return;
            }

            SetInt("areaIndex", index);
            StartCoroutine(nameof(LoadingScreen));
        }

        IEnumerator LoadingScreen()
        {
            isChangingProcess = true;

            HideEnv();
            LeanTween.alpha(targetSphereArea, 0, 2f).setOnComplete(() => StartCoroutine(nameof(CheckState)));

            yield return new WaitUntil(() => isChangingProcess == false);

            LeanTween.alpha(targetSphereArea, 1, 1f).setOnComplete(loadedComplete);
        }

        IEnumerator CheckState()
        {
            if (currentArea)
            {
                if (currentArea.isRestartOnExitArea)
                {
                    Debug.Log($"start load area {Getint("areaIndex")} with load scene ");
                    AsyncOperation opration = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

                    yield return new WaitUntil(() => opration.isDone);

                    SetUpMaterial(Getint("areaIndex"));
                }
                else
                {
                    Debug.Log($"start load area {Getint("areaIndex")} without load scene ");
                    SetUpMaterial(Getint("areaIndex"));
                }
            }
            else
            {
                SetUpMaterial(Getint("areaIndex"));
            }

            yield return null;
        }

        private void SetUpMaterial(int index)
        {

            // Material newMat = new Material(formatShader);
            // newMat.mainTexture = EnvAreaHandlers[index].areaTexture;
            // newMat.color = new Color(1, 1, 1, 0);

            formatMaterial.mainTexture = EnvAreaHandlers[index].areaTexture;
            formatMaterial.color = new Color(1, 1, 1, 0);

            // targetSphereArea.GetComponent<MeshRenderer>().material = newMat;
            targetSphereArea.GetComponent<MeshRenderer>().material = formatMaterial;

            Debug.Log($"Area number: {Getint("areaIndex")} is ready...");
            isChangingProcess = false;
        }

        private void loadedComplete()
        {
            EnvAreaHandlers[Getint("areaIndex")].SetActiveObjsState(true);

            currentArea = EnvAreaHandlers[Getint("areaIndex")];
        }

        private void HideEnv()
        {
            foreach (var item in EnvAreaHandlers)
            {
                item.SetActiveObjsState(false);
            }
        }

        private void SetInt(string KeyName, int Value)
        {
            PlayerPrefs.SetInt(KeyName, Value);
        }

        private int Getint(string KeyName)
        {
            return PlayerPrefs.GetInt(KeyName);
        }
    }
}