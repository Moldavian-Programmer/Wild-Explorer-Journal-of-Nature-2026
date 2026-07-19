using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlacementSystem))]
[CanEditMultipleObjects]
public class PrefabPlacementEditor : Editor
{
    private SerializedProperty ParentObjName;
    private SerializedProperty prefab;
    private SerializedProperty layerMask;
    private SerializedProperty prefabTag;
    private SerializedProperty minRage, maxRage;
    private SerializedProperty randomizePrefab;
    private SerializedProperty toInstantiate;

    private SerializedProperty spread;
    private SerializedProperty radius;
    private SerializedProperty amount;
    private SerializedProperty Min_size;
    private SerializedProperty Max_size;
    private SerializedProperty positionOffset;

    public SerializedProperty canPlaceOver;
    private SerializedProperty canAling;
    private SerializedProperty isRandomS;
    private SerializedProperty isRandomR;
    private SerializedProperty hideInHierarchy;

    private Vector3 lastPos;
    private Vector3 mousePos;
    private Quaternion mouseRot;

    private void OnEnable()
    {
        ParentObjName = serializedObject.FindProperty("ParentObjName");
        prefab = serializedObject.FindProperty("prefab");
        layerMask = serializedObject.FindProperty("layerMask");
        prefabTag = serializedObject.FindProperty("prefabTag");
        minRage = serializedObject.FindProperty("minRage");
        maxRage = serializedObject.FindProperty("maxRage");
        randomizePrefab = serializedObject.FindProperty("randomizePrefab");
        toInstantiate = serializedObject.FindProperty("toInstantiate");
        spread = serializedObject.FindProperty("spread");
        radius = serializedObject.FindProperty("radius");
        amount = serializedObject.FindProperty("amount");
        Min_size = serializedObject.FindProperty("Min_size");
        Max_size = serializedObject.FindProperty("Max_size");
        positionOffset = serializedObject.FindProperty("positionOffset");
        canPlaceOver = serializedObject.FindProperty("canPlaceOver");
        canAling = serializedObject.FindProperty("canAling");
        isRandomS = serializedObject.FindProperty("isRandomS");
        isRandomR = serializedObject.FindProperty("isRandomR");
        hideInHierarchy = serializedObject.FindProperty("hideInHierarchy");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(prefab, new GUIContent("Prefab"), true);
        layerMask.intValue = EditorGUILayout.LayerField(new GUIContent("Layer Mask"), layerMask.intValue);
        prefabTag.stringValue = EditorGUILayout.TagField(new GUIContent("Prefab Tag"), prefabTag.stringValue);
        EditorGUILayout.PropertyField(randomizePrefab, new GUIContent("Randomize Prefab"));

        if (!randomizePrefab.boolValue)
        {
            EditorGUILayout.PropertyField(toInstantiate, new GUIContent("Prefab Index"));
        }
        else
        {
            EditorGUILayout.PropertyField(minRage, new GUIContent("minRage"));
            EditorGUILayout.PropertyField(maxRage, new GUIContent("maxRage"));
        }

        EditorGUILayout.PropertyField(ParentObjName, new GUIContent("ParentObjName"));
        EditorGUILayout.PropertyField(canPlaceOver, new GUIContent("Override"));
        EditorGUILayout.Slider(spread, 1f, 10f, new GUIContent("Spread"));
        EditorGUILayout.Slider(radius, 1f, 100f, new GUIContent("Radius"));
        EditorGUILayout.IntSlider(amount, 1, 200, new GUIContent("Amount"));
        EditorGUILayout.Slider(Min_size, 0.1f, 10f, new GUIContent("Min_Size"));
        EditorGUILayout.Slider(Max_size, 0.1f, 10f, new GUIContent("Max_Size"));
        EditorGUILayout.PropertyField(canAling, new GUIContent("Aling with normal"));
        EditorGUILayout.PropertyField(positionOffset, new GUIContent("yOffset"));
        EditorGUILayout.PropertyField(isRandomS, new GUIContent("Randomize Size"));
        EditorGUILayout.PropertyField(isRandomR, new GUIContent("Randomize Rotation"));
        EditorGUILayout.PropertyField(hideInHierarchy, new GUIContent("Hide in Hierarchy"));

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        Event current = Event.current;
        int controlId = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);

        MousePosition();

        bool hasPrefabArray = prefab != null && prefab.isArray && prefab.arraySize > 0;

