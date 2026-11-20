using UnityEngine;

public class TablaPreparacion : MonoBehaviour
{
    [Header("Modelos Falsos (Hijos inactivos)")]
    public GameObject visualPanVacio;
    public GameObject visualPanchoArmado;

    [Header("El resultado final")]
    public GameObject prefabPanchoTerminado; 

    private int estado = 0; 

    void Start()
    {
        visualPanVacio.SetActive(false);
        visualPanchoArmado.SetActive(false);
    }

    
    public bool RecibirIngrediente(ItemPancheria item)
    {
        if (estado == 0 && item.nombrePedido == "Pan")
        {
            estado = 1;
            visualPanVacio.SetActive(true);
            return true;
        }
        if (estado == 1 && item.nombrePedido == "Salchicha")
        {
            estado = 2;
            visualPanVacio.SetActive(false);
            visualPanchoArmado.SetActive(true);
            return true; 
        }
        return false; 
    }

    public bool TienePanchoListo() { return estado == 2; }

    
    public ItemPancheria EntregarPanchoListo()
    {
        if (estado == 2)
        {
            estado = 0;
            visualPanchoArmado.SetActive(false); 
            GameObject nuevo = Instantiate(prefabPanchoTerminado, transform.position, Quaternion.identity);
            return nuevo.GetComponent<ItemPancheria>();
        }
        return null;
    }
}