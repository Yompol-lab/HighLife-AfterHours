using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ComportamientoNPC : MonoBehaviour
{
    
    public enum EstadoNPC { Paseando, YendoALaBarra, EsperandoTrago }
    public EstadoNPC estadoActual = EstadoNPC.YendoALaBarra;

    private NavMeshAgent agente;
    private Animator animator;

    [Header("Configuración de Rutas")]
    public Transform barraBoliche;
    public float radioDePaseo = 10f;

    [Header("Tiempos")]
    public float pacienciaEnBarra = 15f; 
    public float tiempoPaseoMin = 10f;
    public float tiempoPaseoMax = 20f;

    [Header("Interfaz UI")]
    public GameObject burbujaTrago; 

    private bool rutinaActiva = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (burbujaTrago != null) burbujaTrago.SetActive(false);

       
        if (barraBoliche != null) IrALaBarra();
        else IrPuntoAleatorio();
    }

    void Update()
    {
        animator.SetFloat("Velocidad", agente.velocity.magnitude);

      
        if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance && !rutinaActiva)
        {
            if (estadoActual == EstadoNPC.YendoALaBarra)
            {
                StartCoroutine(EsperarEnBarra());
            }
            else if (estadoActual == EstadoNPC.Paseando)
            {
                StartCoroutine(PasearYVolver());
            }
        }
    }

 
    private IEnumerator EsperarEnBarra()
    {
        rutinaActiva = true;
        estadoActual = EstadoNPC.EsperandoTrago;
        agente.isStopped = true; 


        if (burbujaTrago != null) burbujaTrago.SetActive(true);


        yield return new WaitForSeconds(pacienciaEnBarra);

        if (estadoActual == EstadoNPC.EsperandoTrago)
        {
            Debug.Log("Se cansó de esperar el trago y se va a pasear.");
            IrPuntoAleatorio();
        }
    }

    public void RecibirTrago()
    {
        if (estadoActual == EstadoNPC.EsperandoTrago)
        {
            Debug.Log("Recibió el trago, está feliz y se va a la pista.");
            IrPuntoAleatorio();
        }
    }

    private IEnumerator PasearYVolver()
    {
        rutinaActiva = true;
        agente.isStopped = true;

        
        float tiempo = Random.Range(tiempoPaseoMin, tiempoPaseoMax);
        yield return new WaitForSeconds(tiempo);

        IrALaBarra();
    }

  
    public void IrALaBarra()
    {
        estadoActual = EstadoNPC.YendoALaBarra;
        if (burbujaTrago != null) burbujaTrago.SetActive(false); 

        if (agente.isOnNavMesh && barraBoliche != null)
        {
            agente.isStopped = false;
            
            Vector3 puntoCercano = barraBoliche.position + (Random.insideUnitSphere * 2f);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(puntoCercano, out hit, 2f, 1))
                agente.SetDestination(hit.position);
            else
                agente.SetDestination(barraBoliche.position);
        }
        rutinaActiva = false;
    }

    private void IrPuntoAleatorio()
    {
        estadoActual = EstadoNPC.Paseando;
        if (burbujaTrago != null) burbujaTrago.SetActive(false); 

        agente.isStopped = false;
        Vector3 direccionAleatoria = Random.insideUnitSphere * radioDePaseo;
        direccionAleatoria += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(direccionAleatoria, out hit, radioDePaseo, 1))
        {
            agente.SetDestination(hit.position);
        }
        rutinaActiva = false;
    }
}