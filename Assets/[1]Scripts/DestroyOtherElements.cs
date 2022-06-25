using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOtherElements : MonoBehaviour
{
    [SerializeField] private GameObject missEffect = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<Bullet>())
        {
            FindObjectOfType<RFX4_CameraShake>().PlayShake();
            Destroy(collision.collider.gameObject);
            GameObject effect = Instantiate(missEffect, collision.collider.transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }
}
