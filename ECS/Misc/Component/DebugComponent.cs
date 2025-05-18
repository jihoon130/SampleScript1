using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct DebugComponent : IComponentData
{
    public int Value;
}
