// using 统一放在最顶部
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 信件ID枚举（替换原来int targetLetterId，避免数字填错）
public enum LetterID
{
    None = 0,
    Letter_01_TailorLove,
    Letter_02_Sailor,
    Letter_03_Grandma
}

// 剧情分支锁枚举（纯英文，无中文标识符，解决编译报错）
public enum StoryChoice
{
    None,
    Choice_Letter01_ToTailor,
    Choice_Letter01_ToSister,
    Choice_Letter02_DeliverSailor,
    Choice_End_Gentle,
    Choice_End_Empty
}

// 交互类型枚举
public enum InteractType
{
    GiveLetter,    // 送信交互
    NormalTalk     // 普通对话NPC
}

public class InteractTrigger : MonoBehaviour
{
    [Header("剧情交互基础配置｜策划面板填写")]
    public InteractType interactType;
    [Tooltip("主线剧情Yarn节点名称")]
    public string yarnNodeName;
    [Tooltip("重复闲聊对话节点（已完成剧情后触发）")]
    public string yarnRepeatNode;
    [Tooltip("无信件提示对话节点")]
    public string yarnNoLetterNode;
    [Tooltip("章节未解锁提示对话节点")]
    public string yarnLockTipNode;

    [Tooltip("回忆动画Clip名称，无动画留空")]
    public string recallAnimClipName;
    [Tooltip("镜头特写目标，为空默认自身")]
    public Transform cameraFocusTarget;
    [Tooltip("绑定这条交互对应的剧情分支锁")]
    public StoryChoice bindChoice;

    [Header("前置解锁限制：需要完成该分支才能交互")]
    public StoryChoice needCompleteChoice;

    [Header("信件投递专用配置")]
    public bool needTargetLetter;
    public LetterID targetLetterId;

    // 移除本地bool _alreadyTriggered，全程读取存档判断

    // 玩家交互按键触发入口（Player2D角色脚本调用）
    public void OnPlayerInteract()
    {
        // 管理器空防护
        if (YarnDialogueManager.Instance == null || StoryTriggerManager.Instance == null)
        {
            Debug.LogError("场景缺少GameManager及对话/剧情管理器！");
            return;
        }

        // ========== 1. 章节前置解锁判断 ==========
        if (needCompleteChoice != StoryChoice.None)
        {
            bool finishPre = StoryBranchManager.Instance.HasMadeChoice(needCompleteChoice);
            if (!finishPre)
            {
                if (!string.IsNullOrEmpty(yarnLockTipNode))
                    YarnDialogueManager.Instance.StartDialogue(yarnLockTipNode);
                return;
            }
        }

        // ========== 2. 判断剧情是否已经完成，播放重复闲聊 ==========
        bool storyFinished = false;
        if (bindChoice != StoryChoice.None)
            storyFinished = StoryBranchManager.Instance.HasMadeChoice(bindChoice);

        if (storyFinished)
        {
            if (!string.IsNullOrEmpty(yarnRepeatNode))
                YarnDialogueManager.Instance.StartDialogue(yarnRepeatNode);
            return;
        }

        // ========== 3. 送信交互：校验信件 ==========
        bool passLetterCheck = true;
        if (interactType == InteractType.GiveLetter && needTargetLetter)
        {
            if (ItemInventoryManager.Instance == null)
            {
                Debug.LogError("未挂载道具背包管理器 ItemInventoryManager");
                return;
            }
            bool hasLetter = ItemInventoryManager.Instance.CheckHasLetter(targetLetterId);
            if (!hasLetter)
            {
                passLetterCheck = false;
                if (!string.IsNullOrEmpty(yarnNoLetterNode))
                    YarnDialogueManager.Instance.StartDialogue(yarnNoLetterNode);
            }
        }
        if (!passLetterCheck) return;

        // ========== 4. 送信成功，移除背包信件 ==========
        if (interactType == InteractType.GiveLetter && needTargetLetter)
        {
            ItemInventoryManager.Instance.RemoveLetter(targetLetterId);
        }

        // ========== 5. 启动完整剧情流程（镜头、动画、主线对话、存档分支） ==========
        StoryTriggerManager.Instance.TriggerStory(
            yarnNode: yarnNodeName,
            recallAnimName: recallAnimClipName,
            focusTarget: cameraFocusTarget ?? transform,
            choice: bindChoice
        );
    }

    // 2D碰撞：玩家靠近弹出交互提示图标
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && UIIndicatorManager.Instance != null)
        {
            UIIndicatorManager.Instance.ShowInteractTip(transform.position);
        }
    }

    // 2D碰撞：玩家离开隐藏提示
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && UIIndicatorManager.Instance != null)
        {
            UIIndicatorManager.Instance.HideInteractTip();
        }
    }
}