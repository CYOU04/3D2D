using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLevelLoader : MonoBehaviour
{
    [SerializeField] private string initialLevelScene = "Level01";
    [SerializeField] private string spawnPointName = "SpawnPoint";

    private IEnumerator Start()
    {
        Scene levelScene = SceneManager.GetSceneByName(initialLevelScene);

        if (!levelScene.isLoaded)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(
                initialLevelScene,
                LoadSceneMode.Additive
            );

            if (loadOperation == null)
            {
                Debug.LogError($"Could not load level scene '{initialLevelScene}'.");
                yield break;
            }

            yield return loadOperation;
            levelScene = SceneManager.GetSceneByName(initialLevelScene);
        }

        MovePlayerToSpawnPoint(levelScene);
    }

    private void MovePlayerToSpawnPoint(Scene levelScene)
    {
        if (!levelScene.IsValid() || !levelScene.isLoaded)
        {
            return;
        }

        Transform spawnPoint = null;

        foreach (GameObject rootObject in levelScene.GetRootGameObjects())
        {
            Transform candidate = FindChildByName(rootObject.transform, spawnPointName);
            if (candidate != null)
            {
                spawnPoint = candidate;
                break;
            }
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (spawnPoint == null || player == null)
        {
            Debug.LogWarning("Level loaded, but the Player or SpawnPoint could not be found.");
            return;
        }

        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        if (controller != null)
        {
            controller.enabled = true;
        }
    }

    private Transform FindChildByName(Transform current, string targetName)
    {
        if (current.name == targetName)
        {
            return current;
        }

        foreach (Transform child in current)
        {
            Transform result = FindChildByName(child, targetName);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
