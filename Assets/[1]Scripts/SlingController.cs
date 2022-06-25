using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingController : MonoBehaviour
{
    [SerializeField] private Transform sling = null;
    [SerializeField] private Transform slingShot = null;

    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float rotationSlerpSpeed = 3f;
    [SerializeField] private Vector2 clampXRotation = new Vector2();
    [SerializeField] private Vector2 clampYRotation = new Vector2();
    [SerializeField] private Vector3 baseRotation = new Vector3();

    [SerializeField] private LineRenderer[] ropeRenderer = new LineRenderer[2];
    [SerializeField] private LineRenderer pathRenderer = null;

    [SerializeField] private Transform[] rightRopePositions = new Transform[2];
    [SerializeField] private Transform[] leftRopePositions = new Transform[2];

    [SerializeField] private List<GameObject> bulletList;
    [SerializeField] private GameObject currentBullet = null;
    [SerializeField] private Transform bulletPosition = null;

    private LimitCameraRotation cameraInput = null;
    private Vector3 originalPosition = new Vector3();
    private bool canShot = false;
    private bool canReturn = false;
    private float power = 0;
    private Vector3 speed;

    [SerializeField] private GameObject prewiev = null;

    public enum BulletType
    {
        Window,
        SmallWindow,
        Door,
        Roof,
        GarageDoor,
        WindowCircle,
        Truba
    }

    private void Start()
    {
        cameraInput = FindObjectOfType<LimitCameraRotation>();
        originalPosition = sling.localPosition;
        StartCoroutine(ChangeReturnFlag());
    }

    private void FixedUpdate()
    {
        SetupRopeRenderersPosition(0, leftRopePositions);
        SetupRopeRenderersPosition(1, rightRopePositions);
    }

    private void Update()
    {
        if (cameraInput == null || slingShot == null) return;

        float handleX = cameraInput.HandleAxisInputDelegate("Mouse X") * rotationSpeed;
        float handleY = cameraInput.HandleAxisInputDelegate("Mouse Y") * rotationSpeed;

        if (cameraInput.pressed)
        {
            if (currentBullet == null)
            {
                Destroy(prewiev);
                prewiev = null;

                GameObject bullet = bulletList[0];
                currentBullet = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
                currentBullet.transform.SetParent(bulletPosition);
                bulletList.Remove(bullet);
            }

            canShot = true;
            canReturn = false;

            slingShot.Rotate(-Vector3.up, handleX);
            slingShot.Rotate(-Vector3.right, handleY);

            Vector3 currentRotation = slingShot.rotation.eulerAngles;
            currentRotation.x = Mathf.Clamp(currentRotation.x, clampXRotation.x, clampXRotation.y);
            currentRotation.y = Mathf.Clamp(currentRotation.y, clampYRotation.x, clampYRotation.y);
            currentRotation.z = 0;
            slingShot.rotation = Quaternion.Euler(currentRotation);

            currentBullet.transform.rotation = slingShot.rotation;
            power = currentRotation.x;

            speed = -currentBullet.transform.forward * power * 5;
            ShowTrajectory(currentBullet.transform.localPosition, speed);

            Vector3 newSlingPos = sling.localPosition;
            newSlingPos.z = power * 2 * 0.01f;
            sling.localPosition = newSlingPos;
        }
        else
        {
            if (canShot)
            {
                Shot();
            }

            if (canReturn)
            {
                slingShot.rotation = Quaternion.Slerp(slingShot.rotation, Quaternion.Euler(baseRotation), rotationSlerpSpeed);
            }
        }
    }

    private void SetupRopeRenderersPosition(int index, Transform[] positions)
    {
        ropeRenderer[index].SetPosition(0, positions[0].position);
        ropeRenderer[index].SetPosition(1, positions[1].position);
    }

    private void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[15];
        pathRenderer.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = origin + speed * time + Physics.gravity * time * time / 2f;
        }

        pathRenderer.SetPositions(points);
    }

    private void Shot()
    {
        canShot = false;
        
        currentBullet.transform.parent = null;
        currentBullet.GetComponent<Bullet>().AddForceToBullet(power * 5);
        currentBullet = null;

        sling.localPosition = originalPosition;
        pathRenderer.positionCount = 0;
        StartCoroutine(ChangeReturnFlag());
    }

    private IEnumerator ChangeReturnFlag()
    {
        yield return new WaitForSeconds(0.05f);

        if (prewiev == null)
        {
            prewiev = Instantiate(bulletList[0], bulletPosition.position, Quaternion.identity);
            prewiev.transform.SetParent(bulletPosition);
            prewiev.transform.rotation = slingShot.rotation;
        }

        canReturn = true;
    }
}
