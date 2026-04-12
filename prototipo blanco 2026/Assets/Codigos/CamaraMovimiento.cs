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
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadY * Time.deltaTime;

        // Suavizado
        mouseXSuave = Mathf.Lerp(mouseXSuave, mouseX, Time.deltaTime * suavizado);
        mouseYSuave = Mathf.Lerp(mouseYSuave, mouseY, Time.deltaTime * suavizado);

        // Rotaciones
        rotacionX -= mouseYSuave;
        rotacionX = Mathf.Clamp(rotacionX, limiteAbajo, limiteArriba);

        rotacionY += mouseXSuave;

        // Inclinación lateral
        float objetivoInclinacion = -mouseXSuave * inclinacionMax;
        inclinacionActual = Mathf.Lerp(inclinacionActual, objetivoInclinacion, Time.deltaTime * velocidadInclinacion);

        // Aplicar rotación completa a la cámara
        transform.rotation = Quaternion.Euler(rotacionX, rotacionY, inclinacionActual);

        // El cuerpo sigue lentamente a la cámara (más realista)
        Quaternion rotacionObjetivo = Quaternion.Euler(0f, rotacionY, 0f);
        cuerpoJugador.rotation = Quaternion.Lerp(
            cuerpoJugador.rotation,
            rotacionObjetivo,
            Time.deltaTime * velocidadSeguimiento
        );
    }
}
