using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryManager : MonoBehaviour
{
    public static ItemInventoryManager Instance;
    // 存储玩家持有的所有信件
    public List<LetterID> letterList = new List<LetterID>();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 【新版枚举接口】给InteractTrigger脚本调用
    public bool CheckHasLetter(LetterID id)
    {
        return letterList.Contains(id);
    }

    public void RemoveLetter(LetterID id)
    {
        if (letterList.Contains(id))
        {
            letterList.Remove(id);
            Debug.Log($"已移除信件：{id}");
        }
        else
        {
            Debug.LogWarning($"背包没有该信件，移除失败：{id}");
        }
    }

    // 【旧版int兼容接口】保留你原来的方法，防止其他旧脚本报错
    public bool CheckHasLetter(int letterId)
    {
        // 将数字转成枚举判断
        LetterID target = (LetterID)letterId;
        return letterList.Contains(target);
    }

    // 可选：拾取信件加入背包
    public void AddLetter(LetterID id)
    {
        if (!letterList.Contains(id))
        {
            letterList.Add(id);
            Debug.Log($"拾取信件：{id}");
        }
    }
}