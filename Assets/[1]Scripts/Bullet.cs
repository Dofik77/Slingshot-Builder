using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRb = null;
    public SlingController.BulletType bulletType;

    private bool shoodIncSize = false;
    [SerializeField] private Vector3 sizeInc = new Vector3(0.001f, 0.001f, 0.001f);
    [SerializeField] private float sizeIncDelay = 2f;

    public void AddForceToBullet(float force)
    {
        bulletRb = GetComponent<Rigidbody>();
        bulletRb.isKinematic = false;
        bulletRb.AddForce(-transform.forward * force, ForceMode.Impulse);
        shoodIncSize = true;
    }

    public void Update()
    {
        if (shoodIncSize)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, sizeInc, sizeIncDelay * Time.deltaTime);
        }
    }
}
