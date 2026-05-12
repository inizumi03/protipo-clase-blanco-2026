using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Canvas de pausa")]
    public GameObject menuPausa;

    private bool juegoPausado = false;

    void Update()
    {
        // Abrir/Cerrar pausa con ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                ReanudarJuego();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    // =========================
    // PAUSA
    // =========================

    public void PausarJuego()
    {
        menuPausa.SetActive(true);

        Time.timeScale = 0f;
        juegoPausado = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReanudarJuego()
    {
        menuPausa.SetActive(false);

        Time.timeScale = 1f;
        juegoPausado = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // =========================
    // ESCENAS
    // =========================

    // Cargar escena por nombre
    public void CambiarEscena(string nombreEscena)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscena);
    }

    // Reiniciar escena actual
    public void ReiniciarEscena()
    {
        Time.timeScale = 1f;

        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.buildIndex);
    }

    // =========================
    // SALIR DEL JUEGO
    // =========================

    public void SalirDelJuego()
    {
        Debug.Log("Salir del juego");

        Application.Quit();
    }
}
