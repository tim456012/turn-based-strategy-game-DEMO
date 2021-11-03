﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneState : BattleState
{
    private ConversationController conversationController;
    private ConversationData data;

    protected override void Awake()
    {
        base.Awake();
        conversationController = owner.GetComponentInChildren<ConversationController>();
        data = Resources.Load<ConversationData>("Conversation/IntroScene");
    }

    protected override void onDestroy()
    {
        base.onDestroy();
        if (data)
        {
            Resources.UnloadAsset(data);
        }
    }

    public override void Enter()
    {
        base.Enter();
        conversationController.Show(data);
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        ConversationController.completeEvent += OnCompleteConversation;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        ConversationController.completeEvent -= OnCompleteConversation;
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        base.OnFire(sender, e);
        conversationController.Next();
    }

    private void OnCompleteConversation(object sender, EventArgs e)
    {
        owner.ChangeState<SelectUnitState>();
    }
}