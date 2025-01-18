using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    [SerializeField] private List<Transform> points; 
    [SerializeField] private float interval = 0.05f; 
    private List<Vector3> path;

    private void OnEnable()
    {
        BuildPath();
    }

    private void OnDrawGizmos()
    {
        if (points == null || points.Count < 2)
        {
            Debug.LogWarning("You need at least two points!");
            return;
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < GetSegmentCount(); i++)
        {
            DrawSpline(i);
        }
    }

    private void BuildPath()
    {
        path = new List<Vector3>();

        for (int i = 0; i < GetSegmentCount(); i++)
        {
            Vector3 p0 = GetPoint(i - 1);
            Vector3 p1 = GetPoint(i);
            Vector3 p2 = GetPoint(i + 1);
            Vector3 p3 = GetPoint(i + 2);

            CatmullRomCurve curve = new CatmullRomCurve(p0, p1, p2, p3);

            for (float t = 0f; t <= 1f; t += interval)
            {
                Vector3 position = curve.GetPoint(t);
                path.Add(position);
            }
        }
    }

    private void DrawSpline(int segmentIndex)
    {
        Vector3 p0 = GetPoint(segmentIndex - 1);
        Vector3 p1 = GetPoint(segmentIndex);
        Vector3 p2 = GetPoint(segmentIndex + 1);
        Vector3 p3 = GetPoint(segmentIndex + 2);

        CatmullRomCurve curve = new CatmullRomCurve(p0, p1, p2, p3);

        Vector3 previousPoint = curve.GetPoint(0f);

        for (float t = interval; t <= 1f + Mathf.Epsilon; t += interval)
        {
            Vector3 currentPoint = curve.GetPoint(t);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }


    private Vector3 GetPoint(int index)
    {
        if (index < 0)
            return points[0].position;
        if (index >= points.Count)
            return points[points.Count - 1].position;
        return points[index].position;
    }


    private int GetSegmentCount()
    {
        return Mathf.Max(points.Count - 1, 1);
    }


    public List<Vector3> GetTargets()
    {
        return path;
    }


    private struct CatmullRomCurve
    {
        public Vector3 P0, P1, P2, P3;

        public CatmullRomCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            P0 = p0;
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }
        public Vector3 GetPoint(float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            return 0.5f * (
                (2f * P1) +
                (-P0 + P2) * t +
                (2f * P0 - 5f * P1 + 4f * P2 - P3) * t2 +
                (-P0 + 3f * P1 - 3f * P2 + P3) * t3
            );
        }
    }
}