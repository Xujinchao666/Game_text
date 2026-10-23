using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jiuxin_Enter_Prompt : MonoBehaviour
{
    [Header("目标场景")]
    public string JiuxinyouguanScene;

    private bool playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"进入触发区，碰撞物体：{other.name}，Tag：{other.tag}");
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("✅玩家在门口，可以按E");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("❌玩家离开门口");
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"按下E，尝试加载场景：{JiuxinyouguanScene}");
            SceneManager.LoadScene(JiuxinyouguanScene);
        }
    }
}
