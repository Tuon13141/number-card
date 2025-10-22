using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolingConfig", menuName = "Scriptable Objects/PoolingConfig")]
public class PoolingConfig : ScriptableObject
{
    [Tooltip("Danh sách prefab được quản lý bởi PoolingManager")]
    public List<MonoBehaviour> prefabs = new List<MonoBehaviour>();
}
