using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class SplitGameSceneTool
{
    private const string SourceScenePath = "Assets/Scenes/GameScene.unity";
    private const string BootstrapScenePath = "Assets/Scenes/GameBootstrap.unity";
    private const string LevelScenePath = "Assets/Scenes/Level01.unity";

    private static readonly HashSet<string> LevelRootNames = new HashSet<string>
    {
        "Directional Light",
        "Global Volume",
        "Cube1",
        "Cube2",
        "Cube3",
        "Cube4",
        "Cube5",
        "Cube6",
        "Cube7",
        "Ground"
    };

    static SplitGameSceneTool()
    {
        EditorApplication.delayCall += CreateSplitScenesIfNeeded;
    }

    [MenuItem("Tools/3D2D/Rebuild Bootstrap And Level01")]
    public static void RebuildSplitScenes()
    {
        CreateSplitScenes(true);
    }

    private static void CreateSplitScenesIfNeeded()
    {
        if (File.Exists(BootstrapScenePath) && File.Exists(LevelScenePath))
        {
            return;
        }

        CreateSplitScenes(false);
    }

    private static void CreateSplitScenes(bool overwrite)
    {
        if (!File.Exists(SourceScenePath))
        {
            Debug.LogError($"Source scene not found: {SourceScenePath}");
            return;
        }

        if (!overwrite && (File.Exists(BootstrapScenePath) || File.Exists(LevelScenePath)))
        {
            Debug.LogError("Only one split scene exists. Use Tools/3D2D/Rebuild Bootstrap And Level01.");
            return;
        }

        SceneSetup[] previousSetup = EditorSceneManager.GetSceneManagerSetup();
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.IsValid() && activeScene.isDirty)
        {
            Debug.LogError("Scene split was not run because the active scene has unsaved changes.");
            return;
        }

        try
        {
            CreateBootstrapScene();
            CreateLevelScene();
            UpdateBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Created GameBootstrap and Level01 from GameScene.");
        }
        finally
        {
            if (previousSetup.Length > 0)
            {
                EditorSceneManager.RestoreSceneManagerSetup(previousSetup);
            }
        }
    }

    private static void CreateBootstrapScene()
    {
        Scene scene = EditorSceneManager.OpenScene(SourceScenePath, OpenSceneMode.Single);

        foreach (GameObject rootObject in scene.GetRootGameObjects())
        {
            if (LevelRootNames.Contains(rootObject.name))
            {
                Object.DestroyImmediate(rootObject);
            }
        }

        GameObject loaderObject = new GameObject("BootstrapLevelLoader");
        loaderObject.AddComponent<BootstrapLevelLoader>();

        EditorSceneManager.SaveScene(scene, BootstrapScenePath);
    }

    private static void CreateLevelScene()
    {
        Scene scene = EditorSceneManager.OpenScene(SourceScenePath, OpenSceneMode.Single);
        Vector3 playerPosition = Vector3.zero;
        Quaternion playerRotation = Quaternion.identity;

        GameObject player = scene.GetRootGameObjects().FirstOrDefault(root => root.CompareTag("Player"));
        if (player != null)
        {
            playerPosition = player.transform.position;
            playerRotation = player.transform.rotation;
        }

        foreach (GameObject rootObject in scene.GetRootGameObjects())
        {
            if (!LevelRootNames.Contains(rootObject.name))
            {
                Object.DestroyImmediate(rootObject);
            }
        }

        GameObject spawnPoint = new GameObject("SpawnPoint");
        spawnPoint.transform.SetPositionAndRotation(playerPosition, playerRotation);

        EditorSceneManager.SaveScene(scene, LevelScenePath);
    }

    private static void UpdateBuildSettings()
    {
        List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes
            .Where(scene => scene.path != SourceScenePath
                && scene.path != BootstrapScenePath
                && scene.path != LevelScenePath)
            .ToList();

        int insertionIndex = scenes.FindIndex(scene => scene.path.EndsWith("/Result.unity"));
        if (insertionIndex < 0)
        {
            insertionIndex = scenes.Count;
        }

        scenes.Insert(insertionIndex, new EditorBuildSettingsScene(BootstrapScenePath, true));
        scenes.Insert(insertionIndex + 1, new EditorBuildSettingsScene(LevelScenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }
}
