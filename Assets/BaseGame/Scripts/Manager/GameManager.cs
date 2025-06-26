using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
//using SDK;
using Sirenix.OdinInspector;
//using TurnBaseField;
using TW.UGUI.Core.Activities;
using TW.UGUI.Core.Modals;
using TW.UGUI.Core.Views;
using TW.Utility.CustomType;
using TW.Utility.DesignPattern;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //[field: SerializeField] public AnimationCurve TimeScaleCurve { get; private set; }
    //private CancellationTokenSource TimeScaleCancellationTokenSource { get; set; }
    //private float TimeScaleProcess { get; set; }
    private bool IsOpeningAllChest { get; set; } = false;
    private bool IsOpenChestCompleted { get; set; } = false;
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
}
