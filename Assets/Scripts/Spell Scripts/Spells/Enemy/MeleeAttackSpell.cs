using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackSpell : Spell
{
    [SerializeField] private Animator _anim;
    private List<Collider> _cols = new List<Collider>();
    private List<MeleeWeaponTrail> _trails = new List<MeleeWeaponTrail>();
    private List<SpellEffect> _spellEffects = new List<SpellEffect>();
    [SerializeField] private float _attackTime = 1;
    [SerializeField] private float _casingIntensity = -2;
    [SerializeField] private Vector3 _offset = new Vector3(0.001f, 0, 0.001f);
    [SerializeField] private Material _magicalWeaponMat;

    public override IEnumerator CastSpell()
    {
        startingCooldown = cooldown;
        cooldown = Mathf.Infinity;

        for (int i = 0; i < castAmount; i++)
        {
            for (int j = 0; j < _cols.Count; j++)
            {
                if (j != 0)
                {
                    _cols[j].gameObject.SetActive(true);
                }
                _cols[j].enabled = true;
                _trails[j].Emit = true;
            }
            
            _anim.SetTrigger("Attack");

            yield return new WaitForSeconds(_attackTime);            
        }

        for (int j = 0; j < _cols.Count; j++)
        {
            if (j != 0)
            {
                _cols[j].gameObject.SetActive(false);
            }

            _cols[j].enabled = false;
            _trails[j].Emit = false;
        }

        cooldown = startingCooldown;
    }

    //Makes multiple melee weapons
    protected override void FireSpellEffect(SpellEffect effect, float amount)
    {
        int evenIndex = 0;
        int unevenIndex = -1;
        Color effectColor = GetMixedElementsColor();

        for (int i = 0; i < amount; i++)
        {
            SpellEffect pooledEffect = spellEffectPool.Get();
            pooledEffect.damage = damage;
            pooledEffect.transform.localScale = Vector3.one * effectScale;
            MeleeWeaponTrail trail = pooledEffect.GetComponent<MeleeWeaponTrail>();
            _trails.Add(trail);
            trail.Emit = false;
            _cols.Add(pooledEffect.GetComponent<Collider>());            

            int index = 0;
            if (i % 2 == 0)
            {
                index = evenIndex;
                evenIndex++;
            }
            else
            {
                index = unevenIndex;
                unevenIndex--;
            }

            Renderer renderer = i != 0 ? pooledEffect.GetComponent<Renderer>() : pooledEffect.transform.GetChild(0).GetComponent<Renderer>();
            List<Material> mats = new List<Material>();

            if (i == 0 && effectColor != Color.black)
            {
                renderer.gameObject.SetActive(true);
            }

            _magicalWeaponMat.SetColor("_EmissionColor", effectColor);

            for (int j = 0; j < renderer.materials.Length; j++)
            {
                if (i == 0 && effectColor != Color.black)
                    renderer.materials[j].SetColor("_EmissionColor", effectColor * Mathf.Pow(2, _casingIntensity));

                if (i != 0)
                    mats.Add(_magicalWeaponMat);
            }

            if (i != 0)
            {
                pooledEffect.gameObject.SetActive(false);
                renderer.SetMaterials(mats);
            }

            pooledEffect.transform.localPosition += new Vector3(_offset.x * index, 0, _offset.z * Mathf.Abs(index));
        }
    }

    private void Start()
    {
        _magicalWeaponMat = new Material(_magicalWeaponMat);
        if (modifiers.Count == 0)
        {
            FireSpellEffect(spellEffect, 1);
        }

        OnApplyModifiers.AddListener(() => FireSpellEffect(spellEffect, effectAmount));

        OnApplyModifiers.AddListener(() => {
            Color finalColor = GetMixedElementsColor();

            if (finalColor != Color.black && _trails.Count > 0)
            {
                foreach (var trail in _trails)
                {
                    trail._colors[0] = finalColor;
                }
            }
        });
    }

    private Color GetMixedElementsColor()
    {
        List<Elements> elements = GetElementsFromMods();

        if (elements.Count == 0)
            return Color.black;

        Color finalColor = Color.black;

        foreach (var element in elements)
        {
            finalColor += ElementsManager.instance.ElementColors[element];
        }

        finalColor /= elements.Count;

        return finalColor;
    }
}
