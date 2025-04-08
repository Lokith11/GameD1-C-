using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSlot : IventorySlot
{
    public Transform handTransform; // จุดที่ไอเท็มจะปรากฏบนมือผู้เล่น
    public PlayerController playerController;
    private GameObject currentHeldItem;
    private Rigidbody heldItemRigidbody;
    private Collider heldItemCollider;
    private float throwForce = 8.0f;

    void Start()
    {
        
    }
    private void Awake()
    {
        // ถ้าไม่ได้กำหนด handTransform ใน Inspector ให้ลองหา Player
        if (handTransform == null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null && player.holdArea != null)
            {
                handTransform = player.holdArea;
            }
            else
            {
                Debug.LogWarning("ไม่พบ Hold Area ใน PlayerController, สร้างจุดจับใหม่");
                handTransform = new GameObject("DefaultHandTransform").transform;
                handTransform.SetParent(Camera.main.transform); // หรือผู้เล่น
                handTransform.localPosition = new Vector3(0.5f, -0.5f, 1f);
            }
        }
    }

    public override void SetThisSlot(ItemSO newItem, int amount)
    {
        base.SetThisSlot(newItem, amount);
        
        // ลบไอเท็มเก่าถ้ามี
        if (currentHeldItem != null)
        {
            Destroy(currentHeldItem);
        }
        
        // สร้างไอเท็มใหม่ถ้าไม่ใช่สล็อตว่าง
        if (newItem != iventory.Empty_Item && newItem.gamePrefab != null)
        {
            currentHeldItem = Instantiate(newItem.gamePrefab, handTransform);
            currentHeldItem.transform.localPosition = Vector3.zero;
            currentHeldItem.transform.localRotation = Quaternion.identity;
            
            // ปิดการทำงานของ Rigidbody และ Collider ตอนถือ
            DisablePhysics(currentHeldItem);
        }
    }

    public void UseItemInHand()
    {
        if (item != iventory.Empty_Item && item.gamePrefab != null)
        {
            // สร้างไอเท็มในโลกเกม (เช่นขว้างหรือวาง)
            GameObject spawnedItem = Instantiate(item.gamePrefab, handTransform.position, handTransform.rotation);
            
            // เปิดการทำงานของ Physics สำหรับไอเท็มที่ถูกใช้งาน
            EnablePhysics(spawnedItem);
            
            // ลดจำนวนไอเท็ม
            UseItem();
        }
    }

    private void DisablePhysics(GameObject itemObject)
    {
        // หา Rigidbody และ Collider
        heldItemRigidbody = itemObject.GetComponent<Rigidbody>();
        heldItemCollider = itemObject.GetComponent<Collider>();
        
        // ปิด Physics ถ้ามี
        if (heldItemRigidbody != null)
        {
            heldItemRigidbody.isKinematic = true;
            heldItemRigidbody.detectCollisions = false;
        }
        
        if (heldItemCollider != null)
        {
            heldItemCollider.enabled = false;
        }
    }

    private void EnablePhysics(GameObject itemObject)
    {
        // หา Rigidbody และ Collider
        Rigidbody rb = itemObject.GetComponent<Rigidbody>();
        Collider col = itemObject.GetComponent<Collider>();
        
        // เปิด Physics ถ้ามี
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            
            // สามารถเพิ่มแรงขว้างได้ที่นี่ (ถ้าต้องการ)
            rb.AddForce(handTransform.forward * throwForce, ForceMode.Impulse);
        }
        
        if (col != null)
        {
            col.enabled = true;
        }
    }
}
