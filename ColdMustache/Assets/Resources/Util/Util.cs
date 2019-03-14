using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour {

    //the following is useful for variables that dont support actual null, such as vectors
    public const int NullValue = -999999;
    public static Vector3 NullVector3 = new Vector3(NullValue, NullValue, NullValue);
    public static Vector2 NullVector2 = new Vector2(NullValue, NullValue);
    public static Vector2Int NullVector2Int = new Vector2Int(NullValue, NullValue);


    public const float DegToRadRatio = 57.2957795131f;

    public static float GetAngleBetweenVectors(Vector2 From, Vector2 To)
    {
        Vector2 PosDifference = To - From;
        float Angle = 57.295779513f * Mathf.Acos(PosDifference.x / PosDifference.magnitude);

        if (PosDifference.y >= 0)
        {
            return Angle;
        }
        else
        {
            return 360 - Angle;
        }
    }
    public static Vector2 RotateVector(Vector2 Original, float Angle)
    {
        return new Vector2(
            Mathf.Cos(Angle/DegToRadRatio) * Original.x - Mathf.Sin(Angle / DegToRadRatio) * Original.y,
            Mathf.Sin(Angle/DegToRadRatio) * Original.x + Mathf.Cos(Angle / DegToRadRatio) * Original.y
            );
    }
    public static float GetLowestAngleBetweenVectors(Vector2 From, Vector2 To)
    {
        float Angle = 57.295779513f * Mathf.Acos((From.x * To.x + From.y * To.y) / (From.magnitude * To.magnitude));
        return Angle;
    }
    public static Vector2 GetVectorToward(Transform From, Transform To) {
        return To.position - From.position;
    }
    public static Vector2 GetVectorToward(Transform From, Vector2 To)
    {
        return To - (Vector2)From.position;
    }
    public static Vector2 GetVectorToward(Vector3 From, Vector2 To)
    {
        return To - (Vector2)From;
    }
    public static Vector2 GetDirectionVectorToward(Transform From, Transform To)
    {
        Vector2 Output = GetVectorToward(From, To);
        return Output / Output.magnitude;
    }
    public static Vector2 GetDirectionVectorToward(Transform From, Vector2 To)
    {
        Vector2 Output = GetVectorToward(From, To);
        return Output / Output.magnitude;
    }

    public static Vector2 GetDirectionVectorToward(Vector3 From, Vector2 To)
    {
        Vector2 Output = GetVectorToward(From, To);
        return Output / Output.magnitude;
    }

    public static void RotateTransformToward(Transform From, Transform To)
    {
        float Angle = GetAngleBetweenVectors(From.position, To.position);
        
        From.rotation = Quaternion.Euler(0, 0, Angle);
    }
    public static void RotateTransformToward(Transform From, Vector3 To)
    {
        float Angle = GetAngleBetweenVectors(From.position, To);

        From.rotation = Quaternion.Euler(0, 0, Angle);
    }
    public static void RotateTransformToward(Transform From, Transform To, float AngleDifference)
    {
        float Angle = GetAngleBetweenVectors(From.position, To.position);

        From.rotation = Quaternion.Euler(0, 0, Angle+AngleDifference);
    }
    public static void RotateTransformToward(Transform From, Vector3 To, float AngleDifference)
    {
        float Angle = GetAngleBetweenVectors(From.position, To);

        From.rotation = Quaternion.Euler(0, 0, Angle+AngleDifference);
    }

    public static bool Coinflip()
    {
        if (Random.Range((int)1,(int)3) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static Vector2Int Vector3To2Int(Vector3 vector3)
    {
        return new Vector2Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y) );
    }
    public static Vector2Int Vector2To2Int(Vector2 vector2)
    {
        return new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
    }
    public static Vector3 Vector2IntTo3(Vector2Int vector2int)
    {
        return new Vector3(vector2int.x, vector2int.y, 0);
    }

    public static float AngleDifference(float Angle, float AngleB)
    {
        float f = Angle - AngleB;
        f = Mathf.Abs(f);

        if(f > 180)
        {
            return 360 - f;
        }
        else
        {
            return f;
        }


    }

    //makes every angle be 0-180, so from 350 makes 10
    public static float NormalizeAngle(float Angle)
    {
        if (Angle > 180)
        {
            return 360 - Angle;
        }
        else
        {
            return Angle;
        }
    }

    public static Color MakeColorDarker(Color c, float Ratio){
        return new Color(c.r*Ratio, c.g*Ratio, c.b*Ratio, c.a);
    }
    

}
