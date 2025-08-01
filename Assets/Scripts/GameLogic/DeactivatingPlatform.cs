using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeactivatingPlatform : MonoBehaviour
{
    [SerializeField] private float activeDuration = 3f;
    [SerializeField] private float inactiveDuration = 3f;  
    [SerializeField] private float fadeDuration = 1f; 

    private SpriteRenderer[] _spriteRenderers;
    private Collider2D _collider;

    private Coroutine _cycleCoroutine;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out PlayerController player) && _cycleCoroutine == null)
        {
            _cycleCoroutine = StartCoroutine(DeactivateActivateCycle());
        }
    }

    private IEnumerator DeactivateActivateCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(activeDuration);

            yield return FadeTo(0f, fadeDuration);

            _collider.enabled = false;

            yield return new WaitForSeconds(inactiveDuration);

            _collider.enabled = true;

            yield return FadeTo(1f, fadeDuration);
        }
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float timer = 0f;
        float[] startAlphas = new float[_spriteRenderers.Length];

        for (int i = 0; i < _spriteRenderers.Length; i++)
            startAlphas[i] = _spriteRenderers[i].color.a;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                Color c = _spriteRenderers[i].color;
                c.a = Mathf.Lerp(startAlphas[i], targetAlpha, t);
                _spriteRenderers[i].color = c;
            }
            yield return null;
        }

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            Color c = _spriteRenderers[i].color;
            c.a = targetAlpha;
            _spriteRenderers[i].color = c;
        }
    }
}
