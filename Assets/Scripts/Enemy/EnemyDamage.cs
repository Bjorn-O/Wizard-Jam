using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamage : MonoBehaviour
{
    private Animator _anim;
    private Renderer renderer;
    private EnemyReferences _enemyReferences;

    private Material[] opaqueMats;
    [SerializeField] private Material[] transparentMats;

    [Header("Damage effects")]
    [HideInInspector] public bool showHitEffect = false;
    [SerializeField] private float _blinkTimer = 0;
    [SerializeField] private float _blinkDuration = 0.1f;
    [SerializeField] private float _blinkIntesity = 10;
    private Color _startingColor;

    [Header("Death")]
    [SerializeField] private float _fadeDelay = 1;
    [SerializeField] private float _fadeDuration = 3;
    [SerializeField] private Collider _colToDisable;
    [SerializeField] private Transform _ragdollParent;
    [SerializeField] private TansformFollowRagdollBone[] _transformsToFollowRagdollBones;
    private Rigidbody[] ragdollBodies;
    private List<Transform> _oldParentRagdoll = new List<Transform>();
    private Dictionary<Transform, ResetRagdollTransform> _oldTransformBeforeRagdoll = new Dictionary<Transform, ResetRagdollTransform>();
    private bool _ragdolling = false;

    private void Awake()
    {
        _enemyReferences = GetComponent<EnemyReferences>();
        _anim = GetComponentInChildren<Animator>();
        renderer = GetComponentInChildren<Renderer>();
        ragdollBodies = _ragdollParent.GetComponentsInChildren<Rigidbody>();

        opaqueMats = renderer.materials;

        _startingColor = renderer.material.color;

        foreach (var ragdollBody in ragdollBodies)
        {
            _oldTransformBeforeRagdoll.Add(ragdollBody.transform, 
                    new ResetRagdollTransform(ragdollBody.transform.localPosition, ragdollBody.transform.localRotation));
        }

        foreach (var fol in _transformsToFollowRagdollBones)
        {
            _oldTransformBeforeRagdoll.Add(fol.followTransform,
                    new ResetRagdollTransform(fol.followTransform.localPosition, fol.followTransform.localRotation));
        }
    }

    private void Start()
    {
        ResetAllSettings();
    }


    public void ResetAllSettings()
    {
        ResetRagdoll();

        _colToDisable.enabled = true;
        _enemyReferences.navMeshAgent.enabled = true;

        if (renderer is SkinnedMeshRenderer)
        {
            (renderer as SkinnedMeshRenderer).updateWhenOffscreen = false;
        }

        if (transparentMats.Length > 0)
        {
            renderer.materials = opaqueMats;
        }

        _anim.enabled = true;
    }

    private void ResetRagdoll()
    {
        _ragdolling = false;

        foreach (var ragdollBody in ragdollBodies)
        {
            ragdollBody.gameObject.layer = LayerMask.NameToLayer("Enemy");
            ragdollBody.isKinematic = true;

            Transform ragdollTransformBody = ragdollBody.transform;
            ragdollTransformBody.localPosition = _oldTransformBeforeRagdoll[ragdollTransformBody].resetLocalPosition;
            ragdollTransformBody.localRotation = _oldTransformBeforeRagdoll[ragdollTransformBody].resetLocalRotation;
        }

        foreach (var fol in _transformsToFollowRagdollBones)
        {
            fol.followTransform.localPosition = _oldTransformBeforeRagdoll[fol.followTransform].resetLocalPosition;
            fol.followTransform.localRotation = _oldTransformBeforeRagdoll[fol.followTransform].resetLocalRotation;
        }
    }

    private void Update()
    {
        HitEffect();
        UpdateFollowTranforms();
    }

    public void StartBlinkEffect()
    {
        showHitEffect = true;
        _blinkTimer = _blinkDuration;
    }

    private void UpdateFollowTranforms()
    {
        if (!_ragdolling)
            return;

        foreach (var fol in _transformsToFollowRagdollBones)
        {
            fol.followTransform.position = fol.followBone.position;
        }
    }

    private void HitEffect()
    {
        if (!showHitEffect)
            return;

        _blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(_blinkTimer / _blinkDuration);
        float intensity = (lerp * _blinkIntesity) + 1;

        renderer.material.color = _startingColor * intensity;

        if (_blinkTimer <= 0)
        {
            showHitEffect = false;
        }
    }

    public void Die(Vector3 force)
    {
        _enemyReferences.navMeshAgent.enabled = false;

        if (renderer is SkinnedMeshRenderer)
        {
            (renderer as SkinnedMeshRenderer).updateWhenOffscreen = true;
        }

        _colToDisable.enabled = false;
        _anim.enabled = false;

        foreach (var ragdollBody in ragdollBodies)
        {
            ragdollBody.gameObject.layer = LayerMask.NameToLayer("Ragdoll");
            ragdollBody.isKinematic = false;
        }

        _ragdolling = true;
        ragdollBodies[0].AddForce(force, ForceMode.Impulse);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(_fadeDelay);

        renderer.materials = transparentMats;

        float elapsedTime = 0f;
        List<Color> initialColors = new List<Color>();

        foreach (var item in renderer.materials)
        {
            initialColors.Add(item.color);
        }

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material mat = renderer.materials[i];
                Color color = initialColors[i];
                Color targetColor = color;
                targetColor.a = 0;

                mat.color = Color.Lerp(color, targetColor, elapsedTime / _fadeDuration);
                print(mat.color);
                yield return null;
            }
        }

        gameObject.SetActive(false);
    }
}

[System.Serializable]
sealed class TansformFollowRagdollBone
{
    public Transform followTransform;
    public Transform followBone;
}

sealed class ResetRagdollTransform
{
    public Vector3 resetLocalPosition;
    public Quaternion resetLocalRotation;

    public ResetRagdollTransform(Vector3 pos, Quaternion rot)
    {
        resetLocalPosition = pos;
        resetLocalRotation = rot;
    }
}
