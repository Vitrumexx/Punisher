using UnityEngine;
using System.Collections.Generic;

public class SupplyBox : MonoBehaviour
{
    [SerializeField] private int suppliesAmount;
    [SerializeField] private List<GameObject> boxPrefab = new List<GameObject>();
    [SerializeField] private List<float> offset = new List<float>();

    private bool playerInZone;
    private PlayerParameters player;
    private GameObject box;
    private int index;

    void Start()
    {
        playerInZone = false;
        suppliesAmount = Random.Range(20, 300);

        if (suppliesAmount < 100)
            index = 0;
        else if (suppliesAmount < 200)
            index = 1;
        else
            index = 2;

        SpawnBox(boxPrefab[index], index);
    }

    void SpawnBox(GameObject obj, int index)
    {
        Vector3 spawnPos = transform.position + Vector3.up * offset[index];
        box = Instantiate(obj, spawnPos, transform.rotation, transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            player = other.GetComponent<PlayerParameters>();

            InteractionHint hint = GetComponent<InteractionHint>();
            if (hint == null)
            {
                hint = gameObject.AddComponent<InteractionHint>();
                hint.Setup("E");
            }
            hint.Show();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            player = null;

            InteractionHint hint = GetComponent<InteractionHint>();
            if (hint != null) hint.Hide();
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            player._supplies += suppliesAmount;
            Destroy(gameObject);
        }
    }
}