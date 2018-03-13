using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detection_chute : MonoBehaviour {

    public main reference_main; //La référence au script main

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player") //Le tag est une caractéristique d'un GameObject que l'on peut modifier dans l'éditeur
        {
            int ID_joueur = int.Parse(collision.name[7].ToString());
            reference_main.Tuer_joueur(ID_joueur);
        }
        collision.gameObject.SetActive(false); //Desactive l'objet, que ce soit une case ou le joueur (c'est à dire qu'il disparait de manière visuelle et physique)
    }
}