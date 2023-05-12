using UnityEngine;

public static class MonoBehaviourExtensions
{
    /// <summary>
    /// Get's component in parent max 5 layers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static T GetComponentInParentRecursive<T>(this MonoBehaviour monoBehaviour) where T : Component
    {
        int depth = 0;
        const int maxDepth = 5;

        Transform currentTransform = monoBehaviour.transform.parent;

        while (currentTransform != null && depth < maxDepth)
        {
            T component = currentTransform.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            currentTransform = currentTransform.parent;
            depth++;
        }

        return null;
    }
}
