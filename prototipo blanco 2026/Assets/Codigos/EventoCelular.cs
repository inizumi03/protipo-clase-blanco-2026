using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventoObjeto
{
    public GameObject objeto;     // objeto a activar
    public float tiempoEspera;    // tiempo antes de activarse
}

public class EventoCelular : MonoBehaviour
{
    [Header("Lista de eventos")]
    public List<EventoObjeto> eventos = new List<EventoObjeto>();

    [Header("Referencia cámara")]
    public CamaraMovimiento camaraScript;

    private GameObject objetoActual;

    void Start()
    {
        StartCoroutine(SecuenciaEventos());
    }

    IEnumerator SecuenciaEventos()
    {
        foreach (EventoObjeto evento in eventos)
        {
            //  Esperar tiempo
            yield return new WaitForSeconds(evento.tiempoEspera);

            //  Desactivar el anterior
            if (objetoActual != null)
            {
                objetoActual.SetActive(false);
            }

            //  Activar nuevo objeto
            objetoActual = evento.objeto;
            objetoActual.SetActive(true);

            //  Forzar cámara
            if (camaraScript != null)
            {
                camaraScript.ActivarMiradaForzada(objetoActual.transform);
            }
        }
    }


}
