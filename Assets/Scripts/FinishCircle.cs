using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCircle : MonoBehaviour
{
    private bool _entered = false;
    [SerializeField] private Chest chest1;
    private EnemySpawner enemySpawner;
    private PanelManagerUI panelManagerUI;

    private void Awake()
    {
        panelManagerUI = FindObjectOfType<PanelManagerUI>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_entered)
            return;

        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
        {
            _entered = true;

            //other.attachedRigidbody.GetComponent<PlayerInventory>().SaveInventory();
            GameManager.instance.loopCount++;
            panelManagerUI.ShowFade();
            Invoke(nameof(ReloadScene), 0.2f);
        }
    }

    public void ReloadScene()
    {
        chest1.opened = false;
        chest1.CloseChest();

        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0, 1.6f, 0);

        panelManagerUI.loopCounter.text = GameManager.instance.loopCount.ToString();
        panelManagerUI.ShowFadeOut();
        enemySpawner._enemiesKilled = 0;
        enemySpawner.finishCircle.SetActive(false);
        enemySpawner.StartSpawnEnemies();

        _entered = false;
    }
}
