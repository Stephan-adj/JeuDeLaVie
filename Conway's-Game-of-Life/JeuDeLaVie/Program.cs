//Made by ADJARIAN Stéphan
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EsilvGui;


namespace JeuDeLaVie
{
    class Program
    {
        //Pas vraiment utile pour le jeu, mais très utile pour tester nos fonctions !
        
        static void AfficherMatrice(int[,] game)
        {
            for (int i = 0; i < game.GetLength(0); i++)
            {
                for (int k = 0; k < game.GetLength(1); k++)
                {
                    Console.Write(game[i, k] + " ");
                }
                Console.WriteLine("\t\t");
            }
            Console.WriteLine();
        }
        static void AfficheNbrVoisinsDuneCellule(int[,] game)
        {
            Console.WriteLine("Saisir l puis c =>");
            int l = Convert.ToInt32(Console.ReadLine()) - 1;
            int c = Convert.ToInt32(Console.ReadLine()) - 1;
            Console.WriteLine("la cellule [" + (l + 1) + ", " + (c + 1) + "] possède " + CompteurVoisinsVivantsRang1(game, l, c, 1) + " voisin(s) vivant(s).");
            Console.ReadKey();
        }
        static void AfficheLaMatriceNbrDeVoisinsRand1(int[,] game, int population)
        {           
            int[,] MatriceNbrVoisinsVivants = RemplissageMatriceVoisinsVivantsRang1(game, population);
            AfficherMatrice(MatriceNbrVoisinsVivants);
        }
        static void AfficheLaMatriceNbrDeVoisinsRang2(int[,] game, int population)
        {
            int[,] MatriceNbrVoisinsVivants = RemplissageMatriceVoisinsVivantsRang2(game, population);
            AfficherMatrice(MatriceNbrVoisinsVivants);
        }

