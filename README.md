# Arene-war-hammer
projet 3

Corentin MARCOU
Camille MATHIEU
Clément PICOT
Anouar SMAOUI
Lucas BRIERE
Matéo GOMEZ CASTELLON


Règles du jeux :

principe du jeux :

         War hammer est un jeux de survie. Le classement est donc simple : plus la survie est longue, plus ton 
         classement sera élevé. Le gagnant est donc le dernier survivant.

         Initialement les joueurs se trouvent sur une plateforme composées de cases carrée ayant toutes le même 
         nombre de points de vie. La plateforme vole au dessus du vide. Au cours de la partie les joueur 
         effectueront des action qui enlèveront des points de vie aux cases.

         Lorsque les PV d’une case tombent à 0, cette case est détruite créant un trou dans la plateforme.
         Peut importe le moment de la partie, il est impossible de régénérer les cases. 

         Les différents joueurs vont jouer chacuns leurs tours. L'ordre des joueurs est choisi aléatoirement au 
         début de la partie puis reste le même jusqu'à la fin.
         A chaque fois qu'un joueur aura terminé ses actions durant un tour, tous les joueurs situés sur un case 
         ayant disparu meurent.

mort d’un joueur :

         Un joueur meurt si la case sur laquelle il est situé est détruite.
         Les deux seules façons de mourir dans le jeux sont d’effectuer une actions invalide ou de se trouver 
         sur une case détruite.

tour d’un joueur :

         Lors de son tour, le joueur doit choisir obligatoirement deux action.

première action :

         Pour la première, il a le choix entre se déplacer vers l’une des 8 case qui lui sont adjacentes 
         ou lancer une grenade vers l’un des 4 points cardinaux.
         
         
         Il est interdit de se déplacer vers une case occupée par un autre joueur ou vers une case hors 
         de la carte sous peine de mort (dans le jeux). Si un joueur se déplace vers une case détruite, 
         il meurt.


         Un lancé de grenade s’effectue de la manière suivante : la grenade avance dans la direction 
         choisie et s'arrête à 5 case du lanceur s'il ne rencontre pas d'autre joueur sur son trajet. 
         Sinon la grenade s’arrête juste avant le premier joueur qu’elle rencontre (il est donc 
         impossible de lancer une grenade sur une case occupée par un autre joueur. Cependant, il est 
         possible que la grenade explose sur le lanceur). 
         
         Si la grenade est lancée hors de la map elle s’arrête à la bordure. Une fois arrêtée la 
         grenade explose même si elle est située au dessus d’une case détruite. Cette explosion va
         produire la destuction de la case sur laquelle elle se situe et réduit de moitié le nombre 
         de points de vie initial les 4 cases adjacentes.
         Si le lanceur détruit la case sur laquelle il se situe, il meurt. S'il a aussi détruit la 
         case sous un autre joueur, ce joueur mourra mais après le lanceur.



          A la fin de la première action, la case sur laquelle était situé le joueur au début de son 
          tour perd 1 point de vie. Si le joueur a lancé une grenade, la  case sur laquelle il se situe 
          perd 1 PV même s'il n'a pas bougé. Dans le cas où les PV de la casesur laquelle il  se trouve 
          tombent à 0, le joueur meurt. S'il a cassé la case situé sous un autre joueur grâce à sa grenade 
          celui-ci mourra ensuite.

deuxième action :

         Si le joueur n’est pas mort dès sa première action, il pourra effectuer sa deuxième action.
         Les joueurs sont tous équipés d’un gros marteau qui les démangent, ils sont donc obligés de 
         donner un et un seul coup à chaque tour. Ils peuvent atteindre les 8 cases autours d’eux. Un coup 
         de marteau casse entièrement une case.
            -Si un joueur donne un coup sur une case située hors de la map il meurt.
            -Si un joueur donne un coup sur une case déjà détruite précedement, il se fait entraîner par le 
            poids de son marteau et tombe dans le trou (il meurt donc).

fin du tour :

         Le tour du joueur est enfin fini, c’est seulement maintenant que les joueurs situés sur des 
         cases détruites par le joueur venant de jouer perdent la vie. On ne peut donc pas gagner d’une action 
         suicide. Si plusieurs joueurs meurent lors du même tour, l’ordre des mort est l’ordre dans 
         lequel les joueurs ont étés inscrits dans le programme.


ce que font les ia :

         Les ia sont des fonctions dont le prototype est le suivant :
         int my_ai_play(int **map, int height, int width)

Pour nous :
         int mon_ia(int map[TAILLE][TAILLE+NBJOUEURS], int hauteur, int largeur)
         avec - hauteur = TAILLE
              - largeur = TAILLE + NBJOUEURS

Le tableau donné en paramètre contient la map sur les premières colonnes (elle est carrée de côté hauteur). Chaque case de la map est représentée par un entier qui correspond à la durée de vie restante à cette case, si elle est représentée par un 0, c’est que la case est détruite.

Les colonnes restantes donnent les informations sur les joueurs. Les joueurs sont rangés dans l’ordre, c’est à dire que la première colonne suivant le plateau de jeu représente le joueur jouant immédiatement puis dans la colonne suivante celui jouant juste après et ainsi de suite. Dans chacunes de ces colonnes, la première ligne correspond aux états des joueurs (0 pour mort et 1 pour vivant), la seconde aux indices des lignes du plateau sur lesquelles sont situés les joueur et la troisième aux colonnes (la première ligne est la ligne 0 et la première colonne la colonne 0).

exemple (avec 2 joueurs) :
map =

         0    0    4    4    0    0        1   1

         0    4    4    4    4    0        2   3

         4    4    4    4    4    4        1   4

         4    4    4    4    4    4        0   0

         0    4    4    4    4    0        0   0

         0    0    4    4    0    0        0   0

hauteur = 6

largeur = 8


Les ia retournent un entier : 8 * choix1 + choix2

Avec pour choix1:

         -0, lancer une grenade à droite;

         -1, lancer une grenade en haut;

         -2, lancer une grenade à gauche;

         -3, lancer une grenade en bas;

         -4, aller à droite;

         -5, aller en haut à droite;

         -6, aller en haut;

         -7, aller en haut à gauche;

         -8, aller à gauche;

         -9, aller en bas à gauche;

         -10, aller en bas;

         -11, aller en bas à droite.


et pour choix2 :

         -0, casser la case à droite;

         -1, casser la case en haut à droite;

         -2, casser la case en haut;

         -3, casser la case en haut à gauche;

         -4, casser la case à gauche;

         -5, casser la case en bas à gauche;

         -6, casser la case en bas;

         -7, casser la case en bas à droite.


L'entier retourné est donc compris entre 0 et 95.
