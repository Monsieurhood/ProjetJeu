//-----------------------------------------------------------------------
// <copyright file="Game.cs" company="Marco Lavoie">
// Marco Lavoie, 2010. Tous droits r�serv�s
// 
// L'utilisation de ce mat�riel p�dagogique (pr�sentations, code source 
// et autres) avec ou sans modifications, est permise en autant que les 
// conditions suivantes soient respect�es:
//
// 1. La diffusion du mat�riel doit se limiter � un intranet dont l'acc�s
//    est imit� aux �tudiants inscrits � un cours exploitant le dit 
//    mat�riel. IL EST STRICTEMENT INTERDIT DE DIFFUSER CE MAT�RIEL 
//    LIBREMENT SUR INTERNET.
// 2. La redistribution des pr�sentations contenues dans le mat�riel 
//    p�dagogique est autoris�e uniquement en format Acrobat PDF et sous
//    restrictions stipul�es � la condition #1. Le code source contenu 
//    dans le mat�riel p�dagogique peut cependant �tre redistribu� sous 
//    sa forme  originale, en autant que la condition #1 soit �galement 
//    respect�e.
// 3. Le mat�riel diffus� doit contenir int�gralement la mention de 
//    droits d'auteurs ci-dessus, la notice pr�sente ainsi que la
//    d�charge ci-dessous.
// 
// CE MAT�RIEL P�DAGOGIQUE EST DISTRIBU� "TEL QUEL" PAR L'AUTEUR, SANS 
// AUCUNE GARANTIE EXPLICITE OU IMPLICITE. L'AUTEUR NE PEUT EN AUCUNE 
// CIRCONSTANCE �TRE TENU RESPONSABLE DE DOMMAGES DIRECTS, INDIRECTS, 
// CIRCONSTENTIELS OU EXEMPLAIRES. TOUTE VIOLATION DE DROITS D'AUTEUR 
// OCCASIONN� PAR L'UTILISATION DE CE MAT�RIEL P�DAGOGIQUE EST PRIS EN 
// CHARGE PAR L'UTILISATEUR DU DIT MAT�RIEL.
// 
// En utilisant ce mat�riel p�dagogique, vous acceptez implicitement les
// conditions et la d�charge exprim�s ci-dessus.
// </copyright>
//-----------------------------------------------------------------------

