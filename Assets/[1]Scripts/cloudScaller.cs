using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudScaller : MonoBehaviour
{
    [SerializeField] private Transform cloudParent = null;
    [SerializeField] private Transform cloudParent1 = null;
    [SerializeField] private Transform cloudParent2 = null;
    [SerializeField] private Transform background = null;
    [SerializeField] private Vector3 backgroundScale = new Vector3();

    private void FixedUpdate()
    {
        ResetCloudSize(cloudParent);
        ResetCloudSize(cloudParent1);
        ResetCloudSize(cloudParent2);

        if (background.localScale.x < backgroundScale.x)
        {
            background.localScale = backgroundScale;
        }
    }

    private void ResetCloudSize(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.localScale.x < 2)
            {
                child.localScale = new Vector3(2, 2, 2);
            }
        }
    }
}
