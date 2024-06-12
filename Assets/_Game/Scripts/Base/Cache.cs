using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cache
{
    public const string CACHE_TAG_BRICK = "Obstacle";
    public const string CACHE_TAG_BRIDGE = "Bridge";
    public const string CACHE_TAG_DOOR = "Door";
    public const string CACHE_TAG_FINNISH = "Finnish";

    private static Dictionary<Collider, object> cache = new Dictionary<Collider, object>();

    public static T GetComponentFromCache<T>(Collider collider) where T : Component
    {
        if (collider == null) return null;

        if (!cache.ContainsKey(collider))
        {
            T component = collider.GetComponent<T>();
            if (component != null)
            {
                cache.Add(collider, component);
            }
        }

        return cache.ContainsKey(collider) ? cache[collider] as T : null;
    }
}
