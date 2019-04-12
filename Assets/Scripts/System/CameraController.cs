using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Rigidbody2D _rigidBody { get; set; }

    void Awake()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        SubscribeToMessages();
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<MoveCameraToMessage>(MoveCameraTo);
    }

    private void MoveCameraTo(MoveCameraToMessage msg)
    {
        _rigidBody.MovePosition(msg.Position);
    }
}
