using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenElement : MonoBehaviour
{
    [SerializeField] private SlingController.BulletType brokenObjectType;
    [SerializeField] private GameObject brokenObject = null;
    [SerializeField] private GameObject repairedObject = null;
    [SerializeField] private GameObject winEffect = null;

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet == null) return;

        if (bullet.bulletType == brokenObjectType)
        {
            brokenObject.SetActive(false);
            repairedObject.SetActive(true);

            Destroy(bullet.gameObject);

            Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f);
            Instantiate(winEffect, position, Quaternion.identity);
        }
    }
}
