using UnityEngine;

public class FenceBetweenPosts : MonoBehaviour
{
    [Header("Posts from Hierarchy")]
    public GameObject startPost;
    public GameObject endPost;

    [Header("Fence prefab from Project")]
    public GameObject fenceSegmentPrefab;

    [Header("Optional parent for generated pieces")]
    public Transform generatedFenceParent;

    [Header("Settings")]
    public float segmentLength = 1.0f;
    public float yOffset = 0f;
    public float rotationOffsetY = 0f;
    public bool clearOldFenceBeforeGenerating = true;

    [ContextMenu("Generate Fence")]
    public void GenerateFence()
    {
        if (startPost == null || endPost == null || fenceSegmentPrefab == null)
        {
            Debug.LogError("Wopa! Pune startPost, endPost si fenceSegmentPrefab.");
            return;
        }

        if (generatedFenceParent == null)
        {
            GameObject parentObject = new GameObject("GeneratedFence");
            generatedFenceParent = parentObject.transform;
        }

        if (clearOldFenceBeforeGenerating)
        {
            ClearFence();
        }

        Vector3 start = startPost.transform.position;
        Vector3 end = endPost.transform.position;

        start.y += yOffset;
        end.y += yOffset;

        Vector3 direction = end - start;
        float distance = direction.magnitude;

        if (distance < 0.01f)
        {
            Debug.LogWarning("Wopa! Pilonii sunt prea apropiati.");
            return;
        }

        direction.Normalize();

        int segmentCount = Mathf.Max(1, Mathf.FloorToInt(distance / segmentLength));
        float actualSpacing = distance / segmentCount;

        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion finalRotation = lookRotation * Quaternion.Euler(0f, rotationOffsetY, 0f);

        for (int i = 0; i < segmentCount; i++)
        {
            Vector3 spawnPos = start + direction * (actualSpacing * (i + 0.5f));

            GameObject newSegment = Instantiate(
                fenceSegmentPrefab,
                spawnPos,
                finalRotation,
                generatedFenceParent
            );

            newSegment.name = $"FenceSegment_{i}";
        }
    }

    [ContextMenu("Clear Fence")]
    public void ClearFence()
    {
        if (generatedFenceParent == null)
            return;

        for (int i = generatedFenceParent.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            DestroyImmediate(generatedFenceParent.GetChild(i).gameObject);
#else
            Destroy(generatedFenceParent.GetChild(i).gameObject);
#endif
        }
    }
}