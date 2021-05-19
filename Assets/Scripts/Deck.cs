using UnityEngine;
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
    public Text prob1721;
    public Text probMayor21;
    public Text puntosJugador;
    public Text puntosDealer;


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
        CalculateProbabilities();

    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
        probabilidadDealerMayorPuntuacion();
        probabilidad17_21();
        probabilidadMayor21();





    }

    void probabilidadMayor21()
    {
        int puntosJugador = player.GetComponent<CardHand>().points;
        int numCartas = 0;

        bool asEsUno = false;

        //Si els punts són igual o superior a 11, els asos valen 1
        if (puntosJugador >= 11)
        {
            asEsUno = true;
        }

        for (int i = cardIndex; i < values.Length; i++)
        {
            int puntosCarta = values[i];
            if (asEsUno && values[i] == 11)
            {
                puntosCarta = 1;
            }
            if ((puntosJugador + puntosCarta) > 21)
            {
                numCartas++;
            }
        }

        //Es mira quantes cartes hi ha a la baralla sense contar la carta oculta ja que és del dealer
        float numCartasBaraja = values.Length - cardIndex;
        //Calcular la probabilitat de tindre 
        float a = numCartas / numCartasBaraja;

        probMayor21.text = "Probabilidad de tener más de 21 en la próxima carta: " + (System.Math.Round(a * 100, 2)).ToString() + " %";
    }

    void probabilidad17_21()
    {
        int puntosJugador = player.GetComponent<CardHand>().points;
        int numCartas = 0;

        bool asEsUno = false;

        //Si els punts són igual o superior a 11, els asos valen 1
        if (puntosJugador >= 11)
        {
            asEsUno = true;  
        }

        for (int i = cardIndex; i < values.Length; i++)
        {
            int puntosCarta = values[i];
            if (asEsUno && values[i] == 11)
            {
                puntosCarta = 1;
            }
            if ((puntosJugador + puntosCarta) >= 17 && (puntosJugador + puntosCarta) <= 21)
            {
                numCartas++;
            }
        }

        //Es mira quantes cartes hi ha a la baralla sense contar la carta oculta ja que és del dealer
        float numCartasBaraja = values.Length - cardIndex;
        //Calcular la probabilitat de tindre 
        float a = numCartas / numCartasBaraja;

        prob1721.text = "Probabilidad de tener entre un 17 y un 21 en la próxima carta: " + (System.Math.Round(a * 100, 2)).ToString() + " %";
    }

    void probabilidadDealerMayorPuntuacion()
    {
        int puntosJugador = player.GetComponent<CardHand>().points;
        int puntosCartaDescubiertaDealer = dealer.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().value;
        int numCartasMayor = 0;
        bool asEsUno = false;

        //Es calcula el valor de la carta per a que la puntuació dels 2 jugadors siga igual
        int valorMinCartaOculta = puntosJugador - puntosCartaDescubiertaDealer;

        //Si la carta descoberta del dealer és un as, els altres asos valdràn 1 (cas que el jugador tinga 12 i dealer tinga dos asos = 12 punts)
        if (puntosCartaDescubiertaDealer == 11)
        {
            asEsUno = true;

        }
        //Valor de la carta oculta
        int valorCarta = values[1];


        //Calcular el nº de cartes amb valor igual o superior a "valorCartaSuperior" 
        //Primer, es mira si el valor de la carta oculta és major que el valor que faria empatar als 2 jugadors
        if (asEsUno && valorCarta == 11)
        {
            valorCarta = 1;
        }

        if (valorCarta > valorMinCartaOculta)
        {
            numCartasMayor++;
        }

        //A continuació es mira el valor de les cartes restants de la baralla
        for (int i = cardIndex; i < values.Length; i++)
        {
            valorCarta = values[i];
            if (asEsUno && valorCarta == 11)
            {
                valorCarta = 1;
            }
            if (valorMinCartaOculta < valorCarta)
            {
                numCartasMayor++;
            }
        }

        //Es mira quantes cartes hi ha a la baralla contant la carta oculta
        float numCartasBaraja = values.Length - cardIndex + 1f;
        //Calcular la probabilitat de tindre 
        float a = numCartasMayor / numCartasBaraja;

        Debug.Log(valorMinCartaOculta);
        Debug.Log(numCartasBaraja);
        Debug.Log(numCartasMayor);

        probMessage.text = "Probabilidad de que el dealer tenga más puntuación: " + (System.Math.Round(a * 100, 2)).ToString() + " %";
    }


    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;
        puntosDealer.text = "Puntos del dealer: " + dealer.GetComponent<CardHand>().points;
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;

        try
        {
            CalculateProbabilities();
        }
        catch
        {

        }

       // int puntos = player.GetComponent<CardHand>().points;
        puntosJugador.text = "Puntos del jugador: " + (player.GetComponent<CardHand>().points).ToString();


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
        hitButton.interactable = false;
        GameObject card = dealer.GetComponent<CardHand>().cards[0];
        card.GetComponent<CardModel>().ToggleFace(true);

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */

        while (dealer.GetComponent<CardHand>().points < 17)
        {
            PushDealer();
        }

        puntuacionTurnoDealer();


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
        hitButton.interactable = false;
        stickButton.interactable = false;

        //En caso de ser una victoria al inicio, mostrar la carta oculta del dealer
        GameObject card = dealer.GetComponent<CardHand>().cards[0];
        card.GetComponent<CardModel>().ToggleFace(true);
    }

    //Metodo para comprovar si la puntuacion del dealer es superior a 16 y, en tal caso, comprobar el ganador de la partida
    // Devuelve true si hay ganador, y false si no lo hay;
    void puntuacionTurnoDealer()
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
            stickButton.interactable = false;
        }
    }

}
