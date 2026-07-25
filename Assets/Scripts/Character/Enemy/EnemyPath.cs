using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public enum PathType
    {
        Curvy,   
        ZigZag
    }

    [Header("Path Mode")]
    [SerializeField]
    private PathType pathType;

    [Header("Path Points (in order)")]
    [SerializeField] private Transform[] waypoints;

    [Header("Gizmo Settings")]
    [SerializeField] private bool showLine;
    [SerializeField] private int gizmoResolution = 20; // segments per waypoint gap
    [SerializeField] private Color pathColor = Color.yellow;
    [SerializeField] private Color pointColor = Color.red;

    public int PointCount => waypoints != null ? waypoints.Length : 0;

    public Vector2 GetSplinePoint(float t)
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            if (waypoints != null && waypoints.Length == 1 && waypoints[0] != null)
                return waypoints[0].position;

            return Vector2.zero;
        }

        t = Mathf.Clamp01(t);
        int segmentCount = waypoints.Length - 1;
        float scaledT = t * segmentCount;
        int seg = Mathf.Clamp(Mathf.FloorToInt(scaledT), 0, segmentCount - 1);
        float localT = scaledT - seg;

        // Pilih perhitungan titik berdasarkan mode
        if (pathType == PathType.ZigZag)
        {
            Vector2 pStart = GetSafePoint(seg);
            Vector2 pEnd = GetSafePoint(seg + 1);

            // Garis lurus biasa antar waypoint
            return Vector2.Lerp(pStart, pEnd, localT);
        }
        else
        {
            Vector2 p0 = GetSafePoint(seg - 1);
            Vector2 p1 = GetSafePoint(seg);
            Vector2 p2 = GetSafePoint(seg + 1);
            Vector2 p3 = GetSafePoint(seg + 2);

            // Garis melengkung Catmull-Rom
            return CatmullRom(p0, p1, p2, p3, localT);
        }
    }

    private Vector2 GetSafePoint(int index)
    {
        if (waypoints == null || waypoints.Length == 0)
            return transform.position;

        index = Mathf.Clamp(index, 0, waypoints.Length - 1);

        if (waypoints[index] == null)
            return transform.position;

        return waypoints[index].position;
    }

    private static Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

    private void OnDrawGizmos()
    {
        if (!showLine || waypoints == null || waypoints.Length < 2)
            return;

        Gizmos.color = pathColor;

        // Mode ZigZag cuma butuh 1 garis antar-waypoint untuk menggambar Gizmo (lebih efisien)
        int stepsPerSegment = (pathType == PathType.ZigZag) ? 1 : gizmoResolution;
        int totalSteps = stepsPerSegment * (waypoints.Length - 1);

        Vector3 prev = GetSplinePoint(0f);
        for (int i = 1; i <= totalSteps; i++)
        {
            float t = i / (float)totalSteps;
            Vector3 point = GetSplinePoint(t);
            Gizmos.DrawLine(prev, point);
            prev = point;
        }

        Gizmos.color = pointColor;
        foreach (var wp in waypoints)
        {
            if (wp != null)
                Gizmos.DrawSphere(wp.position, 0.15f);
        }
    }
}
