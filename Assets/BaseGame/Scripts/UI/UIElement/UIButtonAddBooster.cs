using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonAddBooster : Button
{
    [field: SerializeField] public GameResource.Type BoosterType { get; private set; }

}
