using System;
using System.Collections;
using System.Collections.Generic;
using Core.Manager;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{
    [field: SerializeField] public LevelManager LevelManager { get; set; }

    private void Start()
    {
        LevelManager.InitLevel();
    }
}
