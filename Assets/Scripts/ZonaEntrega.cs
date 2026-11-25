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
                PrecioObjeto producto = vasoEntregado.GetComponent<PrecioObjeto>();
                if (producto != null && DineroManager.Instance != null)
                {
                    DineroManager.Instance.SumarDinero(producto.precio);
                }

                npc.RecibirTrago();
                Destroy(vasoEntregado.gameObject);
                return;
            }
        }
    }
}