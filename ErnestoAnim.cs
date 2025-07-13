using UnityEngine;

namespace FifthModJam;
[RequireComponent(typeof(Animator))]
public class ErnestoAnim : MonoBehaviour
{
    [SerializeField]
    public Animator _animator;

    [SerializeField]
    private OWAudioSource audio;

    private AnglerfishController.AnglerState currentState;

    [Space]
    [SerializeField]
    private float _jawOpenSpeed = 1f;

    [SerializeField]
    private float _jawCloseSpeed = 1f;

    [Space]
    [SerializeField]
    private float _spinesFlareSpeed = 1f;

    [SerializeField]
    private float _spinesFlareVariation = 1f;

    [SerializeField]
    private Transform[] _spines = new Transform[0];

    [Space]
    [SerializeField]
    private float _lookSpeed = 1f;

    private float _lastStateChangeTime;

    private float _jawCurrent;

    private float _jawTarget;

    private float _jawStart;

    private float[] _spinesCurrent;

    private float _spinesTarget;

    private float[] _spinesStart;

    private Quaternion[] _spineRotations;

    private float[] _spineOffsets;

    private Vector2 _lookCurrent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _lastStateChangeTime = 0f;
        _jawCurrent = (_jawTarget = (_jawStart = 0.33f));
        _spinesTarget = 0f;
        _spinesCurrent = new float[_spines.Length];
        _spinesStart = new float[_spines.Length];
        _spineRotations = new Quaternion[_spines.Length];
        _spineOffsets = new float[_spines.Length];
        for (int i = 0; i < _spines.Length; i++)
        {
            _spinesCurrent[i] = (_spinesStart[i] = 0f);
            _spineRotations[i] = _spines[i].localRotation;
            _spineOffsets[i] = Random.value * _spinesFlareVariation;
        }
    }

    private void Start()
    {
        currentState = AnglerfishController.AnglerState.Lurking;
    }

    private void LateUpdate()
    {
        float num = Time.time - _lastStateChangeTime;
        float t = Mathf.Clamp01(num / ((_jawTarget > _jawStart) ? _jawOpenSpeed : _jawCloseSpeed));
        _jawCurrent = Mathf.SmoothStep(_jawStart, _jawTarget, t);
        for (int i = 0; i < _spines.Length; i++)
        {
            float num3 = Mathf.Clamp01((num - _spineOffsets[i]) / _spinesFlareSpeed);
            num3 = (num3 - 2f) * (0f - num3);
            _spinesCurrent[i] = Mathf.SmoothStep(_spinesStart[i], _spinesTarget, num3);
        }
        Vector2 target = Vector2.zero;
        _lookCurrent = Vector2.MoveTowards(_lookCurrent, target, _lookSpeed * Time.deltaTime);
        _animator.SetFloat("Jaw", _jawCurrent);
        for (int j = 0; j < _spines.Length; j++)
        {
            _spines[j].localRotation = _spineRotations[j] * Quaternion.AngleAxis(_spinesCurrent[j] * -60f, Vector3.forward);
        }
        _animator.SetFloat("LookX", _lookCurrent.x);
        _animator.SetFloat("LookY", _lookCurrent.y);
    }

    public void OnChangeAnglerState(AnglerfishController.AnglerState newState)
    {
        switch (newState)
        {
            case AnglerfishController.AnglerState.Chasing:
                audio.PlayOneShot(global::AudioType.DBAnglerfishDetectTarget, 0.5f);
                _jawTarget = 1f;
                break;
            case AnglerfishController.AnglerState.Consuming:
                _jawTarget = 0f;
                break;
            default:
                _jawTarget = 0.33f;
                break;
        }
        if (newState == AnglerfishController.AnglerState.Chasing)
        {
            _spinesTarget = 1f;
        }
        else
        {
            _spinesTarget = 0f;
        }
        _jawStart = _jawCurrent;
        for (int i = 0; i < _spines.Length; i++)
        {
            _spinesStart[i] = _spinesCurrent[i];
        }
        _lastStateChangeTime = Time.time;
    }

    private void OnAnglerSuspended(AnglerfishController.AnglerState state)
    {
        base.enabled = false;
        _animator.enabled = false;
    }

    private void OnAnglerUnsuspended(AnglerfishController.AnglerState state)
    {
        base.enabled = true;
        _animator.enabled = true;
    }

    private void OnAnglerTurn()
    {
        _animator.SetTrigger("Impulse");
    }
}
