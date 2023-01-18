using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Launcher : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Launcher::Start");
        //统一初始化单例类
        SceneController.Instance.Init();
     
        UIManager.Instance.Init();

        //加载主场景
        SceneController.Instance.SceneLoaded += OnSceneLoaded;
        SceneController.Instance.LoadAsync(EnumScene.Main);

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        Debug.Log("Platform Mobile");
#elif UNITY_STANDALONE
        Debug.Log("Platform Standalone");
#elif UNITY_WSA
        Debug.Log("Platform WSA");
#endif
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void OnSceneLoaded(EnumScene prevScene, EnumScene currentScene)
    {
        Debug.LogFormat("scene [{0}] loaded.", currentScene.ToString());
    }

    private void Update()
    {
        //更新单例类的Update函数
        SceneController.Instance.OnUpdate();
    }
}
