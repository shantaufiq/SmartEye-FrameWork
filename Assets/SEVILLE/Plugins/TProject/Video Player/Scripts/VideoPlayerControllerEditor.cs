using UnityEngine;
using UnityEditor;

namespace TProject
{
#if UNITY_EDITOR
    [CustomEditor(typeof(VideoPlayerController))]
    public class VideoPlayerControllerEditor : Editor
    {
        SerializedProperty videoClipProp;
        SerializedProperty controlTimeProp;
        SerializedProperty playOnStartProp;
        SerializedProperty onVideoFinishedProp;

        SerializedProperty videoplayerProp;
        SerializedProperty panelThumbnailProp;
        SerializedProperty controllerGroupProp;
        SerializedProperty buttonPlayProp;
        SerializedProperty buttonPauseProp;
        SerializedProperty buttonReverseProp;
        SerializedProperty buttonForwardProp;
        SerializedProperty buttonReplayProp;


        private bool showComponentDependencies = false;

        private void OnEnable()
        {
            videoClipProp = serializedObject.FindProperty("videoClip");
            playOnStartProp = serializedObject.FindProperty("playOnStart");
            controlTimeProp = serializedObject.FindProperty("hideScreenControlTime");
            onVideoFinishedProp = serializedObject.FindProperty("OnVideoFinished");

            videoplayerProp = serializedObject.FindProperty("videoplayer");
            panelThumbnailProp = serializedObject.FindProperty("panelThumbnail");
            controllerGroupProp = serializedObject.FindProperty("controllerGroup");
            buttonPlayProp = serializedObject.FindProperty("buttonPlay");
            buttonPauseProp = serializedObject.FindProperty("buttonPause");
            buttonReverseProp = serializedObject.FindProperty("buttonReverse");
            buttonForwardProp = serializedObject.FindProperty("buttonForward");
            buttonReplayProp = serializedObject.FindProperty("buttonReplay");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(videoClipProp);
            EditorGUILayout.PropertyField(controlTimeProp);
            EditorGUILayout.PropertyField(playOnStartProp);
            EditorGUILayout.PropertyField(onVideoFinishedProp);

            // Toggle showComponents
            showComponentDependencies = EditorGUILayout.Toggle("Show Component Dependencies", showComponentDependencies);

            if (showComponentDependencies)
            {
                EditorGUILayout.PropertyField(videoplayerProp);
                EditorGUILayout.PropertyField(panelThumbnailProp);
                EditorGUILayout.PropertyField(controllerGroupProp);
                EditorGUILayout.PropertyField(buttonPlayProp);
                EditorGUILayout.PropertyField(buttonPauseProp);
                EditorGUILayout.PropertyField(buttonReverseProp);
                EditorGUILayout.PropertyField(buttonForwardProp);
                EditorGUILayout.PropertyField(buttonReplayProp);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}