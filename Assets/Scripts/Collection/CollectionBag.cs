using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CollectionBag : MonoBehaviour
{
    public class Collection { 
        public GameObject collection;
        public Vector3 returnedPosition;
        public Collection(GameObject collection, Vector3 position)
        {
            this.collection = collection;
            this.returnedPosition = position;
        }
    }

    public static CollectionBag Instance;
    public List<Collection> Collections = new List<Collection>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        Collections.Clear();
    }
    public void AddCollection(GameObject collection)
    {
        Collections.Add(new Collection(collection, collection.transform.position));
        collection.SetActive(false);
    }
    public void TakeOutCollection(GameObject c)
    {
        for (int i = 0; i < Collections.Count; i++)
        {
            if (Collections[i].collection == c)
            {
                c.transform.position = Collections[i].returnedPosition;
                c.SetActive(true);
                Collections.RemoveAt(i);
                return;
            }
        }
    }
    public void RemoveCollection(GameObject collection)
    {
        for (int i = 0; i < Collections.Count; i++)
        {
            if (Collections[i].collection == collection)
            {
                Collections.RemoveAt(i);
                return;
            }
        }
    }
    public GameObject ContainsCollection(string name)
    {
        foreach(Collection collection in Collections)
        {
            if (collection.collection.name.Contains(name))
                return collection.collection;
        }
        return null;
    }
}
