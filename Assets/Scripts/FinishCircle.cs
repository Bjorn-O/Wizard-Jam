using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCircle : MonoBehaviour
{
    private bool _entered = false;
    [SerializeField] private Chest chest1;
    [SerializeField] private Chest chest2;

    private void OnTriggerEnter(Collider other)
    {
        if (_entered)
            return;

        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
        {
            _entered = true;

            other.attachedRigidbody.GetComponent<PlayerInventory>().SaveInventory();
            GameManager.instance.loopCount++;
            FindObjectOfType<PanelManagerUI>().ShowFade();
            Invoke(nameof(ReloadScene), 0.2f);
        }
    }

    public void ReloadScene()
    {
        chest1.opened = true;
        chest2.opened = true;
        chest1.CloseChest();
        chest2.CloseChest();

        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0, 1.6f, 0);

        FindObjectOfType<PanelManagerUI>().ShowFadeOut();
        FindObjectOfType<EnemySpawner>()._enemiesKilled = 0;
        FindObjectOfType<EnemySpawner>().finishCircle.SetActive(false);
        FindObjectOfType<EnemySpawner>().StartSpawnEnemies();
    }
}
