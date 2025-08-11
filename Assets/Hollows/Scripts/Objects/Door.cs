using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private string levelSceneToLoad;
    [SerializeField] private GameObject button;
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionDuration;

    private void Start()
    {
        if (button.activeInHierarchy)
        {
            button.SetActive(false);
        }
    }

    private void Update()
    {
        if(button.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.S))
                StartCoroutine(LoadNextScene());
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            button.SetActive(false);
        }
    }

    IEnumerator LoadNextScene()
    {
        transition.SetTrigger("Load");
        yield return new WaitForSeconds(transitionDuration);
        SceneManager.LoadScene("BaseScene");
        SceneManager.LoadScene(levelSceneToLoad, LoadSceneMode.Additive);
    }
}
