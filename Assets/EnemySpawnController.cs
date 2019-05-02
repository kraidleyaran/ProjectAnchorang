using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Auras;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [Header("Settings")]
    public List<Aura> SpawnAuras;
    public List<Transform> Pathing;
    public float SpawnEvery = 3f;
    public int MaxSpawns = 1;


    private List<UnitController> _spawns { get; set; }
    private Sequence _spawnTimer { get; set; }
    private List<Vector2> _pathingPositions { get; set; }

    void Awake()
    {
        _spawns = new List<UnitController>();
        _pathingPositions = new List<Vector2>();
        foreach (var pos in Pathing)
        {
            _pathingPositions.Add(pos.position);
        }
    }

    void Start()
    {
        SpawnTimer();
    }

    private void SpawnTimer()
    {
        _spawnTimer = DOTween.Sequence().AppendInterval(SpawnEvery).OnComplete(() =>
        {
            if (_spawns.Count < MaxSpawns)
            {
                var controller = Instantiate(FactoryController.UNIT, transform.position, Quaternion.identity);
                foreach (var aura in SpawnAuras)
                {
                    gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, controller.gameObject);
                }
                gameObject.SendMessageTo(new SetPathingMessage { Positions = _pathingPositions.ToList() }, controller.gameObject);
                _spawns.Add(controller);
            }
            _spawnTimer = null;
            SpawnTimer();
        });
    }

    void OnDestroy()
    {
        if (_spawnTimer != null && _spawnTimer.IsActive())
        {
            _spawnTimer.Kill();
            _spawnTimer = null;
        }
        gameObject.UnsubscribeFromAllMessages();
    }



    
}
