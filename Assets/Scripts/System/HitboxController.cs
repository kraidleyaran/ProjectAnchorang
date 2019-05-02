using System.Collections.Generic;
using Assets.Scripts.Auras;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    private List<GameObject> _collidedObjects = new List<GameObject>();
    private AuraController _parentController { get; set; }

    public void Setup(AuraController controller)
    {
        _parentController = controller;
    }
    

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!_collidedObjects.Contains(col.transform.parent.gameObject) && _parentController)
        {
            gameObject.SendMessageTo(new ObjectHitMessage { ObjectHit = col.transform.parent.gameObject }, _parentController.gameObject);
            _collidedObjects.Add(col.transform.parent.gameObject);
        }
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (_collidedObjects.Contains(col.transform.parent.gameObject) && _parentController)
        {
            gameObject.SendMessageTo(new ObjectLeftMessage { Object = col.transform.parent.gameObject }, _parentController.gameObject);
            _collidedObjects.Remove(col.transform.parent.gameObject);
        }
        
    }
}
