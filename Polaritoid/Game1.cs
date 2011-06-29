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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;
        Field field;
        Dictionary<Type, Texture2D> textures;
        /// <summary>
        /// The position of the bottom-left corner of the view window.
        /// </summary>
        Vector2 viewCornerPosition;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 480;

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
            viewCornerPosition = Vector2.Zero;
            textures = new Dictionary<Type, Texture2D>();

            base.Initialize();
        }

        void PlayPress()
        {
            field.Player.polarity = field.Player.polarity == Polarity.Red ? Polarity.Blue : Polarity.Red;
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
            field = new Field(480, 800);
            field.Spawn(typeof(Player), new Vector2(40, 40), Polarity.Red);
            field.Spawn(typeof(Chaser), new Vector2(80, 40), Polarity.Red);
            field.Spawn(typeof(Smarty), new Vector2(80, 80), Polarity.Blue);
            field.Spawn(typeof(Stander), new Vector2(40, 80), Polarity.Blue);
            field.Spawn(typeof(Rover), new Vector2(80, 80), Polarity.Red);
            field.Spawn(typeof(Dual), new Vector2(160, 160), Polarity.Blue);
            field.Spawn(typeof(Shooter), new Vector2(160, 200), Polarity.Red);
            field.Spawn(typeof(Layer), new Vector2(40, 200), Polarity.Blue);

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

            TouchCollection touches = TouchPanel.GetState();
            if (touches.Count > 0)
            {
                field.Player.velocity = ScreenToField(touches[0].Position) - field.Player.position;
                field.Player.velocity.Normalize();
                field.Player.velocity *= 3F;
            }
            else field.Player.velocity = Vector2.Zero;

            if (field.Update(gameTime))
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
            foreach (Shape s in field)
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

        private Vector2 FieldToScreen(Vector2 v)
        {
            return new Vector2(v.X - viewCornerPosition.X,
                GraphicsDevice.Viewport.Height - v.Y + viewCornerPosition.Y);
        }

        private Vector2 ScreenToField(Vector2 v)
        {
            return new Vector2(v.X + viewCornerPosition.X,
                GraphicsDevice.Viewport.Height - v.Y + viewCornerPosition.Y);
        }

        /// <summary>
        /// Radius of sprites.
        /// </summary>
        private const int RAD = 16;

        protected Sprite GenerateSprite(Shape s)
        {
            return new Sprite(textures[s.GetType()],
                FieldToScreen(s.position),
                (s.polarity == Polarity.Blue ? Color.Blue : (s.polarity == Polarity.Red ? Color.Red : Color.Purple)),
                -s.Direction,
                new Vector2(RAD, RAD), (float)s.radius / RAD, 0F);
        }

        protected Sprite GenerateOtherSprite(Dual s)
        {
            return new Sprite(textures[typeof(Dual)],
                FieldToScreen(s.position),
                (s.polarity == Polarity.Blue ? Color.Red : (s.polarity == Polarity.Red ? Color.Blue : Color.Purple)),
                -s.Direction + MathHelper.Pi,
                new Vector2(RAD, RAD), (float)s.radius / RAD, 0F);
        }
    }
}
