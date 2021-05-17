﻿using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
        int valor = 2;

        for(int i = 0; i < 52; i++)
        {
            if(i%13 == 0)
            {
                valor = 11;
                values[i] = valor;
                valor = 2;
            }
            else if (i % 13 >= 10 && i % 13 <= 12)
            {
                valor = 10;
                values[i] = valor;
            }
            else
            {
                values[i] = valor;
                valor++;
            }
        }
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */

        int n = values.Length;
        while (n > 1)
        {
            n--;
            int r = Random.Range(0, n + 1);

            var value = values[r];
            var face = faces[r];

            values[r] = values[n];
            faces[r] = faces[n];

            values[n] = value;
            faces[n] = face;
        }
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
        }
        /*TODO:
         * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
         */
        comprovarPuntuacionJugador();

    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        comprovarPuntuacionJugador();

    }

    public void Stand()
    {

        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        hitButton.enabled = false;
        GameObject card = dealer.GetComponent<CardHand>().cards[0];
        card.GetComponent<CardModel>().ToggleFace(true);

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */

        if (!puntuacionTurnoDealer())
        {
            PushDealer();
            puntuacionTurnoDealer();
        }


        Debug.Log("Jugador: " + player.GetComponent<CardHand>().points);
        Debug.Log("Dealer: " + dealer.GetComponent<CardHand>().points);

    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
        hitButton.enabled = true;
        stickButton.enabled = true;
    }

    //Metodo para comprovar las puntuaciones al repartir las cartas (caso de que alguno de los 2 tena 21 al principio)
    void comprovarPuntuacionJugador()
    {
        Debug.Log("Jugador: " + player.GetComponent<CardHand>().points);
        Debug.Log("Dealer: " + dealer.GetComponent<CardHand>().points);
        if (player.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points == dealer.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Empate";
            finalMessage.color = Color.grey;
            final();
        }
        else if (player.GetComponent<CardHand>().points == 21 || dealer.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Has Ganado";
            finalMessage.color = Color.green;
            final();
        }
        else if (dealer.GetComponent<CardHand>().points == 21 || player.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Has perdido";
            finalMessage.color = Color.red;
            final();
        }
    }


    void final()
    {
        //Deshabilitar botones de Hit y stand
        hitButton.enabled = false;
        stickButton.enabled = false;

        //En caso de ser una victoria al inicio, mostrar la ca carta oculta del dealer
        GameObject card = dealer.GetComponent<CardHand>().cards[0];
        card.GetComponent<CardModel>().ToggleFace(true);
    }

    //Metodo para comprovar si la puntuacion del dealer es superior a 16 y, en tal caso, comprobar el ganador de la partida
    // Devuelve true si hay ganador, y false si no lo hay;
    bool puntuacionTurnoDealer()
    {
        if (dealer.GetComponent<CardHand>().points >= 17)
        {
            if (player.GetComponent<CardHand>().points == dealer.GetComponent<CardHand>().points)
            {
                finalMessage.text = "Empate";
                finalMessage.color = Color.grey;
            }
            else if (player.GetComponent<CardHand>().points > dealer.GetComponent<CardHand>().points || dealer.GetComponent<CardHand>().points > 21)
            {
                finalMessage.text = "Has Ganado";
                finalMessage.color = Color.green;
            }
            else
            {
                finalMessage.text = "Has perdido";
                finalMessage.color = Color.red;
            }
            stickButton.enabled = false;

            return true;
        }
        else
        {
            return false;
        }
    }

}
