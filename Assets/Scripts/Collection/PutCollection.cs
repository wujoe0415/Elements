using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutCollection : Interactable
{
    [System.Serializable]
    public class CollectionSlot
    {
        public GameObject collection;
        public Vector3 position;
        public bool isCollected = false;
    }
    public List<CollectionSlot> Collections = new List<CollectionSlot>();
    public bool isRandom = false;

    public void Start()
    {
        foreach(Transform transform in transform)
        {
            if(transform.gameObject.GetComponent<Collectable>() == null)
                continue;
            CollectionSlot slot = new CollectionSlot();
            
            slot.collection = transform.gameObject;
            slot.position = transform.position;
            slot.isCollected = false;
            Collections.Add(slot);
        }
    }
    public override void Interact()
    {
        base.Interact();
        PutCollections();
    }
    public void PutCollections()
    {
        List<CollectionSlot> tempCollections = new List<CollectionSlot>();
        foreach(CollectionSlot slot in Collections)
        {
            if (slot.isCollected)
                tempCollections.Add(slot);
        }
        Debug.Log(tempCollections.Count);
        if (tempCollections.Count == 0)
            return;
        CollectionSlot finalSlot = tempCollections[(isRandom ? Random.Range(0, tempCollections.Count) : 0)];
        
        CollectionBag.Instance.TakeOutCollection(finalSlot.collection);
        finalSlot.isCollected = false;
    }
}
