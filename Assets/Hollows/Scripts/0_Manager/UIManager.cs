using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Heart HUD")]
    [SerializeField] private List<GameObject> emptyHearts = new List<GameObject>();
    [SerializeField] private List<GameObject> hearts = new List<GameObject>();
    [SerializeField] private GameObject emptyHeartPrefab;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private ObjectPooler emptyHeartPool;
    [SerializeField] private ObjectPooler heartPool;
    private PlayerController player;
    public static UIManager Instance;   // singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();

        UpdateHeartHUD();
    }

    private void UpdateHeartHUD()
    {
        for (int i = 0; i < player.GetMaxHP(); i++)
        {
            emptyHearts.Add(emptyHeartPool.GetObject());
        }
        for (int i = 0; i < player.GetHP(); i++)
        {
            hearts.Add(heartPool.GetObject());
        }
    }

    /// Update Heart UI
    public void AddHeart()
    {
        hearts.Add(heartPool.GetObject());
    }
    public void SubtractHeart()
    {
        hearts[hearts.Count - 1].GetComponent<Animator>().SetTrigger("LostHeart");
        StartCoroutine(RemoveHeart());
    }
    IEnumerator RemoveHeart()
    {
        yield return new WaitForSeconds(0.2f);
        heartPool.ReturnToPool(hearts[hearts.Count - 1]);
        hearts.RemoveAt(hearts.Count - 1);
    }
}
