using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
        }
    }

    [Header("Components")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator blackFadingTransition;

    private void Start()
    {
    }

    private void Update()
    {
        if (playerController.GetHP() <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        /// Reload scene
        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.5f);
        blackFadingTransition.SetTrigger("Load");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("BaseScene");
        SceneManager.LoadScene("Level#1", LoadSceneMode.Additive);
    }
}
