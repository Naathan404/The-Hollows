using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Confiner2DLoader : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner2D confiner;

    private void Start()
    {
        // Lắng nghe khi có scene được load Additively
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Additive)
            return;

        // Tìm Collider2D trong scene vừa load
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Collider2D collider = root.GetComponent<Collider2D>();
            if (collider != null)
            {
                confiner.BoundingShape2D = null;
                confiner.BoundingShape2D = collider;
                Debug.Log($"Set confiner collider to: {collider.name}");
                break;
            }
        }
    }
}