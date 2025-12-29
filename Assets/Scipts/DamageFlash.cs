using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float flashTime = 0.15f;
    SpriteRenderer spriteRenderer;
    private Material material;
    private Coroutine _damageFlashCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = Instantiate(spriteRenderer.material);
        material = spriteRenderer.material;

    }
    public void callDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(damageFlasher());
    }
    private IEnumerator damageFlasher()
    {
        setFlashColor();
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = math.lerp(1f, 0f, (elapsedTime / flashTime));
            setFlashAmount(currentFlashAmount);

            yield return null;
        }
        setFlashAmount(0f);

    }
    private void setFlashColor()
    {
        material.SetColor("_HitEffectColor", _flashColor);
    }

    private void setFlashAmount(float amount)
    {
        material.SetFloat("_HitEffectAmount", amount);
    }
}
