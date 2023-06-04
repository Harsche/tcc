using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            string projectPath = Application.dataPath.Replace("Assets", "");
            string scenesPath = Application.dataPath;
            scenesPath += "/_Game/Scenes/Game Scenes";
            List<string> files = Directory.GetFiles(scenesPath).ToList();
            List<string> scenesList = new();
            files.ForEach(file => {
                string relativePath = file.Replace(projectPath, "");
                if (file.Contains(".unity") && !file.Contains(".meta")){ scenesList.Add(relativePath); }
            });
            return scenesList.ToArray();
        }
    }

    [MenuItem("Build/Debug")]
    public static void DebugPath(){
        string path = Application.dataPath.Replace("Assets", "Builds/OquiraGame");
        Debug.Log(path);
        Directory.CreateDirectory(path);
    }
}