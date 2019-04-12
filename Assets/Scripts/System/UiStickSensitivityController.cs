using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;
using UnityEngine.UI;

public class UiStickSensitivityController : MonoBehaviour
{
    [Header("Settings")]
    public RectTransform.Axis Axis;
    public int Stick;

    private Slider _slider { get; set; }

    void Awake()
    {
        _slider = gameObject.GetComponent<Slider>();
        SubscribeToMessages();
    }

    public void ValueChanged()
    {
        gameObject.SendMessage(new SetStickSensitivityMessage{Axis = Axis, Stick = Stick, Sensitivity = _slider.value});
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<UpdateStickSensitivityMessage>(UpdateStickSensitivity);
    }

    private void UpdateStickSensitivity(UpdateStickSensitivityMessage msg)
    {
        if (msg.Axis == Axis && msg.Stick == Stick && msg.Sensitivity != _slider.value)
        {
            _slider.value = msg.Sensitivity;
        }
    }

}
