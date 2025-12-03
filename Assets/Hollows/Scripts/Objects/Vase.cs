using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour
{
    private Animator animator;
    public List<RewardedItem> rewardedItems;
    [Serializable]
    public class RewardedItem
    {
        public GameObject item;
        public float spawnRatio;
    }


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slash"))
        {
            Debug.Log("1");
            StartCoroutine(WaitToBeDisappeared());
        }
    }

    IEnumerator WaitToBeDisappeared()
    {
        Debug.Log("2");
        animator.SetTrigger("Break");
        SpawnRewardedItem();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void SpawnRewardedItem()
    {
        float ratio = UnityEngine.Random.Range(0f, 1f);
        for (int i = 0; i < rewardedItems.Count; i++)
        {
            if (ratio < rewardedItems[i].spawnRatio)
            {
                Instantiate(rewardedItems[i].item, this.transform.position, Quaternion.identity);
                return;
            }
            ratio -= rewardedItems[i].spawnRatio;
        }
    }
}
