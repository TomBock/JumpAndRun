using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Controls;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{

public class PointsDisplay : MonoBehaviour
{
    public static PointsDisplay instance;

    [SerializeField] private TMP_Text _pointsDisplay;
    [SerializeField] private TMP_Text _largeDisplay;
    [SerializeField] private InputActionProperty _goInputAction;
    [SerializeField] private InputActionProperty _randomGoInputAction;
    [SerializeField] private InputActionProperty _resetInputAction;

    public readonly HashSet <Target> targets = new ();

    private int _targetPoints;
    private bool _started = false;
    private Stopwatch _stopwatch;
    private bool _randomSpawn;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        _goInputAction.action.performed += OnGo;
        _randomGoInputAction.action.performed += OnGoRandom;
        _resetInputAction.action.performed += OnReset;
        _largeDisplay.text = $"Ready? Hit 'F' to start!";
    }

    private void OnDisable()
    {
        _goInputAction.action.performed -= OnGo;
        _randomGoInputAction.action.performed -= OnGoRandom;
        _resetInputAction.action.performed -= OnReset;
    }

    private async void HideLargeDisplayAfter(int milliseconds)
    {
        await Task.Delay(milliseconds);
        _largeDisplay.gameObject.SetActive(false);

        _targetPoints = _randomSpawn ? targets.Count : 5;
    }

    private void Update()
    {
        _pointsDisplay.text = $"Points: {PlayerData.points}/{_targetPoints}";

        if (targets.Count == 0 && _started)
        {
            _stopwatch.Stop();
            var milliseconds = _stopwatch.Elapsed.ToString("mm':'ss':'ff");
            _largeDisplay.text = $"Success! \n Final Time: {milliseconds} for {PlayerData.points} \n Wanna go again?";
            _largeDisplay.gameObject.SetActive(true);
            
            if(!_randomSpawn)
                HighScoreDisplay.Instance.AddResult(_stopwatch.Elapsed, PlayerData.points);
            _started = false;
        }
    }
    
    private void OnReset(InputAction.CallbackContext obj)
    {
        _started = false;
        for (var i = targets.Count - 1; i >= 0; i--)
        {
            Destroy(targets.ElementAt(i).gameObject);
        }
        OnGo(obj);
    }

    private void OnGoRandom(InputAction.CallbackContext obj)
    {
        if(_started) 
            return;
        _randomSpawn = true;
        Start();
    }

    private void OnGo(InputAction.CallbackContext obj)
    {
        if(_started) 
            return;
        _randomSpawn = false;
        Start();
    }
    

    private void Start()
    {
        
        _started = true;
        _largeDisplay.text = $"Go!";
        _largeDisplay.gameObject.SetActive(true);
        
        if(_randomSpawn)
            Target.SpawnRandomlyOnMap();
        else
            Target.SpawnOnMap();
        _stopwatch = Stopwatch.StartNew();

        PlayerData.points = 0;
        FindObjectOfType <PlayerMoveController>().transform.position = Vector3.up * 2;

        HideLargeDisplayAfter(1000);
    }
}

}