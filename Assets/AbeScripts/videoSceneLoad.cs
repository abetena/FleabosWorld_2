using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class videoSceneLoad : MonoBehaviour
{
    // This script plays a video and transitions to a specified scene when the video ends.
    // Designed for Unity projects, including WebGL builds. Useful for intros or cutscenes.
    //
    // Setup Instructions:
    // 1. Create a folder named "StreamingAssets" inside your "Assets" directory and place your video file there.
    // 2. In your scene, create a Canvas (Screen Space - Overlay) and add a Raw Image component to it. Resize it to fit the screen.
    // 3. Create a new Render Texture in your project. Set its size to match your video resolution (e.g., 1920 x 1080).
    // 4. Assign this Render Texture to the Raw Image's "Texture" field.
    // 5. Add an empty GameObject to your scene (e.g., "VideoManager") and attach a Video Player component to it.
    //    - Set the Video Player's "Source" to "URL" and the "Target Texture" to your new Render Texture.
    // 6. Attach this script to the VideoManager GameObject and fill in the public fields:
    //    - "Video File Name": Enter the name of your video file (e.g., "Intro.mp4").
    //    - "Next Scene Name": Enter the name of the scene to load after the video ends.

    private VideoPlayer videoPlayer;

    [Header("Video Settings")]
    [Tooltip("The name of the video file in the StreamingAssets folder (e.g., Unitylogo.mp4)")]
    public string videoFileName;  // Specify the video file name in the Inspector

    [Header("Scene Settings")]
    [Tooltip("The name of the scene to load after the video ends")]
    public string nextSceneName;  // Specify the next scene name in the Inspector

    // Start is called before the first frame update
    void Start()
    {
        // Attempt to get the VideoPlayer component
        videoPlayer = GetComponentInChildren<VideoPlayer>();

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not found in children. Please ensure a VideoPlayer is attached to the GameObject.");
            return;  // Prevent further execution if VideoPlayer is missing
        }

        // Construct the full path to the video file
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        if (!System.IO.File.Exists(videoPath))
        {
            Debug.LogError($"Video file not found at path: {videoPath}. Ensure the file exists in the StreamingAssets folder.");
            return;  // Prevent execution if the video file is missing
        }

        videoPlayer.url = videoPath;

        // Subscribe to the event when the video ends
        videoPlayer.loopPointReached += LoadScene;
    }

    // Loads the next scene when the video ends
    void LoadScene(UnityEngine.Video.VideoPlayer vp)
    {
        // Check if the scene exists in the build settings
        if (!SceneExists(nextSceneName))
        {
            Debug.LogError($"Scene '{nextSceneName}' does not exist in the build settings. Please ensure it is added.");
            return;  // Prevent scene loading if the scene does not exist
        }

        SceneManager.LoadScene(nextSceneName);
    }

    // Utility function to check if a scene exists in the build settings
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
            {
                return true;
            }
        }
        return false;
    }

}
