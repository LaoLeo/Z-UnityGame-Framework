using System.Collections.Generic;
using UnityEngine;
using System;
using Common.Util;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public Transform PopupRoot;
    public Transform PopupTopRoot;
    public Transform TopRoot;
    class UIElement
    {
        public string Resoures;
        //public GameObject Resoures;
        public bool Cache;
        public GameObject Instance;
        public Transform Root;
    }

    private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();
    Vector3 m_posOutScreen = new(10000, 10000, 0);

    public void Init()
    {
        //this.UIResources.Add(typeof(UIProgressBar), new UIElement() { Resoures = "LoadingBar", Cache = true, Root = PopupTopRoot });
        //this.UIResources.Add(typeof(UIMainView), new UIElement() { Resoures = "MainView", Cache = false });
        //this.UIResources.Add(typeof(UICampView), new UIElement() { Resoures = "CampView", Cache = false });
        //this.UIResources.Add(typeof(UIJoystick), new UIElement() { Resoures = "joystick", Cache = false, Root = PopupTopRoot });
        //this.UIResources.Add(typeof(UIPlayerSelectView), new UIElement() { Resoures = "playerselectview", Cache = false });
        //this.UIResources.Add(typeof(UIStoryView), new UIElement() { Resoures = "StoryView", Cache = false });
        //this.UIResources.Add(typeof(UIDialogPopup), new UIElement() { Resoures = "DialogPopup", Cache = true, Root = TopRoot });
        //this.UIResources.Add(typeof(UIPetView), new UIElement() { Resoures = "PetView", Cache = false });
        //this.UIResources.Add(typeof(UIBattleStartView), new UIElement() { Resoures = "BattleStartView", Cache = true });
        //this.UIResources.Add(typeof(UIBattleResultView), new UIElement() { Resoures = "BattleResultView", Cache = true });
    }

    //public UIManager()
    //{
    //    this.UIResources.Add(typeof(UIProgressBar), new UIElement() { Resoures = ProgressBarPrefab, Cache = true });
    //}

    //~UIManager()
    //{

    //}

    ///<summary>
    ///Show UI
    ///</summary>>

    public T Show<T>()
    {
        Type type = typeof(T);
        if (this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            if (info.Instance != null)
            {
                //info.Instance.SetActive(true);
                //info.Instance.transform.localPosition = Vector3.zero;
                CanvasGroup canvasGroup = info.Instance.TryAddComponent<CanvasGroup>();
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                if (info.Resoures == null)
                {
                    return default(T);
                }
                info.Instance = Resources.Load<GameObject>(info.Resoures);
                if (info.Root != null)
                {
                    info.Instance.transform.SetParent(info.Root, false);
                }
                else
                {
                    info.Instance.transform.SetParent(PopupRoot, false);
                }

            }
            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }

    public T Get<T>()
    {
        Type type = typeof(T);
        if (this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }

    public void Close<T>()
    {
        Type type = typeof(T);
        if (this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            if (info.Cache)
            {
                //info.Instance.SetActive(false);
                //info.Instance.transform.localPosition = m_posOutScreen;
                // 设置透明度为0，才不会渲染
                CanvasGroup canvasGroup = info.Instance.TryAddComponent<CanvasGroup>();
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }

    public void Close()
    {
        foreach (var item in this.UIResources)
        {
            UIElement info = this.UIResources[item.Key];
            if (info.Cache == false)
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }

}