//-----------------------------------------------------------------------
// <copyright file="Sprite.cs" company="Marco Lavoie">
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
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Classe abstraite de base des sprites du jeu.
    /// </summary>
    public abstract class Sprite
    {
        /// <summary>
        /// Attribut stockant la position du centre du sprite.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Attribut statique contenant le rectangle confinant les mouvements du sprite.
        /// </summary>
        private Rectangle boundsRect;

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public Sprite(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Sprite(float x, float y)
        {
            this.Position = new Vector2(x, y);
        }

        /// <summary>
        /// Propriété abstraite pour manipuler la texture du sprite. Doit être
        /// surchangée dans les classes dérivées afin de manipuler une Texture2D.
        /// </summary>
        public abstract Texture2D Texture
        {
            get;
        }

        /// <summary>
        /// Accesseur de l'attribut privé position contrôlant la position centrale du
        /// sprite. Le mutateur s'assure que la position fournie est confinée aux bornes
        /// du monde (en fonction de l'attribut BoundsRect).
        /// </summary>
        public virtual Vector2 Position
        {
            get
            {
                return this.position;
            }

            // Le setter s'assure que la nouvelle position n'excède pas les bornes de mouvements
            // si elles sont fournies.
            set
            {
                this.position = value;

                // Limiter le mouvement si un boundsRect est fourni.
                this.ClampPositionToBoundsRect();
            }
        }

        /// <summary>
        /// Accesseur de l'attribut privé boundsRect contrôlant les bornes de positionnement
        /// du sprite. Le mutateur s'assure que la position courante du sprite est confinée
        /// aux nouvelles bornes du monde.
        /// </summary>
        public virtual Rectangle BoundsRect
        {
            get
            {
                return this.boundsRect;
            }

            // Le setter s'assurer que la position courante est confinée au nouvelles bornes.
            set
            {
                this.boundsRect = value;
                this.Position = this.position;       // exploiter le setter de _position 
            }
        }

        /// <summary>
        /// Accesseur surchargeable pour obtenir la largeur du sprite.
        /// </summary>
        public virtual int Width
        {
            get { return this.Texture.Width; }
        }

        /// <summary>
        /// Accesseur surchargeable pour obtenir la hauteur du sprite.
        /// </summary>
        public virtual int Height
        {
            get { return this.Texture.Height; }
        }

        /// <summary>
        /// Fonction membre abstraite (doit être surchargée) mettant à jour le sprite.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public abstract void Update(GameTime gameTime, GraphicsDeviceManager graphics);

        /// <summary>
        /// Affiche à l'écran le sprite en fonction de la position de la camera, si une est 
        /// fournie.
        /// </summary>
        /// <param name="camera">Caméra à exploiter pour l'affichage.</param>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public virtual void Draw(Camera camera, SpriteBatch spriteBatch)
        {
            // Comme l'attribut _position contient la position centrée du sprite mais
            // que Draw() considère la position fournie comme celle de l'origine du
            // sprite, il faut décaler _position en conséquence avant d'invoquer Draw().
            ForcerPosition(Position.X - (this.Width / 2), Position.Y - (this.Height / 2));

            // Créer destRect aux coordonnées du sprite dans le monde. À noter que
            // les dimensions de destRect sont constantes.
            Rectangle destRect = new Rectangle((int)Position.X, (int)Position.Y, this.Width, this.Height);

            // Afficher le sprite s'il est visible.
            if (camera == null)
            {
                // Afficher la texture.
                spriteBatch.Draw(this.Texture, destRect, Color.White);
            }
            else if (camera.EstVisible(destRect))
            {
                // Décaler la destination en fonction de la caméra. Ceci correspond à transformer destRect 
                // de coordonnées logiques (i.e. du monde) à des coordonnées physiques (i.e. de l'écran).
                camera.Monde2Camera(ref destRect);

                // Afficher la texture à l'écran.
                spriteBatch.Draw(this.Texture, destRect, Color.White);
            }

            // Remettre _position au centre du sprite.
            ForcerPosition(Position.X + (this.Width / 2), Position.Y + (this.Height / 2));
        }

        /// <summary>
        /// Affiche à l'écran le sprite. L'affichage est délégué à l'autre surcharge de Draw.
        /// </summary>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.Draw(null, spriteBatch);
        }

        /// <summary>Fonction restreignant _position à l'intérieur des limites fournies par boundsRect si
        /// de telles limites sont fournies.
        /// </summary>
        protected virtual void ClampPositionToBoundsRect()
        {
            // Limiter le mouvement si un boundsRect est fourni.
            if (!this.boundsRect.IsEmpty)
            {
                // On divise la taille du sprite par 2 car _position indique le centre du sprite.
                this.position.X = MathHelper.Clamp(this.position.X, this.BoundsRect.Left + this.Width / 2, this.BoundsRect.Right - (this.Width / 2));
                this.position.Y = MathHelper.Clamp(this.position.Y, this.BoundsRect.Top + this.Height / 2, this.BoundsRect.Bottom - (this.Height / 2));
            }
        }

        /// <summary>
        /// Cette fonction permet de forcer la position du sprite sans égard aux bornes
        /// de confinement du déplacement (i.e. sans égard à BoundsRect). Cette fonction
        /// membre est exploitée par cetraines classes dérivées pour contourner
        /// l'accesseur de l'attribut privé position.
        /// </summary>
        /// <param name="x">Coordonnée x de la position du sprite du joueur.</param>
        /// <param name="y">Coordonnée y de la position du sprite du joueur.</param>
        protected void ForcerPosition(float x, float y)
        {
            this.position.X = x;
            this.position.Y = y;
        }
    }
}
