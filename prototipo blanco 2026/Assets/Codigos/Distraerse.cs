using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraerse : MonoBehaviour
{
    public float distancia = 3f;
    public LayerMask capaCelular;
    public GameObject imagenUI;
    public Transform puntoCara;

    public MiniJuegoCelular miniJuego; // REFERENCIA AL MINIJUEGO

    private GameObject celularActual;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distancia, capaCelular))
        {
            if (hit.collider.CompareTag("Ventana"))
            {
                celularActual = hit.collider.gameObject;

                // Mostrar UI solo si todavía existe
                if (imagenUI != null)
                    imagenUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(MoverSuave(celularActual));
                }
            }
        }
        else
        {
            if (imagenUI != null)
                imagenUI.SetActive(false);

            celularActual = null;
        }
    }

    IEnumerator MoverSuave(GameObject obj)
    {
        float tiempo = 0;

        Vector3 inicioPos = obj.transform.position;
        Quaternion inicioRot = obj.transform.rotation;

        Quaternion rotacionFinal = puntoCara.rotation * Quaternion.Euler(-180f, 0f, 0f);

        while (tiempo < 1)
        {
            tiempo += Time.deltaTime * 5f;

            obj.transform.position = Vector3.Lerp(inicioPos, puntoCara.position, tiempo);
            obj.transform.rotation = Quaternion.Lerp(inicioRot, rotacionFinal, tiempo);

            yield return null;
        }

        obj.transform.position = puntoCara.position;
        obj.transform.rotation = rotacionFinal;

        obj.transform.SetParent(puntoCara);

        obj.tag = "Aburrido";

        // DESTRUIR LA UI
        if (imagenUI != null)
        {
            Destroy(imagenUI);
        }

        // ACTIVAR MINIJUEGO
        if (miniJuego != null)
        {
            miniJuego.ActivarMinijuego(obj);
        }
    }
}


