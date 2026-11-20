using UnityEngine;

public class VasoInteractuable : MonoBehaviour
{
    public string tipoDeTrago = "CubaLibre"; 
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

   
    public void BloquearEnPosicion(Transform nuevaPosicion)
    {
        if (rb != null)
        {
            rb.isKinematic = true; 
        }

        transform.position = nuevaPosicion.position;
        transform.rotation = nuevaPosicion.rotation; 
    }

   
    public void LiberarVaso()
    {
        if (rb != null)
        {
            rb.isKinematic = false; 
        }
    }
}