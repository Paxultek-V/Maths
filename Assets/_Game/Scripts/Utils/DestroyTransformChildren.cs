using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DestroyTransformChildren
{
    public static void DestroyAllTransformChildren(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            if (!Application.isPlaying)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
            else
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
