using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{

    int ancho = 10;
    int alto = 10;
    public GameObject puzzle;
    private GameObject[,] grid;
    bool jugador = false; //false = red, true = blue.
    Color turnoempty;
    Color turnoJugador;
    bool ganador;
    bool empate;

    //textos de la escena de jugadores
    public GameObject turnoJugador1_text;
    public GameObject turnoJugador2_text;

    //texto de jugadores ganadores
    public GameObject ganador1_text;
    public GameObject ganador2_text;
    public GameObject empate_text;

    // tiempo de partida 
    public float tiempoDePartida = 30f;
    public Text temporalizadorText;
    private int tiempo;

    private void Start()
    {

        Setup();
      
    }

    void Setup()
    {

        turnoJugador1_text.SetActive(true);
        turnoJugador2_text.SetActive(false);
        ganador1_text.SetActive(false);
        ganador2_text.SetActive(false);
        empate_text.SetActive(false);


        grid = new GameObject[ancho, alto];

        for (int x = 0; x < ancho; x++)
        {

            for (int y = 0; y < alto; y++)
            {

                GameObject go = GameObject.Instantiate(puzzle) as GameObject;
                Vector3 position = new Vector3(x, y, 0);
                go.transform.position = position;
                grid[x, y] = go;
                go.GetComponent<Renderer>().material.SetColor("_Color", Color.white);

            }

        }

        

    }

    void Update()
    {

        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int x = (int)(mPosition.x + 0.5f);
        int y = (int)(mPosition.y + 0.5f);

        TiempoDeTurno();

        if (ganador == false && tiempo > 0)
        {

            if (Input.GetMouseButtonDown(0))
            {

                Jugadores(mPosition, x, y);
                CheckBoardHorizontal(x, y, turnoJugador);
                CheckBoardVertical(x, y, turnoJugador);
                CheckBoardDiagonal1(x, y, turnoJugador);
                CheckBoardDiagonal2(x, y, turnoJugador);

                if (turnoJugador == Color.red && jugador == true && ganador == false)
                {

                }
                else
                {

                    if (turnoJugador == Color.blue && jugador == false)
                    {

                    }

                }

            }

        }

        if (tiempo == 0 && ganador == false)
        {

            empate = true;

           
        }



        if (ganador == true || empate == true)

        {

            if (ganador == true && turnoJugador == Color.red)
            {

                ganador1_text.SetActive(true);
                tiempoDePartida = 0;




            }

            if (ganador == true && turnoJugador == Color.blue)
            {

                ganador2_text.SetActive(true);
                tiempoDePartida = 0;


            }

            if (empate == true)
            {

                empate_text.SetActive(true);
                tiempoDePartida = 0;
            }



        }

    }

    

    void Jugadores(Vector3 mPosition, int x, int y)
    {

        GameObject go = grid[x, y];
        go.gameObject.GetComponent<Renderer>().material.SetColor("_color", Color.white);
        turnoempty = go.gameObject.GetComponent<Renderer>().material.color;

        if (turnoempty == Color.white && jugador == false)
        {
            
            if (x >= 0 && y >= 0 && x < ancho && y < alto )
            {

                GameObject gored = grid[x, y];
                gored.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                turnoJugador = go.gameObject.GetComponent<Renderer>().material.color;
                CambioDeJugador();

            }
            else
                CambioDeJugador();

        }

        else
        if (turnoempty == Color.white && jugador == true )
        {
            if (x >= 0 && y >= 0 && x < ancho && y < alto)
            {

                GameObject goblue = grid[x, y];
                goblue.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                turnoJugador = go.gameObject.GetComponent<Renderer>().material.color;
                CambioDeJugador();

            }
            else
                CambioDeJugador();

        }

    }

    private bool CheckBoardHorizontal(int x, int y, Color turnoJugador)
    {

        int contador = 0;
        for (int i = x - 3; i <= x + 3; i++)
        {
            if (i >= 0 && i < ancho)
            {
                if (grid[i, y].GetComponent<Renderer>().material.color == turnoJugador)
                {

                    ++contador;

                }
                else
                    contador = 0;

                if (contador >= 4)
                {

                    return ganador = true;

                }

            }

        }

        return false;

    }

    private bool CheckBoardVertical(int x, int y, Color turnoJugador)
    {

        int contador = 0;

        for (int j = y - 3; j <= y + 3; j++)
        {
            if (j >= 0 && j < alto)
            {

                if (grid[x, j].GetComponent<Renderer>().material.color == turnoJugador)
                {

                    ++contador;

                }
                else
                    contador = 0;

                if (contador >= 4)
                {
                    
                    return ganador = true;

                }
            }
        }

        return false;
    }

    private bool CheckBoardDiagonal1(int x, int y, Color turnoJugador)
    {

        int yV = y - 3;
        int contador = 0;

        for (int i = x - 3; i <= x + 3; i++)
        {

            if (i >= 0 && i < ancho && yV >= 0 && yV < alto)
            {
                if (grid[i, yV].GetComponent<Renderer>().material.color == turnoJugador)
                {

                    ++contador;

                }

                else
                    contador = 0;

                if (contador >= 4)
                {

                    return ganador = true;

                }
            }

            yV++;

        }

        return  false;

    }

    private bool CheckBoardDiagonal2(int x, int y, Color turnoJugador)
    {

        int xV = x + 3;
        int contador = 0;

        for (int i = y - 3; i <= y + 3; i++)
        {

            if (i >= 0 && i < alto && xV >= 0 && xV < ancho)
            {

                if (grid[xV, i].GetComponent<Renderer>().material.color == turnoJugador)
                {

                    ++contador;

                }

                else
                    contador = 0;

                if (contador >= 4)
                {

                    return ganador = true;

                }
            }

            xV--;

        }

        return false;

    }

    void CambioDeJugador()
    {
       
        jugador = !jugador;
        TurnoDeTexto();

    }

    void TiempoDeTurno()
    {
 
        tiempoDePartida-= Time.deltaTime;
        tiempo = (int)tiempoDePartida;
        temporalizadorText.text = tiempo.ToString("f0");    

    }

    void TurnoDeTexto()
    {

        if (jugador == false)
        {

            turnoJugador1_text.SetActive(true);
            turnoJugador2_text.SetActive(false);

        }

        else
        {

            turnoJugador1_text.SetActive(false);
            turnoJugador2_text.SetActive(true);

        }

    }

}




