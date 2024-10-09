using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dial Visual Data", order = 1)]
public class Dial_VisualData_SO : SerializedScriptableObject
{
    public List<Tuple<Material, Material>> VisualDataList;
    public Material OutlineMaterial;
}
