using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [Header("Child References")]
    public UiEnemyInfoController EnemyInfo;

    void Awake()
    {
        SubscribeToMessages();
    }

    void Start()
    {
        EnemyInfo.gameObject.SetActive(false);
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<ShowEnemyInfoMessage>(ShowEnemyInfo);
        gameObject.Subscribe<CloseEnemyInfoMessage>(CloseEnemyInfo);
    }

    private void ShowEnemyInfo(ShowEnemyInfoMessage msg)
    {
        EnemyInfo.gameObject.SetActive(true);
    }

    private void CloseEnemyInfo(CloseEnemyInfoMessage msg)
    {
        EnemyInfo.Clear();
        EnemyInfo.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }
}
