using System.Collections.Generic;
using UnityEngine;

public class TargetablePart : MonoBehaviour
{
    public static List<TargetablePart> activeParts = new List<TargetablePart>();

    void OnEnable()
    {
        activeParts.Add(this);
    }

    void OnDisable()
    {
        activeParts.Remove(this);
    }
}