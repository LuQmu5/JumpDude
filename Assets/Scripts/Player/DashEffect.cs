using System.Collections;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _shadows;
    [SerializeField] private SpriteRenderer _characterRenderer;
    [SerializeField] private Transform _characterTransform;
    [SerializeField] private TrailRenderer _dashTrailVFX;
    [SerializeField] private float _shadowInterval = 0.05f;
    [SerializeField] private float _shadowLifetime = 0.2f;

    private const float ShadowAlpha = 0.5f;

    private void Awake()
    {
        foreach (var s in _shadows)
        {
            s.color = new Color(s.color.r, s.color.g, s.color.b, ShadowAlpha);
            s.gameObject.SetActive(false);
        }

        if (_dashTrailVFX != null)
            _dashTrailVFX.emitting = false;
    }

    public void Play(float duration, Vector2 direction)
    {
        StartCoroutine(PlayEffect(duration, direction));
    }

    private IEnumerator PlayEffect(float duration, Vector2 direction)
    {
        int index = 0;
        float timer = 0f;

        if (_dashTrailVFX != null)
        {
            _dashTrailVFX.Clear();
            _dashTrailVFX.emitting = true;
        }

        while (timer < duration)
        {
            SpriteRenderer shadow = _shadows[index % _shadows.Length];
            shadow.transform.position = _characterTransform.position;
            shadow.transform.rotation = _characterTransform.rotation;
            shadow.transform.parent = null;
            shadow.gameObject.SetActive(true);

            StartCoroutine(DisableShadow(shadow, _shadowLifetime));

            index++;
            timer += _shadowInterval;

            yield return new WaitForSeconds(_shadowInterval);
        }

        if (_dashTrailVFX != null)
            _dashTrailVFX.emitting = false;
    }

    private IEnumerator DisableShadow(SpriteRenderer shadow, float delay)
    {
        yield return new WaitForSeconds(delay);
        shadow.transform.parent = _characterTransform;
        shadow.gameObject.SetActive(false);
    }
}
