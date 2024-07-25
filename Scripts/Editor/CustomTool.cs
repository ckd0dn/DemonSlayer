using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class CustomTool : EditorWindow
{
    [MenuItem("MyTool/SceneMover")]
    static void MyMenu()
    {
        GetWindow<CustomTool>();
    }

    private void OnGUI()
    {
        ChangeScene();
    }

    private void ChangeScene()
    {
        EditorGUILayout.LabelField("æ¿ ¿Ãµø πˆ∆∞");

        if (GUILayout.Button("Ω√¿€ æ¿"))
        {
            if (Application.isPlaying)
                SceneManager.LoadScene("StartScene");
            else
                EditorSceneManager.OpenScene("Assets/Scenes/StartScene.unity");
        }

        if (GUILayout.Button("∞‘¿” æ¿"))
        {
            if (Application.isPlaying)
                SceneManager.LoadScene("GameScene");
            else
                EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity");
        }

        if (GUILayout.Button("¿Œ∆Æ∑Œ æ¿"))
        {
            if (Application.isPlaying)
                SceneManager.LoadScene("IntroScene");
            else
                EditorSceneManager.OpenScene("Assets/Scenes/IntroScene.unity");
        }

        if (GUILayout.Button("YCH æ¿"))
        {
            if (Application.isPlaying)
                SceneManager.LoadScene("YCH");
            else
                EditorSceneManager.OpenScene("Assets/Scenes/YCH.unity");
        }

        if (GUILayout.Button("PCW æ¿"))
        {
            if (Application.isPlaying)
                SceneManager.LoadScene("PCW");
            else
                EditorSceneManager.OpenScene("Assets/Scenes/PCW.unity");
        }

        if (GUILayout.Button("KYS æ¿"))
        {
            if (Application.isPlaying)
                SceneManager.LoadScene("KYS");
            else
                EditorSceneManager.OpenScene("Assets/Scenes/KYS.unity");
        }

        if (GUILayout.Button("PSJ æ¿"))
        {
            if (Application.isPlaying)
                SceneManager.LoadScene("PSJ");
            else
                EditorSceneManager.OpenScene("Assets/Scenes/PSJ.unity");
        }
    }
}
