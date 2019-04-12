using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Auras;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public List<Aura> Auras;

    private List<AuraController> _currentAuras { get; set; }

    void Awake()
    {
        _currentAuras = new List<AuraController>();
        SubscribeToMessages();
    }

    void Start()
    {
        foreach (var aura in Auras)
        {
            gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, gameObject);
        }
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<AddAuraToObjectMessage>(AddAuraToObject);
        gameObject.Subscribe<RemoveAuraFromObjectMessage>(RemoveAuraFromObject);
        gameObject.Subscribe<RemoveAuraByControllerFromObjectMessage>(RemoveAuraByControllerFromObject);
        gameObject.Subscribe<AuraCheckMessage>(AuraCheck);
        gameObject.Subscribe<AddAuraByControllerToObjectMessage>(AddAuraByControllerToObject);
    }

    private void AddAuraToObject(AddAuraToObjectMessage msg)
    {
        var onCooldown = false;
        gameObject.SendMessageTo(new AuraCooldownCheckMessage
        {
            Predicate = aura => aura.Name == msg.Aura.Name,
            Action = auraList =>
            {
                onCooldown = auraList.Count > 0;
            }
        }, gameObject);

        if (!onCooldown)
        {
            var currentAuraCount = _currentAuras.Count(a => a.CurrentAura.Name == msg.Aura.Name);
            if (msg.Aura.MaxStack <= 0 || currentAuraCount < msg.Aura.MaxStack)
            {
                var controller = Instantiate(FactoryController.AURA, transform);
                _currentAuras.Add(controller);
                controller.Setup(msg.Aura);
            }
        }
    }

    private void RemoveAuraFromObject(RemoveAuraFromObjectMessage msg)
    {
        var currentAuras = _currentAuras.FindAll(a => a.CurrentAura.Name == msg.Aura.Name);
        if (currentAuras.Count > 0)
        {
            var aura = currentAuras[0];
            _currentAuras.Remove(aura);
            Destroy(aura.gameObject);
        }
    }

    private void RemoveAuraByControllerFromObject(RemoveAuraByControllerFromObjectMessage msg)
    {
        var auraController = _currentAuras.Find(a => a == msg.Controller);
        if (auraController)
        {
            _currentAuras.Remove(auraController);
            Destroy(auraController.gameObject);
        }
    }

    private void AuraCheck(AuraCheckMessage msg)
    {
        var auras = _currentAuras.FindAll(a => msg.Predicate.Invoke(a.CurrentAura)).Select(a => a.CurrentAura).ToList();
        msg.DoAfter?.Invoke(auras);
    }

    private void AddAuraByControllerToObject(AddAuraByControllerToObjectMessage msg)
    {
        var currentAuraCount = _currentAuras.Count(a => a.CurrentAura == msg.Aura);
        if (msg.Aura.CurrentAura.MaxStack <= 0 || currentAuraCount < msg.Aura.CurrentAura.MaxStack)
        {
            _currentAuras.Add(msg.Aura);
        }
        else
        {
            Destroy(msg.Aura.gameObject);
        }
    }

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }
}
