using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Banca : MonoBehaviour
{

    public int deposito = 1000;
    public int apuesta = 0;

    public Text cifraDeposito;
    public Text cifraApuesta;
    public Button mas10;
    public Button menos10;
    public bool apostar = true;




    // Start is called before the first frame update
    void Start()
    {
        cifraApuesta.text = apuesta.ToString();
        cifraDeposito.text = deposito.ToString();
        comprobarApuesta();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void comprobarApuesta()
    {
        if(apuesta == 0)
        {
            menos10.interactable = false;
        }
        else
        {
            menos10.interactable = true;
        }

        if(deposito > 0)
        {
            mas10.interactable = true;
        }
        else
        {
            mas10.interactable = false;
        }

    }


    public void sumarApuesta()
    {
        apuesta += 10;
        deposito -= 10;
        comprobarApuesta();
        cifraApuesta.text = apuesta.ToString();
        cifraDeposito.text = deposito.ToString();
    }

    public void restarApuesta()
    {
        apuesta -= 10;
        deposito += 10;
        comprobarApuesta();
        cifraApuesta.text = apuesta.ToString();
        cifraDeposito.text = deposito.ToString();
    }

    public void apuestaGanada()
    {
        deposito += apuesta * 2;
        apuesta = 0;
        cifraApuesta.text = apuesta.ToString();
        cifraDeposito.text = deposito.ToString();
        comprobarApuesta();
    }

    public void apuestaPerdida()
    {
        apuesta = 0;
        cifraApuesta.text = apuesta.ToString();
        comprobarApuesta();
    }

    public void apuestaEmpatada()
    {
        deposito += apuesta;
        apuesta = 0;
        cifraApuesta.text = apuesta.ToString();
        cifraDeposito.text = deposito.ToString();
        comprobarApuesta();
    }
}
