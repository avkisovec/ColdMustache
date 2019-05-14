using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button01 : MonoBehaviour
{
    
    public bool IsHovered;

    public Transform Area;

    //0 = not hovered, 1 = hovered for some time (animation complete)
    public float HoverPhase = 0;

    float HoverPhaseChangePerSecond = 5;

    Camera c;

    public SpriteRenderer CutoutMakerSr;
    public SpriteRenderer HighlightTopSr;
    public SpriteRenderer HightlightBotSr;

    public Transform Bg;

    float BgOriginalXScale;

    void Start()
    {
        c = Camera.main;
        BgOriginalXScale = Bg.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHover();

        if(IsHovered) HoverPhase += Time.deltaTime * HoverPhaseChangePerSecond;        
        else HoverPhase -= Time.deltaTime * HoverPhaseChangePerSecond;        
        if(HoverPhase > 1) HoverPhase = 1;
        else if(HoverPhase < 0) HoverPhase = 0;

        CutoutMakerSr.color = new Color(CutoutMakerSr.color.r,CutoutMakerSr.color.g,CutoutMakerSr.color.b,HoverPhase);
        
        HightlightBotSr.color = HighlightTopSr.color = new Color(HighlightTopSr.color.r, HighlightTopSr.color.g, HighlightTopSr.color.b, HoverPhase);
        
        Bg.transform.localScale = new Vector3(BgOriginalXScale+HoverPhase/2, Bg.transform.localScale.y, 1);

    }

    void CheckHover(){
        Vector2 MouseWorldPos = c.ScreenToWorldPoint(Input.mousePosition);
        if (MouseWorldPos.x > Area.position.x - (Area.lossyScale.x / 2) &&
            MouseWorldPos.x < Area.position.x + (Area.lossyScale.x / 2) &&
            MouseWorldPos.y > Area.position.y - (Area.lossyScale.y / 2) &&
            MouseWorldPos.y < Area.position.y + (Area.lossyScale.y / 2)
            )
        {
            IsHovered = true;
        }
        else
        {
            IsHovered = false;
        }
    }
}
