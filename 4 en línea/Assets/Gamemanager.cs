using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int ancho = 10; // Rango de columnas
    private int alto = 10; // Rango de filas 
    public GameObject puzzle; // prefab que se va a duplicar
    private GameObject[,] grid; // matriz
    bool jugador = false; //false = red, true = blue.
    Color turnoempty; // Estado de las esferas en blanco
    Color turnoJugador; // estado  que albergara el color del jugador 
    bool ganador; // boleano que determina el ganador del juego
    bool empate; // boleano que determina la regla de tiempo del juego y declara un empate (ambos pierden)

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
 
    // funcion que crea esferas de color blanco desde el inicio de la escena. 
    // ademas de desactivar los texto que no necesitan ser iniciados al principio de la escena.
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


    // pintar las esferas dependiendo el turno del jugador en el lugar donde el mouse da click en la grid, siempre y cuando la esfera este en blanco.
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
                turnoJugador = gored.gameObject.GetComponent<Renderer>().material.color;
                CambioDeJugador();

            }

        }

        else
        if (turnoempty == Color.white && jugador == true )
        {
            if (x >= 0 && y >= 0 && x < ancho && y < alto)
            {

                GameObject goblue = grid[x, y];
                goblue.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                turnoJugador = goblue.gameObject.GetComponent<Renderer>().material.color;
                CambioDeJugador();

            }

        }

    }

    //Checkea horizontalmente si hay esferas del mismo color en cada turno, si es asi va aumentado el contador,en caso de que no encuentre colores
    //consecutivos el contador reiniciara a 0 y retornara un bool false para que continue el siguiente turno, cuando el contador llegue a 4 retornara un bool verdarero indicando
    //que hay un ganador.
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

    //Checkea verticalmente si hay esferas del mismo color en cada turno, si es asi va aumentado el contador,en caso de que no encuentre colores
    //consecutivos el contador reiniciara a 0 y retornara un bool false para que continue el siguiente turno, cuando el contador llegue a 4 retornara un bool verdarero indicando
    //que hay un ganador.
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

    //Checkea diagonalmente hacia la derecha. si hay esferas del mismo color en cada turno, si es asi va aumentado el contador,en caso de que no encuentre colores
    //consecutivos el contador reiniciara a 0 y retornara un bool false para que continue el siguiente turno, cuando el contador llegue a 4 retornara un bool verdarero indicando
    //que hay un ganador.
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

    //Checkea diagonalmente hacia la izquierda. si hay esferas del mismo color en cada turno, si es asi va aumentado el contador,en caso de que no encuentre colores
    //consecutivos el contador reiniciara a 0 y retornara un bool false para que continue el siguiente turno, cuando el contador llegue a 4 retornara un bool verdarero indicando
    //que hay un ganador.
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

    // realiza el cambio de del varible jugador pasando de falso a verdareo y viceversa para que pueda generar los intercambios de turnos a cada jugador
    void CambioDeJugador()
    {
       
        jugador = !jugador;
        TurnoDeTexto();

    }

    // tiempo de ejecucion en la partida global, para que el temporizador reste x cantidad de tiempo de duracion de la partida
    void TiempoDeTurno()
    {
 
        tiempoDePartida-= Time.deltaTime;
        tiempo = (int)tiempoDePartida;
        temporalizadorText.text = tiempo.ToString("f0");    

    }

    // cambia los texto dentro de la escena según el turno del jugador 
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




