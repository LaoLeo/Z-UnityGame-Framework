using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchUtils
{
    public static bool IsTouchOverUI(int i)
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)

        if (Touch.activeTouches.Count > i)
            return EventSystem.current.IsPointerOverGameObject(Touch.activeTouches[i].touchId);
        else
            return false;

#endif
        if (null == EventSystem.current)
            return false;
        return EventSystem.current.IsPointerOverGameObject();
    }


    public static bool IsTouchPhaseDown(int i)
    {
        if (Touch.activeTouches.Count > i)
            return Touch.activeTouches[i].phase == TouchPhase.Began;
        return false;
    }


    public static bool IsTouchPhaseMove(int i)
    {
        if (Touch.activeTouches.Count > i)
            return Touch.activeTouches[i].phase == TouchPhase.Moved;
        return false;
    }


    public static bool IsTouchPhaseUp(int i)
    {
        if (Touch.activeTouches.Count > i)
            return Touch.activeTouches[i].phase == TouchPhase.Ended || Touch.activeTouches[i].phase == TouchPhase.Canceled;
        return true;
    }


    public static Vector2 GetTouchPosition(int i)
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        if (Touch.activeTouches.Count > i)
            return Touch.activeTouches[i].screenPosition;
        else
            return Vector2.zero;
#endif
        return Pointer.current.position.ReadValue();
    }


    public static int GetTouchCount()
    {
        return Touch.activeTouches.Count;
    }

    public static Vector3 GetTouchInGround(Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(GetTouchPosition(0));

        float projDir = Vector3.Dot(ray.direction, Vector3.up);
        if (projDir >= 0)
        {
            //射线与地面不相交，取水平方向1000米外的点
            return Vector3.ProjectOnPlane(ray.direction, Vector3.up) * 1000;
        }
        float d = Vector3.Dot(-ray.origin, Vector3.up) / projDir;
        Vector3 hitPoint = d * ray.direction + ray.origin;
        return hitPoint;
    }

    public static int IsEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
        float leng1 = (oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y);
        float leng2 = (nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y);
        if (leng1 < leng2)
        {
            return 2;
        }
        else if (leng1 > leng2)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}