using System.Collections.Generic;
using UnityEngine;

public class GestorPancheria : MonoBehaviour
{
    [Header("Tus 3 Slots de Prefabs de NPC")]
    public GameObject prefabCliente1;
    public GameObject prefabCliente2;
    public GameObject prefabCliente3;

    [Header("Configuración de Spawn")]
    public Transform puntoSpawn;
    public Transform puntoDespawn;
    public float intervaloSpawn = 45f;
    public int maxClientesEnFila = 6;

    [Header("Configuración de Fila")]
    public Transform puntoMostrador; 
    public float distanciaEntreClientes = 1.5f;

    private float temporizador;
    private List<ClientePancheria> filaClientes = new List<ClientePancheria>();

    void Update()
    {
        temporizador += Time.deltaTime;

        if (temporizador >= intervaloSpawn && filaClientes.Count < maxClientesEnFila)
        {
            GenerarCliente();
            temporizador = 0f;
        }
    }

    void GenerarCliente()
    {
        
        List<GameObject> opciones = new List<GameObject>();
        if (prefabCliente1 != null) opciones.Add(prefabCliente1);
        if (prefabCliente2 != null) opciones.Add(prefabCliente2);
        if (prefabCliente3 != null) opciones.Add(prefabCliente3);

        if (opciones.Count == 0) return;

        int indiceAzar = Random.Range(0, opciones.Count);
        GameObject prefabElegido = opciones[indiceAzar];

        GameObject nuevoObj = Instantiate(prefabElegido, puntoSpawn.position, Quaternion.identity);
        ClientePancheria cliente = nuevoObj.GetComponent<ClientePancheria>();

        cliente.Inicializar(this, puntoDespawn, puntoMostrador);
        filaClientes.Add(cliente);

        ActualizarPosicionesFila();
    }

    public void ClienteAtendido(ClientePancheria cliente)
    {
        if (filaClientes.Contains(cliente))
        {
            filaClientes.Remove(cliente);
            ActualizarPosicionesFila();
        }
    }

    public void ActualizarPosicionesFila()
    {
        for (int i = 0; i < filaClientes.Count; i++)
        {
            
            Vector3 posicionFila = puntoMostrador.position + (puntoMostrador.forward * (i * distanciaEntreClientes));
            bool esElPrimero = (i == 0);

            filaClientes[i].MoverAPosicionFila(posicionFila, esElPrimero);
        }
    }
}