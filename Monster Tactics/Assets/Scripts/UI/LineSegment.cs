using UnityEngine;
using Utilities;

public class LineSegment : MonoBehaviour
{
    [SerializeField]
    private Transform FromMarker;

    [SerializeField]
    private Transform ToMarker;

    public void UpdateSegment(Vector3 from, Vector3 to)
    {
        RotateMarkerBasedOnDirection(FromMarker, from.ToVector2IntXZ());
        RotateMarkerBasedOnDirection(ToMarker, to.ToVector2IntXZ());
    }

    private void RotateMarkerBasedOnDirection(Transform marker, Vector2Int target)
    {
        Vector2Int dir = transform.position.ToVector2IntXZ() - target;
        if (dir.Equals(Vector2Int.up)) marker.localRotation = Quaternion.Euler(0, 0, 0);
        else if (dir.Equals(Vector2Int.right)) marker.localRotation = Quaternion.Euler(0, 0, 270);
        else if (dir.Equals(Vector2Int.down)) marker.localRotation = Quaternion.Euler(0, 0, 180);
        else if (dir.Equals(Vector2Int.left)) marker.localRotation = Quaternion.Euler(0, 0, 90);
    }
}