using UnityEngine;

public class ChargeHandler
{
    private readonly float _maxChargeTime;
    private readonly float _minValue;
    private readonly float _maxValue;

    private float _currentTime;
    public bool IsCharging { get; private set; }

    public float NormalizedCharge => Mathf.Clamp01(_currentTime / _maxChargeTime);

    public ChargeHandler(float maxChargeTime, float minValue, float maxValue)
    {
        _maxChargeTime = maxChargeTime;
        _minValue = minValue;
        _maxValue = maxValue;
    }

    public void StartCharge()
    {
        IsCharging = true;
        _currentTime = 0f;
    }

    public void StopCharge()
    {
        IsCharging = false;
    }

    public void Update(float deltaTime)
    {
        if (!IsCharging) 
            return;

        _currentTime += deltaTime;

        if (_currentTime >= _maxChargeTime)
        {
            _currentTime = _maxChargeTime;
            StopCharge();
        }
    }

    public float GetFinalValue()
    {
        float value = Mathf.Lerp(_minValue, _maxValue, NormalizedCharge);
        return value;
    }

    public void Reset()
    {
        _currentTime = 0f;
        IsCharging = false;
    }
}
