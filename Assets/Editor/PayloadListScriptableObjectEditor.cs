using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PayloadListScriptableObject))]
public class PayloadListScriptableObjectEditor : Editor
{
    SerializedProperty payloadList;
    SerializedObject serializedPayloadList;
    float totalWeight = 0f;
    bool showDefaultInspector = false;

    private void OnEnable()
    {
        payloadList = serializedObject.FindProperty("payloadList");
        serializedPayloadList = new SerializedObject(target);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        serializedPayloadList.Update();

        totalWeight = 0f;

        // Calculate total weight
        for (int i = 0; i < payloadList.arraySize; i++)
        {
            SerializedProperty payload = payloadList.GetArrayElementAtIndex(i);
            if (payload != null)
            {
                PayloadScriptableObject payloadObject = payload.objectReferenceValue as PayloadScriptableObject;
                if (payloadObject != null)
                {
                    SerializedObject serializedPayloadObject = new SerializedObject(payloadObject);
                    SerializedProperty weight = serializedPayloadObject.FindProperty("weight");
                    if (weight != null)
                    {
                        totalWeight += weight.floatValue;
                    }
                }
            }
        }

        // Display payload list properties
        for (int i = 0; i < payloadList.arraySize; i++)
        {
            SerializedProperty payload = payloadList.GetArrayElementAtIndex(i);
            if (payload != null)
            {
                PayloadScriptableObject payloadObject = payload.objectReferenceValue as PayloadScriptableObject;
                if (payloadObject != null)
                {
                    SerializedObject serializedPayloadObject = new SerializedObject(payloadObject);
                    SerializedProperty weight = serializedPayloadObject.FindProperty("weight");
                    if (weight != null)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(payload, true); // true means show children of this property
                        EditorGUILayout.PropertyField(weight);

                        serializedPayloadObject.ApplyModifiedProperties(); // Apply changes to the serialized object

                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }

        EditorGUILayout.LabelField("Total Weight: " + totalWeight);

        // Normalize the weights
        if (totalWeight > 0f)
        {
            float normalizationFactor = 1f / totalWeight;

            for (int i = 0; i < payloadList.arraySize; i++)
            {
                SerializedProperty payload = payloadList.GetArrayElementAtIndex(i);
                if (payload != null)
                {
                    PayloadScriptableObject payloadObject = payload.objectReferenceValue as PayloadScriptableObject;
                    if (payloadObject != null)
                    {
                        SerializedObject serializedPayloadObject = new SerializedObject(payloadObject);
                        SerializedProperty weight = serializedPayloadObject.FindProperty("weight");
                        if (weight != null)
                        {
                            weight.floatValue *= normalizationFactor;
                            serializedPayloadObject.ApplyModifiedProperties(); // Apply changes to the serialized object
                        }
                    }
                }
            }
        }

        serializedPayloadList.ApplyModifiedProperties(); // Apply changes to the main serialized object

        // Button for adding new elements
        if (GUILayout.Button("Add Element"))
        {
            showDefaultInspector = true;
        }

        if (showDefaultInspector)
        {
            EditorGUILayout.Space();

            // Draw the default inspector for adding a new element
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Done"))
            {
                showDefaultInspector = false;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
