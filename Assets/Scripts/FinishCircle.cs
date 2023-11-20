using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCircle : MonoBehaviour
{
    private bool _entered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_entered)
            return;

        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
        {
            _entered = true;

            other.attachedRigidbody.GetComponent<PlayerInventory>().SaveInventory();
            GameManager.instance.loopCount++;
            Invoke(nameof(ReloadScene), 0.2f);
        }
    }

    public void ReloadScene()
    {
        Time.timeScale = 0;
        FindObjectOfType<PanelManagerUI>().Retry();
    }
}