        //FONCTIONNELLES 
        //Fonction servant à la GENERATION 
        static int[,] GenerationGame(Random rand, int answer)
            //Cette fonction génère la matrice du jeu. Elle demande à l'utilisateur le nb de ligne, de colonne, le taux de remplissage de la grille,
            //puis génère aléatoirement une grille respectant ces critères.
        {
            //Création de la matrice
            int[,] game = null;

            //BONUS !
            //UNE GRILLE QUI GENERE DES PLANEURS !
            string choix = "";
            Console.SetCursorPosition(0, 8);
            Console.WriteLine("Souhaitez vous la grille mystère ? (oui/non) => ");
            choix = Convert.ToString(Console.ReadLine());
            if (choix == "oui" || choix == "o" || choix == "Oui" || choix == "O")
            {   //Grille mystère
                game = CanonAPlaneur();
            }

            //GRILLE BASIQUE
            else
            {   
                //Saisi des données par l'utilisateur
                Console.WriteLine("Saississez le nombre de ligne => ");
                int nbrLignes = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Saississez le nombre de colonne => ");
                int nbrColonnes = Convert.ToInt32(Console.ReadLine());

                //Génération de la matrice
                game = new int[nbrLignes, nbrColonnes];
                //on initialisation les cases à 0 = mort
                for (int i = 0; i < game.GetLength(0); i++)
                {
                    for (int k = 0; k < game.GetLength(1); k++)
                    {
                        game[i, k] = 0;
                    }
                }

                //Demande le taux de remplissage
                Console.WriteLine("Saississez le taux de remplissage de la population (compris entre 0.1 et 0.9) => ");
                double tauxRemplissage = Convert.ToDouble(Console.ReadLine());
                double population = tauxRemplissage * nbrLignes * nbrColonnes;

                //Si mode deux population
                int numeroAssocieAPopulation = 1;
                int nbrPopulation = 1;
                if (answer == 2 || answer == 3)
                {
                    //Utile pour passer 2 fois dans la boucle juste après
                    nbrPopulation = 2;
                    //Comme il y a 2 populations le taux de remplissage est global : 
                    //Il vaut pour moitié pour les cellules de la première population et pour moitié les cellules de la seconde population
                    population = population / 2;
                }

                //on répartit soit 1 population soit 2 populations
                for (int numeroPopulation = 0; numeroPopulation < nbrPopulation; numeroPopulation++)
                {
                    //on répartie aléatoirement les cellules vivantes dans la matrices en fonction du taux
                    for (int i = 0; i < population; i++)
                    {
                        int ligneMatrice;
                        int colonneMatrice;
                        //random
                        do
                        {
                            ligneMatrice = rand.Next(0, game.GetLength(0));
                            colonneMatrice = rand.Next(0, game.GetLength(1));
                        }
                        //pour ne pas repasser 2 fois au même endroit
                        while (game[ligneMatrice, colonneMatrice] != 0);
                        //On fait vivre la cellule tirée aléatoirement
                        game[ligneMatrice, colonneMatrice] = numeroAssocieAPopulation;
                    }
                    numeroAssocieAPopulation = numeroAssocieAPopulation + 3;
                }
            }
            //Netoyage du menu
            Console.Clear();
            return game;
        }
        static int[,] CanonAPlaneur()
        //Nous avons utilisé cette fonction pour tester si le programme fonctionne
        {
            int[,] game = new int[24, 40];
            //on initialisation les cases à 0 = mort
            for (int i = 0; i < game.GetLength(0); i++)
            {
                for (int k = 0; k < game.GetLength(1); k++)
                {
                    game[i, k] = 0;
                }
            }
            //Puis on place les cellules vivantes judicieusement          
            //Le manutentionneur
            game[1, 14] = 1; game[1, 15] = 1; game[2, 13] = 1; game[3, 12] = 1; game[4, 12] = 1; game[5, 12] = 1; game[6, 13] = 1; game[7, 14] = 1; game[7, 15] = 1;
            //L'autre
            game[5, 28] = 1; game[6, 28] = 1; game[7, 28] = 1; game[5, 29] = 1; game[6, 29] = 1; game[7, 29] = 1; game[6, 30] = 1;
            //Les munitions
            //A gauche
            game[5, 2] = 1; game[5, 3] = 1; game[4, 3] = 1;
            //Au centre
            game[4, 25] = 1; game[4, 26] = 1; game[3, 26] = 1; game[8, 25] = 1; game[8, 26] = 1; game[9, 26] = 1;
            //A droite
            game[6, 37] = 1; game[7, 37] = 1;  game[7, 36] = 1;
            return game;
        }
        //Fonction servant à l'AFFICHAGE
        static void DimensionEtc()
            //Cette fonction définie les dimensions, la couleur et le titre de la fénêtre 
        {
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(800, 800);
            Console.SetWindowSize(200, 55);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Title = "Bienvenue dans le jeu de la vie !";
            Console.Clear();
        }
        static int ModeDeJeu()
            //Cette fonction demande et définie le mode de jeu (avec 1 ou 2 population et avec ou sans prévisualisation)
        {
            Console.WriteLine("Tapez =>");
            Console.WriteLine("0 : Jeu DLV classique sans visualisation intermédiaire des états futurs");
            Console.WriteLine("1 : Jeu DLV classique avec visualisation des états futurs (à naître et à mourir)");
            Console.WriteLine("2 : Jeu DLV variante (2 populations) sans visualisation des états futurs ");
            Console.WriteLine("3 : Jeu DLV variante (2 populations) avec visualisation des états futurs (à naître et à mourir)");
            //On empèche l'utilisateur de mettre n'importe quoi
            int answer;
            do
            {
                Console.SetCursorPosition(10, 2);
                answer = Convert.ToInt32(Console.ReadLine());
            }
            while (answer < 0 || answer > 3);
            //On stock la réponse dans la variable answer
            return answer;
        }
        static void AffichageInformations(int[,] game, int nbrGeneration, int answer, Fenetre gui)
            //Cette fonction affiche la matrice dans la console, la GUI, le nombre de génération et de cellule en vie        
        {
            //Actualise la GUI
            gui.Rafraichir();
            if (answer > 1)
            {   //Cas où il y a 2 populations
                Console.WriteLine("Nombre de génération = " + nbrGeneration + "\t");
                Console.WriteLine("Nombre de cellule vivante population 1 = " + NbrCellulesVivantes(game, 1) + "\t");
                Console.WriteLine("Nombre de cellule vivante population 2 = " + NbrCellulesVivantes(game, 4) + "\t");
                gui.ChangerMessage("Nombre de génération = " + nbrGeneration + "\n" + "Nombre de cellule vivante population noire = " + NbrCellulesVivantes(game, 1) + "\n" + "Nombre de cellule vivante population bleue = " + NbrCellulesVivantes(game, 4));
            }
            else
            {   //Cas où il y a 1 population
                Console.WriteLine("Nombre de génération = " + nbrGeneration + "\t");
                Console.WriteLine("Nombre de cellule vivante population = " + NbrCellulesVivantes(game, 1) + "\t");
                gui.ChangerMessage("Nombre de génération = " + nbrGeneration + "\n" + "Nombre de cellule vivante = " + NbrCellulesVivantes(game, 1) + "\t");
            }
        }
        static void AfficherGameSansVisualisation(int[,] game)
            //Cette fonction affiche la grille dans le mode sans prévisualisation
        {
            for (int i = 0; i < game.GetLength(0); i++)
            {
                for (int k = 0; k < game.GetLength(1); k++)
                {
                    switch (game[i, k])
                    {
                        case 0:
                            Console.Write(".");
                            break;
                        case 1:
                            Console.Write("#");
                            break;
                        case 2:
                            Console.Write(".");
                            break;
                        case 3:
                            Console.Write("#");
                            break;
                        case 4:
                            Console.Write("#");
                            break;
                        case 5:
                            Console.Write(".");
                            break;
                        case 6:
                            Console.Write("#");
                            break;
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        static void AfficherGameAvecVisualisation(int [,] previsualisation)
            //Cette fonction affiche la grille dans le mode avec prévisualisation
        {
            for (int i = 0; i < previsualisation.GetLength(0); i++)
            {
                for (int k = 0; k < previsualisation.GetLength(1); k++)
                {
                    switch (previsualisation[i, k])
                    {
                        case 0:
                            Console.Write(".");
                            break;
                        case 1:
                            Console.Write("#");
                            break;
                        case 2:
                            Console.Write("-");
                            break;
                        case 3:
                            Console.Write("*");
                            break;
                        case 4:
                            Console.Write("#");
                            break;
                        case 5:
                            Console.Write("-");
                            break;
                        case 6:
                            Console.Write("*");
                            break;
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        static int NbrCellulesVivantes(int[,] game, int numeroAssocieAlaPopulation)
            //Cette fonction compte le nombre de cellule vivante
        {
            int NbrCellulesVivantes = 0;
            for (int i = 0; i < game.GetLength(0); i++)
            {
                for (int k = 0; k < game.GetLength(1); k++)
                {
                    if (game[i, k] == numeroAssocieAlaPopulation)
                    {
                        NbrCellulesVivantes = NbrCellulesVivantes + 1;
                    }
                }
            }
            return NbrCellulesVivantes;
        }
        //Fonction récoltant et stockant les données pour le tour suivant
        static bool ChoixPasAPas()
            //Cette fonction demande à l'utilisateur le mode de passage à l'étape suivante qu'il souhaite (automatique ou non)
        {
            bool deroulement = true;
            Console.WriteLine("Si vous voulez que le jeu se déroule automatiquement appuyer sur entrée, sinon appuyez sur n'importe quelle autre touche puis faites entrée (le \"0\" sera la sortie).");
            string choice = Convert.ToString(Console.ReadLine());
            if (choice == "")
            {
                deroulement = false;
            }
            return deroulement;
        }
        static string PassageNextStep(bool pasApas)
            //Cette fonction effectue  : _soit une temporisation si le mode pas à pas est activé (automatique)
            //                           _soit attend que l'utilisateur fasse "entrée" auquel cas le jeu passe à l'étape d'après  (manuelle)
            //Et si l'utilisateur entre "0" le jeu s'arrete
        {
            string choix = "";
            //Si le mode pas à pas est activé
            if (pasApas)
            {   //L'utilisateur doit appuyer sur entrée pour continuer, si choix = "0" alors le jeu s'arrête
                choix = Convert.ToString(Console.ReadLine());
            }
            else
            {   //Temporisation, l'utilisateur n'a rien à faire
                System.Threading.Thread.Sleep(25);
            }
            //On replace le curseur 
            Console.SetCursorPosition(0, 0);
            return choix;
        }
            //récolte
        static int CompteurVoisinsVivantsRang1(int[,] game, int indiceLignes, int indiceColonnes, int population)
            //Cette fonction compte le nombre de voisin vivant de la même population d'une cellule au rang 1
        {
            //On va compter le nombre de voisins vivants (au maximum 8)
            int voisinsAlive = 0;

            //On se place en haut à droite pour commencer à regarder si les voisins sont vivants ou non
            for (int lignes = indiceLignes - 1; lignes < indiceLignes + 2; lignes++)
            {
                for (int colonnes = indiceColonnes - 1; colonnes < indiceColonnes + 2; colonnes++)
                {
                    int nbrColonnes = colonnes;
                    int nbrLignes = lignes;

                    //On ne repasse pas sur la cellule étudiée
                    if (nbrLignes == indiceLignes && nbrColonnes == indiceColonnes)
                    {
                        continue;
                    }

                    //Cas particulier ou on "sort" de la matrice
                    if (nbrColonnes < 0 && indiceColonnes!=0)
                    {
                        nbrColonnes = indiceColonnes - 1;
                    }
                    //On sort vers la gauche
                    if (nbrColonnes < 0 && indiceColonnes == 0)
                    {
                        //nbrColonnes = indiceColonnes;
                        nbrColonnes = game.GetLength(1) - 1;
                    }
                    if (nbrLignes < 0 && indiceLignes !=0)
                    {
                        nbrLignes = indiceLignes - 1;
                    }
                    if (nbrLignes < 0 && indiceLignes == 0)
                    {
                        //nbrLignes = indiceLignes;
                        nbrLignes = game.GetLength(0) - 1;
                    }
                    //On sort vers la droite
                    if (nbrColonnes > game.GetLength(1) - 1)
                    {
                        nbrColonnes = 0;
                    }
                    //On sort vers le bas 
                    if (nbrLignes > game.GetLength(0) - 1)
                    {
                        nbrLignes = 0;
                    }

                    //Si le voisin est vivant on incrémente le compteur
                    if (game[nbrLignes, nbrColonnes] == population)
                    {
                        voisinsAlive++;
                    }
                }
            }
            return voisinsAlive;
        }
        static int CompteurVoisinsVivantsRang2(int[,] game, int indiceLignes, int indiceColonnes, int population)
            //Cette fonction compte le nombre de voisin vivant de la même population d'une cellule au rang 2
        {
            //On va compter le nombre de voisins vivants (au maximum 24)
            int voisinsAlive = 0;

            //On se place en haut à droite pour commencer à regarder si les voisins sont vivants ou non
            for (int lignes = indiceLignes - 2; lignes < indiceLignes + 3; lignes++)
            {
                for (int colonnes = indiceColonnes - 2; colonnes < indiceColonnes + 3; colonnes++)
                {
                    int nbrColonnes = colonnes;
                    int nbrLignes = lignes;

                    //On ne repasse pas sur la cellule étudiée
                    if (nbrLignes == indiceLignes && nbrColonnes == indiceColonnes)
                    {
                        continue;
                    }

                    //Cas particulier où on "sort" 1 fois de la matrice
                    if (nbrColonnes ==-1 && indiceColonnes != 0)
                    {
                        nbrColonnes = indiceColonnes - 1;
                    }
                    //On sort vers la gauche
                    if (nbrColonnes ==-1 && indiceColonnes == 0)
                    {
                        nbrColonnes = game.GetLength(1) - 1;
                    }
                    if (nbrLignes == -1 && indiceLignes != 0)
                    {
                        nbrLignes = indiceLignes - 1;
                    }
                    if (nbrLignes == -1 && indiceLignes == 0)
                    {
                        nbrLignes = game.GetLength(0) - 1;
                    }
                    //On sort vers la droite
                    if (nbrColonnes == game.GetLength(1))
                    {
                        nbrColonnes = 0;
                    }
                    //On sort vers le bas 
                    if (nbrLignes == game.GetLength(0))
                    {
                        nbrLignes = 0;
                    }
                    //Cas particulier ou on "sort" 2 fois de la matrice
                    if (nbrColonnes == -2 && indiceColonnes != 0)
                    {
                        nbrColonnes = indiceColonnes - 2;
                    }
                    //On sort vers la gauche
                    if (nbrColonnes == -2 && indiceColonnes == 0)
                    {
                        nbrColonnes = game.GetLength(1) - 2;
                    }
                    if (nbrLignes == -2 && indiceLignes != 0)
                    {
                        nbrLignes = indiceLignes - 2;
                    }
                    if (nbrLignes == -2 && indiceLignes == 0)
                    {
                        nbrLignes = game.GetLength(0) - 2;
                    }
                    //On sort vers la droite
                    if (nbrColonnes == game.GetLength(1) + 1)
                    {
                        nbrColonnes = 1;
                    }
                    //On sort vers le bas 
                    if (nbrLignes == game.GetLength(0) + 1)
                    {
                        nbrLignes = 1;
                    }

                    //Si le voisin est vivant on incrémente le compteur
                    if (game[nbrLignes, nbrColonnes] == population)
                    {
                        voisinsAlive++;
                    }
                }
            }
            return voisinsAlive;
        }
            //stockage
        static int[,] RemplissageMatriceVoisinsVivantsRang1(int[,] game, int population)
            //Cette fonction remplie la matrice possédant le nombre de voisin vivant à la case correspondante, d'une certaine population au rang 1
        {
            //Création
            int[,] MatriceNbrVoisinsVivants1 = new int[game.GetLength(0), game.GetLength(1)];

            //On remplit toute la matrice
            for (int i = 0; i < game.GetLength(0); i++)
            {
                for (int k = 0; k < game.GetLength(1); k++)
                {
                    //Aux mêmes indices où se trouve la cellule, on place le nombre de voisins vivants
                    MatriceNbrVoisinsVivants1[i, k] = CompteurVoisinsVivantsRang1(game, i, k, population);
                }
            }
            return MatriceNbrVoisinsVivants1;
        }
        static int[,] RemplissageMatriceVoisinsVivantsRang2(int[,] game, int population)
            //Cette fonction remplie la matrice possédant le nombre de voisin vivant à la case correspondante, d'une certaine population au rang 2
        {
            //Création
            int[,] MatriceNbrVoisinsVivants2 = new int[game.GetLength(0), game.GetLength(1)];

            //On remplit toute la matrice
            for (int i = 0; i < game.GetLength(0); i++)
            {
                for (int k = 0; k < game.GetLength(1); k++)
                {
                    //Aux mêmes indices où se trouve la cellule, on place le nombre de voisins vivants
                    MatriceNbrVoisinsVivants2[i, k] = CompteurVoisinsVivantsRang2(game, i, k, population);
                }
            }
            return MatriceNbrVoisinsVivants2;
        }
        //Fonction donnant l'état futur de la cellule
        static int EtatCelluleAuTourSuivant(int[,] game, int indexLigne, int indexColonne, int nbdeVoisinVivant)
            //Cette fonction définie l'état futur d'une cellule en fonction des règles de l'étape 1
        {
            //Etat de la cellule au tour suivant, qui sera retournée
            int etatCellule = game[indexLigne, indexColonne];

            //dans le cas où la cellule est morte
            if (game[indexLigne, indexColonne] == 0)
            {
                if (nbdeVoisinVivant == 3)
                {
                    etatCellule = 1;
                }
            }

            //dans le cas où la cellule est vivante
            if (game[indexLigne, indexColonne] == 1)
            {
                //Sous-population
                if (nbdeVoisinVivant < 2)
                {
                    etatCellule = 0;
                }

                //Sur-population
                if (nbdeVoisinVivant > 3)
                {
                    etatCellule = 0;
                }
            }
            //Dans tous les autres cas, les cellules restent dans le même état,
            //ils n'ont donc pas besoin d'être étudiée car int etatCellule = game[indexLigne, indexColonne]

            return etatCellule;

        }
        static int EtatCelluleAuTourSuivantPourPrevisualiser(int[,] game, int indexLigne, int indexColonne, int nbdeVoisinVivant)
            //Cette fonction définie l'état futur d'une cellule en fonction des règles de l'étape 1 avec prévisualisation
        {
            //Etat de la cellule au tour suivant, qui sera retournée
            int etatCellule = game[indexLigne, indexColonne];

            //dans le cas où la cellule est morte
            if (game[indexLigne, indexColonne] == 0)
            {
                if (nbdeVoisinVivant == 3)
                {
                    etatCellule = 2;
                }
            }

            //dans le cas où la cellule est vivante
            if (game[indexLigne, indexColonne] == 1)
            {
                //Sous-population
                if (nbdeVoisinVivant < 2)
                {
                    etatCellule = 3;
                }

                //Sur-population
                if (nbdeVoisinVivant > 3)
                {
                    etatCellule = 3;
                }
            }
            //Dans tous les autres cas, les cellules restent dans le même état,
            //ils n'ont donc pas besoin d'être étudiée car int etatCellule = game[indexLigne, indexColonne]
            return etatCellule;
        }
        static int EtatCelluleAuTourSuivant2Popu(int[,] game, int indexLigne, int indexColonne, int[,] MatriceNbrVoisinsVivantsPopulation1rang1, int[,] MatriceNbrVoisinsVivantsPopulation2rang1, int[,] MatriceNbrVoisinVivantPopulation1rang2, int[,] MatriceNbrVoisinVivantPopulation2rang2)
            //Cette fonction définie l'état futur d'une cellule en fonction des règles de l'étape 2          
        {
            //j'ai différencié les cas : j'ai d'abord appliqué les règles pour la population 1 puis pour la population 2
            int EtatCelluleAuTourSuivant = game[indexLigne, indexColonne];

            //La première population
            if (game[indexLigne, indexColonne] == 1)
            {
                //R1b (Sous-population)
                if (MatriceNbrVoisinsVivantsPopulation1rang1[indexLigne, indexColonne] < 2)
                {
                    EtatCelluleAuTourSuivant = 0;
                }
                //R2b (Sur-population)
                if (MatriceNbrVoisinsVivantsPopulation1rang1[indexLigne, indexColonne] > 3)
                {
                    EtatCelluleAuTourSuivant = 0;
                }
            }
            //R3b
            if (game[indexLigne, indexColonne] == 0)
            {
                if (MatriceNbrVoisinsVivantsPopulation1rang1[indexLigne, indexColonne] == 3)
                {
                    EtatCelluleAuTourSuivant = 1;
                }
            }
            //R4b
            if (game[indexLigne, indexColonne] == 0)
            {
                if ((MatriceNbrVoisinsVivantsPopulation1rang1[indexLigne, indexColonne] == 3) && (MatriceNbrVoisinsVivantsPopulation2rang1[indexLigne, indexColonne] == 3))
                {
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] > MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        EtatCelluleAuTourSuivant = 1;
                    }
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] < MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        EtatCelluleAuTourSuivant = 4;
                    }//cas d'égalité des voisin au rang 2
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] == MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        if (NbrCellulesVivantes(game, 1) > NbrCellulesVivantes(game, 2))
                        {
                            EtatCelluleAuTourSuivant = 1;
                        }
                        if (NbrCellulesVivantes(game, 1) < NbrCellulesVivantes(game, 2))
                        {
                            EtatCelluleAuTourSuivant = 4;
                        }
                        //les populations sont égales
                        if (NbrCellulesVivantes(game, 1) == NbrCellulesVivantes(game, 2))
                        {   //elle reste à 0 = morte
                            EtatCelluleAuTourSuivant = game[indexLigne, indexColonne];
                        }
                    }
                }
            }

            //La seconde population
            if (game[indexLigne, indexColonne] == 4)
            {
                //R1b (Sous-population)
                if (MatriceNbrVoisinsVivantsPopulation2rang1[indexLigne, indexColonne] < 2)
                {
                    EtatCelluleAuTourSuivant = 0;
                }
                //R2b (Sur-population)
                if (MatriceNbrVoisinsVivantsPopulation2rang1[indexLigne, indexColonne] > 3)
                {
                    EtatCelluleAuTourSuivant = 0;
                }
            }
            //R3b
            if (game[indexLigne, indexColonne] == 0)
            {
                if (MatriceNbrVoisinsVivantsPopulation2rang1[indexLigne, indexColonne] == 3)
                {
                    EtatCelluleAuTourSuivant = 4;
                }
            }
            //R4b
            if (game[indexLigne, indexColonne] == 0)
            {
                if ((MatriceNbrVoisinsVivantsPopulation1rang1[indexLigne, indexColonne] == 3) && (MatriceNbrVoisinsVivantsPopulation2rang1[indexLigne, indexColonne] == 3))
                {
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] > MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        EtatCelluleAuTourSuivant = 1;
                    }
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] < MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        EtatCelluleAuTourSuivant = 4;
                    }//cas d'égalité des voisin au rang 2
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] == MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        if (NbrCellulesVivantes(game, 1) > NbrCellulesVivantes(game, 2))
                        {
                            EtatCelluleAuTourSuivant = 1;
                        }
                        if (NbrCellulesVivantes(game, 1) < NbrCellulesVivantes(game, 2))
                        {
                            EtatCelluleAuTourSuivant = 4;
                        }//les populations sont égales
                        if (NbrCellulesVivantes(game, 1) == NbrCellulesVivantes(game, 2))
                        {   //elle reste à 2
                            EtatCelluleAuTourSuivant = game[indexLigne, indexColonne];
                        }
                    }
                }
            }
            return EtatCelluleAuTourSuivant;
        }
        static int EtatCelluleAuTourSuivant2PourPrevisualiser(int[,] game, int indexLigne, int indexColonne, int[,] MatriceNbrVoisinsVivantsPopulation1rang1, int[,] MatriceNbrVoisinsVivantsPopulation2rang1, int[,] MatriceNbrVoisinVivantPopulation1rang2, int[,] MatriceNbrVoisinVivantPopulation2rang2)
            //Cette fonction définie l'état futur d'une cellule en fonction des règles de l'étape 2 avec prévisualisation                                                                                                                                                                                                                                                                                  //Cette fonction définie l'état futur d'une cellule en fonction des règles de l'étape 2          
        {
            //j'ai différencié les cas : j'ai d'abord appliqué les règles pour la population 1 puis pour la population 2
            int EtatCelluleAuTourSuivant = game[indexLigne, indexColonne];

            //La première population
            if (game[indexLigne, indexColonne] == 1)
            {
                //R1b (Sous-population)
                if (MatriceNbrVoisinsVivantsPopulation1rang1[indexLigne, indexColonne] < 2)
                {
                    EtatCelluleAuTourSuivant = 3;
                }
                //R2b (Sur-population)
                if (MatriceNbrVoisinsVivantsPopulation1rang1[indexLigne, indexColonne] > 3)
                {
                    EtatCelluleAuTourSuivant = 3;
                }
            }
            //R3b
            if (game[indexLigne, indexColonne] == 0)
            {
                if (MatriceNbrVoisinsVivantsPopulation1rang1[indexLigne, indexColonne] == 3)
                {
                    EtatCelluleAuTourSuivant = 2;
                }
            }
            //R4b
            if (game[indexLigne, indexColonne] == 0)
            {
                if ((MatriceNbrVoisinsVivantsPopulation1rang1[indexLigne, indexColonne] == 3) && (MatriceNbrVoisinsVivantsPopulation2rang1[indexLigne, indexColonne] == 3))
                {
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] > MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        EtatCelluleAuTourSuivant = 2;
                    }
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] < MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        EtatCelluleAuTourSuivant = 5;
                    }//cas d'égalité des voisin au rang 2
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] == MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        if (NbrCellulesVivantes(game, 1) > NbrCellulesVivantes(game, 2))
                        {
                            EtatCelluleAuTourSuivant = 2;
                        }
                        if (NbrCellulesVivantes(game, 1) < NbrCellulesVivantes(game, 2))
                        {
                            EtatCelluleAuTourSuivant = 5;
                        }
                        //les populations sont égales
                        if (NbrCellulesVivantes(game, 1) == NbrCellulesVivantes(game, 2))
                        {   //elle reste à 0 = morte
                            EtatCelluleAuTourSuivant = game[indexLigne, indexColonne];
                        }
                    }
                }
            }

            //La seconde population
            if (game[indexLigne, indexColonne] == 4)
            {
                //R1b (Sous-population)
                if (MatriceNbrVoisinsVivantsPopulation2rang1[indexLigne, indexColonne] < 2)
                {
                    EtatCelluleAuTourSuivant = 6;
                }
                //R2b (Sur-population)
                if (MatriceNbrVoisinsVivantsPopulation2rang1[indexLigne, indexColonne] > 3)
                {
                    EtatCelluleAuTourSuivant = 6;
                }
            }
            //R3b
            if (game[indexLigne, indexColonne] == 0)
            {
                if (MatriceNbrVoisinsVivantsPopulation2rang1[indexLigne, indexColonne] == 3)
                {
                    EtatCelluleAuTourSuivant = 5;
                }
            }
            //R4b
            if (game[indexLigne, indexColonne] == 0)
            {
                if ((MatriceNbrVoisinsVivantsPopulation1rang1[indexLigne, indexColonne] == 3) && (MatriceNbrVoisinsVivantsPopulation2rang1[indexLigne, indexColonne] == 3))
                {
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] > MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        EtatCelluleAuTourSuivant = 2;
                    }
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] < MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        EtatCelluleAuTourSuivant = 5;
                    }//cas d'égalité des voisin au rang 2
                    if (MatriceNbrVoisinVivantPopulation1rang2[indexLigne, indexColonne] == MatriceNbrVoisinVivantPopulation2rang2[indexLigne, indexColonne])
                    {
                        if (NbrCellulesVivantes(game, 1) > NbrCellulesVivantes(game, 2))
                        {
                            EtatCelluleAuTourSuivant = 2;
                        }
                        if (NbrCellulesVivantes(game, 1) < NbrCellulesVivantes(game, 2))
                        {
                            EtatCelluleAuTourSuivant = 5;
                        }//les populations sont égales
                        if (NbrCellulesVivantes(game, 1) == NbrCellulesVivantes(game, 2))
                        {   //elle reste à 2
                            EtatCelluleAuTourSuivant = game[indexLigne, indexColonne];
                        }
                    }
                }
            }
            return EtatCelluleAuTourSuivant;
        }
        //Fonction modifiant les cellules pour le tour suivant 
        static int[,] GameTourDaprès(int[,] game, int NbrPopulation, int[,] MatriceNbrVoisinsVivantsPopulation1rang1, int[,] MatriceNbrVoisinsVivantsPopulation2, int[,] MatriceNbrVoisinVivantPopulation1rang2, int[,] MatriceNbrVoisinVivantPopulation2rang2)
            //Cette fonction modifie la matrice (appelé game) afin qu'elle représente la grille du tour suivant
        {
            for (int i = 0; i < game.GetLength(0); i++)
            {
                for (int k = 0; k < game.GetLength(1); k++)
                {
                    //Cas pour une seule population
                    if(NbrPopulation==1)
                    {
                        game[i, k] = EtatCelluleAuTourSuivant(game, i, k, MatriceNbrVoisinsVivantsPopulation1rang1[i, k]);
                    }
                    //Cas pour deux populations
                    if (NbrPopulation == 2)
                    {
                        game[i, k] = EtatCelluleAuTourSuivant2Popu(game, i, k, MatriceNbrVoisinsVivantsPopulation1rang1, MatriceNbrVoisinsVivantsPopulation2, MatriceNbrVoisinVivantPopulation1rang2, MatriceNbrVoisinVivantPopulation2rang2);
                    }
                }
            }
            return game;
        }
        static void ModificationPourPrevisualisation(int[,] game, int[,] MatriceNbrVoisinsVivantsPopulation1rang1, int population)
            //Cette fonction permet de créer une matrice (temporaire uniquement pour la prévisualisation) 
            //Pour calculer les états futurs de chaque cellule, elle copie ensuite tout dans la matrice game
        {
            int[,] previsualisation = new int[game.GetLength(0), game.GetLength(1)];
            //On remplie la matrice en regardant au tour suivant ce qui se passe (avec des 0, 1, 2 et 3)
            for (int i = 0; i < previsualisation.GetLength(0); i++)
            {
                for (int k = 0; k < previsualisation.GetLength(1); k++)
                {
                    previsualisation[i, k] = EtatCelluleAuTourSuivantPourPrevisualiser(game, i, k, MatriceNbrVoisinsVivantsPopulation1rang1[i, k]);
                }
            }
            //Copie dans la matrice game
            for (int i = 0; i < previsualisation.GetLength(0); i++)
            {
                for (int k = 0; k < previsualisation.GetLength(1); k++)
                {
                    game[i, k] = previsualisation[i, k];
                }
            }
        }
        static void ModificationPourPrevisualisationAvec2Population(int[,] game, int[,] MatriceNbrVoisinsVivantsPopulation1rang1, int[,] MatriceNbrVoisinsVivantsPopulation2, int[,] MatriceNbrVoisinVivantPopulation1rang2, int[,] MatriceNbrVoisinVivantPopulation2rang2) 
            //Cette fonction permet de créer une matrice (temporaire uniquement pour la prévisualisation) 
            //Pour calculer les états futurs de chaque cellule, elle copie ensuite tout dans la matrice game
        {
            int[,] previsualisation = new int[game.GetLength(0), game.GetLength(1)];
            //On remplie la matrice en regardant au tour suivant ce qui se passe (avec des 0, 1, 2 et 3)
            for (int i = 0; i < previsualisation.GetLength(0); i++)
            {
                for (int k = 0; k < previsualisation.GetLength(1); k++)
                {
                    previsualisation[i, k] = EtatCelluleAuTourSuivant2PourPrevisualiser(game, i, k, MatriceNbrVoisinsVivantsPopulation1rang1, MatriceNbrVoisinsVivantsPopulation2, MatriceNbrVoisinVivantPopulation1rang2, MatriceNbrVoisinVivantPopulation2rang2);
                }
            }
            //Copie dans la matrice game
            for (int i = 0; i < previsualisation.GetLength(0); i++)
            {
                for (int k = 0; k < previsualisation.GetLength(1); k++)
                {
                    game[i, k] = previsualisation[i, k];
                }
            }
        }
        static void GameTourDApresAvecPrevisualisation(int[,] game)
            //Cette fonction nous fait passer de la prévisualisation à l'étape d'après
        {
            //On parcourt toute la matrice
            for (int i = 0; i < game.GetLength(0); i++)
            {
                for (int k = 0; k < game.GetLength(1); k++)
                {
                    //Population 1
                    //dans le cas où la cellule est à naitre
                    if (game[i, k] == 2)
                    {
                        game[i, k] = 1;
                    }
                    //dans le cas où la cellule est à mourir
                    if (game[i, k] == 3)
                    {
                        game[i, k] = 0;
                    }

                    //Population 2
                    //dans le cas où la cellule est à naitre
                    if (game[i, k] == 5)
                    {
                        game[i, k] = 4;
                    }
                    //dans le cas où la cellule est à mourir
                    if (game[i, k] == 6)
                    {
                        game[i, k] = 0;
                    }
                    //Dans tous les autres cas, les cellules restent dans le même état,
                }
            }
        }

        [System.STAThreadAttribute()]
        static void Main(string[] args)
        {
            //Preset           
            DimensionEtc();
            int nbrGeneration = 1;
            string choix = "";
            Random rand = new Random();

            //L'utilisateur décide du mode de jeu et de déroulement
            bool pasApas = ChoixPasAPas();
            int answer = ModeDeJeu();

            //Generation du monde et de la GUI
            int[,] game = GenerationGame(rand, answer); //CanonAPlaneur(); 
            Fenetre gui = new Fenetre(game, 14, 0, 0, "Bienvenue dans le jeu de la vie !");

            do
            {
                //Affichage      
                AfficherGameSansVisualisation(game);
                AffichageInformations(game, nbrGeneration, answer, gui);

                switch (answer)
                {
                    // JEU DLV classique sans visualisation intermédiaire des états futurs
                    case 0:
                        //Passage à l'étape d'après
                        int[,] MatriceNbrVoisinsVivantsPopulation1rang1 = RemplissageMatriceVoisinsVivantsRang1(game, 1);
                        game = GameTourDaprès(game, 1, MatriceNbrVoisinsVivantsPopulation1rang1, null, null, null);
                        break;

                    // JEU DLV classique avec visualisation des états futurs (à naître et à mourir)
                    case 1:
                        //Prévisualisation
                        choix = PassageNextStep(pasApas);
                        MatriceNbrVoisinsVivantsPopulation1rang1 = RemplissageMatriceVoisinsVivantsRang1(game, 1);
                        ModificationPourPrevisualisation(game, MatriceNbrVoisinsVivantsPopulation1rang1, 1);
                        AfficherGameAvecVisualisation(game);
                        gui.Rafraichir();                    
                        //Passage à l'étape d'après
                        GameTourDApresAvecPrevisualisation(game);
                        break;

                    // JEU DLV variante (2 populations) sans visualisation des états futurs 
                    case 2:
                        //Prépare l'étape d'après
                        MatriceNbrVoisinsVivantsPopulation1rang1 = RemplissageMatriceVoisinsVivantsRang1(game, 1);
                        int[,] MatriceNbrVoisinsVivantsPopulation2 = RemplissageMatriceVoisinsVivantsRang1(game, 4);
                        int[,] MatriceNbrVoisinVivantPopulation1rang2 = RemplissageMatriceVoisinsVivantsRang2(game, 1);
                        int[,] MatriceNbrVoisinVivantPopulation2rang2 = RemplissageMatriceVoisinsVivantsRang2(game, 4);
                        game = GameTourDaprès(game, 2, MatriceNbrVoisinsVivantsPopulation1rang1, MatriceNbrVoisinsVivantsPopulation2, MatriceNbrVoisinVivantPopulation1rang2, MatriceNbrVoisinVivantPopulation2rang2);
                        break;

                    // JEU DLV variante (2 populations) avec visualisation des états futurs
                    case 3:
                        //Prévisualisation
                        choix = PassageNextStep(pasApas);
                        MatriceNbrVoisinsVivantsPopulation1rang1 = RemplissageMatriceVoisinsVivantsRang1(game, 1);
                        MatriceNbrVoisinsVivantsPopulation2 = RemplissageMatriceVoisinsVivantsRang1(game, 4);
                        MatriceNbrVoisinVivantPopulation1rang2 = RemplissageMatriceVoisinsVivantsRang2(game, 1);
                        MatriceNbrVoisinVivantPopulation2rang2 = RemplissageMatriceVoisinsVivantsRang2(game, 4);
                        ModificationPourPrevisualisationAvec2Population(game, MatriceNbrVoisinsVivantsPopulation1rang1, MatriceNbrVoisinsVivantsPopulation2, MatriceNbrVoisinVivantPopulation1rang2, MatriceNbrVoisinVivantPopulation2rang2);
                        AfficherGameAvecVisualisation(game);
                        gui.Rafraichir();
                        //Prépare l'étape d'après
                        GameTourDApresAvecPrevisualisation(game);
                        break;

                    //Input error
                    default:
                        Console.WriteLine("\nErreur de saisi, veuillez quitter et recommencer la procédure.\n");
                        break;
                }
                
                //Incrémente le compteur de génération
                nbrGeneration++;

                //L'utilisateur passe ou non à l'étape d'après
                choix = PassageNextStep(pasApas);               
            }
            while (choix != "0");
        }//End static Main
    }//End class Program    
}//End namespace

//Merci,
//Cordialement.