using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Polaritoid
{
    public enum Polarity { Red, Blue, Both };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;
        Field enemies;
        Dictionary<Type, Texture2D> textures;
        int fieldWidth, fieldHeight;
        /// <summary>
        /// The position of the bottom-left corner of the view window.
        /// </summary>
        Vector2 viewCornerPosition;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            fieldWidth = 240;
            fieldHeight = 320;
            viewCornerPosition = Vector2.Zero;
            textures = new Dictionary<Type, Texture2D>();

            base.Initialize();
        }

        void PlayPress()
        {
            enemies.Player.polarity = enemies.Player.polarity == Polarity.Red ? Polarity.Blue : Polarity.Red;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("font");
            textures.Add(typeof(Player), Content.Load<Texture2D>("player"));
            textures.Add(typeof(Chaser), Content.Load<Texture2D>("chaser"));
            textures.Add(typeof(Smarty), Content.Load<Texture2D>("smarty"));
            textures.Add(typeof(Stander), Content.Load<Texture2D>("stander"));
            textures.Add(typeof(Rover), Content.Load<Texture2D>("rover"));
            textures.Add(typeof(Dual), Content.Load<Texture2D>("dual"));
            textures.Add(typeof(Shooter), Content.Load<Texture2D>("shooter"));
            textures.Add(typeof(Layer), Content.Load<Texture2D>("layer"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void BeginRun()
        {
            enemies = new Field(fieldWidth, fieldHeight);
            enemies.Spawn(typeof(Player), new Vector2(40, 40), Polarity.Red);
            enemies.Spawn(typeof(Chaser), new Vector2(80, 40), Polarity.Red);
            enemies.Spawn(typeof(Smarty), new Vector2(80, 80), Polarity.Blue);
            enemies.Spawn(typeof(Stander), new Vector2(40, 80), Polarity.Blue);
            enemies.Spawn(typeof(Rover), new Vector2(80, 80), Polarity.Red);
            enemies.Spawn(typeof(Dual), new Vector2(160, 160), Polarity.Blue);
            enemies.Spawn(typeof(Shooter), new Vector2(160, 200), Polarity.Red);
            enemies.Spawn(typeof(Layer), new Vector2(40, 200), Polarity.Blue);

            base.BeginRun();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (enemies.Update(gameTime))
            {
                //player is dead
                //this.Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (Shape s in enemies)
            {
                GenerateSprite(s).Draw(spriteBatch);
                if (s is Dual) GenerateOtherSprite((Dual)s).Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void DrawBoundaries(SpriteBatch batch)
        {
            //left bound from (-1, -1) to (-1, fieldHeight + 1)
            //top bound from (-1, fieldHeight + 1) to (fieldWidth + 1, fieldHeight + 1)
            //right bound from (fieldWidth + 1, fieldHeight + 1) to (fieldWidth + 1, -1)
            //bottom bound from (fieldWidth + 1, -1) to (-1, -1)
        }

        protected Sprite GenerateSprite(Shape s)
        {
            return new Sprite(textures[s.GetType()],
                new Vector2(s.position.X - viewCornerPosition.X, 320 - s.position.Y + viewCornerPosition.Y),
                (s.polarity == Polarity.Blue ? Color.Blue : (s.polarity == Polarity.Red ? Color.Red : Color.Purple)),
                VecOps.Direction(new Vector2(s.GetOrientation().X, -s.GetOrientation().Y)),
                new Vector2(16, 16), (float)s.radius / 16F, 0F);
        }

        protected Sprite GenerateOtherSprite(Dual s)
        {
            return new Sprite(textures[typeof(Dual)],
                new Vector2(s.position.X - viewCornerPosition.X, 320 - s.position.Y + viewCornerPosition.Y),
                (s.polarity == Polarity.Blue ? Color.Red : (s.polarity == Polarity.Red ? Color.Blue : Color.Purple)),
                VecOps.Direction(Vector2.Negate(new Vector2(s.GetOrientation().X, -s.GetOrientation().Y))),
                new Vector2(16, 16), (float)s.radius / 16F, 0F);
        }
    }
}
