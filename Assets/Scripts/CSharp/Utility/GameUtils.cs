using UnityEngine;

public static class GameUtils
{
    public static void Destroy(UnityEngine.Object obj)
    {
        if (obj != null)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                UnityEngine.Object.Destroy(obj);
            else
                UnityEngine.Object.DestroyImmediate(obj);
#else
                UnityEngine.Object.Destroy(obj);
#endif
        }
    }

    public static T TryAddComponent<T>(this GameObject go) where T : Component
    {
        if (!go.TryGetComponent<T>(out var comp))
            comp = go.AddComponent<T>();
        return comp;
    }
}