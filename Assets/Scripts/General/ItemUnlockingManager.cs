using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUnlockingManager : MonoBehaviour
{
    public List<ItemUnlockInfo> _heightItems;
    public List<ItemUnlockInfo> _distanceItems;
    [SerializeField] FlyingManager _flyingManager;

    private int _currentBestDistIndex = 0;
    private int _currentBestHeightIndex = 0;

    public List<GameObject> _initiallyUnlockedItems;

    private List<GameObject> _unlockedItems;
    private int _instantiatedIndex = 0;
    public Transform _itemCollection;
    public List<GameObject> UnlockedItems => _unlockedItems;

    private GameObject _lastUnlockedDistItem;
    private GameObject _lastUnlockedHeightItem;
    private Transform[] _itemCollectionTransforms;
    void Start()
    {
        _itemCollectionTransforms = _itemCollection.GetComponentsInChildren<Transform>();
        if (_flyingManager == null)
            _flyingManager = FindObjectOfType<FlyingManager>();
        _unlockedItems = new List<GameObject>();
        _unlockedItems.AddRange(_initiallyUnlockedItems);
        InstantiateItems();
    }

    private void InstantiateItems()
    {
        for (int i = _instantiatedIndex; i < UnlockedItems.Count; i++)
        {
            if (_itemCollectionTransforms.Length > i)
            {
                var itemParent = _itemCollectionTransforms[i];
                GameObject item = Instantiate(_unlockedItems[i], itemParent);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.transform.localScale= Vector3.one;
                _instantiatedIndex++;
            }
        }
    }

    public ItemUnlockInfo GetNextDistanceItem()
    {
        ItemUnlockInfo nextItem = null;
        if (_currentBestDistIndex < _distanceItems.Count)
        {
            nextItem = _distanceItems[_currentBestDistIndex];
        }
        return nextItem;
    }

    public ItemUnlockInfo GetNextHeightItem()
    {
        ItemUnlockInfo nextItem = null;
        if (_currentBestHeightIndex < _heightItems.Count)
        {
            nextItem = _heightItems[_currentBestHeightIndex];
        }
        return nextItem;
    }

    public GameObject GetUnlockedHeightItem()
    {
        return _lastUnlockedHeightItem;
    }

    public GameObject GetUnlockedDistItem()
    {
        return _lastUnlockedDistItem;
    }


    public void RefreshUnlocks()
    {
        _lastUnlockedDistItem = null;
        _lastUnlockedHeightItem = null;
        float bestDist = _flyingManager.bestDistanceInRun;
        float bestHeight = _flyingManager.bestHeightInRun;
        UnlockItems(bestDist, bestHeight);
    }

    public void UnlockItems(float bestDist, float bestHeight)
    {
        MaybeUnlockDistItem(bestDist);
        MaybeUnlockHeightItem(bestHeight);
        InstantiateItems();
    }

    void MaybeUnlockDistItem(float currentDist)
    {
        if (_currentBestDistIndex < _distanceItems.Count)
        {
            var item = _distanceItems[_currentBestDistIndex];
            if (currentDist >= item.metersNeeded)
            {
                if (!_unlockedItems.Contains(item.itemPrefab))
                {
                    _unlockedItems.Add(item.itemPrefab);
                    _lastUnlockedDistItem = item.itemPrefab;
                    _currentBestDistIndex++;
                }
            }
        }
    }    
    void MaybeUnlockHeightItem(float currentHeight)
    {
        if (_currentBestHeightIndex < _heightItems.Count)
        {
            var item = _heightItems[_currentBestHeightIndex];
            if (currentHeight >= item.metersNeeded)
            {
                if (!_unlockedItems.Contains(item.itemPrefab))
                {
                    _unlockedItems.Add(item.itemPrefab);
                    _lastUnlockedHeightItem = item.itemPrefab;
                    _currentBestHeightIndex++;
                }
            }
        }
    }

}

[Serializable]
public class ItemUnlockInfo
{
    public int metersNeeded;
    public GameObject itemPrefab;
}
