using UnityEngine;

public class ItemPancheria : MonoBehaviour
{
    public string nombrePedido = "Pan"; 

   
    public void BloquearEnMano(Transform puntoMano)
    {
        transform.SetParent(puntoMano);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

   
    public void DejarEnMesa(Vector3 puntoDestino)
    {
        transform.SetParent(null);
        transform.position = puntoDestino;
        
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}