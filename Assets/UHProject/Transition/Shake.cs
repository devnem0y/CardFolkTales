using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] private RectTransform _shakeTransform;
    [SerializeField] private float _duration;
    [SerializeField] private float _magnitude;
    [SerializeField] private float _force;
    
    private bool _isShake;

    private void ShakeStart()
    {
        _isShake = true;
        StartCoroutine(Begin());
    }
    
    private IEnumerator Begin()
    {
        if (!_isShake) yield break;
        
        var originalPos = _shakeTransform.anchoredPosition;
        var originalRot = _shakeTransform.localRotation;

        var elapsed = 0f;

        while (elapsed < _duration)
        {
            var x = Random.Range(_force * -1, _force) * _magnitude;
            var y = Random.Range(_force * -1, _force) * _magnitude;

            var pos = _shakeTransform.anchoredPosition;
            
            _shakeTransform.anchoredPosition = new Vector2(pos.x + x, pos.y + y);
            _shakeTransform.localRotation = new Quaternion(originalRot.x, originalRot.y, _shakeTransform.localRotation.z, originalRot.w);
            elapsed += Time.deltaTime;

            yield return null;
        }

        _shakeTransform.localRotation = originalRot;
        _shakeTransform.anchoredPosition = originalPos;
        _isShake = false;
    }
}