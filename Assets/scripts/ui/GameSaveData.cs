using System;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public int slotIndex;
    public string savedAtText;
    public string sceneName;

    public Vector3 playerPosition;
    public Quaternion playerRotation;
}