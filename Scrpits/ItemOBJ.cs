using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemOBJ : MonoBehaviour
{
    public ItemSO item;
    public int amount = 1;
    public TextMeshProUGUI amountText;
    public int IDs;
    

    void Start()
    {
        
    }
    public void SetAmount(int newAmount)
    {
        amount = newAmount;
        amountText.text = amount.ToString();
    }
    public void RandomAmount()
    {
        amount = Random.Range(1, item.maxStack + 1);
        amountText.text = amount.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().inventory.AddItem(item, amount);
            Destroy(gameObject);

        }
    }

    
}
