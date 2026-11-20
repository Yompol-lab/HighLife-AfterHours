using UnityEngine;

public class ZonaPreparado : MonoBehaviour
{
    public Transform puntoDeAnclaje;

   
    public void RecibirVaso(VasoInteractuable vaso)
    {
        vaso.transform.SetParent(null); 
        vaso.BloquearEnPosicion(puntoDeAnclaje != null ? puntoDeAnclaje : transform);
    }
}