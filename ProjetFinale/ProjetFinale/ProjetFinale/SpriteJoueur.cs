//-----------------------------------------------------------------------
// <copyright file="JoueurSprite.cs" company="Marco Lavoie">
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
    using System.Text;



    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;



    /// <summary>
    /// Définition de fonction déléguée permettant de calculer la résistance aux déplacements
    /// dans le monde à la position donnée.
    /// </summary>
    /// <param name="position">Position du pixel en coordonnées du monde.</param>
    /// <returns>Facteur de résistance entre 0.0f (aucune résistance) et 1.0f (résistance maximale).</returns>
    public delegate float ResistanceAuMouvement(Vector2 position);

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
    public delegate void ValiderDeplacement(Vector2 posSource, ref int deltaX, ref int deltaY, float resistanceMax);

    /// <summary>
    /// Classe implantant le sprite représentant le personnage contrôlé par le joueur du jeu. Ce sprite
    /// est constitué de séquences d'animation correspondant aux 2 directions de mouvement (gauche et droite). 
    /// </summary>
    public class JoueurSprite : SpriteAnimation
    {

        // Effet sonore joué lors du saut.
        private static SoundEffect effetSonoreSaut;

        /// <summary>
        /// Attribut statique (i.e. partagé par toutes les instances) constituant une 
        /// liste de palettes à exploiter selon la direction et l'état du joueur.
        /// </summary>
        private static List<PaletteTuiles> palettes = new List<PaletteTuiles>();

        /// <summary>
        /// Fonction déléguée permettant de valider les déplacements du sprite
        /// dans le monde de tuiles. Si aucune fonction déléguée n'est fournie, aucune
        /// résistance n'est appliquée aux déplacements.
        /// </summary>
        private ValiderDeplacement getValiderDeplacement;

        /// <summary>
        /// Fonction déléguée permettant d'obtenir la résistance aux déplacements du sprite
        /// dans le monde de tuiles. Si aucune fonction déléguée n'est fournie, aucune
        /// résistance n'est appliquée aux déplacements.
        /// </summary>
        private ResistanceAuMouvement getResistanceAuMouvement;

        /// <summary>
        /// Vitesse de marche du joueur, avec valeur par défaut.
        /// </summary>
        private float vitesseMarche = 0.1f;

        /// <summary>
        /// Vitesse verticale de déplacement, exploitée lors des sauts et lorsque le sprite tombre dans
        /// un trou.
        /// </summary>
        private float vitesseVerticale = 0.0f;

        /// <summary>
        /// Attribut indiquant la direction de déplacement courante.
        /// </summary>
        private Directions direction;

        /// <summary>
        /// Attribut indiquant la direction de déplacement courante.
        /// </summary>
        private Etats etat;

        /// <summary>
        /// Attribut indiquant l'index du périphérique contrôlant le sprite (voir
        /// dans Update (1 par défaut).
        /// </summary>
        private int indexPeripherique = 1;

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public JoueurSprite(float x, float y)
            : base(x, y)
        {
            // Par défaut, le sprite est celui faisant face au joueur.
            this.direction = Directions.Droite;
            this.etat = Etats.Stationnaire;
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public JoueurSprite(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Enumération des directions potentielles de déplacement du joueur.
        /// </summary>
        public enum Directions
        {
            /// <summary>
            /// Déplacement vers la droite de l'écran.
            /// </summary>
            Droite,

            /// <summary>
            /// Déplacement vers la gauche de l'écran.
            /// </summary>
            Gauche,

            /// <summary>
            /// Déplacement vers le haut de l'écran.
            /// </summary>
            Haut,

            /// <summary>
            /// Déplacement vers le bas de l'écran.
            /// </summary>
            Bas
        }

        /// <summary>
        /// Enumération des états disponibles du personnage.
        /// </summary>
        public enum Etats
        {
            /// <summary>
            /// Le personnage ne se déplace pas.
            /// </summary>
            Stationnaire,

            /// <summary>
            /// Le personnage se déplace en marchant.
            /// </summary>
            Marche,

            /// <summary>
            /// Le personnage saute.
            /// </summary>
            Saut,

            /// <summary>
            /// Le personnage monte l'echelle.
            /// </summary>
            Monte,

            /// <summary>
            /// Le personnage descend l'echelle.
            /// </summary>
            Descend,

            /// <summary>
            /// Le personnage Tire un obus.
            /// </summary>
            Tire
        }

        /// <summary>
        /// Accesseur pour attribut vitesseMarche.
        /// </summary>
        public float VitesseMarche
        {
            get { return this.vitesseMarche; }
            set { this.vitesseMarche = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour getValiderDeplacement) retournant ou changeant la fonction déléguée 
        /// de validation des déplacements.
        /// </summary>
        /// <value>Fonction de calcul de résistance aux déplacements.</value>
        public ValiderDeplacement GetValiderDeplacement
        {
            get { return this.getValiderDeplacement; }
            set { this.getValiderDeplacement = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour getResistanceAuMouvement) retournant ou changeant la fonction déléguée 
        /// de calcul de résistance aux déplacements.
        /// </summary>
        /// <value>Fonction de calcul de résistance aux déplacements.</value>
        public ResistanceAuMouvement GetResistanceAuMouvement
        {
            get { return this.getResistanceAuMouvement; }
            set { this.getResistanceAuMouvement = value; }
        }

        /// <summary>
        /// Propriété (accesseur de etat) retournant et modifiant l'état du joueur.
        /// </summary>
        public Etats Etat
        {
            get
            {
                return this.etat;
            }

            // Le setter modifie les attributs (hérités) d'animation du sprite afin que les tuiles d'animation
            // correspondant au nouvel état du joueur soient exploitées.
            set
            {             
                this.etat = value;
            }
        }

        /// <summary>
        /// Propriété (accesseur de lecture seulement) retournant la position des pattes du sprite.
        /// Cette position est utilisée pour déterminer si le sprite est debout sur une tuile solide.
        /// </summary>
        public Vector2 PositionPourCollisions
        {
            get
            {
                int dx = 0, dy = (Height / 2) - 1;

                // La position considérée est celle des pattes devant le personnage,
                // ce qui dépend de la direction de déplacement
                if (this.direction == Directions.Droite)
                {
                    dx += (Width / 2) - 1;
                }
                else if (this.direction == Directions.Gauche)
                {
                    dx -= (Width / 2) - 1;
                }

                return new Vector2(this.Position.X + dx, this.Position.Y + dy);
            }
        }

        /// <summary>
        /// Propriété indiquant l'index du périphérique contrôlant le sprite (1 à 4).
        /// </summary>
        public int IndexPeripherique
        {
            get { return this.indexPeripherique; }
            set { this.indexPeripherique = Math.Min(Math.Max(value, 0), 4); }
        }

        /// <summary>
        /// Surchargé afin de retourner la palette correspondant à la direction de 
        /// déplacement et l'état du joueur.
        /// </summary>
        protected override PaletteTuiles Palette
        {
            // Les palettes sont stockées dans la liste en groupes d'état (i.e.
            // 8 palettes de direction pour chaque état).
            get { return palettes[((int)this.etat * 2) + (int)this.direction]; }
        }

        /// <summary>
        /// Charge les images associées au sprite du joueur.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            // Puisque les palettes sont répertoriées selon l'état, on procède ainsi,
            // chargeant les huit palettes directionnelles un état à la fois.
            foreach (Etats etat in Enum.GetValues(typeof(Etats)))
            {
                // Déterminer le répertoire contenant les palettes selon l'état.
                string repertoire;
                switch (etat)
                {
                    case Etats.Marche:
                        repertoire = "Joueur\\Marche\\";
                        break;
                    case Etats.Monte:
                        repertoire = "Joueur\\Grimpe\\";
                        break;
                    case Etats.Tire:
                        repertoire = "Joueur\\Tire\\";
                        break;
                    case Etats.Saut:
                        repertoire = "Joueur\\Saut\\";
                        break;
                    default:
                        repertoire = "Joueur\\Stationnaire\\";
                        break;
                }

                // Charger les différentes palettes du personnage selon les directions.
                palettes.Add(new PaletteTuiles(content.Load<Texture2D>(repertoire + "terry_Right"), 50, 45));
                palettes.Add(new PaletteTuiles(content.Load<Texture2D>(repertoire + "terry_Left"), 50, 45));
            }
            // Charger les effets sonores.
            effetSonoreSaut = content.Load<SoundEffect>("Audio\\Waves\\boing");


        }

        /// <summary>
        /// Ajuste la position du sprite en fonction de l'input.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {

            // Calcul de la vitesse de marche du joueur (indépendante du matériel)
            float vitesseH = gameTime.ElapsedGameTime.Milliseconds * this.vitesseMarche;
            float vitesseV = 0.0f;

            // Obtenir les vitesses de déplacements (toutes entre 0.0 et 1.0) de l'input
            float vitesseD = ServiceHelper.Get<IInputService>().DeplacementDroite(this.indexPeripherique);
            float vitesseG = ServiceHelper.Get<IInputService>().DeplacementGauche(this.indexPeripherique);

            // Éviter les directions contradictoires
            if (vitesseD > 0.0)
            {
                vitesseG = 0.0f;
            }

            // Changer le sprite selon la direction 
            if (vitesseD > 0.0)
            {
                this.direction = Directions.Droite;
            }
            else if (vitesseG > 0.0)
            {
                this.direction = Directions.Gauche;
            }

            // Calculer le déplacement horizontal du sprite selon la direction indiquée. Notez que deux directions
            // opposées s'annulent
            int deltaX = 0;
            if (this.direction == Directions.Gauche)
            {
                deltaX = (int)(-vitesseH * vitesseG);
            }

            if (this.direction == Directions.Droite)
            {
                deltaX = (int)(vitesseH * vitesseD);
            }

            // Déterminer si le sprite doit sauter. Si c'est le cas, une vitesse verticale négative (i.e. vers le
            // haut) est initiée.
            if (ServiceHelper.Get<IInputService>().Sauter(this.indexPeripherique) && this.etat != Etats.Saut)
            {
                this.Etat = Etats.Saut;

                // Vitesse initiale vers le haut de l'écran
                this.vitesseVerticale = -1.3f;               
            }

            // Si le sprite est en état de saut, modifier graduellement sa vitesse verticale
            if (this.Etat == Etats.Saut)
            {
                this.vitesseVerticale += 0.098f;    // selon la constante de gravité (9.8 m/s2)
            }

            // Moduler la vitesse verticale en fonction du matériel
            vitesseV = gameTime.ElapsedGameTime.Milliseconds * this.vitesseVerticale;
            int deltaY = (int)vitesseV;

            // Si une fonction déléguée est fournie pour valider les mouvements sur les tuiles
            // y faire appel pour valider la position résultante du mouvement
            bool sautTermine = false;
            if (this.getValiderDeplacement != null && (deltaX != 0.0 || deltaY != 0.0))
            {
                // Déterminer le déplacement maximal permis vers la nouvelle position en fonction
                // de la résistance des tuiles. Une résistance maximale de 0.95 est indiquée afin de
                // permettre au sprite de traverser les tuiles n'étant pas complètement solides.
                Vector2 newPos = this.PositionPourCollisions;
                Vector2 newPos2 = this.PositionPourCollisions;
                Vector2 newPosHaut = this.PositionPourCollisions;
                Vector2 newPosMil = this.PositionPourCollisions;                                            ///////////Swaaaaag///////////
                newPosHaut.Y -= 30;
                newPosMil.Y -= 15;
                if (this.direction == Directions.Gauche)
                {
                    newPos.X = newPos.X + 18;
                    newPos2.X = newPos2.X + 27;
                    newPosHaut.X = newPosHaut.X + 18;
                    newPosMil.X = newPosMil.X + 18;
                }
                else if (this.direction == Directions.Droite)
                {
                    newPos.X = newPos.X -18;
                    newPos2.X = newPos2.X - 27;
                    newPosHaut.X = newPosHaut.X - 18;
                    newPosMil.X = newPosMil.X - 18;
                }
                //Ajoute un point de colission a l'arriere des pied.
                this.getValiderDeplacement(newPos2, ref deltaX, ref deltaY, 0.95f);
                //Ajoute un point de collisions a l'avant des pied.
                this.getValiderDeplacement(newPos, ref deltaX, ref deltaY, 0.95f);
                //Ajoute un point de collisions au dessus de la tete.                                                   ///////////AJOUTER////////////////////
                this.getValiderDeplacement(newPosHaut, ref deltaX, ref deltaY, 0.95f);
                //Ajoute un point de collisions a l'avant de la main.
                this.getValiderDeplacement(newPosMil, ref deltaX, ref deltaY, 0.95f);

                // Si aucun déplacement verticale n'est déterminé lors d'un saut (parce que le sprite 
                // a rencontré une tuile solide), indiquer que le saut est terminé.
                sautTermine = (this.Etat == Etats.Saut) && (deltaY == 0);
            }

            // Si un saut est terminé, annuler la vitesse verticale et changer l'état du sprite
            if (sautTermine)
            {
                this.Etat = Etats.Stationnaire;  // le prochain Update() le remettra en état
                // de marche au besoin
                this.vitesseVerticale = 0.0f;
            }

            // Vérifier si le sol est solide sous les pieds du sprite, sinon faire tomber ce dernier
            if (this.getResistanceAuMouvement != null && this.Etat != Etats.Saut)
            {
                // Déterminer les coordonnées de destination et tenant compte que le sprite est
                // centré sur Position, alors que ses mouvements doivent être autorisés en fonction
                // de la position de ses pieds.
                Vector2 newPos = this.PositionPourCollisions;
                Vector2 newPos2 = this.PositionPourCollisions;
                newPos.Y += 1;
                newPos2.Y += 1;
                if (this.direction == Directions.Gauche)
                {
                    newPos.X = newPos.X + 27;
                    newPos2.X = newPos2.X + 18;
                }
                else if (this.direction == Directions.Droite)
                {
                    newPos.X = newPos.X - 27;
                    newPos2.X = newPos2.X - 18;
                }


                // Calculer la résistance à la position du sprite.
                float resistance = this.getResistanceAuMouvement(newPos);
                float resistance2 = this.getResistanceAuMouvement(newPos2);                                 //////////////////////

                // Déterminer si le sol est solide à la position du sprite. Sinon activer l'état de
                // saut pour simuler la chute du sprite
                if (resistance < 0.95f && resistance2 < 0.95f)
                {
                    this.Etat = Etats.Saut;
                    this.vitesseVerticale = 0.0f;
                }
            }

            // Modifier la position et l'état du sprite en conséquence
            if (deltaX != 0 || deltaY != 0)
            {
                if (this.Etat == Etats.Stationnaire)
                {
                    this.Etat = Etats.Marche;
                }

                this.Position = new Vector2(this.Position.X + deltaX, this.Position.Y + deltaY);
            }
            else if (this.Etat != Etats.Saut)
            {
                this.Etat = Etats.Stationnaire;    // aucun mouvement: le joueur est stationnaire
            }

            // La fonction de base s'occupe de l'animation.
            base.Update(gameTime, graphics);
        }
    }
}
