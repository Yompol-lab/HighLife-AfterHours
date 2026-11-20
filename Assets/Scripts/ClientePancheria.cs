using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ClientePancheria : MonoBehaviour
{
    public enum EstadoNPC { YendoEnFila, EsperandoPedido, Saliendo }
    public EstadoNPC estadoActual = EstadoNPC.YendoEnFila;

    [Header("Interfaz UI y Pedidos")]
    public GameObject burbujaPedido;
    public Image[] slotsPedido;
    public Sprite[] opcionesDeComida;

    [HideInInspector]
    public List<string> listaPedidoActual = new List<string>();

    private NavMeshAgent agente;
    private Animator animator;
    private GestorPancheria gestor;
    private Transform puntoDespawn;
    private Transform mostrador; 
    private bool esPrimero = false;

    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (burbujaPedido != null) burbujaPedido.SetActive(false);

        GenerarPedidoMultiple();
    }

    void GenerarPedidoMultiple()
    {
        if (opcionesDeComida.Length == 0 || slotsPedido.Length == 0) return;

        int cantidadAComprar = Random.Range(1, slotsPedido.Length + 1);

        for (int i = 0; i < slotsPedido.Length; i++)
        {
            if (i < cantidadAComprar)
            {
                int indiceAzar = Random.Range(0, opcionesDeComida.Length);
                slotsPedido[i].sprite = opcionesDeComida[indiceAzar];
                slotsPedido[i].gameObject.SetActive(true);
                listaPedidoActual.Add(opcionesDeComida[indiceAzar].name);
            }
            else
            {
                slotsPedido[i].gameObject.SetActive(false);
            }
        }
    }

    public void Inicializar(GestorPancheria gestorPadre, Transform salida, Transform refMostrador)
    {
        gestor = gestorPadre;
        puntoDespawn = salida;
        mostrador = refMostrador; 
    }

    public void MoverAPosicionFila(Vector3 posicion, bool primero)
    {
        esPrimero = primero;
        estadoActual = EstadoNPC.YendoEnFila;

        if (burbujaPedido != null && !esPrimero) burbujaPedido.SetActive(false);

        agente.SetDestination(posicion);
        agente.isStopped = false;
    }

    void Update()
    {
        animator.SetFloat("Velocidad", agente.velocity.magnitude);

        if (estadoActual == EstadoNPC.YendoEnFila && !agente.pathPending && agente.remainingDistance <= 0.2f)
        {
            agente.isStopped = true;

           
            if (mostrador != null)
            {
                Vector3 direccionMirar = (mostrador.position - transform.position).normalized;
                direccionMirar.y = 0; 
                if (direccionMirar != Vector3.zero)
                {
                    Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMirar);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
                }
            }

            if (esPrimero)
            {
                estadoActual = EstadoNPC.EsperandoPedido;
                if (burbujaPedido != null) burbujaPedido.SetActive(true);
            }
        }
        else if (estadoActual == EstadoNPC.EsperandoPedido)
        {
            
            if (mostrador != null)
            {
                Vector3 direccionMirar = (mostrador.position - transform.position).normalized;
                direccionMirar.y = 0;
                if (direccionMirar != Vector3.zero)
                {
                    Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMirar);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
                }
            }
        }
        else if (estadoActual == EstadoNPC.Saliendo && !agente.pathPending && agente.remainingDistance <= 0.5f)
        {
            Destroy(gameObject);
        }
    }

    public void EntregarComida(string nombreComidaEntregada)
    {
        if (estadoActual == EstadoNPC.EsperandoPedido)
        {
            if (listaPedidoActual.Contains(nombreComidaEntregada))
            {
                listaPedidoActual.Remove(nombreComidaEntregada);

                foreach (Image slot in slotsPedido)
                {
                    if (slot.gameObject.activeSelf && slot.sprite.name == nombreComidaEntregada)
                    {
                        slot.gameObject.SetActive(false);
                        break;
                    }
                }

                if (listaPedidoActual.Count == 0)
                {
                    TerminarYSalir();
                }
            }
        }
    }

    private void TerminarYSalir()
    {
        if (burbujaPedido != null) burbujaPedido.SetActive(false);
        estadoActual = EstadoNPC.Saliendo;

        gestor.ClienteAtendido(this);

        agente.isStopped = false;
        agente.SetDestination(puntoDespawn.position);
    }
}