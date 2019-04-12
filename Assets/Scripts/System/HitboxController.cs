using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        gameObject.SendMessageTo(new ObjectHitMessage{ObjectHit = col.transform.parent.gameObject}, transform.parent.gameObject);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        gameObject.SendMessageTo(new ObjectLeftMessage{Object = col.transform.parent.gameObject}, transform.parent.gameObject);
    }


}
