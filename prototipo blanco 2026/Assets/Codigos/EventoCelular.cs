using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventoCelular : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject celular; // objeto desactivado
    public CamaraMovimiento camaraScript; // script de cámara

    [Header("Tiempo")]
    public float tiempoEspera = 5f;

    void Start()
    {
        StartCoroutine(Evento());
    }

    IEnumerator Evento()
    {
        // Espera antes de activar
        yield return new WaitForSeconds(tiempoEspera);

        // Activar celular
        celular.SetActive(true);

        //  FORZAR MIRADA (PERMANENTE)
        if (camaraScript != null)
        {
            camaraScript.ActivarMiradaForzada(celular.transform);
        }
    }


}
