using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polaritoid
{
    public class EnemyList : List<Shape>
    {
        private int fieldWidth, fieldHeight;
        Dictionary<String, Texture2D> textures;

        public EnemyList(int fieldWidth, int fieldHeight, Dictionary<String, Texture2D> textures)
            : base()
        {
            this.fieldWidth = fieldWidth;
            this.fieldHeight = fieldHeight;
            this.textures = textures;
        }

        public void Spawn(Enemy enemy, Vector2 position, Polarity polarity)
        {
            switch (enemy)
            {
                case Enemy.Chaser:
                    this.Add(new Chaser(position, polarity, textures["chaser"], fieldWidth, fieldHeight));
                    break;
                case Enemy.Dual:
                    this.Add(new Dual(position, polarity, textures["dual"], fieldWidth, fieldHeight));
                    break;
                case Enemy.Rover:
                    this.Add(new Rover(position, polarity, textures["rover"], fieldWidth, fieldHeight));
                    break;
                case Enemy.Smarty:
                    this.Add(new Smarty(position, polarity, textures["smarty"], fieldWidth, fieldHeight));
                    break;
                case Enemy.Stander:
                    this.Add(new Stander(position, polarity, textures["stander"], fieldWidth, fieldHeight));
                    break;
                case Enemy.Shooter:
                    this.Add(new Shooter(position, polarity, textures["shooter"], fieldWidth, fieldHeight));
                    break;
                case Enemy.Layer:
                    this.Add(new Layer(position, polarity, textures["layer"], fieldWidth, fieldHeight));
                    break;
            }
        }

        public void Spawn(Vector2 position, Polarity polarity, float direction)
        {
            this.Add(new Rover(position, direction, polarity, textures["rover"], fieldWidth, fieldHeight));
        }

        public bool UpdateEnemies(GameTime gameTime, Vector2 playerPosition, Polarity playerPolarity, Vector2 viewCornerPosition)
        {
            bool playerDead = false;
            foreach (Shape enemy in this)
            {
                enemy.Update(gameTime, playerPosition, playerPolarity, viewCornerPosition);
                if (enemy.CollisionCheck(gameTime, playerPosition, playerPolarity))
                {
                    //player is dead
                    playerDead = true;
                }
            }
            for (int counter = this.Count - 1; counter >= 0; counter--)
            {
                if (this[counter].dead) this.RemoveAt(counter);
            }
            //spawn
            foreach (Shape shooter in FindAll(new Predicate<Shape>(IsShooter)))
            {
                if (gameTime.TotalGameTime.Subtract((shooter as Shooter).lastShot.Value).Seconds > 2)
                {
                    Spawn(shooter.position, shooter.polarity, (shooter as Shooter).direction);
                    (shooter as Shooter).lastShot = gameTime.TotalGameTime;
                }
            }
            foreach (Shape layer in FindAll(new Predicate<Shape>(IsLayer)))
            {
                if (gameTime.TotalGameTime.Subtract((layer as Layer).lastMine.Value).Seconds > 2)
                {
                    Spawn(Enemy.Stander, layer.position, layer.polarity);
                    (layer as Layer).lastMine = gameTime.TotalGameTime;
                }
            }
            return playerDead;
        }

        public void DrawEnemies(SpriteBatch batch)
        {
            foreach (Shape enemy in this)
            {
                enemy.Draw(batch);
            }
        }

        private bool IsShooter(Shape shape)
        {
            return shape is Shooter;
        }

        private bool IsLayer(Shape shape)
        {
            return shape is Layer;
        }
    }
}