        if ((current.type == EventType.MouseDrag || current.type == EventType.MouseDown) &&
            !current.alt &&
            hasPrefabArray)
        {
            if (current.button == 0 && (lastPos == Vector3.zero || CanDraw()) && !current.shift)
            {
                lastPos = mousePos;

                for (int i = 0; i < amount.intValue; i++)
                {
                    if (randomizePrefab.boolValue)
                    {
                        int min = Mathf.Clamp(minRage.intValue, 0, prefab.arraySize - 1);
                        int max = Mathf.Clamp(maxRage.intValue, 0, prefab.arraySize - 1);

                        if (max < min)
                        {
                            int temp = min;
                            min = max;
                            max = temp;
                        }

                        PrefabInstantiate(Random.Range(min, max + 1));
                    }
                    else
                    {
                        int index = Mathf.Clamp(toInstantiate.intValue, 0, prefab.arraySize - 1);
                        PrefabInstantiate(index);
                    }
                }
            }
            else if (current.button == 0 && current.shift)
            {
                lastPos = mousePos;
                PrefabRemove();
            }
        }

        if (current.type == EventType.MouseUp)
            lastPos = Vector3.zero;

        if (current.type == EventType.Layout)
            HandleUtility.AddDefaultControl(controlId);

        SceneView.RepaintAll();
    }

    public bool CanDraw()
    {
        float dist = Vector3.Distance(mousePos, lastPos);
        return dist >= spread.floatValue / 2f;
    }

    public void MousePosition()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, 1 << layerMask.intValue))
        {
            mousePos = hit.point;
            mouseRot = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Handles.color = Color.blue;
            Handles.DrawWireDisc(mousePos, hit.normal, radius.floatValue / 2f);
        }
    }

    public void PrefabInstantiate(int index)
    {
        if (prefab == null || !prefab.isArray || prefab.arraySize == 0)
            return;

        if (index < 0 || index >= prefab.arraySize)
            return;

        Object prefabRef = prefab.GetArrayElementAtIndex(index).objectReferenceValue;
        if (prefabRef == null)
            return;

        GameObject tempParent = GameObject.Find(ParentObjName.stringValue);
        if (tempParent == null)
        {
            Debug.LogWarning("Wopa! Nu exista obiectul parinte: " + ParentObjName.stringValue);
            return;
        }

        GameObject instanceOf = PrefabUtility.InstantiatePrefab(prefabRef) as GameObject;
        if (instanceOf == null)
            return;

        RaycastHit hit;
        Vector3 radiusAdjust = Random.insideUnitSphere * radius.floatValue / 2f;
        float prefabSize = Min_size.floatValue;

        if (hideInHierarchy.boolValue)
            instanceOf.hideFlags = HideFlags.HideInHierarchy;

        instanceOf.transform.localScale = new Vector3(prefabSize, prefabSize, prefabSize);
        instanceOf.transform.position = mousePos;
        instanceOf.transform.rotation = Quaternion.identity;

        if (canAling.boolValue)
            instanceOf.transform.rotation = mouseRot;

        if (amount.intValue > 1)
        {
            instanceOf.transform.Translate(radiusAdjust.x, positionOffset.floatValue, radiusAdjust.y);

            if (Physics.Raycast(instanceOf.transform.position, -instanceOf.transform.up, out hit))
            {
                if (!canPlaceOver.boolValue && hit.collider != null && hit.collider.CompareTag(instanceOf.tag))
                {
                    Undo.DestroyObjectImmediate(instanceOf);
                    return;
                }

                instanceOf.transform.position = hit.point;

                if (canAling.boolValue)
                    instanceOf.transform.up = hit.normal;
            }
            else
            {
                Undo.DestroyObjectImmediate(instanceOf);
                return;
            }
        }

        if (isRandomR.boolValue)
            instanceOf.transform.Rotate(0f, Random.Range(0, 8) * 45f, 0f);

        if (isRandomS.boolValue)
        {
            float randomValue = Random.Range(Min_size.floatValue, Max_size.floatValue);
            instanceOf.transform.localScale = new Vector3(randomValue, randomValue, randomValue);
        }

        instanceOf.transform.parent = tempParent.transform;
        Undo.RegisterCreatedObjectUndo(instanceOf, "Instantiate");
    }

    private void PrefabRemove()
    {
        GameObject[] prefabsInRadius = GameObject.FindGameObjectsWithTag(prefabTag.stringValue);

        for (int i = prefabsInRadius.Length - 1; i >= 0; i--)
        {
            GameObject p = prefabsInRadius[i];

            if (p == null)
                continue;

            float dist = Vector3.Distance(mousePos, p.transform.position);

            if (dist <= radius.floatValue / 2f)
            {
                Undo.DestroyObjectImmediate(p);
            }
        }
    }
}