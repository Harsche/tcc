using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildManager{
    [MenuItem("Build/Build Windows")]
    public static void PerformWindowsBuild(){
        string[] scenes = GetScenePaths();
        string buildPath = Application.dataPath.Replace("Assets", "Builds/OquiraGame");
        Directory.CreateDirectory(buildPath);

        // Configure the build options
        var buildOptions = new BuildPlayerOptions{
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.StandaloneWindows
        };

        // Start the build process
        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        BuildSummary summary = report.summary;

        // Check if the build succeeded
        if (summary.result == BuildResult.Succeeded){ Debug.Log("Build succeeded!"); }
        else{ Debug.LogError("Build failed! Error message: " + summary); }

        string[] GetScenePaths(){
            string scenesPath = Application.dataPath;
            scenesPath += "/_Game/Scenes/Game Scenes";
            Debug.Log(scenesPath);
            string[] scenes = Directory.GetFiles(scenesPath);
            for (int index = 0; index < scenes.Length; index++){
                string scene = scenes[index];
                scenes[index] = scene.Replace(scenesPath, "");
            }
            return scenes;
        }
    }

    [MenuItem("Build/Debug")]
    public static void DebugPath(){
        string path = Application.dataPath.Replace("Assets", "Builds/OquiraGame");
        Debug.Log(path);
        Directory.CreateDirectory(path);
    }
}