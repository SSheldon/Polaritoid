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
    public enum Enemy { Chaser, Smarty, Stander, Rover, Dual, Shooter, Layer };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TouchInputManager tI;

        SpriteFont font;
        Player player;
        EnemyList enemies;
        Dictionary<String, Texture2D> textures;
        int fieldWidth, fieldHeight;
        /// <summary>
        /// The position of the bottom-left corner of the view window.
        /// </summary>
        Vector2 viewCornerPosition;

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

            fieldWidth = 240;
            fieldHeight = 320;
            viewCornerPosition = Vector2.Zero;
            textures = new Dictionary<string, Texture2D>();

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
            textures.Add("player", Content.Load<Texture2D>("player"));
            textures.Add("chaser", Content.Load<Texture2D>("chaser"));
            textures.Add("smarty", Content.Load<Texture2D>("smarty"));
            textures.Add("stander", Content.Load<Texture2D>("stander"));
            textures.Add("rover", Content.Load<Texture2D>("rover"));
            textures.Add("dual", Content.Load<Texture2D>("dual"));
            textures.Add("shooter", Content.Load<Texture2D>("shooter"));
            textures.Add("layer", Content.Load<Texture2D>("layer"));
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
            enemies = new EnemyList(fieldWidth, fieldHeight, textures);
            player = new Player(new Vector2(40, 40), Polarity.Red, textures["player"], fieldWidth, fieldHeight);
            enemies.Spawn(Enemy.Chaser, new Vector2(80, 40), Polarity.Red);
            enemies.Spawn(Enemy.Smarty, new Vector2(80, 80), Polarity.Blue);
            enemies.Spawn(Enemy.Stander, new Vector2(40, 80), Polarity.Blue);
            enemies.Spawn(Enemy.Rover, new Vector2(80, 80), Polarity.Red);
            enemies.Spawn(Enemy.Dual, new Vector2(160, 160), Polarity.Blue);
            enemies.Spawn(Enemy.Shooter, new Vector2(160, 200), Polarity.Red);
            enemies.Spawn(Enemy.Layer, new Vector2(40, 200), Polarity.Blue);

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

            player.Update(gameTime, tI.TouchpadPosition, viewCornerPosition);
            if (enemies.UpdateEnemies(gameTime, player.position, player.polarity, viewCornerPosition))
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
            player.Draw(spriteBatch);
            enemies.DrawEnemies(spriteBatch);
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
    }
}
