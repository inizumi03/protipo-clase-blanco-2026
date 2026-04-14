using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraMovimiento : MonoBehaviour
{
    [Header("Referencias")]
    public Transform cuerpoJugador;

    [Header("Sensibilidad")]
    public float sensibilidadX = 200f;
    public float sensibilidadY = 200f;

    [Header("Límites verticales")]
    public float limiteArriba = 80f;
    public float limiteAbajo = -80f;

    [Header("Suavizado")]
    public float suavizado = 10f;

    [Header("Inclinación lateral")]
    public float inclinacionMax = 5f;
    public float velocidadInclinacion = 5f;

    [Header("Seguimiento del cuerpo")]
    public float velocidadSeguimiento = 5f;

    
    [Header("Forzar mirada")]
    public bool forzarMirada = false;
    public Transform objetivoMirada;
    public float velocidadForzado = 15f;

    private float rotacionX = 0f;
    private float rotacionY = 0f;
    private float inclinacionActual = 0f;

    private float mouseXSuave;
    private float mouseYSuave;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rotacionY = transform.eulerAngles.y;
    }

    void Update()
    {
        
        if (forzarMirada)
        {
            
            if (objetivoMirada == null || !objetivoMirada.gameObject.activeInHierarchy)
            {
                DesactivarMiradaForzada();
                return;
            }

            Vector3 direccion = objetivoMirada.position - transform.position;
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacionObjetivo,
                Time.deltaTime * velocidadForzado
            );

            // Rotación del cuerpo
            Vector3 direccionHorizontal = direccion;
            direccionHorizontal.y = 0;

            if (direccionHorizontal != Vector3.zero)
            {
                cuerpoJugador.rotation = Quaternion.Slerp(
                    cuerpoJugador.rotation,
                    Quaternion.LookRotation(direccionHorizontal),
                    Time.deltaTime * velocidadForzado
                );
            }

            return;
        }

        

        float mouseX = Input.GetAxis("Mouse X") * sensibilidadX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadY * Time.deltaTime;

        mouseXSuave = Mathf.Lerp(mouseXSuave, mouseX, Time.deltaTime * suavizado);
        mouseYSuave = Mathf.Lerp(mouseYSuave, mouseY, Time.deltaTime * suavizado);

        rotacionX -= mouseYSuave;
        rotacionX = Mathf.Clamp(rotacionX, limiteAbajo, limiteArriba);

        rotacionY += mouseXSuave;

        float objetivoInclinacion = -mouseXSuave * inclinacionMax;
        inclinacionActual = Mathf.Lerp(inclinacionActual, objetivoInclinacion, Time.deltaTime * velocidadInclinacion);

        transform.rotation = Quaternion.Euler(rotacionX, rotacionY, inclinacionActual);

        Quaternion rotacionObjetivoCuerpo = Quaternion.Euler(0f, rotacionY, 0f);
        cuerpoJugador.rotation = Quaternion.Lerp(
            cuerpoJugador.rotation,
            rotacionObjetivoCuerpo,
            Time.deltaTime * velocidadSeguimiento
        );
    }

    //ACTIVAR/DESACTIVAR
    public void ActivarMiradaForzada(Transform objetivo)
    {
        objetivoMirada = objetivo;
        forzarMirada = true;
    }

    public void DesactivarMiradaForzada()
    {
        forzarMirada = false;
        objetivoMirada = null;
    }
}
