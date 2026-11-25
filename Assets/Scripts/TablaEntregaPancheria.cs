using UnityEngine;

public class TablaEntregaPancheria : MonoBehaviour
{
    public float radioBusquedaNPC = 4f;

    public void ProcesarEntrega(ItemPancheria item)
    {
        Collider[] cercanos = Physics.OverlapSphere(transform.position, radioBusquedaNPC);

        foreach (Collider col in cercanos)
        {
            ClientePancheria npc = col.GetComponent<ClientePancheria>();
            if (npc != null && npc.estadoActual == ClientePancheria.EstadoNPC.EsperandoPedido)
            {
                if (npc.EntregarComida(item.nombrePedido))
                {
                    PrecioObjeto producto = item.GetComponent<PrecioObjeto>();
                    if (producto != null && DineroManager.Instance != null)
                    {
                        DineroManager.Instance.SumarDinero(producto.precio);
                    }
                }

                Destroy(item.gameObject);
                return;
            }
        }

        item.DejarEnMesa(transform.position);
    }
}