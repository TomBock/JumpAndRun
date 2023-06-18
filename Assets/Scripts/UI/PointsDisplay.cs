using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public readonly HashSet <Target> targets = new ();

    private bool _started = false;
    private Stopwatch _stopwatch;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        _goInputAction.action.performed += OnGo;
        _largeDisplay.text = $"Ready? Hit 'F' to start!";
    }

    private void OnDisable()
    {
        _goInputAction.action.performed -= OnGo;
    }

    private async void HideLargeDisplayAfter(int milliseconds)
    {
        await Task.Delay(milliseconds);
        _largeDisplay.gameObject.SetActive(false);
    }

    private void Update()
    {
        _pointsDisplay.text = $"Points: {PlayerData.points}";

        if (targets.Count == 0 && _started)
        {
            _stopwatch.Stop();
            var milliseconds = _stopwatch.Elapsed.ToString("mm':'ss':'ff");
            _largeDisplay.text = $"Success! \n Final Time: {milliseconds} for {PlayerData.points} \n Wanna go again?";
            _largeDisplay.gameObject.SetActive(true);
            _started = false;
        }
    }

    private void OnGo(InputAction.CallbackContext obj)
    {
        if(_started) 
            return;
        
        _started = true;
        _largeDisplay.text = $"Go!";
        _largeDisplay.gameObject.SetActive(true);
            
        Target.SpawnOnMap();
        _stopwatch = Stopwatch.StartNew();

        PlayerData.points = 0;
        FindObjectOfType <PlayerMoveController>().transform.position = Vector3.up * 2;

        HideLargeDisplayAfter(1000);
    }
}

}