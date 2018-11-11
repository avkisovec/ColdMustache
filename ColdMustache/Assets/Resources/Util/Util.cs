using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour {

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
    
}
