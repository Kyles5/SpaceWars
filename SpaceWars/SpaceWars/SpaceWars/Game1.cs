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

namespace SpaceWars
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 

    /*
        Requirements:
     *  celebratory event/conclusion
     *  leaderboard
     *  audio/visual indicator of completeion
    */

    /* TODO:
     * enemies shoot back
     * user upgrades:
     *      missile speed
     *      missiles spread
     *      missile damager
     *      ship speed
     */
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle background;

        Texture2D backgroundTexture;
        Texture2D userShipTexture;
        Texture2D missileTexture;
        Texture2D enemyBasicTexture;
        Texture2D enemy2Texture;
        Texture2D enemy3Texture;

        SpriteFont font;

        Boolean isGameRunning;
        Boolean isUserHit;
        Boolean autofire;
        Boolean firstRun;
        Boolean[] levelComplete;

        String gameState;
        String name;
        String[] select;

        UserShip usership;

        FileReader file;

        List<EnemyShip> enemyShipList;
        List<Rock> rockList;
        List<Missile> missiles;
        List<Rectangle> backgroundParticles;
        List<String> highScoresFile;
        List<String[]> highScoresDisplay;

        TimeSpan missileLastTime;
        TimeSpan missileInterval;
        TimeSpan userShipFlashLastTime;
        TimeSpan userShipFlashInterval;
        TimeSpan flashTimeLastTime;
        TimeSpan flashTimeInterval;

        KeyboardState previousKeyboard;

        Random random = new Random();

        int userShipSpeed;
        int screenHeight;
        int screenWidth;
        int userMissileSpeed;
        int level;
        int enemyShipSpeed;
        int levelScore;
        int totalPoints;
        int lives;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferredBackBufferWidth = 600;
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
            background = new Rectangle(0, 0, 600, 500);
            isGameRunning = false;
            gameState = "start screen";
            usership = new UserShip(20, 280, 450, 20, 35);
            userShipSpeed = 3;
            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;
            userMissileSpeed = 3;
            enemyShipList = new List<EnemyShip>();
            rockList = new List<Rock>();
            missiles = new List<Missile>();
            missileLastTime = new TimeSpan();
            missileInterval = new TimeSpan(0, 0, 0, 0, 500);
            level = 0;
            enemyShipSpeed = 3;
            isUserHit = false;
            userShipFlashLastTime = new TimeSpan();
            userShipFlashInterval = new TimeSpan(0, 0, 0, 0, 100);
            flashTimeLastTime = new TimeSpan();
            flashTimeInterval = new TimeSpan(0, 0, 0, 3, 500);
            levelScore = 0;
            totalPoints = 0;
            levelComplete = new Boolean[5];
            select = new String[5];
            select[0] = "1";
            autofire = false;
            previousKeyboard = Keyboard.GetState();
            backgroundParticles = new List<Rectangle>();
            lives = 3;
            file = new FileReader();
            highScoresFile = new List<String>();
            highScoresDisplay = new List<String[]>();
            make_high_scores_list();
            firstRun = true;
            name = "";

            base.Initialize();
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
            backgroundTexture = this.Content.Load<Texture2D>("background");
            userShipTexture = this.Content.Load<Texture2D>("usership");
            missileTexture = this.Content.Load<Texture2D>("missile");
            enemyBasicTexture = this.Content.Load<Texture2D>("enemyBasic");
            font = this.Content.Load<SpriteFont>("font");
            enemy2Texture = this.Content.Load<Texture2D>("enemyTwo");
            enemy3Texture = this.Content.Load<Texture2D>("enemyThree");
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
            IsMouseVisible = false;
            KeyboardState keyboard = Keyboard.GetState();
            // Allows the game to exit
            if (keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            if (firstRun)
            {
                if (keyboard.IsKeyDown(Keys.Enter))
                {
                    firstRun = false;
                }
                else
                {
                    // Get user name
                }
            }
            else
            {
                if (isGameRunning)
                {
                    // User Ship movement (Does not move off screen)
                    if (keyboard.IsKeyDown(Keys.Left))
                    {
                        if (!(usership.get_user_ship().X < 0))
                        {
                            usership.move_left(userShipSpeed);
                        }
                    }
                    if (keyboard.IsKeyDown(Keys.Right))
                    {
                        if (usership.get_user_ship().X < screenWidth - usership.get_user_ship().Width)
                        {
                            usership.move_right(userShipSpeed);
                        }
                    }
                    if (keyboard.IsKeyDown(Keys.Up))
                    {
                        if (!(usership.get_user_ship().Y < 0))
                        {
                            usership.move_down(userShipSpeed);
                        }
                    }
                    if (keyboard.IsKeyDown(Keys.Down))
                    {
                        if (usership.get_user_ship().Y < screenHeight - usership.get_user_ship().Height)
                        {
                            usership.move_up(userShipSpeed);
                        }
                    }

                    // Fire a Missile (User Ship)
                    if (keyboard.IsKeyDown(Keys.Space) || autofire)
                    {
                        // Wait before adding missiles, so you dont just fire a continuous stream
                        if (gameTime.TotalGameTime - missileInterval >= missileLastTime)
                        {
                            missiles.Add(new Missile(10, userMissileSpeed, usership.get_user_ship().X + usership.get_user_ship().Width / 2, usership.get_user_ship().Y, 2, 15));
                            missileLastTime = gameTime.TotalGameTime;
                        }
                    }

                    // Turn on autofire
                    if (keyboard.IsKeyDown(Keys.A) && keyboard != previousKeyboard)
                    {
                        if (!autofire)
                        {
                            autofire = true;
                        }
                        else
                        {
                            autofire = false;
                        }
                    }

                    if (missiles.Count > 0)
                    {
                        for (int index = 0; index < missiles.Count; index++)
                        {
                            // Missile collides with enemy ship
                            //for (int index2 = 0; index2 < enemyShipList.Count; index2++)
                            //{
                            //    if (missiles[index].get_missile().Intersects(enemyShipList[index2].get_enemy_ship()))
                            //    {
                            //        switch (enemyShipList[index2].get_type())
                            //        {
                            //            case "basic":
                            //                levelScore += 10;
                            //                break;
                            //            default:
                            //                break;
                            //        }
                            //        enemyShipList[index2].decrease_health(missiles[index].get_collision_damage());
                            //        missiles.RemoveAt(index);
                            //        if (enemyShipList[index2].get_health() <= 0)
                            //        {
                            //            enemyShipList.RemoveAt(index2);
                            //        }
                            //    }
                            //}

                            // Remove the missiles from the list when they go off screen
                            if (missiles[index].get_missile().Y < 0 - missiles[index].get_missile().Y)
                            {
                                missiles.RemoveAt(index);
                            }
                        }
                    }
                    foreach (Missile m in missiles)
                    {
                        m.move();
                    }

                    for (int index = 0; index < missiles.Count; index++)
                    {
                        // Remove missiles when they hit enemy ships
                        foreach (EnemyShip e in enemyShipList)
                        {
                            if (missiles[index].get_missile().Intersects(e.get_enemy_ship()))
                            {
                                missiles[index].decrease_health(e.get_collision_damage());
                                e.decrease_health(missiles[index].get_collision_damage());
                            }
                        }
                        if (missiles[index].get_health() <= 0)
                        {
                            missiles.RemoveAt(index);
                        }
                    }

                    // Move all enemy ships down
                    foreach (EnemyShip e in enemyShipList)
                    {
                        e.move_down(enemyShipSpeed);
                    }

                    // Check if the enemy ships collide with a missile
                    for (int index = 0; index < enemyShipList.Count; index++)
                    {
                        if (enemyShipList[index].get_health() <= 0)
                        {
                            switch (enemyShipList[index].get_type())
                            {
                                case "basic":
                                    levelScore += 10;
                                    break;
                                case "level 2":
                                    levelScore += 15;
                                    break;
                                case "level 3":
                                    levelScore += 30;
                                    break;
                                default:
                                    break;
                            }
                            enemyShipList.RemoveAt(index);
                        }
                    }

                    // Remove enemy ships when they go off the screen
                    for (int index = 0; index < enemyShipList.Count; index++)
                    {
                        if (enemyShipList[index].get_enemy_ship().Y > screenHeight)
                        {
                            enemyShipList.RemoveAt(index);
                        }
                    }

                    // Check if enemy ship collides with user ship
                    for (int index = 0; index < enemyShipList.Count; index++)
                    {
                        if (enemyShipList[index].get_enemy_ship().Intersects(usership.get_user_ship()))
                        {
                            // usership.decrease_health(enemyShipList[index].get_collision_damage());
                            lives--;
                            enemyShipList.RemoveAt(index);
                            isUserHit = true;
                        }
                    }

                    // Check if the user ship has 0 health, and end game if it does
                    if (/*usership.get_health()*/lives <= 0)
                    {
                        //for (int index = 0; index < enemyShipList.Count; index++)
                        //{
                        //    enemyShipList.RemoveAt(index);
                        //}
                        isGameRunning = false;
                        gameState = "dead screen";
                    }

                    // Check if the level is over (All the enemies have passed)
                    if (enemyShipList.Count <= 0)
                    {
                        isGameRunning = false;
                        if (level == 5)
                        {

                        }
                        else
                        {
                            gameState = "level complete";
                        }
                        totalPoints += levelScore;
                        switch (level)
                        {
                            case 1:
                                levelComplete[level - 1] = true;
                                break;
                            case 2:
                                levelComplete[level - 1] = true;
                                break;
                            case 3:
                                levelComplete[level - 1] = true;
                                break;
                            case 4:
                                levelComplete[level - 1] = true;
                                break;
                            case 5:
                                levelComplete[level - 1] = true;
                                break;
                            default:
                                break;
                        }
                    }

                    // Move background
                    for (int index = 0; index < backgroundParticles.Count; index++)
                    {
                        //backgroundParticles[index].Y--;
                    }
                }
                else
                {
                    switch (gameState)
                    {
                        case "start screen":
                            initialize_background_particles();
                            if (levelComplete[0])
                            {
                                select[0] = "1*";
                                select[1] = "2";
                            }
                            if (levelComplete[1])
                            {
                                select[1] = "2*";
                                select[2] = "3";
                            }
                            if (levelComplete[2])
                            {
                                select[2] = "3*";
                                select[3] = "4";
                            }
                            if (levelComplete[3])
                            {
                                select[3] = "4*";
                                select[4] = "5";
                            }
                            if (levelComplete[4])
                            {
                                select[4] = "5*";
                            }

                            levelScore = 0;
                            // Go to level 1
                            if (keyboard.IsKeyDown(Keys.NumPad1))
                            {
                                level_one();
                                isGameRunning = true;
                                gameState = "resume";
                                usership = new UserShip(20, 280, 450, 20, 35);
                            }
                            // Go to level 2
                            if (keyboard.IsKeyDown(Keys.NumPad2))
                            {
                                if (levelComplete[0])
                                {
                                    level_two();
                                    isGameRunning = true;
                                    gameState = "resume";
                                    usership = new UserShip(20, 280, 450, 20, 35);
                                }
                            }
                            // Go to level 3
                            if (keyboard.IsKeyDown(Keys.NumPad3))
                            {
                                if (levelComplete[1])
                                {
                                    level_three();
                                    isGameRunning = true;
                                    gameState = "resume";
                                    usership = new UserShip(20, 280, 450, 20, 35);
                                }
                            }
                            // Go to level 4
                            if (keyboard.IsKeyDown(Keys.NumPad4))
                            {
                                if (levelComplete[2])
                                {
                                    level_four();
                                    isGameRunning = true;
                                    gameState = "resume";
                                    usership = new UserShip(20, 280, 450, 20, 35);
                                }
                            }
                            // Go to level 5
                            if (keyboard.IsKeyDown(Keys.NumPad5))
                            {
                                if (levelComplete[3])
                                {
                                    level_five();
                                    isGameRunning = true;
                                    gameState = "resume";
                                    usership = new UserShip(20, 280, 450, 20, 35);
                                }
                            }
                            // Go to Control screen
                            if (keyboard.IsKeyDown(Keys.C))
                            {
                                gameState = "control screen";
                            }
                            // Go to upgrade screen
                            if (keyboard.IsKeyDown(Keys.U))
                            {
                                gameState = "upgrade";
                            }
                            // Go to leaderboard
                            if (keyboard.IsKeyDown(Keys.L))
                            {
                                gameState = "leaderboard";
                            }
                            break;
                        case "dead screen":
                            if (keyboard.IsKeyDown(Keys.S))
                            {
                                gameState = "start screen";
                            }
                            break;
                        case "level complete":
                            if (keyboard.IsKeyDown(Keys.S))
                            {
                                gameState = "start screen";
                            }
                            break;
                        case "control screen":
                            if (keyboard.IsKeyDown(Keys.Back))
                            {
                                gameState = "start screen";
                            }
                            break;
                        case "upgrade":
                            if (keyboard.IsKeyDown(Keys.Back))
                            {
                                gameState = "start screen";
                            }
                            break;
                        case "leaderboard":
                            if (keyboard.IsKeyDown(Keys.Back))
                            {
                                gameState = "start screen";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            previousKeyboard = keyboard;
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
            if (firstRun)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Please enter your name for the leaderboards", new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(font, "Press the Enter key to confirm", new Vector2(10, 50), Color.White);
                spriteBatch.DrawString(font, name, new Vector2(10, 90), Color.White);
                spriteBatch.End();
            }
            else
            {
                if (isGameRunning)
                {
                    //switch (gameState)
                    //{
                    //    case "resume":
                            spriteBatch.Begin();
                            spriteBatch.DrawString(font, "score: " + levelScore, new Vector2(10, 10), Color.White);
                            spriteBatch.DrawString(font, "Lives: " + lives, new Vector2(10, 50), Color.White);
                            foreach (EnemyShip e in enemyShipList)
                            {
                                switch (e.get_type())
                                {
                                    case "basic":
                                        spriteBatch.Draw(enemyBasicTexture, e.get_enemy_ship(), Color.White);
                                        break;
                                    case "level 2":
                                        spriteBatch.Draw(enemy2Texture, e.get_enemy_ship(), Color.White);
                                        break;
                                    case "level 3":
                                        spriteBatch.Draw(enemy3Texture, e.get_enemy_ship(), Color.White);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (isUserHit)
                            {
                                if (gameTime.TotalGameTime - flashTimeInterval >= flashTimeLastTime)
                                {
                                    isUserHit = false;
                                    flashTimeLastTime = gameTime.TotalGameTime;
                                }
                                else
                                {
                                    if (gameTime.TotalGameTime - userShipFlashInterval >= userShipFlashLastTime)
                                    {
                                        spriteBatch.Draw(userShipTexture, usership.get_user_ship(), Color.White);
                                        userShipFlashLastTime = gameTime.TotalGameTime;
                                    }
                                }
                            }
                            else
                            {
                                spriteBatch.Draw(userShipTexture, usership.get_user_ship(), Color.White);
                            }
                            spriteBatch.End();
                            //if (isMissileFiring)
                            //{
                            foreach (Missile m in missiles)
                            {
                                spriteBatch.Begin();
                                spriteBatch.Draw(missileTexture, m.get_missile(), Color.White);
                                spriteBatch.End();
                            }
                            //}
                    //        break;
                    //    case "pause":
                    //        break;
                    //    default:
                    //        break;
                    //}
                }
                else
                {
                    switch (gameState)
                    {
                        case "start screen":
                            spriteBatch.Begin();
                            spriteBatch.DrawString(font, "Please type the number of the level you wish to enter", new Vector2(10, 10), Color.White);
                            spriteBatch.DrawString(font, "A '*' indicates you completed that level once", new Vector2(10, 50), Color.White);
                            spriteBatch.DrawString(font, select[0] + "     " + select[1] + "     " + select[2] + "     " + select[3] + "     " + select[4], new Vector2(10, 90), Color.White);
                            spriteBatch.DrawString(font, "C - controls", new Vector2(10, 130), Color.White);
                            spriteBatch.DrawString(font, "U - upgrade shop", new Vector2(10, 170), Color.White);
                            spriteBatch.DrawString(font, "L - Leaderboards", new Vector2(10, 210), Color.White);
                            spriteBatch.End();
                            break;
                        case "dead screen":
                            spriteBatch.Begin();
                            spriteBatch.DrawString(font, "You have died! Press 'S' to return to home screen", new Vector2(10, 10), Color.White);
                            spriteBatch.End();
                            break;
                        case "level complete":
                            spriteBatch.Begin();
                            spriteBatch.DrawString(font, "Level complete! Press 'S' to return to home screen", new Vector2(10, 10), Color.White);
                            spriteBatch.DrawString(font, "Points earned: " + levelScore, new Vector2(10, 50), Color.White);
                            spriteBatch.DrawString(font, "You now have " + totalPoints + " points in total", new Vector2(10, 90), Color.White);
                            spriteBatch.End();
                            break;
                        case "control screen":
                            spriteBatch.Begin();
                            spriteBatch.DrawString(font, "Controls", new Vector2(10, 10), Color.White);
                            spriteBatch.DrawString(font, "Left Arrow - Move left", new Vector2(10, 50), Color.White);
                            spriteBatch.DrawString(font, "Right Arrow - Move Right", new Vector2(10, 90), Color.White);
                            spriteBatch.DrawString(font, "Up Arrow - Move Up", new Vector2(10, 130), Color.White);
                            spriteBatch.DrawString(font, "Down Arrow - Move Down", new Vector2(10, 170), Color.White);
                            spriteBatch.DrawString(font, "Space - Fire Missile", new Vector2(10, 210), Color.White);
                            spriteBatch.DrawString(font, "A - Autofire", new Vector2(10, 250), Color.White);
                            spriteBatch.End();
                            break;
                        case "upgrade":
                            spriteBatch.Begin();
                            spriteBatch.DrawString(font, "Upgrade Shop", new Vector2(10, 10), Color.White);
                            spriteBatch.DrawString(font, "You have " + totalPoints + " points to spend", new Vector2(10, 50), Color.White);
                            spriteBatch.End();
                            break;
                        case "leaderboard":
                            spriteBatch.Begin();
                            spriteBatch.DrawString(font, "Top 5 Leaderboard", new Vector2(10, 10), Color.White);
                            spriteBatch.DrawString(font, "Name: " + highScoresDisplay[0][0] + ", Score: " + highScoresDisplay[0][1], new Vector2(10, 50), Color.White);
                            for (int index = 1; index < 5; index++)
                            {
                                if (index < highScoresDisplay.Count)
                                {
                                    spriteBatch.DrawString(font, "Name: " + highScoresDisplay[index][0] + ", Score: " + highScoresDisplay[index][1], new Vector2(10, (50 + (40 * index))), Color.White);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            spriteBatch.End();
                            break;
                        default:
                            break;
                    }
                }
            }

            base.Draw(gameTime);
        }

        public void level_one()
        {
            level = 1;
            reset();
            // 40 basic enemies
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, -400, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[0].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 50, enemyShipList[1].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[2].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 200, enemyShipList[3].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[4].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[5].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 350, enemyShipList[6].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[7].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 150, enemyShipList[8].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 250, enemyShipList[9].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 150, enemyShipList[10].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[11].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[12].get_enemy_ship().Y - 75, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[13].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 280, enemyShipList[14].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 30, enemyShipList[15].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 10, enemyShipList[16].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 15, enemyShipList[17].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 120, enemyShipList[18].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[19].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 550, enemyShipList[20].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 550, enemyShipList[21].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[22].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 120, enemyShipList[23].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 15, enemyShipList[24].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 10, enemyShipList[25].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 30, enemyShipList[26].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 280, enemyShipList[27].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[28].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[29].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[30].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 150, enemyShipList[31].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 250, enemyShipList[32].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 150, enemyShipList[33].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[34].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 350, enemyShipList[35].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[36].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[37].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 200, enemyShipList[38].get_enemy_ship().Y - 115, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[39].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
        }

        public void level_two()
        {
            level = 2;
            reset();
            // 30 level 2 enemies
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, -400, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[0].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 50, enemyShipList[1].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[2].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 200, enemyShipList[3].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[4].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[5].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 350, enemyShipList[6].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 300, enemyShipList[7].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 150, enemyShipList[8].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 250, enemyShipList[9].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 150, enemyShipList[10].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[11].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[12].get_enemy_ship().Y - 75, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 300, enemyShipList[13].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 280, enemyShipList[14].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 30, enemyShipList[15].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 10, enemyShipList[16].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 15, enemyShipList[17].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 120, enemyShipList[18].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[19].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 550, enemyShipList[20].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 550, enemyShipList[21].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[22].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 120, enemyShipList[23].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 15, enemyShipList[24].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 10, enemyShipList[25].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 30, enemyShipList[26].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 280, enemyShipList[27].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 300, enemyShipList[28].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[29].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
        }

        public void level_three()
        {
            level = 3;
            reset();
            // Combination of level 1 and 2
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, -400, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[0].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 50, enemyShipList[1].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[2].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 200, enemyShipList[3].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[4].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[5].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 350, enemyShipList[6].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[7].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 150, enemyShipList[8].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 250, enemyShipList[9].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 150, enemyShipList[10].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[11].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[12].get_enemy_ship().Y - 75, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[13].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 280, enemyShipList[14].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 30, enemyShipList[15].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 10, enemyShipList[16].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 15, enemyShipList[17].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 120, enemyShipList[18].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[19].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 550, enemyShipList[20].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 550, enemyShipList[21].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[22].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 120, enemyShipList[23].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 15, enemyShipList[24].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 10, enemyShipList[25].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 30, enemyShipList[26].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 280, enemyShipList[27].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[28].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[29].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, 0, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[0].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 50, enemyShipList[1].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[2].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 200, enemyShipList[3].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[4].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[5].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 350, enemyShipList[6].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 300, enemyShipList[7].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 150, enemyShipList[8].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 250, enemyShipList[9].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 150, enemyShipList[10].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[11].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[12].get_enemy_ship().Y - 75, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 300, enemyShipList[13].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 280, enemyShipList[14].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 30, enemyShipList[15].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 10, enemyShipList[16].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 15, enemyShipList[17].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 120, enemyShipList[18].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[19].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 550, enemyShipList[20].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 550, enemyShipList[21].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[22].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 120, enemyShipList[23].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 15, enemyShipList[24].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 10, enemyShipList[25].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 30, enemyShipList[26].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 280, enemyShipList[27].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 300, enemyShipList[28].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[29].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
        }

        public void level_four()
        {
            level = 4;
            reset();
            // 40 level 3
            // 20 level 2
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", (screenWidth / 2) - usership.get_user_ship().Width, -400, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[0].get_enemy_ship().X - 35, enemyShipList[0].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[0].get_enemy_ship().X + 35, enemyShipList[0].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[1].get_enemy_ship().X - 35, enemyShipList[2].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[2].get_enemy_ship().X + 35, enemyShipList[2].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[4].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[5].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 350, enemyShipList[6].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[7].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", ((screenWidth / 2) / 2) - usership.get_user_ship().Width, enemyShipList[8].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[9].get_enemy_ship().X - 35, enemyShipList[9].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[9].get_enemy_ship().X + 35, enemyShipList[9].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[10].get_enemy_ship().X - 35, enemyShipList[10].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[11].get_enemy_ship().X + 35, enemyShipList[11].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[13].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", (screenWidth / 2) + ((screenWidth / 2) / 2) - usership.get_user_ship().Width, enemyShipList[14].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[15].get_enemy_ship().X - 35, enemyShipList[15].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[15].get_enemy_ship().X + 35, enemyShipList[15].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[16].get_enemy_ship().X - 35, enemyShipList[16].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[17].get_enemy_ship().X + 35, enemyShipList[17].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            // level 2
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[19].get_enemy_ship().Y - 155, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[20].get_enemy_ship().Y, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[21].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[22].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[23].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[24].get_enemy_ship().Y, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[25].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[26].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[27].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[28].get_enemy_ship().Y, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[29].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[30].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[31].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[32].get_enemy_ship().Y, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[33].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[34].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[35].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[36].get_enemy_ship().Y, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[37].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[38].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            // level 3, 2, and basic at random
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 150, enemyShipList[39].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 250, enemyShipList[40].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 150, enemyShipList[41].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[42].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 20, enemyShipList[43].get_enemy_ship().Y - 75, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[44].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 280, enemyShipList[45].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 30, enemyShipList[46].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 10, enemyShipList[47].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 15, enemyShipList[48].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 120, enemyShipList[49].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[50].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 550, enemyShipList[51].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 550, enemyShipList[52].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[53].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 120, enemyShipList[54].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 15, enemyShipList[55].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 10, enemyShipList[56].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 30, enemyShipList[57].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 280, enemyShipList[58].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 300, enemyShipList[59].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[60].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
        }

        public void level_five()
        {
            level = 5;
            reset();

            // Combination of all four levels
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, -400, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[0].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 50, enemyShipList[1].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[2].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 200, enemyShipList[3].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[4].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[5].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 350, enemyShipList[6].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[7].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 150, enemyShipList[8].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 250, enemyShipList[9].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 150, enemyShipList[10].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[11].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[12].get_enemy_ship().Y - 75, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[13].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 280, enemyShipList[14].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 30, enemyShipList[15].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 10, enemyShipList[16].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 15, enemyShipList[17].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 120, enemyShipList[18].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[19].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 550, enemyShipList[20].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 550, enemyShipList[21].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[22].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 120, enemyShipList[23].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 15, enemyShipList[24].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 10, enemyShipList[25].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 30, enemyShipList[26].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 280, enemyShipList[27].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[28].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[29].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));

            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[30].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[31].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 50, enemyShipList[32].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[33].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 200, enemyShipList[34].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[35].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[36].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 350, enemyShipList[37].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[38].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 150, enemyShipList[39].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 250, enemyShipList[40].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 150, enemyShipList[41].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[42].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[43].get_enemy_ship().Y - 75, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[44].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 280, enemyShipList[45].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 30, enemyShipList[46].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 10, enemyShipList[47].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 15, enemyShipList[48].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 120, enemyShipList[49].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[50].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 550, enemyShipList[51].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 550, enemyShipList[52].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[53].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 120, enemyShipList[54].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 15, enemyShipList[55].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 10, enemyShipList[56].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 30, enemyShipList[57].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 280, enemyShipList[58].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[59].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[60].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[61].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 150, enemyShipList[62].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 250, enemyShipList[63].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 150, enemyShipList[64].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[65].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 350, enemyShipList[66].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[67].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[68].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 200, enemyShipList[69].get_enemy_ship().Y - 115, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[70].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));

            enemyShipList.Add(new EnemyShip(30, 15, "level 3", (screenWidth / 2) - usership.get_user_ship().Width, enemyShipList[71].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[72].get_enemy_ship().X - 35, enemyShipList[72].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[72].get_enemy_ship().X + 35, enemyShipList[72].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[73].get_enemy_ship().X - 35, enemyShipList[73].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[74].get_enemy_ship().X + 35, enemyShipList[74].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 20, enemyShipList[76].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[77].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 350, enemyShipList[78].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[79].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", ((screenWidth / 2) / 2) - usership.get_user_ship().Width, enemyShipList[80].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[81].get_enemy_ship().X - 35, enemyShipList[81].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[81].get_enemy_ship().X + 35, enemyShipList[81].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[82].get_enemy_ship().X - 35, enemyShipList[82].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[83].get_enemy_ship().X + 35, enemyShipList[83].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[85].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", (screenWidth / 2) + ((screenWidth / 2) / 2) - usership.get_user_ship().Width, enemyShipList[86].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[87].get_enemy_ship().X - 35, enemyShipList[87].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[87].get_enemy_ship().X + 35, enemyShipList[87].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[88].get_enemy_ship().X - 35, enemyShipList[88].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", enemyShipList[89].get_enemy_ship().X + 35, enemyShipList[89].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            // level 2
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[91].get_enemy_ship().Y - 155, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[92].get_enemy_ship().Y, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[93].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[94].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[95].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[96].get_enemy_ship().Y, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[97].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[98].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[99].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[100].get_enemy_ship().Y, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[101].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[102].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[103].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[104].get_enemy_ship().Y, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[105].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[106].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[107].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[108].get_enemy_ship().Y, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 40, enemyShipList[109].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 50), enemyShipList[110].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            // level 3, 2, and basic at random
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 150, enemyShipList[111].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 250, enemyShipList[112].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 150, enemyShipList[113].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[114].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 20, enemyShipList[115].get_enemy_ship().Y - 75, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 300, enemyShipList[116].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 280, enemyShipList[117].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 30, enemyShipList[118].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 10, enemyShipList[119].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 15, enemyShipList[120].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 120, enemyShipList[121].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 400, enemyShipList[122].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 550, enemyShipList[123].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 550, enemyShipList[124].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[125].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 120, enemyShipList[126].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 15, enemyShipList[127].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 10, enemyShipList[128].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 30, enemyShipList[129].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(10, 5, "basic", 280, enemyShipList[130].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(30, 15, "level 3", 300, enemyShipList[131].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[132].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));

            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[133].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[134].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 50, enemyShipList[135].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[136].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 200, enemyShipList[137].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[138].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[139].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 350, enemyShipList[140].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 300, enemyShipList[141].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 150, enemyShipList[142].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 250, enemyShipList[143].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 150, enemyShipList[144].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", screenWidth - (usership.get_user_ship().Width + 30), enemyShipList[145].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[146].get_enemy_ship().Y - 75, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 300, enemyShipList[147].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 280, enemyShipList[148].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 30, enemyShipList[149].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 10, enemyShipList[150].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 15, enemyShipList[151].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 120, enemyShipList[152].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[153].get_enemy_ship().Y - 45, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 550, enemyShipList[154].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 550, enemyShipList[155].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 400, enemyShipList[156].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 120, enemyShipList[157].get_enemy_ship().Y - 95, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 15, enemyShipList[158].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 10, enemyShipList[159].get_enemy_ship().Y - 65, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 30, enemyShipList[160].get_enemy_ship().Y - 35, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 280, enemyShipList[161].get_enemy_ship().Y - 55, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 300, enemyShipList[162].get_enemy_ship().Y - 85, usership.get_user_ship().Width, usership.get_user_ship().Height));
            enemyShipList.Add(new EnemyShip(20, 10, "level 2", 20, enemyShipList[163].get_enemy_ship().Y - 105, usership.get_user_ship().Width, usership.get_user_ship().Height));
        }

        public void initialize_background_particles()
        {
            backgroundParticles.Add(new Rectangle(random.Next(screenWidth), 0, 1, 6));
            for (int index = 0; index < 100; index++)
            {
                backgroundParticles.Add(new Rectangle(random.Next(screenWidth), backgroundParticles[index].Y - 10, 1, 6));
            }
        }

        public void make_high_scores_list()
        {
            char[] splitChars = new char[1];
            splitChars[0] = ',';
            String[] addToList = new String[2];
            highScoresFile = file.read();
            for (int index = 0; index < highScoresFile.Count; index++)
            {
                addToList = highScoresFile[index].Split(splitChars);
                highScoresDisplay.Add(addToList);
            }
            sort_high_scores_display();
        }

        public void sort_high_scores_display()
        {
            int maxIndex = 0;
            String[] temp = new String[2];

            for (int index = 0; index < highScoresDisplay.Count; index++)
            {
                maxIndex = index;
                for (int index2 = index + 1; index2 < highScoresDisplay.Count(); index2++)
                {
                    if (Int32.Parse(highScoresDisplay[index2][1]) > Int32.Parse(highScoresDisplay[maxIndex][1]))
                    {
                        maxIndex = index2;
                    }
                }
                if (maxIndex != index)
                {
                    temp = highScoresDisplay[index];
                    highScoresDisplay[index] = highScoresDisplay[maxIndex];
                    highScoresDisplay[maxIndex] = temp;
                }
            }
        }

        public void reset()
        {
            isUserHit = false;
            lives = 3;
            while (enemyShipList.Count > 0)
            {
                int index = 0;
                enemyShipList.RemoveAt(index);
            }
            for (int index = 0; index < missiles.Count; index++)
            {
                missiles.RemoveAt(index);
            }
        }
    }
}
