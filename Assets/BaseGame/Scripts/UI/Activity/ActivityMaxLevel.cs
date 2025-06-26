using System.Collections;
using System.Collections.Generic;
using TW.UGUI.Core.Activities;
using UnityEngine;

public class ActivityMaxLevel : Activity
{
    [field: SerializeField] public ActivityMaxLevelContext.UIPresenter UIPresenter { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        // The lifecycle event of the view will be added with priority 0.
        // Presenters should be processed after the view so set the priority to 1.
        AddLifecycleEvent(UIPresenter, 1);
    }
}
