using UnityEngine;

public class SnapToTerrain : MonoBehaviour
{
    public float extraOffset = 0f;

    public void SnapNow()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up * 100f, Vector3.down, 500f);
        Collider ownCollider = GetComponent<Collider>();
        RaycastHit closestHit = default;
        bool foundTerrainHit = false;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider == ownCollider)
                continue;

            if (!foundTerrainHit || hits[i].distance < closestHit.distance)
            {
                closestHit = hits[i];
                foundTerrainHit = true;
            }
        }

        if (!foundTerrainHit)
            return;

        if (ownCollider != null)
        {
            float offset = transform.position.y - ownCollider.bounds.min.y;
            transform.position = closestHit.point + Vector3.up * (offset + extraOffset);
        }
        else
        {
            transform.position = closestHit.point + Vector3.up * extraOffset;
        }
    }
}
