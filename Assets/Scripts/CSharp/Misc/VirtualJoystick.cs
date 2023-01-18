using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using LuaInterface;
using System;
//using Astral.ToLuaFramework;
using UnityEngine.InputSystem;
//using Astral.Core;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    RectTransform _dianTrans;
    [SerializeField]
    RectTransform _runTrans;
    [SerializeField]
    RectTransform _dirTrans;
    [SerializeField]
    RectTransform _areaTrans;
    [SerializeField]
    RectTransform _bgTrans;
    [SerializeField]
    RectTransform _joystickTrans;
    [SerializeField]
    float radius = 100.0f;
    [SerializeField]
    float angleOffset = -90.0f;

    Vector2 _inputVector;
    float _shift;
    float _inputMagnitude;

    void Awake()
    {
        _bgTrans.gameObject.SetActive(false);

        if (null != _runTrans)
            _runTrans.gameObject.SetActive(false);

        PlayerInput input = GetComponent<PlayerInput>();
        input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(InputAction.CallbackContext obj)
    {
        if (obj.action.name == "Shift")
        {
            _shift = obj.ReadValue<float>();
            if (_inputVector.x != 0 || _inputVector.y != 0)
            {
                UpdateShift();
            }
        }

        if (obj.action.name == "Move")
        {
            _inputVector = obj.ReadValue<Vector2>();
            if (_inputVector.x != 0 || _inputVector.y != 0)
            {
                FirstInput();
                UpdateShift();
            }
            else
            {
                ReleaseInput();
            }
        }
    }

    public void GetVector(out float h, out float v, out float maginitude)
    {
        h = _inputVector.x;
        v = _inputVector.y;
        maginitude = _inputMagnitude;
    }

    //[NoToHotFix]
    public void GetInput(ref Vector3 v)
    {
        v.x = _inputVector.x;
        v.y = _inputVector.y;
        v.z = _inputMagnitude;
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_bgTrans, ped.position, ped.pressEventCamera, out pos))
        {
            _inputVector.x = pos.x / radius;
            _inputVector.y = pos.y / radius;

            _inputMagnitude = _inputVector.magnitude;
            if (_inputMagnitude > 1)
            {
                _inputVector.x /= _inputMagnitude;
                _inputVector.y /= _inputMagnitude;
                _inputMagnitude = 1;
            }

            UpdateInput();
        }
    }


    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_areaTrans, ped.position, ped.pressEventCamera, out pos);
        _bgTrans.anchoredPosition = pos;

        FirstInput();
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        ReleaseInput();
    }

    void FirstInput()
    {
        if (null != _dianTrans)
            _dianTrans.gameObject.SetActive(false);

        _bgTrans.gameObject.SetActive(true);
    }

    void UpdateShift()
    {
        _inputMagnitude = _shift == 0 ? 0.5f : 1.0f;
        UpdateInput();
    }

    void UpdateInput()
    {
        if (null != _runTrans)
            _runTrans.gameObject.SetActive(_inputMagnitude > 0.5f);

        if (null != _dirTrans)
        {
            float angle = Mathf.Rad2Deg * Mathf.Atan2(_inputVector.y, _inputVector.x) + angleOffset;
            _dirTrans.localEulerAngles = new Vector3(0, 0, angle);
        }

        _joystickTrans.anchoredPosition = new Vector2(_inputVector.x * _inputMagnitude * radius, _inputVector.y * _inputMagnitude * radius);
    }

    void ReleaseInput()
    {
        if (null != _dianTrans)
            _dianTrans.gameObject.SetActive(true);

        if (null != _runTrans)
            _runTrans.gameObject.SetActive(false);

        _inputVector.x = 0f;
        _inputVector.y = 0f;
        _inputMagnitude = 0f;

        _bgTrans.gameObject.SetActive(false);
        _bgTrans.anchoredPosition = Vector2.zero;
        _joystickTrans.anchoredPosition = Vector2.zero;
    }
}