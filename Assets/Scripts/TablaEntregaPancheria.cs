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
                
                npc.EntregarComida(item.nombrePedido);

               
                Destroy(item.gameObject);
                return;
            }
        }


        item.DejarEnMesa(transform.position);
    }
}