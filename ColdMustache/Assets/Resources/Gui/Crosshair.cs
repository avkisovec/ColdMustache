using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {
    
    public float EffectiveRange = 10; // crosshair is very close until effective range, then starts spreading apart, until 2x effective range where it disappears
                //between 2x and 2.5x effective range, edges start disappearing

    public float EdgeDistanceInMenu = 999999f; //originally 0.2f
    public float EdgeDistanceMenuClick = 999999f; //originally 0.25f
    public float EdgeDistanceEffectiveRange = 0.35f;
    public float EdgeSpreadAt2xEffective = 2;

    public float EdgeDistanceMinInaccuracy = 0.5f;
    public float EdgeDistancePerPOintOfInaccuracy = 2.5f;

    public Color DefaultColor;
    public Color IneffectiveRangeColor;
    public Color MenuColor;
    public Color InvisibleColor;

    public Transform Edge0;
    public Transform Edge1;
    public Transform Edge2;
    public Transform Edge3;

    public SpriteRenderer Dot_sr;
    public SpriteRenderer Edge0_sr;
    public SpriteRenderer Edge1_sr;
    public SpriteRenderer Edge2_sr;
    public SpriteRenderer Edge3_sr;

    public Sprite Dot;
    public Sprite PointerEmpty;
    public Sprite PointerFull;
        
    // Use this for initialization
    void Start () {
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {


        //if mouse is outside the window (on the vertical axis, such as being on the top bar hovering close btn)
        if((Screen.height - Input.mousePosition.y) < 0 || Input.mousePosition.y < 0)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

        if(TemporaryColorRemaining > 0)
        {
            TemporaryColorRemaining -= Time.deltaTime;
        }
        
        Vector3 something = UniversalReference.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(something.x, something.y, transform.position.z);

        float Distance = ((Vector2)UniversalReference.PlayerObject.transform.position - UniversalReference.MouseWorldPos).magnitude;
        if (MouseInterceptor.IsMouseHoveringMenu())
        {
            Edge0_sr.color = MenuColor;
            Edge1_sr.color = MenuColor;
            Edge2_sr.color = MenuColor;
            Edge3_sr.color = MenuColor;
            Dot_sr.color = MenuColor;

            //hovering menu and being clicked/held down
            if (MouseInterceptor.MouseBeingIntercepted == true)
            {
                SetEdgesToDistance(EdgeDistanceMenuClick);
                Dot_sr.sprite = PointerFull;
            }
            //just hovering
            else
            {
                SetEdgesToDistance(EdgeDistanceInMenu);
                Dot_sr.sprite = PointerEmpty;
            }
        }
        else
        {
            if(TemporaryColorRemaining > 0)
            {
                Edge0_sr.color = Edge1_sr.color = Edge2_sr.color = Edge3_sr.color = Dot_sr.color = TemporaryColor;
            }
            else
            {
                Edge0_sr.color = Edge1_sr.color = Edge2_sr.color = Edge3_sr.color = Dot_sr.color = DefaultColor;
            }
            
            Dot_sr.sprite = Dot;

            if (Distance <= EffectiveRange)
            {
                //SetEdgesToDistance(EdgeDistanceEffectiveRange);
                if (TemporaryColorRemaining > 0)
                {
                    Edge0_sr.color = Edge1_sr.color = Edge2_sr.color = Edge3_sr.color = Dot_sr.color = TemporaryColor;
                }
                else
                {
                    Edge0_sr.color = Edge1_sr.color = Edge2_sr.color = Edge3_sr.color = Dot_sr.color = DefaultColor;
                }
            }
            else if (Distance <= 2*EffectiveRange)
            {
                
                if (TemporaryColorRemaining > 0)
                {
                    Edge0_sr.color = Edge1_sr.color = Edge2_sr.color = Edge3_sr.color = Dot_sr.color = TemporaryColor;
                }
                else
                {
                    //Dot_sr.color = DefaultColor * (1-((Distance - EffectiveRange) / EffectiveRange));
                    float Ratio = (((Distance - EffectiveRange) / EffectiveRange));
                    Color clr = IneffectiveRangeColor * Ratio + DefaultColor * (1 - Ratio);
                    //Color clr = IneffectiveRangeColor * (1 - ((Distance - 2 * EffectiveRange) / EffectiveRange));
                    Edge0_sr.color = Edge1_sr.color = Edge2_sr.color = Edge3_sr.color = Dot_sr.color = clr;
                }
            }
            else if (Distance <= 3f * EffectiveRange)
            {
                if (TemporaryColorRemaining > 0)
                {
                    Edge0_sr.color = Edge1_sr.color = Edge2_sr.color = Edge3_sr.color = Dot_sr.color = TemporaryColor;
                }
                else
                {
                    Color clr = IneffectiveRangeColor * (1 - ((Distance - 2 * EffectiveRange) / EffectiveRange));                    
                    Edge0_sr.color = Edge1_sr.color = Edge2_sr.color = Edge3_sr.color = Dot_sr.color = clr;
                }
            }
            else
            {
                if (TemporaryColorRemaining > 0)
                {
                    Edge0_sr.color = Edge1_sr.color = Edge2_sr.color = Edge3_sr.color = Dot_sr.color = TemporaryColor;
                }
                else
                {
                    Edge0_sr.color = Edge1_sr.color = Edge2_sr.color = Edge3_sr.color = Dot_sr.color = InvisibleColor;
                }
            }


            if (TemporaryColorRemaining > 0)
            {
                SetEdgesToDistance(EdgeDistanceEffectiveRange);
            }
            else
            {
                SetEdgesToDistance(EdgeDistanceMinInaccuracy + EdgeDistancePerPOintOfInaccuracy * (UniversalReference.PlayerScript.Inaccuracy));
            }
        }

	}

    void SetEdgesToDistance(float Distance)
    {
        Edge0.transform.localPosition = new Vector3(Distance, Distance, 0);
        Edge1.transform.localPosition = new Vector3(-Distance, Distance, 0);
        Edge2.transform.localPosition = new Vector3(-Distance, -Distance, 0);
        Edge3.transform.localPosition = new Vector3(Distance, -Distance, 0);
    }

    Color TemporaryColor = new Color(1, 1, 1, 1);
    float TemporaryColorRemaining = 0;

    public void SetTemporaryColor(Color clr, float time = 0.1f)
    {
        TemporaryColor = clr;
        TemporaryColorRemaining = time;
    }
}
