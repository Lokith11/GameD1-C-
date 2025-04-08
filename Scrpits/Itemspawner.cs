using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemspawner : MonoBehaviour
{
    public static Itemspawner Instance;
    public List<ItemOBJ> itemObject;
    public float minRadius = 2.0f;
    public float maxRadius = 10.0f;

    public GameObject itemPickerTf;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void SpawnItem(ItemSO item, int amount)
    {
        if (item.gamePrefab == null)
        {
            Debug.LogError("No prefab in " + item.name + ", Plesae assign something!!!");
            return;
        }
        
        // สุ่มตำแหน่งในวงกลมและเพิ่มความสูง (y-axis)
        Vector2 randPos = Random.insideUnitCircle.normalized * minRadius;
        Vector3 offset = new Vector3(randPos.x, 3.0f, randPos.y); // เพิ่มค่า y เป็น 3.0f เพื่อให้ spawn สูงขึ้น
        
        GameObject spawnItem = Instantiate(item.gamePrefab, itemPickerTf.transform.position + offset, Quaternion.identity);
        spawnItem.GetComponent<ItemOBJ>().SetAmount(amount);

        // เพิ่ม Rigidbody เพื่อให้ไอเท็มตกตามฟิสิกส์
        Rigidbody rb = spawnItem.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = spawnItem.AddComponent<Rigidbody>();
        }
        rb.AddForce(Vector3.down * 2.0f, ForceMode.Impulse); // ให้ไอเท็มตกเล็กน้อย
    }
    // public void SpawnItem(ItemSO item, int amount)
    // {
    //     if (item.gamePrefab == null)
    //     {
    //         Debug.LogError("No prefab in " + item.name + ", Plesae assign something!!!");
    //         return;
    //     }
    //     Vector2 randPos = Random.insideUnitCircle.normalized * minRadius;
    //     Vector3 offset = new Vector3(randPos.x, 0, randPos.y+3);
    //     GameObject spawnItem = 
    //         Instantiate(item.gamePrefab, itemPickerTf.transform.position + offset, Quaternion.identity);
    //     spawnItem.GetComponent<ItemOBJ>().SetAmount(amount);

    // }
    public void DropSpawnItem(ItemSO item, int amount)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player == null)
        {
            Debug.LogError("No GameObject with tag 'Player' found!");
            return;
        }

        if (item.gamePrefab == null)
        {
            Debug.LogError("No prefab in " + item.name + ", Please assign something!!!");
            return;
        }
        
        Vector2 randPos = Random.insideUnitCircle.normalized * minRadius;
        Vector3 offset = new Vector3(randPos.x, 2.5f, randPos.y); 
        
        Vector3 spawnPosition = player.transform.position + offset;

        GameObject spawnItem = Instantiate(item.gamePrefab, spawnPosition, Quaternion.identity);
        spawnItem.GetComponent<ItemOBJ>().SetAmount(amount);

        // เพิ่ม Rigidbody และแรงกระทำ
        Rigidbody rb = spawnItem.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = spawnItem.AddComponent<Rigidbody>();
        }
        rb.AddForce(Vector3.down * 3.0f, ForceMode.Impulse); // ให้ไอเท็มตกแรงขึ้นเล็กน้อย
        rb.AddTorque(Random.insideUnitSphere * 5.0f, ForceMode.Impulse); // เพิ่มการหมุนแบบสุ่ม
    }
    // public void DropSpawnItem(ItemSO item, int amount)
    // {
    //     GameObject player = GameObject.FindGameObjectWithTag("Player"); // หา GameObject ที่มีแท็ก "Player"
    
    //     if (player == null)
    //     {
    //         Debug.LogError("No GameObject with tag 'Player' found!");
    //         return;
    //     }

    //     if (item.gamePrefab == null)
    //     {
    //         Debug.LogError("No prefab in " + item.name + ", Please assign something!!!");
    //         return;
    //     }
        
    //     if (item.gamePrefab == null)
    //     {
    //         Debug.LogError("No prefab in " + item.name + ", Plesae assign something!!!");
    //         return;
    //     }
    //     Vector2 randPos = Random.insideUnitCircle.normalized * minRadius;
    //     Vector3 offset = new Vector3(randPos.x, 0, randPos.y);
        
    //     Vector3 spawnPosition = player.transform.position + offset;

    //     // สร้างไอเท็ม
    //     GameObject spawnItem = Instantiate(item.gamePrefab, spawnPosition, Quaternion.identity);
    //     spawnItem.GetComponent<ItemOBJ>().SetAmount(amount);
    // }

    public void SpawnItemByGUI(int SpawnAmount = 1)
    {
        for (int i = 0; i < SpawnAmount; i++)
        {
            int ind = Random.Range(0, itemObject.Count);
            float distance = Random.Range(minRadius, maxRadius);
            Vector2 randPos = Random.insideUnitCircle.normalized * distance;
            Vector3 offset = new Vector3(randPos.x, 0, randPos.y);
            
            ItemOBJ itemobj =  Instantiate(itemObject[ind], itemPickerTf.transform.position + offset, Quaternion.identity);
            itemobj.RandomAmount();
        }
        
        
    }
    private void OnGUI()
    {
       if (GUILayout.Button("Spawn a Random Item "))
        {
            SpawnItemByGUI();
        }
        if (GUILayout.Button("Spawn 10 Random Items "))
        {
            SpawnItemByGUI(10);
        }
        if (GUILayout.Button("Spawn 50 Random Items "))
        {
            SpawnItemByGUI(50);
        }
    }
}
