using UnityEngine;

public class InterruptorVidrio : MonoBehaviour
{
    [Header("Asignar en el Inspector")]
    public GameObject filtroNegro; 

   
    public void AlternarVidrio()
    {
       
        if (filtroNegro != null)
        {
            filtroNegro.SetActive(!filtroNegro.activeSelf);
        }
    }
}