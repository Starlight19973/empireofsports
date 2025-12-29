using UnityEngine;

/// <summary>
/// First script for Empire of Sports project
/// Tests that everything is set up correctly
/// </summary>
public class HelloWorld : MonoBehaviour
{
    [Header("Debug Settings")]
    [Tooltip("Enable debug messages in console")]
    public bool enableDebug = true;

    void Start()
    {
        if (!enableDebug) return;

        Debug.Log("=== Empire of Sports 2.0 - Tennis Prototype ===");
        Debug.Log($"Unity Version: {Application.unityVersion}");
        Debug.Log($"Platform: {Application.platform}");
        Debug.Log($"Project Path: {Application.dataPath}");
        Debug.Log("Press SPACE to test input system");
        Debug.Log("=========================================");
    }

    void Update()
    {
        if (!enableDebug) return;

        // Test input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("<color=green>SPACE key pressed! Input system working correctly.</color>");
            Debug.Log("Ready to start coding tennis mechanics!");
        }

        // Test for ESC (useful for quitting in builds)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("<color=yellow>ESC pressed. In build this would quit the game.</color>");
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
