using Common.Util;
using UnityEngine;


public class GameModeBase<T> : MonoBehaviour
{
    public static T Instance
    {
        get { return _instance; }
    }
    static T _instance;

    protected void OnStart(T instance)
    {
        _instance = instance;
    }
}
