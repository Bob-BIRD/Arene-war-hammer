using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class main : MonoBehaviour
{
    /*Notes de création:
     Crée par Clément PICOT (L1-Promo 2022-Groupe E) au cours du projet de combat d'IA en Arène en Février-Mars 2018 à Efrei-Paris.
     Ce script vise à interpréter une série d'action envoyé par un code en C à travers un fichier texte afin de l'afficher dans Unity 3D.
     Temps de création du scipt ainsi que de l'environnement Unity: 45H
     Contact: clement.picot@efrei.net

     Crée à l'aide de l'interface de programmation Visual Studio de Microsoft
    */

    /*Notes à l'attention du lecteur ignorant ce qu'est le C# ou Unity 3D:
        Ici le code est en C# (langage de haut niveau avec beaucoup d'abstraction). Les variables déclarées au départ seront utilisées dans tout le script (elles sont communes à l'ensemble de la classe main)

        Pour pallier au fait que le C# ne supporte pas les pointeurs (pour des questions d'optimisation automatique de la mémoire (voir "Garbage Collector" sur Internet pour plus d'informations), 
nous avons utilisé un fichier texte "tampon" entre le code en C et celui en C#

        Le code ici ne prend aucune mesure de sécurite. Le code principal est censé être parfait et celui-ci ne gère que l'affichage.
        Quelques particularités de syntaxe du C#
        Do While n'est pas supporté en C#*
        int[] <==> tableau 1D
        int[,] <==> tableau 2D
        machin = new int[x,y] <==> malloc un tableau 2D de dimensions x*y

        objet <==> grossièrement à modèle 3D + texture + position + rotation + physique + sa hiérarchie + tous les scripts dessus. A titre informatif,
        c'est ce pourquoi unity est aussi pratique (une fois qu'on a passé quelques dizaines d'heures dessus) d'utilisation*/

    /*Manière dont le texte est transmis entre le C et le C#:
     //Dans le fichier actions.txt A LA VIRGULE ET AU SAUT DE LIGNE PRÈS
<nombre_joueur>
<coordonnée_x_joueur_1>,<coordonnée_y_joueur_1>
<coordonnée_x_joueur_2>,<coordonnée_y_joueur_2>
…
<coordonnée_x_joueur_n>,<coordonnée_y_joueur_n>
<valeur[0,0]>,<valeur[1,0]>,...,<valeur[<taille_x_tableau>-1,0]>,
<valeur[0,1]>,<valeur[1,1]>,...,<valeur[<taille_x_tableau>-1,1]>,
…
<valeur[0,<taille_y_tableau>-1]>,<valeur[1,<taille_y_tableau>-1]>,...,<valeur[<taille_x_tableau>-1,<taille_y_tableau>-1]>,
<ID_joueur_actuel> 
<action_1>,<direction_action_1>
<nombre_de_cases_modifiées>
<coordonnée_x_1>,<coordonnée_y_1>,<nouvelle_valeur_1>
<coordonnée_x_2>,<coordonnée_y_2>,<nouvelle_valeur_2>
…
<coordonnée_x_n>,<coordonnée_y_n>,<nouvelle_valeur_n>
<direction_cassage_case>
<nombre_de_cases_modifiées>
<coordonnée_x_1>,<coordonnée_y_1>,<nouvelle_valeur_1>
<coordonnée_x_2>,<coordonnée_y_2>,<nouvelle_valeur_2>
…
<coordonnée_x_n>,<coordonnée_y_n>,<nouvelle_valeur_n>
-1
<ID_joueur_gagnant>,



Avec:
-nombre_joueur: le nombre de joueur qui vont participer à la partie
-ID joueur : nombre compris entre 1 et 25
-coordonnée_x_joueur_1 et coordonnée_y_joueur_1:les coordonnées de départ des <nombre_joueur> joueur dans l’ordre de leur ID (1,2,...,<nombre_joueur>)
-valeur[x,y]: le nombre de PV de la case aux coordonnées x,y au début du jeu (0 pour inexistant). ACTUELLEMENT LA TAILLE IMPOSÉE DU TABLEAU EST 35x35
Toute la partie orange est répétable autant de fois que nécessaire
-action_1:
    -0:Ne rien faire, peu importe <direction_action_1>
	-1:Grenader à 1 case
	-2:Grenader à 2 cases
	-3:Grenader à 3 cases
	-4:Grenader à 4 cases
	-5:Grenader à 5 cases
	-6:Avancer d’une case
-direction_action_1 : (équivaut au sens direct de trigo) (seulement 4 directions pour le tir de grenade)
    -1:Droit
    -2:Haut-Droit
    -3:Haut
    -4:Haut-Gauche
    -5:Gauche
    -6:Bas-Gauche
    -7:Bas
    -8:Bas-Droit
-coordonnée_x_i et coordonée_y_i:
	Les coordonnées de la case du tableau[x][y] dont la valeur va être modifié
-nouvelle_valeur_i:
	La nouvelle valeur de la case[x][y]
-direction_cassage_case:
    -0:Ne rien faire
    -1:Droit
    -2:Haut-Droit
    -3:Haut
    -4:Haut-Gauche
    -5:Gauche
    -6:Bas-Gauche
    -7:Bas
    -8:Bas-Droit
		- <-1> me sert pour savoir que le combat est terminé
		-<ID_joueur_gagnant>: le joueur qui a gagné le combat

Exemple:
3
0,0
30,25
34,34
1,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,1,2,3,4,5,6,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
1
6,8
0
3
0
1
6,7
0
3
0
-1
1,
*/


    //Les variables modifiables sont en majuscules

    int VITESSE_JEU = 5;

    public TextAsset FICHIER_ACTIONS; //Le fichier contenant les actions du jeu pré-calculées en C. Elles vont être interprétées visuellement dans ce script
    public GameObject TABLEAU_EN_JEU; //L'objet physique qui contient toutes les cases (autrement dit, le plateau de jeu)

    int TAILLE_UTILE_TABLEAU_X = 35; //Nombres de colonnes où évolueront les joueurs
    int TAILLE_UTILE_TABLEAU_Y = 35; //Nombres de lignes où évolueront les joueurs

    public AnimationClip ANIMATION_MARCHER; //Les animations des joueurs crées par Camille
    public AnimationClip ANIMATION_CASSER;
    public AnimationClip ANIMATION_BOMBE_1;
    public AnimationClip ANIMATION_BOMBE_2;
    public AnimationClip ANIMATION_BOMBE_3;
    public AnimationClip ANIMATION_BOMBE_4;
    public AnimationClip ANIMATION_BOMBE_5;

    public Texture TEXTURE_SOL_0_PV;
    public Texture TEXTURE_SOL_1_PV;
    public Texture TEXTURE_SOL_2_PV;
    public Texture TEXTURE_SOL_3_PV;
    public Texture TEXTURE_SOL_4_PV;
    public Texture TEXTURE_SOL_5_PV;
    public Texture TEXTURE_SOL_6_PV;
    public Texture TEXTURE_SOL_7_PV;
    public Texture TEXTURE_SOL_8_PV;

    /*Color[] COULEUR_FONCTION_PV = { new Color(0, 0, 0, 1), new Color(1 / 8f, 1 / 8f, 1 / 8f, 1), new Color(2 / 8f, 2 / 8f, 2 / 8f, 1), new Color(3 / 8f, 3 / 8f, 3 / 8f, 1), new Color(4 / 8f, 4 / 8f, 4 / 8f, 1),
        new Color(5 / 8f, 5 / 8f, 5 / 8f, 1), new Color(6 / 8f, 6 / 8f, 6 / 8f, 1), new Color(7 / 8f, 7 / 8f, 7 / 8f, 1), new Color(1, 1, 1, 1) }; //Contient les couleurs des cases en fonction de leurs points de vie*/

    //Les variables internes au script sont en minuscules.
    string texte_fichier_actions; //Contient la chaine de caractere contenue dans FICHIER_ACTIONS
    int position_actuelle_actions = 0; //Ou en est le script de la lecture de la chaine de caractere du fichier

    int nombre_joueur; //Equivaut au nombre de joueur, on y stocke: leurs coordonées et un booléen (<==> entier 1 ou 0) indiquant s'ils sont en vie

    Transform objet_joueur; //Contient la référence à l'objet qui servira de représentation à tous les joueurs

    Transform objet_joueur_actuel; //Contient la référence à l'objet du joueur actuel
    int[,] tableau; //Le plateau de jeu utile ainsi que les colonnes supplémentaires des joueurs

    int ID_joueur_actuel; //L'ID du joueur jouant actuellement (lecture_actions) ou actuellement modifié (Initialiser_tableau)
    int action_1; //La première action du joueur
    int direction_action_1; //La direction de la première action du joueur
    int direction_cassage_case; //La seconde action du joueur

    int action_actuelle = 1; //Détermine si le joueur en est à sa première ou seconde action

    // Start() est appelée une fois au lancement du jeu
    void Start()
    {
        objet_joueur = transform.Find("modele_joueur"); //On obtient la référence
        objet_joueur_actuel = objet_joueur; //Evite de faire buguer un if dans update. Va être rendu plus propore si notre arène est élue.
        objet_joueur.GetComponent<Animation>().AddClip(ANIMATION_MARCHER, "Marcher"); //On rajoute les animations nécessaires au component "animation" du modele de joueur
        objet_joueur.GetComponent<Animation>().AddClip(ANIMATION_CASSER, "Casser");
        objet_joueur.GetComponent<Animation>().AddClip(ANIMATION_BOMBE_1, "Bombe_1");
        objet_joueur.GetComponent<Animation>().AddClip(ANIMATION_BOMBE_2, "Bombe_2");
        objet_joueur.GetComponent<Animation>().AddClip(ANIMATION_BOMBE_3, "Bombe_3");
        objet_joueur.GetComponent<Animation>().AddClip(ANIMATION_BOMBE_4, "Bombe_4");
        objet_joueur.GetComponent<Animation>().AddClip(ANIMATION_BOMBE_5, "Bombe_5");
        
        Time.timeScale = VITESSE_JEU; //Permet d'accélérer le temps afin d'éviter que ce soit trop long

        texte_fichier_actions = FICHIER_ACTIONS.text;

        Initialiser_tableau(); //Initialise le tableau lui-même et les joueurs

        Lecture_actions_1(); //Fait une première lecture d'actions pour ne pas faire buguer les if dans update
        Lecture_actions_2();
    }

    // Update est appelée à chaque frame (le jeu se déroule en temps réel et pas à la vitesse maximale d'éxécution de la machine, Update est donc nécessaire pour "ralentir" l'exécution)
    void Update()
    {
        //print(position_actuelle_actions);
        //print(texte_fichier_actions[position_actuelle_actions]);
        //Vérifie que le jeu n'est pas terminé ou en train de se terminer [ID_joueur_avancant != -1 (va se terminer) ou -2 (terminé)] et que l'animation du joueur actuel est terminée
        if (ID_joueur_actuel != -1 && ID_joueur_actuel != -2 && !objet_joueur_actuel.GetComponent<Animation>().isPlaying) 
        {
            //Alterne entre la première action et la seconde: nécessaire pour laisser le temps aux animations de se terminer
            if (action_actuelle == 1)
            {
                Lecture_actions_1();
                action_actuelle = 2;
            }
            else if (action_actuelle == 2)
            {
                Modifier_cases();
                action_actuelle = 3;
            }
            else if (action_actuelle == 3)
            {
                Lecture_actions_2();
                action_actuelle = 4;
            }
            else if (action_actuelle == 4)
            {
                Modifier_cases();
                action_actuelle = 1;
            }
        }
        //Vérifie que le jeu va se terminer mais n'est pas terminé
        else if (ID_joueur_actuel == -1 && ID_joueur_actuel != -2)
        {
            Declarer_gagnant(Obtenir_int_de_sous_chaine_de_texte(','));
        }
    }

    void Initialiser_tableau()
    {
        int j;
        int i;

        nombre_joueur = Obtenir_int_de_sous_chaine_de_texte('\n'); //Le nombre de joueurs supplémentaires est indiqué dans le fichier
        tableau = new int[TAILLE_UTILE_TABLEAU_X + nombre_joueur, TAILLE_UTILE_TABLEAU_Y];
        for (j = 0; j < nombre_joueur; j++)//y
        {
            tableau[TAILLE_UTILE_TABLEAU_X + j, 0] = j + 1; // L'ID du joueur compris entre 1 et 25
            tableau[TAILLE_UTILE_TABLEAU_X + j, 1] = Obtenir_int_de_sous_chaine_de_texte(','); // Coordonée x du joueur
            tableau[TAILLE_UTILE_TABLEAU_X + j, 2] = Obtenir_int_de_sous_chaine_de_texte('\n'); // Coordonée y du joueur
            tableau[TAILLE_UTILE_TABLEAU_X + j, 3] = 1; //Précise que le joueur est en vie
            objet_joueur_actuel = Instantiate(objet_joueur, new Vector3(tableau[TAILLE_UTILE_TABLEAU_X + j, 1], 0.7f, tableau[TAILLE_UTILE_TABLEAU_X + j, 2]), new Quaternion(0, 0, 0, 0), transform); //Crée l'objet représentant le joueur aux coordonées du joueur
            objet_joueur_actuel.name = "Joueur " + (j + 1).ToString(); //Rennome le joueur dans la hiérarchie afin de pouvoir y accéder plus tard
            //objet_joueur_actuel.Find("hammer").GetComponent<Material>().SetColor(-55442, new Color(1, 0, 0)); //En develloppement
        }
        for (j = 0; j < TAILLE_UTILE_TABLEAU_Y; j++)//y
        {
            for (i = 0; i < TAILLE_UTILE_TABLEAU_X; i++) //x
            {
                tableau[i, j] = Obtenir_int_de_sous_chaine_de_texte(',');
                Actualiser_cube(i, j, tableau[i, j]);
            }
            position_actuelle_actions++; //saute une ligne
        }
    }

    void Lecture_actions_1()
    {
        ID_joueur_actuel = Obtenir_int_de_sous_chaine_de_texte('\n');
        if (ID_joueur_actuel != -1)
        {
            objet_joueur_actuel = transform.Find("Joueur " + ID_joueur_actuel);

            action_1 = Obtenir_int_de_sous_chaine_de_texte(',');
            direction_action_1 = Obtenir_int_de_sous_chaine_de_texte('\n');
            if (action_1 <= 5)
            {
                Lancer_grenade(action_1, direction_action_1);
            }
            else
            {
                Avancer(ID_joueur_actuel, direction_action_1);
            }
        }
    }

    void Lecture_actions_2()
    {
        if (ID_joueur_actuel != -1)
        {
            direction_cassage_case = Obtenir_int_de_sous_chaine_de_texte('\n');
            Casser_case(ID_joueur_actuel, direction_cassage_case);
        }
    }

    void Modifier_cases()
    {
        if (ID_joueur_actuel != -1)
        {
            int i;
            int coordonnee_modifiee_x;
            int coordonnee_modifiee_y;
            int valeur_modifiee;
            int nombre_de_case_modifie;
            print("Modifier_cases");
            nombre_de_case_modifie = Obtenir_int_de_sous_chaine_de_texte('\n');
            for (i = 0; i < nombre_de_case_modifie; i++)
            {
                coordonnee_modifiee_y = Obtenir_int_de_sous_chaine_de_texte(',');
                coordonnee_modifiee_x = Obtenir_int_de_sous_chaine_de_texte(',');
                valeur_modifiee = Obtenir_int_de_sous_chaine_de_texte('\n');

                tableau[coordonnee_modifiee_x, coordonnee_modifiee_y] = valeur_modifiee;
                if (coordonnee_modifiee_x < TAILLE_UTILE_TABLEAU_X && coordonnee_modifiee_y < TAILLE_UTILE_TABLEAU_Y) //Les PV d'une case du tableau ont étés modifié
                {
                    Actualiser_cube(coordonnee_modifiee_y, coordonnee_modifiee_x, valeur_modifiee);
                }

                if (coordonnee_modifiee_x >= 35 && coordonnee_modifiee_y == 3 && valeur_modifiee == 0)// Un joueur a triché et va mourrir
                {
                    Tuer_tricheur(coordonnee_modifiee_x - 34);
                }
            }
        }
    }

    void Avancer(int ID_joueur_avancant, int direction)
    {
        if (direction == 1)
        {
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 2]++;
            objet_joueur_actuel.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction == 2)
        {
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 2]++;
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 1]--;
            objet_joueur_actuel.eulerAngles = new Vector3(0, 315, 0);
        }
        else if (direction == 3)
        {
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 1]--;
            objet_joueur_actuel.eulerAngles = new Vector3(0, 270, 0);
        }
        else if (direction == 4)
        {
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 2]--;
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 1]--;
            objet_joueur_actuel.eulerAngles = new Vector3(0, 225, 0);
        }
        else if (direction == 5)
        {
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 2]--;
            objet_joueur_actuel.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction == 6)
        {
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 2]--;
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 1]++;
            objet_joueur_actuel.eulerAngles = new Vector3(0, 135, 0);
        }
        else if (direction == 7)
        {
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 1]++;
            objet_joueur_actuel.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (direction == 8)
        {
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 2]++;
            tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant - 1, 1]++;
            objet_joueur_actuel.eulerAngles = new Vector3(0, 45, 0);
        }

        objet_joueur_actuel.GetComponent<Animation>().Play("Marcher");
        objet_joueur_actuel.position = new Vector3(tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant -1, 1], 0.7f, tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_avancant -1, 2]);
    }
    
    /*
     Cette fonction renvoie la sous-chaine de caractere comprise dans chaine_principale entre le caractere à la position_actuelle_actions et le caractere_final:
     Exemple: blabla-machin,truc-bidouille avec les parametres 7 et ',' renvoie machin
     Elle sert uniquement à extraire les informations de la chaine de caractère du texte
     */
    int Obtenir_int_de_sous_chaine_de_texte(char caractere_final) //+position_actuelle modifiée 
    {
        string chaine_actuelle = "";
        char caractere_actuel = '$'; //$ ne sera jamais utilisé
        while (caractere_actuel != caractere_final)
        {
            caractere_actuel = texte_fichier_actions[position_actuelle_actions];
            position_actuelle_actions++;
            if (caractere_actuel != caractere_final)
            {
                chaine_actuelle += caractere_actuel;
            }
        }
        print("indice : " + position_actuelle_actions);
        print(chaine_actuelle);
        return int.Parse(chaine_actuelle); //Transforme une chaîne de caractère en entier
    }

    //Met à jour le visuel du cube en fonction de ses PV
    void Actualiser_cube(int coord_x, int coord_y, int PV)
    {
        GameObject cube = TABLEAU_EN_JEU.transform.Find("Ligne (" + coord_y + ")").Find("Cube (" + coord_x + ")").gameObject; //Trouve le cube correspondant aux coordonées

        if (PV == 1) cube.GetComponent<Renderer>().material.SetTexture("_MainTex", TEXTURE_SOL_1_PV);
        if (PV == 2) cube.GetComponent<Renderer>().material.SetTexture("_MainTex", TEXTURE_SOL_2_PV);
        if (PV == 3) cube.GetComponent<Renderer>().material.SetTexture("_MainTex", TEXTURE_SOL_3_PV);
        if (PV == 4) cube.GetComponent<Renderer>().material.SetTexture("_MainTex", TEXTURE_SOL_4_PV);
        if (PV == 5) cube.GetComponent<Renderer>().material.SetTexture("_MainTex", TEXTURE_SOL_5_PV);
        if (PV == 6) cube.GetComponent<Renderer>().material.SetTexture("_MainTex", TEXTURE_SOL_6_PV);
        if (PV == 7) cube.GetComponent<Renderer>().material.SetTexture("_MainTex", TEXTURE_SOL_7_PV);
        if (PV == 8) cube.GetComponent<Renderer>().material.SetTexture("_MainTex", TEXTURE_SOL_8_PV);

        if (PV <= 0)
        {
            cube.GetComponent<Renderer>().material.SetTexture("_MainTex", TEXTURE_SOL_0_PV);
            cube.GetComponent<Rigidbody>().isKinematic = false; //Kinematic empêche le déplacement du cube, pratique pour éviter un mouvement du à une collision
            cube.GetComponent<Rigidbody>().useGravity = true; //La gravité est appliquée, le cube tombe
        }
        //cube.GetComponent<Renderer>().material.color = COULEUR_FONCTION_PV[PV]; //Change la couleur du cube en fonction de ses PVs
    }

    void Declarer_gagnant(int ID_gagnant)
    {
        print("Wow c'est la pire manière de déclarer le gagnant... Bref. C'est le joueur " + ID_gagnant + " qui a gagné.");
        //Plus sérieusement, si nous sommes élus, des feux d'artifices vont se lancer un peu partout et le nom des gagnants sera affiché en grand.
        ID_joueur_actuel = -2;
    }
    
    void Casser_case(int ID_joueur_casseur, int direction_casse)
    {
        if (direction_casse == 1)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction_casse == 2)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 315, 0);
        }
        else if (direction_casse == 3)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 270, 0);
        }
        else if (direction_casse == 4)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 225, 0);
        }
        else if (direction_casse == 5)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction_casse == 6)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 135, 0);
        }
        else if (direction_casse == 7)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (direction_casse == 8)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 45, 0);
        }
        objet_joueur_actuel.GetComponent<Animation>().Play("Casser");
    }

    //Si un joueur a été détécté comme étant un tricheur, il meurt dans un éclair (valeur renvoyé par l'IA invalide)
    void Tuer_tricheur(int ID_joueur_tue)
    {
        objet_joueur_actuel.transform.position = new Vector3(-100, -100, -100);
        //Un eclair apparait sur le joueur et il disparait dans celui-ci
    }

    //Si le joueur est tombé, il est tué
    public void Tuer_joueur(int ID_joueur_mort) //Tue le joueur qui est tombé dans la lave
    {
        tableau[TAILLE_UTILE_TABLEAU_X + ID_joueur_mort - 1, 3] = 0;
        print("LE JOUEUR " + ID_joueur_mort + " EST MORT C'EST EPIQUE!!! Sauf que je ne suis qu'une petite chaine de caractère et que tout le monde n'en a rien à faire de moi...");
        //Plus sérieusement, si nous sommes élus, en gros va apparaitre le nom du personnage tué et un petit commentaire rigolo aléatoire.
    }

    void Lancer_grenade(int portee, int direction)
    {
        if (direction == 1)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction == 2)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 270, 0);
        }
        else if (direction == 3)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction == 4)
        {
            objet_joueur_actuel.eulerAngles = new Vector3(0, 90, 0);
        }
        if (portee != 0 && direction != 0)
        {
            objet_joueur_actuel.GetComponent<Animation>().Play("Bombe_" + portee.ToString());
        }
    }
}