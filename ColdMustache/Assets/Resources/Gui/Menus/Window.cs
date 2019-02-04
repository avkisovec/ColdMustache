using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{

    Camera c;
    CamControlPixelPerfect cControl;

    float OrigRatio = 0;
    Vector3 OrigScale = new Vector3(1, 1, 1);

    public bool StayOnScreenPosition = true;
    public float ScreenXPercentRatio;
    public float ScreenYPercentRatio;

    public bool PixelPerfectScale = true;
    public float PixelScale = 1;

    Vector2 MouseLastScreenPos;
    Vector2 MouseDelta;

    Vector2 MouseLastWorldPos;
    Vector2 MouseWorldDelta;

    public GameObject TopBar;
    public GameObject CloseButton;
    
    public bool CurrentlyBeingMovedByMouse = false;

    public bool AmIActive = true; //false means window is "closed" (actually moves outside cameras view), therefore not reacting

    public Vector3 HidingCoordinates = new Vector3(-999, -999, -100);
    public Vector3 VisibleCoordinates = new Vector3(0, 0, -50); //this changes, so it returns to the place it was before closing

    public bool OpenableViaKey = false;
    public KeyCode OpenKey = KeyCode.Joystick8Button9;

    // Use this for initialization
    void Start()
    {

        c = Camera.main;
        cControl = UniversalReference.camControlPixelPerfect;

        MouseLastScreenPos = Input.mousePosition;
        MouseLastWorldPos = c.ScreenToWorldPoint(Input.mousePosition);
        MouseDelta = new Vector2(0, 0);
        MouseWorldDelta = new Vector2(0, 0);


        OrigRatio = c.orthographicSize / transform.localScale.y;

        if (StayOnScreenPosition)
        {
            ScreenXPercentRatio = c.WorldToScreenPoint(transform.position).x / c.pixelWidth;
            ScreenYPercentRatio = c.WorldToScreenPoint(transform.position).y / c.pixelHeight;
        }

        //OrigScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (AmIActive)
        {
            Vector2 MouseWorldPos = c.ScreenToWorldPoint(Input.mousePosition);

            MouseDelta = (Vector2)Input.mousePosition - MouseLastScreenPos;
            MouseWorldDelta = ((Vector2)c.ScreenToWorldPoint(Input.mousePosition) - MouseLastWorldPos) / 2;

            if (PixelPerfectScale)
            {
                transform.localScale = OrigScale / cControl.ActualTileScale * PixelScale;
            }
            else
            {
                transform.localScale = OrigScale * (c.orthographicSize / OrigRatio);
            }

            if (StayOnScreenPosition)
            {
                transform.position = new Vector3(
                    c.ScreenToWorldPoint(new Vector3(c.pixelWidth * ScreenXPercentRatio, 1)).x,
                    c.ScreenToWorldPoint(new Vector3(1, c.pixelHeight * ScreenYPercentRatio)).y,
                    transform.position.z
                    );
            }
            
            if (Input.GetKeyUp(KeybindManager.CloseMenus) || (
                Input.GetKeyDown(KeyCode.Mouse0) &&
                MouseWorldPos.x > CloseButton.transform.position.x - (CloseButton.transform.lossyScale.x / 2) &&
                MouseWorldPos.x < CloseButton.transform.position.x + (CloseButton.transform.lossyScale.x / 2) &&
                MouseWorldPos.y > CloseButton.transform.position.y - (CloseButton.transform.lossyScale.y / 2) &&
                MouseWorldPos.y < CloseButton.transform.position.y + (CloseButton.transform.lossyScale.y / 2)
                ))
            {
                Hide();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) &&
                MouseWorldPos.x > TopBar.transform.position.x - (TopBar.transform.lossyScale.x / 2) &&
                MouseWorldPos.x < TopBar.transform.position.x + (TopBar.transform.lossyScale.x / 2) &&
                MouseWorldPos.y > TopBar.transform.position.y - (TopBar.transform.lossyScale.y / 2) &&
                MouseWorldPos.y < TopBar.transform.position.y + (TopBar.transform.lossyScale.y / 2)
                )
            {
                CurrentlyBeingMovedByMouse = true;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                CurrentlyBeingMovedByMouse = false;
            }

            if (Input.mousePosition.x < 0 ||
                Input.mousePosition.y < 0 ||
                Input.mousePosition.x >= c.pixelWidth ||
                Input.mousePosition.y >= c.pixelHeight)
            {
                CurrentlyBeingMovedByMouse = false;
            }

            if (CurrentlyBeingMovedByMouse)
            {
                ScreenXPercentRatio = c.WorldToScreenPoint(transform.position + (Vector3)MouseWorldDelta).x / c.pixelWidth;
                ScreenYPercentRatio = c.WorldToScreenPoint(transform.position + (Vector3)MouseWorldDelta).y / c.pixelHeight;
            }

            MouseLastScreenPos = Input.mousePosition;
            MouseLastWorldPos = c.ScreenToWorldPoint(Input.mousePosition);


        }
        //not active
        else
        {
            if (OpenableViaKey)
            {
                if (Input.GetKeyUp(OpenKey))
                {
                    UnHide();
                }
            }
        }
    }

    public void Hide(){
        VisibleCoordinates = transform.position;
        transform.position = HidingCoordinates;
        AmIActive = false;
    }

    public void UnHide(){
        transform.position = VisibleCoordinates;
        AmIActive = true;
    }

}
