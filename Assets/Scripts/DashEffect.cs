using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DashEffect
{
    private const float ShadowAlpha = 0.5f;

    private readonly SpriteRenderer[] _shadows;
    private readonly SpriteRenderer _characterRenderer;
    private readonly Transform _characterTransform;
    private readonly MonoBehaviour _mono;
    private readonly TrailRenderer _dashTrailVFX;
    private readonly float _shadowInterval;
    private readonly float _shadowLifetime;


    public DashEffect(SpriteRenderer[] shadows, SpriteRenderer characterRenderer, Transform characterTransform, MonoBehaviour mono, TrailRenderer dashTrailVFX,
        float shadowInterval = 0.05f, 
        float shadowLifetime = 0.2f)
    {
        _shadows = shadows;
        _characterRenderer = characterRenderer;
        _characterTransform = characterTransform;
        _mono = mono;
        _shadowInterval = shadowInterval;
        _shadowLifetime = shadowLifetime;

        _dashTrailVFX = dashTrailVFX;
        _dashTrailVFX.emitting = false;

        foreach (var s in _shadows)
        {
            s.color = new Color(s.color.r, s.color.g, s.color.b, ShadowAlpha);
            s.gameObject.SetActive(false);
        }
    }

    public void Play(float dashDuration, Vector2 lookDirection)
    {
        _mono.StartCoroutine(PlayEffect(dashDuration, lookDirection));
    }

    private IEnumerator PlayEffect(float dashDuration, Vector2 lookDirection)
    {
        int index = 0;
        float timer = 0f;

        int minIterations = _shadows.Length;
        float requiredDuration = Mathf.Max(dashDuration, _shadowInterval * minIterations);

        _dashTrailVFX.Clear();
        _dashTrailVFX.emitting = true;

        while (timer < requiredDuration)
        {
            SpriteRenderer shadow = _shadows[index % _shadows.Length];
            shadow.transform.position = _characterTransform.position;
            shadow.transform.rotation = _characterTransform.rotation;
            shadow.transform.localScale = _characterTransform.localScale;
            shadow.transform.parent = null;

            Vector3 scale = shadow.transform.localScale;
            scale.x = Mathf.Sign(lookDirection.x) * Mathf.Abs(scale.x);
            shadow.transform.localScale = scale;

            shadow.gameObject.SetActive(true);

            _mono.StartCoroutine(DisableShadow(shadow, _shadowLifetime));

            index++;
            timer += _shadowInterval;

            yield return new WaitForSeconds(_shadowInterval);
        }

        _dashTrailVFX.emitting = false;
    }


    private IEnumerator DisableShadow(SpriteRenderer shadow, float delay)
    {
        yield return new WaitForSeconds(delay);

        shadow.transform.parent = _mono.transform;
        shadow.gameObject.SetActive(false);
    }
}