namespace ProjetFinale
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using Microsoft.Xna.Framework.Net;
    using Microsoft.Xna.Framework.Storage;

    /// <summary>
    /// Classe principale du jeu.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Largeur d'une tuile en pixels.
        /// </summary>
        private const int LargeurTuile = 32;

        /// <summary>
        /// Hauteur d'une tuile en pixels.
        /// </summary>
        private const int HauteurTuile = 32;

        /// <summary>
        /// Attribut permettant d'obtenir des infos sur la carte graphique et l'�cran.
        /// </summary>
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// Attribut g�rant l'affichage en batch � l'�cran.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Attribut repr�sentant le monde de tuiles � afficherdurant le jeu.
        /// </summary>
        private Monde mondeDeTuiles;

        // Effet sonore contenant la chanson de fond du jeu.
        private SoundEffect chanson;

        // Instance de chanson de fond en cours de sonorisation durant le jeu.
        private static SoundEffectInstance chansonActive;

        /// <summary>
        /// Mappe monde : chaque valeur du tableau correspond � l'index d'une tuile dans mondeDeTuiles.
        /// </summary>
        private int[,] mappeMonde1 =
                 {
                     {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{1,0,0,2,3,0,0,0,9,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{1,16,17,0,0,0,0,11,10,8,0,0,0,0,0,0,0,0,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{1,32,33,0,4,0,0,5,6,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,0,0,0,0,0},
{1,12,12,12,12,12,12,12,12,12,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0},
{13,13,13,13,13,13,13,13,13,13,0,0,14,14,15,0,0,0,0,0,85,0,0,15,85,0,0,0,0,0,85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{13,13,13,13,13,13,13,13,13,13,0,0,0,0,15,0,0,85,0,0,0,0,0,15,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{13,13,13,13,13,13,13,13,13,13,12,0,0,0,15,0,0,0,0,0,0,0,0,15,0,0,0,85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,24,25,26,27,0,29,30,31},
{13,13,13,13,13,13,13,13,13,13,13,12,0,0,15,15,86,86,86,86,86,86,86,15,0,0,0,0,0,0,0,0,0,85,0,0,0,0,0,0,0,0,40,41,42,43,44,45,46,47},
{13,13,18,18,18,18,18,18,18,18,18,18,0,0,13,15,15,15,15,15,15,15,15,15,85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,22,23,72,57,58,75,60,61,62,63},
{13,13,18,18,18,18,18,18,18,18,18,18,0,12,13,13,13,13,13,13,13,13,13,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,38,39,72,73,74,75,76,77,78,63},
{13,13,18,18,18,18,18,18,18,18,18,18,12,13,13,13,13,13,13,13,13,13,13,13,4,0,0,0,0,0,0,0,0,0,0,0,4,0,21,0,54,55,88,90,90,91,92,93,94,79},
{13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,12,12,19,20,86,86,86,86,86,86,86,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12},
{13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,19,20,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34},
{13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,35,36,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,15},
{13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,15},
{52,70,18,18,18,18,18,18,70,18,18,70,70,18,18,70,18,18,66,67,68,18,18,70,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,15},
{52,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,37,18,18,18,18,18,18,18,18,51,51,51,51,51,51,15,15,18,18,18,18,18,18,18,18,18,18,18,18,18,18,15},
{52,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,37,18,18,18,18,18,18,18,51,50,50,50,50,50,50,50,50,15,15,15,15,15,18,18,18,18,18,18,18,18,18,15},
{52,18,18,18,18,34,34,34,34,34,34,34,34,52,18,18,18,18,53,18,18,18,18,18,18,51,50,50,50,50,50,50,50,50,50,50,50,50,50,50,15,15,15,15,15,18,18,18,18,15},
{52,94,18,18,18,18,18,18,18,18,18,18,18,34,34,34,34,34,69,51,51,51,51,51,51,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,15,15,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,15,18,18,18,15},
{52,15,15,15,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,34,34,34,34,34,34,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,18,18,18,18,18,18,51,51,51,51,51,51,51,51,18,18,18,18,18,18,18,51,51,51,51,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,15,15},
{52,18,18,18,18,18,18,50,50,50,50,50,50,50,50,51,18,18,51,51,97,97,51,18,18,18,18,18,18,34,34,34,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,18,18,50,50,51,50,50,18,18,18,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{83,34,34,34,34,34,34,18,70,50,50,50,50,50,50,50,18,18,50,50,50,50,50,34,34,34,34,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,15,18,18,18,15},
{52,18,18,18,18,18,18,48,49,50,50,50,50,50,50,50,18,18,50,50,50,50,50,18,18,18,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,18,18,18,18,18,18,64,65,50,50,50,50,50,50,50,18,18,50,50,50,50,50,18,18,18,18,18,18,34,34,34,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,18,18,50,50,50,50,50,18,18,18,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,15,15},
{52,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,18,18,50,50,50,50,50,18,18,18,34,34,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,18,18,18,34,18,18,50,50,50,50,50,50,50,50,50,18,18,50,50,50,50,50,18,18,18,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,97,97,50,50,50,50,50,34,18,18,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,15,18,18,18,15},
{52,34,34,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,18,18,18,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,34,34,34,34,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,18,18,18,34,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,15,15},
{52,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,34,34,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,34,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,50,50,50,50,50,50,50,50,50,50,50,50,15,18,18,18,18,15},
{52,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,34,34,18,18,18,18,18,50,50,15,15,15,15,15,15,15,15,15,15,15,15,18,18,18,15},
{51,51,51,51,51,51,51,51,51,52,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,50,50,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,15},
{50,50,50,50,50,50,50,50,50,52,18,18,51,51,51,51,51,51,51,51,52,18,18,18,18,18,18,18,18,18,18,18,50,50,18,18,18,18,18,18,18,18,18,18,18,18,18,18,15,15},
{50,50,50,50,50,50,50,50,50,51,51,51,50,50,50,50,50,50,50,50,52,18,18,18,18,18,18,18,18,18,18,18,50,50,84,18,18,18,18,18,18,18,18,18,18,18,18,18,18,15},
{50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,51,52,18,18,18,18,18,18,18,18,18,97,50,50,15,15,18,18,18,18,18,18,18,18,18,18,18,97,97,15},
{50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,51,51,51,51,51,51,51,51,51,51,51,50,50,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51},
{50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50},
{50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50},
{50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50},
{50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50},
{50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50}
};
        
        /// <summary>
        /// Attribut repr�sentant le personnage contr�l� par le joueur.
        /// </summary>
        private JoueurSprite joueur;

        /// <summary>
        /// Attribut repr�sentant la camera.
        /// </summary>
        private Camera camera;

        /// <summary>
        /// Constructeur par d�faut de la classe. Cette classe est g�n�r�e automatiquement
        /// par Visual Studio lors de la cr�ation du projet.
        /// </summary>
        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Fonction retournant le niveau de r�sistance aux d�placements en fonction de la couleur du pixel de tuile
        /// � la position donn�e.
        /// </summary>
        /// <param name="position">Position du pixel en coordonn�es du monde.</param>
        /// <returns>Facteur de r�sistance entre 0.0f (aucune r�sistance) et 1.0f (r�sistance maximale).</returns>
        public float CalculerResistanceAuMouvement(Vector2 position)
        {
            Color pixColor = Color.Black;

            // Extraire la couleur du pixel correspondant � la position donn�e dans privTuilesCollisions.
            try
            {
                pixColor = this.mondeDeTuiles.CouleurDeCollision(position);
            }
            catch (System.IndexOutOfRangeException)
            {
                this.Exit();
            }

            // D�terminer le niveau de r�sistance en fonction de la couleur
            if (pixColor == Color.Black)
            {
                return 1.0f;
            }
            else if (pixColor == Color.Blue)
                return 0.5f;
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// D�finition de fonction d�l�gu�e permettant de valider un d�placement d'une position
        /// � une autre dans le monde. La fonction retourne le point le plus pr�s de 
        /// (posSource.X+deltaX, posSource.Y+DeltaY) jusqu'o� le personnage peut se rendre horizontalement 
        /// et verticalement sans rencontrer de r�sistance plus �l�v�e que la limite donn�e.
        /// </summary>
        /// <param name="posSource">Position du pixel de d�part du d�placement, en coordonn�es du monde.</param>
        /// <param name="deltaX">D�placement total horizontal, en coordonn�es du monde.</param>
        /// <param name="deltaY">D�placement total vertical, en coordonn�es du monde.</param>
        /// <param name="resistanceMax">R�sistance maximale tol�r�e lors du d�placement.</param>
        public void SpriteValiderDeplacement(Vector2 posSource, ref int deltaX, ref int deltaY, float resistanceMax)
        {
            Vector2 dest = new Vector2(posSource.X, posSource.Y);

            // Premi�rement consid�rer le d�placement horizontal. Incr�menter la distance horizontale
            // de d�placement jusqu'� deltaX ou jusqu'� ce qu'une r�sistance sup�rieure � celle tol�r�e
            // soit rencontr�e.
            while (dest.X != posSource.X + deltaX)
            {
                dest.X += Math.Sign(deltaX);        // incr�menter la distance horizontale

                // V�rifier la r�sistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.X -= Math.Sign(deltaX);    // reculer d'un pixel (valid� � l'it�ration pr�c�dente)
                    break;
                }
            }

            // Maintenant consid�rer le d�placement vertical. Incr�menter la distance verticale
            // de d�placement jusqu'� deltaY ou jusqu'� ce qu'une r�sistance sup�rieure � celle tol�r�e
            // soit rencontr�e.
            while (dest.Y != posSource.Y + deltaY)
            {
                dest.Y += Math.Sign(deltaY);        // incr�menter la distance horizontale

                // V�rifier la r�sistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.Y -= Math.Sign(deltaY);    // reculer d'un pixel (valid� � l'it�ration pr�c�dente)
                    break;
                }
            }

            // D�terminer le d�placement maximal dans les deux directions
            deltaX = (int)(dest.X - posSource.X);
            deltaY = (int)(dest.Y - posSource.Y);
        }

        /// <summary>
        /// Permet au jeu d'effectuer toute initialisation avant de commencer � jouer.
        /// Cette fonction membre peut demander les services requis et charger tout contenu
        /// non graphique pertinent. L'invocation de base.Initialize() it�rera parmi les
        /// composants afin de les initialiser individuellement.
        /// </summary>
        protected override void Initialize()
        {
            // Initialiser la vue de la cam�ra � la taille de l'�cran.
            this.camera = new Camera(new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height));

            // Activer le service de gestion de la cam�ra
            ServiceHelper.Game = this;

            // Activer le service de gestion de l'input. Essayer premi�rement
            // d'activer la premi�re manette, sinon le clavier.
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            if (gamepadState.IsConnected)
            {
              //  this.Components.Add(new ManetteService(this));
            }
            else
            {
                this.Components.Add(new ClavierService(this));
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent est invoqu�e une seule fois par partie et permet de
        /// charger tous vos composants.
        /// </summary>
        protected override void LoadContent()
        {


            // Cr�er un nouveau SpriteBatch, utilis�e pour dessiner les textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            // Cr�er une palette de tuiles pour la mappe.
            PaletteTuiles paletteDeTuiles = new PaletteTuiles(Content.Load<Texture2D>("Monde\\PalletteMonde1"), LargeurTuile, HauteurTuile);

            // Cr�er la mappe monde � partir de la palette de tuile et du tableau d'index de tuiles.
            this.mondeDeTuiles = new MondeTuiles(paletteDeTuiles, this.mappeMonde1);

            // Charger la palette de tuiles de d�tection de collisions
            PaletteTuiles tuilesCollisions = new PaletteTuiles(Content.Load<Texture2D>("Monde\\PalletteMonde1Collisions"), LargeurTuile, LargeurTuile);
            (this.mondeDeTuiles as MondeTuiles).PaletteCollisions = tuilesCollisions;

            // Configurer la cam�ra.
            this.camera.MondeRect = new Rectangle(0, 0, this.mondeDeTuiles.Largeur, this.mondeDeTuiles.Hauteur);

           // Charger le sprite repr�sentant le joueur.
            JoueurSprite.LoadContent(Content, this.graphics);

            // Cr�er et initialiser le sprite du joueur.
            this.joueur = new JoueurSprite(100, -32);
            this.joueur.BoundsRect = new Rectangle(0, 0, this.mondeDeTuiles.Largeur, this.mondeDeTuiles.Hauteur);

            // Imposer la palette de collisions au d�placement du joueur.
            this.joueur.GetValiderDeplacement = this.SpriteValiderDeplacement;
            this.joueur.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
        }

        /// <summary>
        /// UnloadContent est invoqu�e une seule fois par partie et permet de
        /// lib�rer vos composants.
        /// </summary>
        protected override void UnloadContent()
        {
            // � FAIRE: Lib�rez tout contenu de ContentManager ici
        }

        /// <summary>
        /// Permet d'implanter les comportements logiques du jeu tels que
        /// la mise � jour du monde, la d�tection de collisions, la lecture d'entr�es
        /// et les effets audio.
        /// </summary>
        /// <param name="gameTime">Fournie un instantan� du temps de jeu.</param>
        protected override void Update(GameTime gameTime)
        {
            // Permettre de quitter le jeu via la manette.
            if (ServiceHelper.Get<IInputService>().Quitter(1))
            {
                this.Exit();
            }

            // Mettre � jour le sprite du joueur puis centrer la camera sur celui-ci
            this.joueur.Update(gameTime, this.graphics);

            // Recentrer la cam�ra sur le sprite du joueur.
           this.camera.Centrer(this.joueur.Position);

            base.Update(gameTime);
        }

        /// <summary>
        /// Cette fonction membre est invoqu�e lorsque le jeu doit mettre � jour son 
        /// affichage.
        /// </summary>
        /// <param name="gameTime">Fournie un instantan� du temps de jeu.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Activer le blending alpha (pour la transparence des sprites)
            this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            // Afficher les tuiles d'arri�re-plan
            this.mondeDeTuiles.Draw(this.camera, this.spriteBatch);

            // Afficher le sprite du joueur
           this.joueur.Draw(this.camera, this.spriteBatch);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
