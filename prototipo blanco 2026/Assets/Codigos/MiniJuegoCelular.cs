using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MiniJuegoCelular : MonoBehaviour
{
    [Header("UI Flechas")]
    public List<Image> flechasUI = new List<Image>();

    public Color colorNormal = Color.gray;
    public Color colorActivo = Color.white;
    public Color colorCorrecto = Color.green;
    public Color colorError = Color.red;

    [Header("Sprites (WASD)")]
    public Sprite spriteW;
    public Sprite spriteS;
    public Sprite spriteA;
    public Sprite spriteD;

    [Header("Config")]
    public int longitudSecuencia = 4;
    public GameObject celular;

    private List<KeyCode> secuencia = new List<KeyCode>();
    private int inputIndex = 0;
    private bool enMinijuego = false;

    void Start()
    {
        GenerarSecuencia();
        OcultarFlechas();
    }

    void Update()
    {
        if (enMinijuego)
        {
            DetectarInput();
        }
    }

    //  ESTA ES LA CLAVE
    public void ActivarMinijuego()
    {
        if (enMinijuego) return;

        enMinijuego = true;
        inputIndex = 0;

        MostrarFlechas();
        ActualizarUI();
        ActualizarIndicador();

        Debug.Log("MINIJUEGO ACTIVADO");
    }

    // =========================================================

    void GenerarSecuencia()
    {
        secuencia.Clear();

        KeyCode[] opciones = {
            KeyCode.W,
            KeyCode.S,
            KeyCode.A,
            KeyCode.D
        };

        for (int i = 0; i < longitudSecuencia; i++)
        {
            secuencia.Add(opciones[Random.Range(0, opciones.Length)]);
        }
    }

    void DetectarInput()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(secuencia[inputIndex]))
            {
                flechasUI[inputIndex].color = colorCorrecto;
                inputIndex++;

                if (inputIndex >= secuencia.Count)
                {
                    Debug.Log("CELULAR QUITADO");

                    enMinijuego = false;

                    if (celular != null)
                        celular.SetActive(false);

                    OcultarFlechas();
                    GenerarSecuencia();
                }
                else
                {
                    ActualizarIndicador();
                }
            }
            else
            {
                flechasUI[inputIndex].color = colorError;

                inputIndex = 0;
                ActualizarIndicador();
            }
        }
    }

    // =========================================================
    // UI
    // =========================================================

    void MostrarFlechas()
    {
        for (int i = 0; i < flechasUI.Count; i++)
        {
            if (i < secuencia.Count)
                flechasUI[i].gameObject.SetActive(true);
            else
                flechasUI[i].gameObject.SetActive(false);
        }
    }

    void OcultarFlechas()
    {
        foreach (Image img in flechasUI)
        {
            img.gameObject.SetActive(false);
        }
    }

    void ActualizarUI()
    {
        for (int i = 0; i < secuencia.Count; i++)
        {
            flechasUI[i].sprite = ObtenerSprite(secuencia[i]);
            flechasUI[i].color = colorNormal;
        }
    }

    void ActualizarIndicador()
    {
        for (int i = 0; i < secuencia.Count; i++)
        {
            if (i == inputIndex)
                flechasUI[i].color = colorActivo;
            else if (flechasUI[i].color != colorCorrecto)
                flechasUI[i].color = colorNormal;
        }
    }

    Sprite ObtenerSprite(KeyCode tecla)
    {
        switch (tecla)
        {
            case KeyCode.W: return spriteW;
            case KeyCode.S: return spriteS;
            case KeyCode.A: return spriteA;
            case KeyCode.D: return spriteD;
        }

        return null;
    }
}
