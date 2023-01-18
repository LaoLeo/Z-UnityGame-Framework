using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public enum EnumScene
{
    None,
    Main,
    Town,
    Camp
}

public class SceneController : Common.Util.Singleton<SceneController>
{
    public EnumScene CurrentScene
    {
        get { return currentScene; }
    }

    readonly static string[] sceneNames = new string[] { "main_scene" };

    static Dictionary<EnumScene, Scene> sceneDict = new Dictionary<EnumScene, Scene>();

    EnumScene currentScene;
    EnumScene prevScene;

    bool isLoading = false;
    AsyncOperation loading;
    UIProgressBar bar;

    public Action<EnumScene, EnumScene> SceneLoaded;

    public void Init()
    {
    }

    public void OnUpdate()
    {
        if (!isLoading)
        {
            if (null != bar)
            {
                UIManager.Instance.Close<UIProgressBar>();
                bar = null;
            }
            return;
        }

        if (loading != null && bar != null)
        {
            float progress = loading.progress;

            // 加载进度
            bar.UpdateProgress(progress);

            if (progress == 1)
            {
                // 隐藏场景摄像机
                string sceneName = sceneNames[(int)currentScene - 1];
                Scene scene = SceneManager.GetSceneByName(sceneName);
                HideSceneCamera(scene);

                loading = null;
                isLoading = false;

                sceneDict.Add(currentScene, scene);

                //加载完成事件
                SceneLoaded?.Invoke(prevScene, currentScene);
            }
        }
    }

    public void LoadAsync(EnumScene enumScene)
    {
        if (currentScene == enumScene)
            return;

        isLoading = true;

        // 进度条
        //bar = UIManager.Instance.Show<UIProgressBar>();
        //bar.UpdateProgress(0.1f);

        UIManager.Instance.Close();

        if ((int)currentScene > 0)
        {
            sceneDict.TryGetValue(currentScene, out Scene scene);
            sceneDict.Remove(currentScene);

            prevScene = currentScene;
            currentScene = enumScene;

            AsyncOperation oper = SceneManager.UnloadSceneAsync(scene);
            oper.completed += (AsyncOperation oper) =>
            {
                Resources.UnloadUnusedAssets();
                GC.Collect();
                LoadScene();
            };
        }
        else
        {
            currentScene = enumScene;
            LoadScene();
        }
    }

    void LoadScene()
    {
        // 1,异步加载scene
        string sceneName = sceneNames[(int)currentScene - 1];
        loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void Back()
    {
        LoadAsync(prevScene);
    }

    void HideSceneCamera(Scene scene)
    {
        GameObject[] gos = scene.GetRootGameObjects();
        for (int i = 0; i < gos.Length; i++)
        {
            if (gos[i].TryGetComponent<Camera>(out var camera))
            {
                if (camera.TryGetComponent<AudioListener>(out var audio))
                    GameUtils.Destroy(audio);

                gos[i].SetActive(false);
            }
        }

    }

}