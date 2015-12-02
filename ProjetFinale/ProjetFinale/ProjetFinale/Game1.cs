//-----------------------------------------------------------------------
// <copyright file="Game.cs" company="Marco Lavoie">
// Marco Lavoie, 2010. Tous droits réservés
// 
// L'utilisation de ce matériel pédagogique (présentations, code source 
// et autres) avec ou sans modifications, est permise en autant que les 
// conditions suivantes soient respectées:
//
// 1. La diffusion du matériel doit se limiter à un intranet dont l'accès
//    est imité aux étudiants inscrits à un cours exploitant le dit 
//    matériel. IL EST STRICTEMENT INTERDIT DE DIFFUSER CE MATÉRIEL 
//    LIBREMENT SUR INTERNET.
// 2. La redistribution des présentations contenues dans le matériel 
//    pédagogique est autorisée uniquement en format Acrobat PDF et sous
//    restrictions stipulées à la condition #1. Le code source contenu 
//    dans le matériel pédagogique peut cependant être redistribué sous 
//    sa forme  originale, en autant que la condition #1 soit également 
//    respectée.
// 3. Le matériel diffusé doit contenir intégralement la mention de 
//    droits d'auteurs ci-dessus, la notice présente ainsi que la
//    décharge ci-dessous.
// 
// CE MATÉRIEL PÉDAGOGIQUE EST DISTRIBUÉ "TEL QUEL" PAR L'AUTEUR, SANS 
// AUCUNE GARANTIE EXPLICITE OU IMPLICITE. L'AUTEUR NE PEUT EN AUCUNE 
// CIRCONSTANCE ÊTRE TENU RESPONSABLE DE DOMMAGES DIRECTS, INDIRECTS, 
// CIRCONSTENTIELS OU EXEMPLAIRES. TOUTE VIOLATION DE DROITS D'AUTEUR 
// OCCASIONNÉ PAR L'UTILISATION DE CE MATÉRIEL PÉDAGOGIQUE EST PRIS EN 
// CHARGE PAR L'UTILISATEUR DU DIT MATÉRIEL.
// 
// En utilisant ce matériel pédagogique, vous acceptez implicitement les
// conditions et la décharge exprimés ci-dessus.
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
        /// Attribut permettant d'obtenir des infos sur la carte graphique et l'écran.
        /// </summary>
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// Attribut gérant l'affichage en batch à l'écran.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Attribut représentant le monde de tuiles à afficherdurant le jeu.
        /// </summary>
        private Monde mondeDeTuiles;

        // Effet sonore contenant la chanson de fond du jeu.
        private SoundEffect chanson;

        // Instance de chanson de fond en cours de sonorisation durant le jeu.
        private static SoundEffectInstance chansonActive;

        /// <summary>
        /// Mappe monde : chaque valeur du tableau correspond à l'index d'une tuile dans mondeDeTuiles.
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
        /// Attribut représentant le personnage contrôlé par le joueur.
        /// </summary>
        private JoueurSprite joueur;

        /// <summary>
        /// Attribut représentant la camera.
        /// </summary>
        private Camera camera;

        /// <summary>
        /// Constructeur par défaut de la classe. Cette classe est générée automatiquement
        /// par Visual Studio lors de la création du projet.
        /// </summary>
        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Fonction retournant le niveau de résistance aux déplacements en fonction de la couleur du pixel de tuile
        /// à la position donnée.
        /// </summary>
        /// <param name="position">Position du pixel en coordonnées du monde.</param>
        /// <returns>Facteur de résistance entre 0.0f (aucune résistance) et 1.0f (résistance maximale).</returns>
        public float CalculerResistanceAuMouvement(Vector2 position)
        {
            Color pixColor = Color.Black;

            // Extraire la couleur du pixel correspondant à la position donnée dans privTuilesCollisions.
            try
            {
                pixColor = this.mondeDeTuiles.CouleurDeCollision(position);
            }
            catch (System.IndexOutOfRangeException)
            {
                this.Exit();
            }

            // Déterminer le niveau de résistance en fonction de la couleur
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
        /// Définition de fonction déléguée permettant de valider un déplacement d'une position
        /// à une autre dans le monde. La fonction retourne le point le plus près de 
        /// (posSource.X+deltaX, posSource.Y+DeltaY) jusqu'où le personnage peut se rendre horizontalement 
        /// et verticalement sans rencontrer de résistance plus élévée que la limite donnée.
        /// </summary>
        /// <param name="posSource">Position du pixel de départ du déplacement, en coordonnées du monde.</param>
        /// <param name="deltaX">Déplacement total horizontal, en coordonnées du monde.</param>
        /// <param name="deltaY">Déplacement total vertical, en coordonnées du monde.</param>
        /// <param name="resistanceMax">Résistance maximale tolérée lors du déplacement.</param>
        public void SpriteValiderDeplacement(Vector2 posSource, ref int deltaX, ref int deltaY, float resistanceMax)
        {
            Vector2 dest = new Vector2(posSource.X, posSource.Y);

            // Premièrement considérer le déplacement horizontal. Incrémenter la distance horizontale
            // de déplacement jusqu'à deltaX ou jusqu'à ce qu'une résistance supérieure à celle tolérée
            // soit rencontrée.
            while (dest.X != posSource.X + deltaX)
            {
                dest.X += Math.Sign(deltaX);        // incrémenter la distance horizontale

                // Vérifier la résistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.X -= Math.Sign(deltaX);    // reculer d'un pixel (validé à l'itération précédente)
                    break;
                }
            }

            // Maintenant considérer le déplacement vertical. Incrémenter la distance verticale
            // de déplacement jusqu'à deltaY ou jusqu'à ce qu'une résistance supérieure à celle tolérée
            // soit rencontrée.
            while (dest.Y != posSource.Y + deltaY)
            {
                dest.Y += Math.Sign(deltaY);        // incrémenter la distance horizontale

                // Vérifier la résistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.Y -= Math.Sign(deltaY);    // reculer d'un pixel (validé à l'itération précédente)
                    break;
                }
            }

            // Déterminer le déplacement maximal dans les deux directions
            deltaX = (int)(dest.X - posSource.X);
            deltaY = (int)(dest.Y - posSource.Y);
        }

        /// <summary>
        /// Permet au jeu d'effectuer toute initialisation avant de commencer à jouer.
        /// Cette fonction membre peut demander les services requis et charger tout contenu
        /// non graphique pertinent. L'invocation de base.Initialize() itèrera parmi les
        /// composants afin de les initialiser individuellement.
        /// </summary>
        protected override void Initialize()
        {
            // Initialiser la vue de la caméra à la taille de l'écran.
            this.camera = new Camera(new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height));

            // Activer le service de gestion de la caméra
            ServiceHelper.Game = this;

            // Activer le service de gestion de l'input. Essayer premièrement
            // d'activer la première manette, sinon le clavier.
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
        /// LoadContent est invoquée une seule fois par partie et permet de
        /// charger tous vos composants.
        /// </summary>
        protected override void LoadContent()
        {


            // Créer un nouveau SpriteBatch, utilisée pour dessiner les textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            // Créer une palette de tuiles pour la mappe.
            PaletteTuiles paletteDeTuiles = new PaletteTuiles(Content.Load<Texture2D>("Monde\\PalletteMonde1"), LargeurTuile, HauteurTuile);

            // Créer la mappe monde à partir de la palette de tuile et du tableau d'index de tuiles.
            this.mondeDeTuiles = new MondeTuiles(paletteDeTuiles, this.mappeMonde1);

            // Charger la palette de tuiles de détection de collisions
            PaletteTuiles tuilesCollisions = new PaletteTuiles(Content.Load<Texture2D>("Monde\\PalletteMonde1Collisions"), LargeurTuile, LargeurTuile);
            (this.mondeDeTuiles as MondeTuiles).PaletteCollisions = tuilesCollisions;

            // Configurer la caméra.
            this.camera.MondeRect = new Rectangle(0, 0, this.mondeDeTuiles.Largeur, this.mondeDeTuiles.Hauteur);

           // Charger le sprite représentant le joueur.
            JoueurSprite.LoadContent(Content, this.graphics);

            // Créer et initialiser le sprite du joueur.
            this.joueur = new JoueurSprite(100, -32);
            this.joueur.BoundsRect = new Rectangle(0, 0, this.mondeDeTuiles.Largeur, this.mondeDeTuiles.Hauteur);

            // Imposer la palette de collisions au déplacement du joueur.
            this.joueur.GetValiderDeplacement = this.SpriteValiderDeplacement;
            this.joueur.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
        }

        /// <summary>
        /// UnloadContent est invoquée une seule fois par partie et permet de
        /// libérer vos composants.
        /// </summary>
        protected override void UnloadContent()
        {
            // À FAIRE: Libérez tout contenu de ContentManager ici
        }

        /// <summary>
        /// Permet d'implanter les comportements logiques du jeu tels que
        /// la mise à jour du monde, la détection de collisions, la lecture d'entrées
        /// et les effets audio.
        /// </summary>
        /// <param name="gameTime">Fournie un instantané du temps de jeu.</param>
        protected override void Update(GameTime gameTime)
        {
            // Permettre de quitter le jeu via la manette.
            if (ServiceHelper.Get<IInputService>().Quitter(1))
            {
                this.Exit();
            }

            // Mettre à jour le sprite du joueur puis centrer la camera sur celui-ci
            this.joueur.Update(gameTime, this.graphics);

            // Recentrer la caméra sur le sprite du joueur.
           this.camera.Centrer(this.joueur.Position);

            base.Update(gameTime);
        }

        /// <summary>
        /// Cette fonction membre est invoquée lorsque le jeu doit mettre à jour son 
        /// affichage.
        /// </summary>
        /// <param name="gameTime">Fournie un instantané du temps de jeu.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Activer le blending alpha (pour la transparence des sprites)
            this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            // Afficher les tuiles d'arrière-plan
            this.mondeDeTuiles.Draw(this.camera, this.spriteBatch);

            // Afficher le sprite du joueur
           this.joueur.Draw(this.camera, this.spriteBatch);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
