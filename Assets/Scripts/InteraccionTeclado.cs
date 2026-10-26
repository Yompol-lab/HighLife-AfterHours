using UnityEngine;
using System.Collections;

public class InteraccionTeclado : MonoBehaviour
{
    [Header("Configuración de Cámara")]
    public Camera camaraAUsar;
    public Transform puntoMonitor;
    public float velocidadTransicion = 3f;

    private Vector3 posicionGuardada;
    private Quaternion rotacionGuardada;
    private bool viendoMonitor = false;
    private Coroutine transicionActual;

    void OnMouseDown()
    {
        if (camaraAUsar == null || puntoMonitor == null) return;

        if (transicionActual != null) StopCoroutine(transicionActual);

        if (!viendoMonitor)
        {
            
            posicionGuardada = camaraAUsar.transform.position;
            rotacionGuardada = camaraAUsar.transform.rotation;

           
            viendoMonitor = true;
            transicionActual = StartCoroutine(MoverCamara(puntoMonitor.position, puntoMonitor.rotation));
        }
        else
        {
            
            viendoMonitor = false;
            transicionActual = StartCoroutine(MoverCamara(posicionGuardada, rotacionGuardada));
        }
    }

    private IEnumerator MoverCamara(Vector3 posDestino, Quaternion rotDestino)
    {
        float tiempo = 0;
        Vector3 posInicio = camaraAUsar.transform.position;
        Quaternion rotInicio = camaraAUsar.transform.rotation;

        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidadTransicion;

            camaraAUsar.transform.position = Vector3.Lerp(posInicio, posDestino, Mathf.SmoothStep(0, 1, tiempo));
            camaraAUsar.transform.rotation = Quaternion.Lerp(rotInicio, rotDestino, Mathf.SmoothStep(0, 1, tiempo));

            yield return null;
        }
    }
}