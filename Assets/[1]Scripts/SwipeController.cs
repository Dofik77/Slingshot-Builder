using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField] private Transform currentHouse = null;
    [SerializeField] private Vector3 rotationTarget;
    [SerializeField] private float rotationSlerpSpeed = 3f;

    private void Start()
    {
        rotationTarget = currentHouse.rotation.eulerAngles;
    }

    public void RotateHouse(bool direction)
    {
        float angle = direction ? rotationTarget.y + 90 : rotationTarget.y - 90;
        rotationTarget = new Vector3(rotationTarget.x, angle, rotationTarget.z);
    }

    private void Update()
    {
        currentHouse.rotation = Quaternion.Slerp(currentHouse.rotation, Quaternion.Euler(rotationTarget), rotationSlerpSpeed);
    }

    /*
    private Vector2 startTouchPosition;
    private Vector2 currentPosition;
    private Vector2 endTouchPosition;
    private bool stopTouch = false;

    public float swipeRange;
    public float tapRange;

    private List<RaycastResult> results = new List<RaycastResult>();
    private PointerEventData pointerData;

     Update is called once per frame
    //void Update()
    {
        Swipe();
    }

    public void Swipe()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            int index = FindSwipeTouch();
            if (index == -1) return;

            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentPosition = Input.GetTouch(0).position;
            Vector2 Distance = currentPosition - startTouchPosition;

            if (!stopTouch)
            {

                if (Distance.x < -swipeRange)
                {
                    //outputText.text = "Left";
                    print("left");
                    stopTouch = true;
                }
                else if (Distance.x > swipeRange)
                {
                    //outputText.text = "Right";
                    stopTouch = true;
                }
                else if (Distance.y > swipeRange)
                {
                    //outputText.text = "Up";
                    stopTouch = true;
                }
                else if (Distance.y < -swipeRange)
                {
                    //outputText.text = "Down";
                    stopTouch = true;
                }
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopTouch = false;

            endTouchPosition = Input.GetTouch(0).position;

            Vector2 Distance = endTouchPosition - startTouchPosition;

            if (Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange)
            {
                //outputText.text = "Tap";
            }
        }
    }

    public int FindSwipeTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            pointerData.position = Input.touches[i].position;
            EventSystem.current.RaycastAll(pointerData, results);

            for (int j = 0; j < results.Count; j++)
            {
                if (results[j].gameObject.CompareTag("SwipeControl"))
                {
                    return i;
                }
            }
        }

        return -1;
    }
    */
}
