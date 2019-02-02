using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextInfoRequester : MonoBehaviour
{
    [TextArea]
    public string HoverDescription = "";
    private void Update()
    {
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (MouseWorldPos.x > transform.position.x - (transform.lossyScale.x / 2) &&
            MouseWorldPos.x < transform.position.x + (transform.lossyScale.x / 2) &&
            MouseWorldPos.y > transform.position.y - (transform.lossyScale.y / 2) &&
            MouseWorldPos.y < transform.position.y + (transform.lossyScale.y / 2)
            )
        {
            ContextInfo.RequestStatic(HoverDescription);
        }
    }
}
