using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellVfxManager : MonoBehaviour
{
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    private List<BaseParticleSystemValues> baseParticleSystemValues = new List<BaseParticleSystemValues>();

    [SerializeField] private List<ParticleSystem> excludeSystems = new List<ParticleSystem>();
    [SerializeField] private GameObject[] objectEffects;
    [SerializeField] private Material[] materialsToChange;
    [SerializeField] private ElementalEffect[] elementalEffects;

    private Dictionary<ParticleSystem, ElementalEffect> elementEffectPsPairs = new Dictionary<ParticleSystem, ElementalEffect>();
    [SerializeField] private float emmisionRateModMultiplier = 0.75f;

    // Start is called before the first frame update
    void Awake()
    {
        particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>(true));

        List<ParticleSystem> removingSystems = new List<ParticleSystem>();

        foreach (ParticleSystem particleSystem in particleSystems)
        {
            if (excludeSystems.Contains(particleSystem))
            {
                removingSystems.Add(particleSystem);
                continue;
            }
            baseParticleSystemValues.Add(new BaseParticleSystemValues(particleSystem.main.startColor.color, particleSystem.emission.rateOverTime.constant));
        }

        foreach (var ps in removingSystems)
        {
            particleSystems.Remove(ps);
        }

        foreach (var item in elementalEffects)
        {
            elementEffectPsPairs.Add(item.effect.GetComponent<ParticleSystem>(), item);
            item.effect.SetActive(false);
        }
    }

    public void ModifyParticleSystems(int emmisionMultiplier, Elements[] elements)
    {
        Color finalColor = Color.black;

        foreach (var element in elements)
        {
            finalColor += ElementsManager.instance.ElementColors[element];
        }

        if (elements.Length > 1)
        {
            finalColor /= elements.Length;
        }

        for (int i = 0; i < particleSystems.Count; i++)
        {
            var particleSystem = particleSystems[i];
            var main = particleSystem.main;

            if (finalColor != Color.black)
            {
                finalColor.a = baseParticleSystemValues[i].color.a;
                ElementalEffect elementalEffect;

                if (elementEffectPsPairs.TryGetValue(particleSystem, out elementalEffect))
                {
                    main.startColor = Color.Lerp(finalColor, main.startColor.color, elementalEffect.originalColorPercentage);
                }
                else
                {
                    main.startColor = finalColor;
                }
            }
            else
            {
                main.startColor = baseParticleSystemValues[i].color;
            }

            if (emmisionMultiplier != 1)
            {
                var emmision = particleSystem.emission.rateOverTime;
                emmision.constant = baseParticleSystemValues[i].emmision;
                emmision.constant *= emmisionMultiplier * emmisionRateModMultiplier;
            }
        }

        for (int i = 0; i < materialsToChange.Length; i++)
        {
            finalColor.a = materialsToChange[i].color.a;
            materialsToChange[i].color = finalColor;
        }

        for (int i = 0; i < elementalEffects.Length; i++)
        {
            var elementalEffect = elementalEffects[i];
            elementalEffect.effect.SetActive(false);

            for (int j = 0; j < elements.Length; j++)
            {
                var element = elements[j];

                if (elementalEffect.element == element)
                {
                    elementalEffect.effect.SetActive(true);
                }
            }
        }

        //print(elements[0]);
    }

    public void PlayEffect(bool showObjectEffects)
    {
        particleSystems[0].Play();

        if (!showObjectEffects)
            return;

        foreach (GameObject effect in objectEffects)
        {
            effect.SetActive(true);
        }
    }

    public void StopEffect()
    {
        particleSystems[0].Stop();

        foreach (GameObject effect in objectEffects)
        {
            effect.SetActive(false);
        }
    }
}

[System.Serializable]
public class ElementalEffect
{
    public Elements element;
    public GameObject effect;
    public float originalColorPercentage = 0.4f;
}

public class BaseParticleSystemValues
{
    public Color color;
    public float emmision;

    public BaseParticleSystemValues(Color color, float emmision)
    {
        this.color = color;
        this.emmision = emmision;
    }
}

