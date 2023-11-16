using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMaterial : MonoBehaviour
{
    private Material _mat;
    [SerializeField] private Texture[] _textures;
    [SerializeField] private float _delayBetweenTextures = 0.1f;
    [SerializeField] private bool _enabled = true;

    private int _matIndex = 0;
    private float delayTimer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        _mat = GetComponent<Renderer>().material;
    }

    private void Start()
    {
        _matIndex = 0;
    }

    private void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            return;
        }

        _mat.mainTexture = _textures[_matIndex];

        _matIndex++;
        delayTimer = _delayBetweenTextures;

        if (_matIndex >= _textures.Length)
        {
            _matIndex = 0;
        }
    }
}
