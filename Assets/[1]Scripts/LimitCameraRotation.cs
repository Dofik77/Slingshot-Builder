using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;


public class LimitCameraRotation : MonoBehaviour//, IPointerClickHandler, IDragHandler, IEndDragHandler
{
    public float TouchSensitivity_x = 10f;
    public float TouchSensitivity_y = 10f;

    private List<RaycastResult> results = new List<RaycastResult>();
    private PointerEventData pointerData;

    private int cameraFingerID;

    public bool pressed = false;

    void Start()
    {
        CinemachineCore.GetInputAxis = this.HandleAxisInputDelegate;
        pointerData = new PointerEventData(EventSystem.current);
        cameraFingerID = -1;

        GameObject[] c = GameObject.FindGameObjectsWithTag("CameraControl");
        for (int i = 0; i < c.Length; i++)
        {
            Debug.Log(c[i].name);
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (cameraFingerID > Input.touchCount - 1)
            {
                cameraFingerID = -1;
            }

            if (cameraFingerID >= 0 && Input.touches[cameraFingerID].phase == TouchPhase.Ended)
            {
                cameraFingerID = -1;
                pressed = false;
            }

            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].phase == TouchPhase.Began && cameraFingerID < 0)
                {
                    cameraFingerID = FindCameraTouch();
                    pressed = true;
                }
            }
        }

        //for deebug - comment this when build on Androind
        if (Input.GetMouseButtonDown(0))
        {
            float y = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
            if(y < 0.6f)
                pressed = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            float y = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
            if (y < 0.6f)
                pressed = false;
        }
        ///
    }


    public int FindCameraTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            pointerData.position = Input.touches[i].position;
            EventSystem.current.RaycastAll(pointerData, results);

            for(int j = 0; j < results.Count; j++)
            {
                if (results[j].gameObject.CompareTag("CameraControl"))
                {
                    return i;
                }
            }
        }

        return -1;
    }


    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public float HandleAxisInputDelegate(string axisName)
    {
        if (cameraFingerID < 0 && Input.touchCount > 0)
        {
            return 0f;
        }

        switch (axisName)
        {
            case "Mouse X":
                if (Input.touchCount > 0)
                {
                    //Is mobile touch
                    return Input.touches[cameraFingerID].deltaPosition.x / TouchSensitivity_x;
                        
                }
                else if (Input.GetMouseButton(0))
                {
                    // is mouse click
                    return Input.GetAxis("Mouse X");
                }
                break;
            case "Mouse Y":
                if (Input.touchCount > 0)
                {
                    //Is mobile touch
                    return -Input.touches[cameraFingerID].deltaPosition.y / TouchSensitivity_y;
                }
                else if (Input.GetMouseButton(0))
                {
                    // is mouse click
                    return Input.GetAxis(axisName);
                }
                break;
            default:
                Debug.LogError("Input <" + axisName + "> not recognized.", this);
                break;
        }

        return 0f;
    }
}
