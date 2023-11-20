using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomGenerator : MonoBehaviour
{
    public int openingDirection;

    private RoomTemplates _templates;
    private int rando;
    private bool spawned = false;

    private void Start()
    {
        _templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    private void Spawn()
    {
        if(spawned == false)
        {
            switch (openingDirection)
            {
                case 0:
                    rando = Random.Range(0, _templates.bottomRooms.Length);
                    Instantiate(_templates.bottomRooms[rando], transform.position, transform.rotation);
                    spawned = true;
                    break;
                case 1:
                    rando = Random.Range(0, _templates.topRooms.Length);
                    Instantiate(_templates.topRooms[rando], transform.position, transform.rotation);
                    spawned = true;
                    break;
                case 2:
                    rando = Random.Range(0, _templates.rightRooms.Length);
                    Instantiate(_templates.rightRooms[rando], transform.position, transform.rotation);
                    break;
                case 3:
                    rando = Random.Range(0, _templates.leftRooms.Length);
                    Instantiate(_templates.leftRooms[rando], transform.position, transform.rotation);
                    break;
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            Destroy(gameObject);
            Debug.Log("D");
        }
    }
}
