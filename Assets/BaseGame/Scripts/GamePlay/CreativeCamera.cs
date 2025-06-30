using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CreativeCamera : MonoBehaviour
{
    public List<CreativeCamStat> camStats = new List<CreativeCamStat>();

    public void StartCreativeCam()
    {
        
    }
}

[System.Serializable]
public class CreativeCamStat
{
    public Vector3 camPos;
    [FormerlySerializedAs("camSize")] public float camSizeScrew;
    public float camSizeAnim;
    public SelectableObject targetObject;
}
