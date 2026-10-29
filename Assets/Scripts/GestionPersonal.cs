using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GestionPersonal : MonoBehaviour
{
    [System.Serializable]
    public class PuestoEmpleado
    {
        public string nombrePuesto;              
        public int costoContratar = 200;        

        [Header("Íconos de Estado")]
        public GameObject iconoNoContratado;    
        public GameObject iconoContratado;       

        [Header("Botones de Acción")]
        public GameObject botonContratar;        
        public GameObject botonDespedir;         

        [HideInInspector] public bool estaContratado = false;
    }

    [Header("Interfaz General")]
    public GameObject panelGestion;         
    public GameObject botonCerrarX;              
    public TextMeshProUGUI textoDinero;         
    public TextMeshProUGUI textoEmpleados;      

    [Header("Configuración de Economía")]
    public int dineroActual = 1000;             
    public float porcentajeRecargoDespido = 10f; 
    private int totalEmpleadosContratados = 0;

    [Header("Lista Escalable de Empleados")]
    public List<PuestoEmpleado> empleados = new List<PuestoEmpleado>();

    void Start()
    {
        
        if (botonCerrarX != null)
        {
            Button btnCerrar = botonCerrarX.GetComponent<Button>();
            if (btnCerrar == null) btnCerrar = botonCerrarX.AddComponent<Button>();
            btnCerrar.onClick.RemoveAllListeners();
            btnCerrar.onClick.AddListener(CerrarPanel);
        }

        
        for (int i = 0; i < empleados.Count; i++)
        {
            int indice = i; 
            PuestoEmpleado emp = empleados[i];

         
            if (emp.botonContratar != null)
            {
                Button btnC = emp.botonContratar.GetComponent<Button>();
                if (btnC == null) btnC = emp.botonContratar.AddComponent<Button>();
                btnC.onClick.RemoveAllListeners();
                btnC.onClick.AddListener(() => ContratarEmpleado(indice));

               
                TextMeshProUGUI txtC = emp.botonContratar.GetComponentInChildren<TextMeshProUGUI>(true);
                if (txtC != null) txtC.text = "Contratar: $" + emp.costoContratar;
            }

           
            if (emp.botonDespedir != null)
            {
                Button btnD = emp.botonDespedir.GetComponent<Button>();
                if (btnD == null) btnD = emp.botonDespedir.AddComponent<Button>();
                btnD.onClick.RemoveAllListeners();
                btnD.onClick.AddListener(() => DespedirEmpleado(indice));

                
                int costoDespido = CalcularCostoDespido(emp.costoContratar);
                TextMeshProUGUI txtD = emp.botonDespedir.GetComponentInChildren<TextMeshProUGUI>(true);
                if (txtD != null) txtD.text = "Despedir: $" + costoDespido;
            }

            ActualizarVisualEmpleado(emp);
        }

        ActualizarContadoresGenerales();
    }

    
    private int CalcularCostoDespido(int costoBase)
    {
       
        return Mathf.RoundToInt(costoBase * (1f + (porcentajeRecargoDespido / 100f)));
    }

    
    public void ContratarEmpleado(int indice)
    {
        PuestoEmpleado emp = empleados[indice];
        if (emp.estaContratado) return;

        if (dineroActual >= emp.costoContratar)
        {
            dineroActual -= emp.costoContratar;
            emp.estaContratado = true;
            totalEmpleadosContratados++;

            ActualizarVisualEmpleado(emp);
            ActualizarContadoresGenerales();
        }
        else
        {
            Debug.Log("No te alcanza la plata para contratar a: " + emp.nombrePuesto);
        }
    }

    public void DespedirEmpleado(int indice)
    {
        PuestoEmpleado emp = empleados[indice];
        if (!emp.estaContratado) return;

        int costoDespido = CalcularCostoDespido(emp.costoContratar);

        if (dineroActual >= costoDespido)
        {
            dineroActual -= costoDespido;
            emp.estaContratado = false;
            totalEmpleadosContratados--;

            ActualizarVisualEmpleado(emp);
            ActualizarContadoresGenerales();
        }
        else
        {
            Debug.Log("No te alcanza la plata para pagar la indemnización de: " + emp.nombrePuesto);
        }
    }

    
    private void ActualizarVisualEmpleado(PuestoEmpleado emp)
    {
        
        if (emp.iconoContratado != null) emp.iconoContratado.SetActive(emp.estaContratado);
        if (emp.iconoNoContratado != null) emp.iconoNoContratado.SetActive(!emp.estaContratado);

        if (emp.botonDespedir != null) emp.botonDespedir.SetActive(emp.estaContratado);
        if (emp.botonContratar != null) emp.botonContratar.SetActive(!emp.estaContratado);
    }

    private void ActualizarContadoresGenerales()
    {
        if (textoDinero != null) textoDinero.text = "Dinero: $" + dineroActual;
        if (textoEmpleados != null) textoEmpleados.text = "Empleados: " + totalEmpleadosContratados;
    }

    
    public void AbrirPanel()
    {
        if (panelGestion != null) panelGestion.SetActive(true);
        ActualizarContadoresGenerales();
    }

    public void CerrarPanel()
    {
        if (panelGestion != null) panelGestion.SetActive(false);
    }
}