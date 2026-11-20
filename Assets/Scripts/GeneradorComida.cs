using UnityEngine;

public class GeneradorComida : MonoBehaviour
{
    public GameObject prefabAGenerar; 

    
    public ItemPancheria GenerarItem()
    {
        GameObject nuevo = Instantiate(prefabAGenerar, transform.position, Quaternion.identity);
        return nuevo.GetComponent<ItemPancheria>();
    }
}