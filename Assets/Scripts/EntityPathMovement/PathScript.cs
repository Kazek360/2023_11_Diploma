using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    [SerializeField] private List<Transform> controlPoints; 
    private List<Vector3> _targets;

    private void OnEnable()
    {
        GeneratePathPoints();
    }

    private void OnDrawGizmos()
    {
        if (controlPoints == null || controlPoints.Count < 2)
        {
            Debug.LogWarning("You need 2 controle points, to make a Catmull-Rom.(PathManager)");
            return;
        }

        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            DrawCatmullRomSpline(i);
        }
    }

    private void GeneratePathPoints()
    {
        _targets = new List<Vector3>();

        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            Vector3 p0 = controlPoints[Mathf.Max(i - 1, 0)].position;
            Vector3 p1 = controlPoints[i].position;
            Vector3 p2 = controlPoints[i + 1].position;
            Vector3 p3 = controlPoints[Mathf.Min(i + 2, controlPoints.Count - 1)].position;

            float step = 0.05f; 

            for (float t = 0; t <= 1; t += step)
            {
                Vector3 point = CatmullRom(p0, p1, p2, p3, t);
                _targets.Add(point); 
            }
        }
    }

    private void DrawCatmullRomSpline(int index)
    {
        Vector3 p0 = controlPoints[Mathf.Max(index - 1, 0)].position;
        Vector3 p1 = controlPoints[index].position;
        Vector3 p2 = controlPoints[index + 1].position;
        Vector3 p3 = controlPoints[Mathf.Min(index + 2, controlPoints.Count - 1)].position;

        float step = 0.05f;
        Vector3 previousPoint = p1;
        for (float t = 0; t <= 1; t += step)
        {
            Vector3 point = CatmullRom(p0, p1, p2, p3, t);
            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2 * p1) +
            (-p0 + p2) * t +
            (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +
            (-p0 + 3 * p1 - 3 * p2 + p3) * t3
        );
    }
    
    public List<Vector3> GetTargets()
    {
        return _targets;
    }
}