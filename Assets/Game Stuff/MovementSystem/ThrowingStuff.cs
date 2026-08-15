using UnityEngine;

public static class ThrowingStuff
{

    public static bool TryGetInterceptPoint( Vector3 qbPos, Vector3 receiverPos, Vector3 receiverVel, float footballSpeed, out Vector3 interceptPoint, out float t)
    {
        Vector3 d = receiverPos - qbPos;

        float a = Vector3.Dot(receiverVel, receiverVel) - footballSpeed * footballSpeed;
        float b = 2f * Vector3.Dot(d, receiverVel);
        float c = Vector3.Dot(d, d);

        float t0, t1;

        if(Mathf.Abs(a) < .0001f)
        {
            if(Mathf.Abs(b) < .0001f)
            {
                interceptPoint = receiverPos;
                t = 0f;
                return false;
            }
            t0 = t1 = -c / b;
        }
        else
        {
            float disc = b * b - 4f * a * c;
            if (disc < 0f)
            {
                interceptPoint = receiverPos;
                t = 0f;
                return false;
            }
            float sqrDist = Mathf.Sqrt(disc);
            t0 = (-b + sqrDist) / (2f * a);
            t1 = (-b - sqrDist) / (2f * a);
        }
        t = (t0 <= 0f && t1 >= 0f) ? Mathf.Min(t0, t1) : Mathf.Max(t0, t1);
        if( t < 0f)
        {
            interceptPoint = receiverPos;
            return false;
        }
        interceptPoint = receiverPos + receiverVel * t;
        return true;

    }
}