using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator _anim;

    [SerializeField] private LootManager _lootManager;
    [SerializeField] private float _extraModChance = 20;
    [SerializeField] private GameObject openText;
    public bool opened = false;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _lootManager = FindObjectOfType<LootManager>();
    }

    public void OpenChest()
    {
        if (opened)
            return;

        opened = true;
        openText.SetActive(false);
        _anim.SetTrigger("Open");

        int modCount = 1;

        if (Random.Range(0, 100) < _extraModChance)
            modCount++;

        List<Modifier> modsToGive = new List<Modifier>();
        for (int i = 0; i < modCount; i++)
        {
            modsToGive.Add(_lootManager.GetRandomModifier());
        }

        _lootManager.GiveModsToPlayer(modsToGive);
        _lootManager.GetPlayerInventory.OnInteraction.RemoveAllListeners();
    }

    public void CloseChest()
    {
        _anim.SetTrigger("Close");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (opened)
            return;

        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
        {
            openText.SetActive(true);
            _lootManager.GetPlayerInventory.OnInteraction.RemoveAllListeners();
            _lootManager.GetPlayerInventory.OnInteraction.AddListener(OpenChest);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
        {
            openText.SetActive(false);
            _lootManager.GetPlayerInventory.OnInteraction.RemoveAllListeners();
        }
    }
}
