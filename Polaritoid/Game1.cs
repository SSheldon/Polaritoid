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
using InputManagement;

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
        TouchInputManager tI;

        SpriteFont font;
        Texture2D playerTex, chaserTex, smartyTex, standerTex;
        Player player;
        List<Shape> shapes;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Zune.
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);

            tI = new TouchInputManager(this);
            this.Components.Add(tI);
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
            tI.TouchpadDeadZone = GamePadDeadZone.IndependentAxes;
            tI.TouchpadPressed += new InputEventHandler(PlayPress);

            shapes = new List<Shape>();

            base.Initialize();
        }

        void PlayPress()
        {
            player.polarity = player.polarity == Polarity.Red ? Polarity.Blue : Polarity.Red;
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
            playerTex = Content.Load<Texture2D>("player");
            chaserTex = Content.Load<Texture2D>("chaser");
            smartyTex = Content.Load<Texture2D>("smarty");
            standerTex = Content.Load<Texture2D>("stander");

            player = new Player(new Vector2(40, 40), Vector2.Zero, Polarity.Red, new Sprite(playerTex, new Vector2(40, 40), new Vector2(16, 16)));
            shapes.Add(player);
            shapes.Add(new Chaser(new Vector2(80, 40), Vector2.Zero, Polarity.Red, new Sprite(chaserTex, new Vector2(80, 40), new Vector2(16, 16))));
            shapes.Add(new Smarty(new Vector2(80, 80), Vector2.Zero, Polarity.Blue, new Sprite(smartyTex, new Vector2(80, 40), new Vector2(16, 16))));
            shapes.Add(new Stander(new Vector2(40, 80), Polarity.Blue, new Sprite(standerTex, new Vector2(40, 80), new Vector2(16, 16))));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            foreach (Shape shape in shapes)
            {
                shape.Update(gameTime, player.position, player.polarity, tI.TouchpadPosition);
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
            //spriteBatch.DrawString(font, shapes[1].position.ToString(), new Vector2(0, 0), Color.White);
            //spriteBatch.DrawString(font, shapes[1].velocity.ToString(), new Vector2(0, 20), Color.White);
            foreach (Shape shape in shapes)
            {
                shape.sprite.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
