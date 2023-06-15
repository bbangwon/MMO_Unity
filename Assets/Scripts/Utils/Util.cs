using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static GameObject FindChild(GameObject parent, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(parent, name, recursive);
        if(transform == null) 
            return null;
        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject parent, string name = null, bool recursive = false) where T : Object
    {
        if(parent == null)
            return null;

        if(recursive == false)
        {
            for(int i=0;i < parent.transform.childCount;i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || child.name == name)
                {
                    T component = child.GetComponent<T>();
                    if(component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach(T component in parent.GetComponentsInChildren<T>())
            {
                if(string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
