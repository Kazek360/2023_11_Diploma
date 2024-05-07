using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour, ITooltippable
{
    [SerializeField] private int ItemId;
    [SerializeField] private int Amount;
    [SerializeField] private GameObject _pickupIndicatorSprite;
    
    private SpriteRenderer _spriteRenderer;
    private bool _inRange;
    

    public void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = ItemLibrary.GetInstance().GetItem(ItemId).ItemSprite;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _inRange)
        {
            GameConfig.PlayerInventory.AddItem(ItemId, Amount);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = true;
            _pickupIndicatorSprite.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = false;
            _pickupIndicatorSprite.SetActive(false);
        }
    }

    public string GetTooltipHeader()
    {
        return ItemLibrary.GetInstance().GetItem(ItemId).ItemName;
    }

    public string GetTooltipDesc()
    {
        return ItemLibrary.GetInstance().GetItem(ItemId).ItemDesc;
    }
}
