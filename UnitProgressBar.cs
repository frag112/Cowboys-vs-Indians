using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProgressBar : MonoBehaviour
{
  [SerializeField] private Transform _playerCamera;
  [SerializeField] private GameObject _bar;
    // get { return _reloadBar.Value >= _reloadBar._maxValue; }
    [SerializeField] private float _maxValue;
  [SerializeField] private float _startValue;
                   private float _currentValue;
    public float MaxValue
    {
        set
        {
            _maxValue = value;
        }
        get
        {
            return _maxValue;
        }
    }
    public float StartValue
    {
        set
        {
            _startValue = value;
        }
    }
  public float Value
    {
        get { return _currentValue; }
        set
        {
            _currentValue = Mathf.Clamp(value, 0, _maxValue);
            UpdateVisuals();
        }
    }
    private void OnEnable()
    {
        Value = _startValue;
        transform.LookAt(_playerCamera);
    }
    void UpdateVisuals()
    {
        float lerpTime = Time.deltaTime;
        _bar.transform.localScale = new Vector3(_currentValue / _maxValue, 1, 1); 
    }
}
