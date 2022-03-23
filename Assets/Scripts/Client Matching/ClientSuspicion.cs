using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientSuspicion : MonoBehaviour
{
    [SerializeField] private CatchPlayer _catchPlayer;
    [SerializeField] private Slider _susSlider;
    private float _suspicionCount;
    [SerializeField] private float _suspicionGain;
    [SerializeField] private float _suspicionLoss;
    [SerializeField] private float _startingSuspicion;

    // Start is called before the first frame update
    void Start()
    {
        _suspicionCount = _startingSuspicion;
    }

    // Update is called once per frame
    void Update()
    {
        _suspicionCount = Mathf.Clamp(_suspicionCount, 0f, 100f);
        if (_suspicionCount < 100f && _catchPlayer.playing)
        {
            _suspicionCount += _suspicionGain * Time.deltaTime;
        }
        if (_suspicionCount < 100f && !_catchPlayer.playing && _suspicionCount > 0f)
        {
            _suspicionCount -= _suspicionLoss * Time.deltaTime;
        }
        _susSlider.value = _suspicionCount / 100f;
        Debug.Log(_suspicionCount);
        Debug.Log(_susSlider.value);
    }

    public void ReduceSus(float loss)
    {
        _suspicionCount -= loss;
    }
}
