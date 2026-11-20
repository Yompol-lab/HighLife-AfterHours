using UnityEngine;

public class ZonaEntrega : MonoBehaviour
{
    public Transform puntoDeAnclaje;
    public float radioBusquedaNPC = 3f;

    public void RecibirVaso(VasoInteractuable vaso)
    {
        vaso.transform.SetParent(null);
        vaso.BloquearEnPosicion(puntoDeAnclaje != null ? puntoDeAnclaje : transform);
        EntregarTragoANPC(vaso);
    }

    private void EntregarTragoANPC(VasoInteractuable vasoEntregado)
    {
        Collider[] objetosCercanos = Physics.OverlapSphere(transform.position, radioBusquedaNPC);
        foreach (Collider obj in objetosCercanos)
        {
            ComportamientoNPC npc = obj.GetComponent<ComportamientoNPC>();
            if (npc != null && npc.estadoActual == ComportamientoNPC.EstadoNPC.EsperandoTrago)
            {
                npc.RecibirTrago();
                Destroy(vasoEntregado.gameObject);
                return;
            }
        }
    }
}