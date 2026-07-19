using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameplayScriptTests
{
    [Test]
    public void GenerateFenceCreatesExpectedNumberOfSegments()
    {
        GameObject startPost = new GameObject("StartPost");
        GameObject endPost = new GameObject("EndPost");
        GameObject fencePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject generatorObject = new GameObject("FenceGenerator");
        GameObject parentObject = new GameObject("FenceParent");

        try
        {
            startPost.transform.position = Vector3.zero;
            endPost.transform.position = new Vector3(5f, 0f, 0f);

            FenceBetweenPosts generator = generatorObject.AddComponent<FenceBetweenPosts>();
            generator.startPost = startPost;
            generator.endPost = endPost;
            generator.fenceSegmentPrefab = fencePrefab;
            generator.generatedFenceParent = parentObject.transform;
            generator.segmentLength = 1f;

            generator.GenerateFence();

            Assert.That(parentObject.transform.childCount, Is.EqualTo(5));
            Assert.That(parentObject.transform.GetChild(0).name, Is.EqualTo("FenceSegment_0"));
            Assert.That(parentObject.transform.GetChild(4).position.x, Is.EqualTo(4.5f).Within(0.001f));
        }
        finally
        {
            Object.DestroyImmediate(startPost);
            Object.DestroyImmediate(endPost);
            Object.DestroyImmediate(fencePrefab);
            Object.DestroyImmediate(generatorObject);
            Object.DestroyImmediate(parentObject);
        }
    }

    [Test]
    public void GenerateFenceUsesAtLeastOneSegmentForShortDistances()
    {
        GameObject startPost = new GameObject("StartPost");
        GameObject endPost = new GameObject("EndPost");
        GameObject fencePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject generatorObject = new GameObject("FenceGenerator");
        GameObject parentObject = new GameObject("FenceParent");

        try
        {
            startPost.transform.position = Vector3.zero;
            endPost.transform.position = new Vector3(0.5f, 0f, 0f);

            FenceBetweenPosts generator = generatorObject.AddComponent<FenceBetweenPosts>();
            generator.startPost = startPost;
            generator.endPost = endPost;
            generator.fenceSegmentPrefab = fencePrefab;
            generator.generatedFenceParent = parentObject.transform;
            generator.segmentLength = 1f;

            generator.GenerateFence();

            Assert.That(parentObject.transform.childCount, Is.EqualTo(1));
            Assert.That(parentObject.transform.GetChild(0).position.x, Is.EqualTo(0.25f).Within(0.001f));
        }
        finally
        {
            Object.DestroyImmediate(startPost);
            Object.DestroyImmediate(endPost);
            Object.DestroyImmediate(fencePrefab);
            Object.DestroyImmediate(generatorObject);
            Object.DestroyImmediate(parentObject);
        }
    }

    [Test]
    public void ClearFenceRemovesGeneratedChildren()
    {
        GameObject generatorObject = new GameObject("FenceGenerator");
        GameObject parentObject = new GameObject("FenceParent");

        try
        {
            new GameObject("OldSegmentA").transform.SetParent(parentObject.transform);
            new GameObject("OldSegmentB").transform.SetParent(parentObject.transform);

            FenceBetweenPosts generator = generatorObject.AddComponent<FenceBetweenPosts>();
            generator.generatedFenceParent = parentObject.transform;

            generator.ClearFence();

            Assert.That(parentObject.transform.childCount, Is.EqualTo(0));
        }
        finally
        {
            Object.DestroyImmediate(generatorObject);
            Object.DestroyImmediate(parentObject);
        }
    }

    [Test]
    public void GenerateFenceLogsErrorWhenRequiredReferencesAreMissing()
    {
        GameObject generatorObject = new GameObject("FenceGenerator");

        try
        {
            FenceBetweenPosts generator = generatorObject.AddComponent<FenceBetweenPosts>();

            LogAssert.Expect(LogType.Error, "Wopa! Pune startPost, endPost si fenceSegmentPrefab.");
            generator.GenerateFence();
        }
        finally
        {
            Object.DestroyImmediate(generatorObject);
        }
    }

    [Test]
    public void SnapNowMovesObjectToRaycastHitWithExtraOffset()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject snapObject = new GameObject("SnapObject");

        try
        {
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(10f, 1f, 10f);

            snapObject.transform.position = new Vector3(0f, 10f, 0f);
            SnapToTerrain snap = snapObject.AddComponent<SnapToTerrain>();
            snap.extraOffset = 0.25f;

            Physics.SyncTransforms();
            snap.SnapNow();

            Assert.That(snapObject.transform.position.y, Is.EqualTo(0.75f).Within(0.001f));
        }
        finally
        {
            Object.DestroyImmediate(ground);
            Object.DestroyImmediate(snapObject);
        }
    }

    [Test]
    public void SnapNowKeepsColliderBottomOnTerrainWhenObjectHasCollider()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject snapObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        try
        {
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(10f, 1f, 10f);

            snapObject.transform.position = new Vector3(0f, 10f, 0f);
            SnapToTerrain snap = snapObject.AddComponent<SnapToTerrain>();
            snap.extraOffset = 0.2f;

            Physics.SyncTransforms();
            snap.SnapNow();
            Physics.SyncTransforms();

            float groundTop = ground.GetComponent<Collider>().bounds.max.y;
            float objectBottom = snapObject.GetComponent<Collider>().bounds.min.y;

            Assert.That(objectBottom, Is.EqualTo(groundTop + 0.2f).Within(0.001f));
        }
        finally
        {
            Object.DestroyImmediate(ground);
            Object.DestroyImmediate(snapObject);
        }
    }

    [Test]
    public void ThirdPersonCameraDoesNothingWhenTargetIsMissing()
    {
        GameObject cameraObject = new GameObject("Camera");
        Vector3 initialPosition = new Vector3(1f, 2f, 3f);
        Quaternion initialRotation = Quaternion.Euler(10f, 20f, 30f);

        try
        {
            cameraObject.transform.SetPositionAndRotation(initialPosition, initialRotation);
            cameraObject.AddComponent<ThirdPersonCamera>().SendMessage("LateUpdate");

            Assert.That(cameraObject.transform.position, Is.EqualTo(initialPosition));
            Assert.That(cameraObject.transform.rotation.eulerAngles.x, Is.EqualTo(initialRotation.eulerAngles.x).Within(0.001f));
        }
        finally
        {
            Object.DestroyImmediate(cameraObject);
        }
    }

    [Test]
    public void SimpleTreeWindStartsFromCurrentLocalRotation()
    {
        GameObject treeObject = new GameObject("SimpleTree");

        try
        {
            treeObject.transform.localEulerAngles = new Vector3(5f, 15f, 25f);
            SimpleTreeWind wind = treeObject.AddComponent<SimpleTreeWind>();
            wind.speed = 0f;
            wind.amount = 10f;

            wind.SendMessage("Start");
            wind.SendMessage("Update");

            Assert.That(treeObject.transform.localEulerAngles.x, Is.EqualTo(5f).Within(0.001f));
            Assert.That(treeObject.transform.localEulerAngles.y, Is.EqualTo(15f).Within(0.001f));
            Assert.That(treeObject.transform.localEulerAngles.z, Is.EqualTo(25f).Within(0.001f));
        }
        finally
        {
            Object.DestroyImmediate(treeObject);
        }
    }

    [Test]
    public void TreeSwayStartsFromCurrentRotationWhenSwayIsZero()
    {
        GameObject treeObject = new GameObject("Tree");
        Quaternion initialRotation = Quaternion.Euler(0f, 45f, 10f);

        try
        {
            treeObject.transform.rotation = initialRotation;
            TreeSway sway = treeObject.AddComponent<TreeSway>();
            sway.swayAmount = 0f;

            sway.SendMessage("Start");
            sway.SendMessage("Update");

            Assert.That(Quaternion.Angle(treeObject.transform.rotation, initialRotation), Is.EqualTo(0f).Within(0.001f));
        }
        finally
        {
            Object.DestroyImmediate(treeObject);
        }
    }
}
