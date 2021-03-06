﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Media;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Media;

namespace The_Wee_Mad_Yin
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont info;
        Vector2 info_position;

        Camera player_camera;

        sprite background_main;

        sprite background_gameover;

        sprite2 player_sprite;

        sprite bottled_juice;

        sprite[] backgrounds = new sprite[7];

        sprite[] buttons = new sprite[4];

        sprite[] option_buttons = new sprite[3];

        sprite back_button;

        sprite restart_button;

        Song grass_music;
        Song main_music;
        Song cave_music;
        Song city_music;
        SoundEffect button_forward;
        SoundEffect button_back;
        SoundEffect bounce;
        SoundEffect taking_damage;
        SoundEffect drinking;
        SoundEffect eating;
        SoundEffect gameover_sound;

        List<Haggis> haggises = new List<Haggis>();
        List<Thistle> thistles = new List<Thistle>();
        List<Eagle> eagles = new List<Eagle>();
        List<Shortbread> shortbreads = new List<Shortbread>();
        List<Block> blocks = new List<Block>();

        Boolean keyboardreleased = true;
        KeyboardState keys;                             // Variable to hold keyboard state
        KeyboardState lastkeystate;

        const int numberofhighscores = 10;                              // Number of high scores to store
        int[] highscores = new int[numberofhighscores];                 // Array of high scores
        string[] highscorenames = new string[numberofhighscores];       // Array of high score names
        const int maxnamelength = 7;   // Maximum name length for high score table
        int lasthighscore = numberofhighscores - 1;

        float keycounter = 0;           // Counter for delay between key strokes
        const float keystrokedelay = 200;   // Delay between key strokes in milliseconds

        int screen_width = 800;
        int screen_height = 600;

        int level_number = 0;
        public int current_time;

        int defaultlives = 3;
        int lives = 3;
        int score = 0;

        const int button_timer = 500;
        int button_timercount = 0;
        const int life_cooldown = 2000;
        int current_lcooldown = 0;

        bool menu = true;
        bool leaderboard = false;
        bool options = false;
        bool gameon = false;
        bool gameover = false;
        bool jump = false;
        bool hard = false;
        bool speedup = false;
        bool songplaying = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = screen_height;
            graphics.PreferredBackBufferWidth = screen_width;
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

            info = Content.Load<SpriteFont>("font");

            player_camera = new Camera(GraphicsDevice.Viewport);

            background_main = new sprite(Content, "main_menu", 0, 0, 1);
            background_gameover = new sprite(Content, "game_over", 0, 0, 1);
            backgrounds[0] = new sprite(Content, "background_blueclouds", 0, 0, 1);
            backgrounds[1] = new sprite(Content, "background_blueclouds", 0, 0, 1);
            backgrounds[2] = new sprite(Content, "background_cave", 0, 0, 1);
            backgrounds[3] = new sprite(Content, "background_cave", 0, 0, 1);
            backgrounds[4] = new sprite(Content, "background_city", 0, 0, 1);
            backgrounds[5] = new sprite(Content, "background_city", 0, 0, 1);

            grass_music = Content.Load<Song>("grass_music");
            cave_music = Content.Load<Song>("cave_music");
            city_music = Content.Load<Song>("city_music");
            main_music = Content.Load<Song>("main_music");
            button_forward = Content.Load<SoundEffect>("button_forward");
            button_back = Content.Load<SoundEffect>("button_back");
            bounce = Content.Load<SoundEffect>("bounce");
            taking_damage = Content.Load<SoundEffect>("player_hit");
            drinking = Content.Load<SoundEffect>("drinking");
            eating = Content.Load<SoundEffect>("eating");
            gameover_sound = Content.Load<SoundEffect>("gameover_sound");


            buttons[0] = new sprite(Content, "button_play", (screen_width * 2 / 3), (screen_height * 1 / 5 - 30), 1);
            buttons[1] = new sprite(Content, "button_leaderboard", (screen_width * 2 / 3), (screen_height * 2 / 5 - 30), 1);
            buttons[2] = new sprite(Content, "button_options", (screen_width * 2 / 3), (screen_height * 3 / 5 - 30), 1);
            buttons[3] = new sprite(Content, "button_exit", (screen_width * 2 / 3), (screen_height * 4 / 5 - 30), 1);

            back_button = new sprite(Content, "button_exit", (screen_width * 2 / 3), (screen_height * 17 / 20 - screen_height / 10), 1);
            restart_button = new sprite(Content, "button_restart", (screen_width * 3 / 8), (screen_height * 17 / 20 - screen_height / 10), 1);

            option_buttons[0] = new sprite(Content, "button_easy", (screen_width * 2 / 3), (screen_height * 1 / 4 - 30), 1);
            option_buttons[1] = new sprite(Content, "button_hard", (screen_width * 2 / 3), (screen_height * 1 / 2 - 30), 1);
            option_buttons[2] = new sprite(Content, "button_exit", (screen_width * 2 / 3), (screen_height * 3 / 4 - 30), 1);

            player_sprite = new sprite2(Content, "running", 200, screen_height - 200, 4, 6, 0.3f);
            bottled_juice = new sprite(Content, "bottled_juice", 2300, 0, 1);

            info_position = new Vector2(50, 20);

            // Load in high scores
            if (File.Exists(@"highscore.txt")) // This checks to see if the file exists
            {
                StreamReader sr = new StreamReader(@"highscore.txt");	// Open the file

                String line;		// Create a string variable to read each line into
                for (int i = 0; i < numberofhighscores && !sr.EndOfStream; i++)
                {
                    line = sr.ReadLine();	// Read the first line in the text file
                    highscorenames[i] = line.Trim(); // Read high score name

                    if (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();	// Read the first line in the text file
                        line = line.Trim(); 	// This trims spaces from either side of the text
                        highscores[i] = Convert.ToInt32(line);	// This converts line to numeric
                    }
                }
                sr.Close();			// Close the file
            }
            // SORT HIGH SCORE TABLE
            Array.Sort(highscores, highscorenames);
            Array.Reverse(highscores);
            Array.Reverse(highscorenames);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            // Save high scores
            StreamWriter sw = new StreamWriter(@"highscore.txt");
            for (int i = 0; i < numberofhighscores; i++)
            {
                sw.WriteLine(highscorenames[i]);
                sw.WriteLine(highscores[i].ToString());
            }
            sw.Close();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keys = Keyboard.GetState();                     // Read keyboard
            keyboardreleased = (keys != lastkeystate);      // Has keyboard input changed

            float timebetweenupdates = (float)gameTime.ElapsedGameTime.TotalMilliseconds; // Time between updates

            current_time = gameTime.ElapsedGameTime.Milliseconds;
            var controller = GamePad.GetState(PlayerIndex.One);
            var mouse = Mouse.GetState();

            if (controller.Buttons.Back == ButtonState.Pressed || keys.IsKeyDown(Keys.Escape) && (menu == false) && (button_timercount >= button_timer))
            {
                returntomain();
                button_timercount = 0;
            }

            Rectangle mouse_box = new Rectangle((int)mouse.X, (int)mouse.Y, 1, 1);
            button_timercount += current_time;
            current_lcooldown += current_time;

            if (menu == true)
            {
                MediaPlayer.Volume = 0.1f;
                player_camera.Position = new Vector2(0, 0);
                IsMouseVisible = true;
                Rectangle button_box1 = new Rectangle((int)buttons[0].position.X, (int)buttons[0].position.Y, buttons[0].graphic.Width, buttons[0].graphic.Height);
                Rectangle button_box2 = new Rectangle((int)buttons[1].position.X, (int)buttons[1].position.Y, buttons[1].graphic.Width, buttons[1].graphic.Height);
                Rectangle button_box3 = new Rectangle((int)buttons[2].position.X, (int)buttons[2].position.Y, buttons[2].graphic.Width, buttons[2].graphic.Height);
                Rectangle button_box4 = new Rectangle((int)buttons[3].position.X, (int)buttons[3].position.Y, buttons[3].graphic.Width, buttons[3].graphic.Height);

                if (songplaying == false)
                {
                    MediaPlayer.Play(main_music);
                    MediaPlayer.IsRepeating = true;
                    songplaying = true;
                }

                if ((mouse_box.Intersects(button_box1)))
                {
                    buttons[0] = new sprite(Content, "play_selected", (screen_width * 2 / 3), (screen_height * 1 / 5 - 30), 1);
                }
                else
                {
                    buttons[0] = new sprite(Content, "button_play", (screen_width * 2 / 3), (screen_height * 1 / 5 - 30), 1);
                }

                if ((mouse_box.Intersects(button_box1)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    startgame();
                }

                if ((mouse_box.Intersects(button_box2)))
                {
                    buttons[1] = new sprite(Content, "leaderboard_selected", (screen_width * 2 / 3), (screen_height * 2 / 5 - 30), 1);
                }
                else
                {
                    buttons[1] = new sprite(Content, "button_leaderboard", (screen_width * 2 / 3), (screen_height * 2 / 5 - 30), 1);
                }
                if ((mouse_box.Intersects(button_box2)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    leaderboard = true;
                    menu = false;
                    button_forward.Play();
                }

                if ((mouse_box.Intersects(button_box3)))
                {
                    buttons[2] = new sprite(Content, "options_selected", (screen_width * 2 / 3), (screen_height * 3 / 5 - 30), 1);
                }
                else
                {
                    buttons[2] = new sprite(Content, "button_options", (screen_width * 2 / 3), (screen_height * 3 / 5 - 30), 1);
                }
                if ((mouse_box.Intersects(button_box3)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    options = true;
                    menu = false;
                    button_forward.Play();
                    button_timercount = 0;
                }

                if ((mouse_box.Intersects(button_box4)))
                {
                    buttons[3] = new sprite(Content, "exit_selected", (screen_width * 2 / 3), (screen_height * 4 / 5 - 30), 1);
                }
                else
                {
                    buttons[3] = new sprite(Content, "button_exit", (screen_width * 2 / 3), (screen_height * 4 / 5 - 30), 1);
                }
                if ((mouse_box.Intersects(button_box4)) && (mouse.LeftButton == ButtonState.Pressed) && (button_timercount >= button_timer))
                {
                    Exit();
                }
            }

            else if (leaderboard == true)
            {

                Rectangle back_box = new Rectangle((int)back_button.position.X, (int)back_button.position.Y, back_button.graphic.Width, back_button.graphic.Height);

                if ((mouse_box.Intersects(back_box)))
                {
                    back_button = new sprite(Content, "exit_selected", (screen_width * 2 / 3), (screen_height * 17 / 20 - screen_height / 10), 1);
                }
                else
                {
                    back_button = new sprite(Content, "button_exit", (screen_width * 2 / 3), (screen_height * 17 / 20 - screen_height / 10), 1);
                }
                if ((mouse_box.Intersects(back_box)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    menu = true;
                    button_back.Play();
                    leaderboard = false;
                    button_timercount = 0;
                }
            }

            else if (options == true)
            {
                IsMouseVisible = true;
                Rectangle option_box1 = new Rectangle((int)option_buttons[0].position.X, (int)option_buttons[0].position.Y,
                    option_buttons[0].graphic.Width, option_buttons[0].graphic.Height);
                Rectangle option_box2 = new Rectangle((int)option_buttons[1].position.X, (int)option_buttons[1].position.Y,
                    option_buttons[1].graphic.Width, option_buttons[1].graphic.Height);
                Rectangle option_box3 = new Rectangle((int)option_buttons[2].position.X, (int)option_buttons[2].position.Y,
                    option_buttons[2].graphic.Width, option_buttons[2].graphic.Height);

                if ((mouse_box.Intersects(option_box1)))
                {
                    option_buttons[0] = new sprite(Content, "easy_selected", (screen_width * 2 / 3), (screen_height * 1 / 4 - 30), 1);
                }
                else
                {
                    option_buttons[0] = new sprite(Content, "button_easy", (screen_width * 2 / 3), (screen_height * 1 / 4 - 30), 1);
                }
                if ((mouse_box.Intersects(option_box1)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    defaultlives = 3;
                    button_forward.Play();
                    hard = false;
                    speedup = false;
                }

                if ((mouse_box.Intersects(option_box2)))
                {
                    option_buttons[1] = new sprite(Content, "hard_selected", (screen_width * 2 / 3), (screen_height * 1 / 2 - 30), 1);
                }
                else
                {
                    option_buttons[1] = new sprite(Content, "button_hard", (screen_width * 2 / 3), (screen_height * 1 / 2 - 30), 1);
                }
                if ((mouse_box.Intersects(option_box2)) && (mouse.LeftButton == ButtonState.Pressed) && (button_timercount >= button_timer))
                {
                    defaultlives = 1;
                    button_forward.Play();
                    hard = true;
                    speedup = true;
                }

                if ((mouse_box.Intersects(option_box3)))
                {
                    option_buttons[2] = new sprite(Content, "exit_selected", (screen_width * 2 / 3), (screen_height * 3 / 4 - 30), 1);
                }
                else
                {
                    option_buttons[2] = new sprite(Content, "button_exit", (screen_width * 2 / 3), (screen_height * 3 / 4 - 30), 1);
                }
                if ((mouse_box.Intersects(option_box3)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    menu = true;
                    options = false;
                    button_back.Play();
                    button_timercount = 0;
                }

            }


            else if (gameon == true)
            {
                IsMouseVisible = false;
                player_camera.Position = new Vector2(player_sprite.position.X - 200, 0);
                Rectangle player_box = new Rectangle((int)player_sprite.position.X, (int)player_sprite.position.Y, player_sprite.sprite_animation.rect.Width, player_sprite.sprite_animation.rect.Height);

                Eagle eagle_hit = null;
                Haggis haggis_hit = null;
                Shortbread shortbread_hit = null;
                Block block_hit = null;

                if (player_sprite.position.X < 200)
                {
                    player_camera.Position = new Vector2(0, 0);
                }
                if (player_sprite.position.X < 0)
                {
                    player_sprite.position.X = 0;
                }

                if (player_sprite.position.X > 1800)
                {
                    player_camera.Position = new Vector2(1600, 0);
                }
                if (player_sprite.position.X > 2300)
                {
                    player_camera.Position = new Vector2(0, 0);
                    player_sprite.position = new Vector2(200, screen_height - 200);
                    songplaying = false;
                    score += 200;
                    drinking.Play();
                    Load_Level();
                    if (hard == true)
                    {
                        speedup = true;
                    }
                }

                if ((player_sprite.position.Y > screen_height - player_sprite.sprite_animation.rect.Height) && (current_lcooldown >= life_cooldown))
                {
                    lives--;
                    player_sprite.position = new Vector2(200, screen_height - 200);
                    player_camera.Position = new Vector2(0, 0);
                    current_lcooldown = 0;
                }

                foreach (Block x in blocks)
                {
                    x.block_box = new Rectangle((int)x.block_position.X, (int)x.block_position.Y, (int)x.block_sprite.Width, (int)x.block_sprite.Height);

                    if (player_box.Intersects(x.block_box))
                    {
                        block_hit = x;
                    }

                    if (player_box.Intersects(x.block_box) && (player_sprite.position.X < x.block_position.X) && (player_sprite.position.Y - (x.block_position.Y - player_sprite.sprite_animation.rect.Height) > 20)
                        && ((player_sprite.position.X - (x.block_position.X - player_sprite.sprite_animation.rect.Width) < 5)))
                    {
                        if ((player_sprite.position.X - (x.block_position.X - player_sprite.sprite_animation.rect.Width) > 1))
                        {
                            player_sprite.velocity.X = 0;
                            player_sprite.position.X = x.block_position.X - player_sprite.sprite_animation.rect.Width;
                        }
                        jump = true;
                    }
                    else if (player_box.Intersects(x.block_box) && (player_sprite.position.X > x.block_position.X) && (player_sprite.position.Y - (x.block_position.Y - player_sprite.sprite_animation.rect.Height) > 20)
                        && ((player_sprite.position.X - (x.block_position.X + x.block_sprite.Width) > 35)))
                    {
                        if ((player_sprite.position.X - (x.block_position.X + x.block_sprite.Width) < 39))
                        {
                            player_sprite.velocity.X = 0;
                            player_sprite.position.X = x.block_position.X + x.block_sprite.Width;
                        }
                    }
                    else if (player_box.Intersects(x.block_box) && (player_sprite.position.Y < x.block_position.Y))
                    {
                        player_sprite.velocity.Y = 0;
                        if ((player_sprite.position.Y - (x.block_position.Y - player_sprite.sprite_animation.rect.Height) > 6))
                        {
                            player_sprite.position.Y = x.block_position.Y - player_sprite.sprite_animation.rect.Height;
                        }
                        jump = false;
                    }
                    else if (player_box.Intersects(x.block_box) && (player_sprite.position.Y > x.block_position.Y))
                    {
                        player_sprite.velocity.Y = 0;
                        if ((player_sprite.position.Y - (x.block_position.Y + x.block_sprite.Height) < 36))
                        {
                            player_sprite.position.Y = x.block_position.Y + x.block_sprite.Height;
                        }
                    }

                }

                if (jump == true)
                {

                }
                else
                {
                    if (keys.IsKeyDown(Keys.Space) || (controller.Buttons.A == ButtonState.Pressed))
                    {
                        jump = true;
                        player_sprite.velocity.Y -= 30;
                    }
                }

                if (keys.IsKeyDown(Keys.A) || (controller.DPad.Left == ButtonState.Pressed) || (controller.ThumbSticks.Left.X < -0.1))
                {
                    player_sprite.velocity.X -= 0.2f;
                    player_camera.Position -= new Vector2(1.5f, 0);

                    player_sprite.sprite_animation.fliphorizontal = true;
                }

                if (keys.IsKeyDown(Keys.D) || (controller.DPad.Right == ButtonState.Pressed) || (controller.ThumbSticks.Left.X > 0.1))
                {
                    player_sprite.velocity.X += 0.2f;
                    player_camera.Position += new Vector2(1.5f, 0);
                    player_sprite.sprite_animation.fliphorizontal = false;
                }

                player_sprite.position += player_sprite.velocity; // Apply velocity to move
                player_sprite.velocity *= 0.95f; // Apply friction

                if (block_hit == null)
                {
                    // Apply Gravity
                    player_sprite.velocity.Y += 2f;
                }

                player_sprite.update(timebetweenupdates);

                foreach (Haggis x in haggises)
                {
                    x.haggis_sprite.update(timebetweenupdates);
                }

                foreach (Thistle x in thistles)
                {
                    x.thistle_sprite.update(timebetweenupdates);
                }


                foreach (Eagle x in eagles)
                {
                    x.eagle_box = new Rectangle((int)x.eagle_position.X, (int)x.eagle_position.Y, x.eagle_sprite.Width, x.eagle_sprite.Height);

                    x.eagle_position.Y += x.eagle_velo * current_time;

                    if (x.eagle_position.Y < 0)
                    {
                        x.eagle_position.Y = 1;
                        x.eagle_velo *= -1;
                    }

                    if (x.eagle_position.Y > (screen_height - x.eagle_sprite.Height))
                    {
                        x.eagle_position.Y = screen_height - x.eagle_sprite.Height - 1;
                        x.eagle_velo *= -1;
                    }

                    if (player_box.Intersects(x.eagle_box) && (player_sprite.position.X < x.eagle_position.X) && (player_sprite.position.Y - (x.eagle_position.Y - player_sprite.sprite_animation.rect.Height) > 20)
                        && ((player_sprite.position.X - (x.eagle_position.X - player_sprite.sprite_animation.rect.Width) < 7)))
                    {
                        if ((player_sprite.position.X - (x.eagle_position.X - player_sprite.sprite_animation.rect.Width) > 1))
                        {
                            player_sprite.position.X = x.eagle_position.X - player_sprite.sprite_animation.rect.Width;
                            lives -= 1;
                            taking_damage.Play(0.5f, 0, 0);
                            current_lcooldown = 0;
                            player_sprite.velocity.X -= 10;
                        }
                    }
                    else if (player_box.Intersects(x.eagle_box) && (player_sprite.position.Y < x.eagle_position.Y))
                    {
                        if ((player_sprite.position.Y - (x.eagle_position.Y - player_sprite.sprite_animation.rect.Height) > 6))
                        {
                            player_sprite.position.Y = x.eagle_position.Y - player_sprite.sprite_animation.rect.Height;
                            eagle_hit = x;
                            player_sprite.velocity.Y -= 60;
                            jump = true;
                            bounce.Play();
                        }

                    }

                    foreach (Block b in blocks)
                    {
                        b.block_box = new Rectangle((int)b.block_position.X, (int)b.block_position.Y, (int)b.block_sprite.Width, (int)b.block_sprite.Height);

                        if (x.eagle_box.Intersects(b.block_box) && (x.eagle_position.X < b.block_position.X))
                        {
                            x.eagle_position.Y = b.block_position.Y - x.eagle_sprite.Width - 5;
                            x.eagle_velo *= -1;
                        }

                        if (x.eagle_box.Intersects(b.block_box) && (x.eagle_position.X > b.block_position.X))
                        {
                            x.eagle_position.Y = b.block_position.Y + b.block_sprite.Width + 5;
                            x.eagle_velo *= -1;
                        }
                    }
                }

                foreach (Haggis x in haggises)
                {
                    x.haggis_box = new Rectangle((int)x.haggis_sprite.position.X, (int)x.haggis_sprite.position.Y, x.haggis_sprite.sprite_animation.rect.Width, x.haggis_sprite.sprite_animation.rect.Height);
                    x.haggis_sprite.velocity.X = 2;

                    x.haggis_sprite.position.X += x.haggis_velo * current_time;

                    if (x.haggis_box.Intersects(player_box) && (jump == true))
                    {
                        haggis_hit = x;
                        jump = true;
                        bounce.Play();
                        player_sprite.position.Y = x.haggis_sprite.position.Y - player_sprite.sprite_animation.rect.Height - 4;
                        player_sprite.velocity.Y -= 50;
                    }

                    if (x.haggis_box.Intersects(player_box) && (jump == false) && (current_lcooldown >= life_cooldown))
                    {
                        lives--;
                        taking_damage.Play(0.5f, 0, 0);
                        current_lcooldown = 0;
                        player_sprite.velocity.X -= 10;
                    }

                    foreach (Block b in blocks)
                    {
                        b.block_box = new Rectangle((int)b.block_position.X, (int)b.block_position.Y, (int)b.block_sprite.Width, (int)b.block_sprite.Height);

                        if (x.haggis_box.Intersects(b.block_box) && (x.haggis_sprite.position.X < b.block_position.X))
                        {
                            x.haggis_velo *= -1;
                            x.haggis_sprite.position.X = b.block_position.X - x.haggis_sprite.sprite_animation.rect.Width - 5;
                            x.haggis_sprite.sprite_animation.fliphorizontal = false;
                        }
                        if (x.haggis_box.Intersects(b.block_box) && (x.haggis_sprite.position.X > b.block_position.X))
                        {
                            x.haggis_velo *= -1;
                            x.haggis_sprite.position.X = b.block_position.X + b.block_sprite.Width + 5;
                            x.haggis_sprite.sprite_animation.fliphorizontal = true;
                        }
                    }

                }

                foreach (Shortbread x in shortbreads)
                {
                    x.sbread_box = new Rectangle((int)x.shortbread_position.X, (int)x.shortbread_position.Y, x.shortbread_sprite.Width, x.shortbread_sprite.Height);

                    if (x.sbread_box.Intersects(player_box))
                    {
                        shortbread_hit = x;
                        eating.Play();
                    }
                }

                foreach (Thistle x in thistles)
                {
                    x.thistle_box = new Rectangle((int)x.thistle_sprite.position.X, (int)x.thistle_sprite.position.Y, x.thistle_sprite.sprite_animation.rect.Width, x.thistle_sprite.sprite_animation.rect.Height);
                    x.thistle_sprite.velocity.X = 2;

                    if (player_box.Intersects(x.thistle_box) && (player_sprite.position.X < x.thistle_sprite.position.X) && (player_sprite.position.Y - (x.thistle_sprite.position.Y - player_sprite.sprite_animation.rect.Height) > 20)
                        && ((player_sprite.position.X - (x.thistle_sprite.position.X - player_sprite.sprite_animation.rect.Width) < 10)))
                    {
                        player_sprite.velocity.X -= 10;
                        taking_damage.Play(0.5f, 0, 0);
                        if ((player_sprite.position.X - (x.thistle_sprite.position.X - player_sprite.sprite_animation.rect.Width) > 1))
                        {
                            player_sprite.position.X = x.thistle_sprite.position.X - player_sprite.sprite_animation.rect.Width;
                        }

                        if (current_lcooldown >= life_cooldown)
                        {
                            lives--;
                            current_lcooldown = 0;
                        }
                    }
                    else if (player_box.Intersects(x.thistle_box) && (player_sprite.position.X > x.thistle_sprite.position.X) && (player_sprite.position.Y >= x.thistle_sprite.position.Y))
                    {
                        player_sprite.velocity.X += 10;
                        taking_damage.Play(0.5f, 0, 0);
                        if ((player_sprite.position.X - (x.thistle_sprite.position.X + x.thistle_sprite.sprite_animation.rect.Width) < 39))
                        {
                            player_sprite.position.X = x.thistle_sprite.position.X + x.thistle_sprite.sprite_animation.rect.Width;
                        }
                        if (current_lcooldown >= life_cooldown)
                        {
                            lives--;
                            current_lcooldown = 0;
                        }
                    }
                    else if (player_box.Intersects(x.thistle_box) && (player_sprite.position.Y < x.thistle_sprite.position.Y))
                    {
                        jump = true;
                        player_sprite.velocity.Y -= 60;
                        taking_damage.Play(0.5f, 0, 0);
                        if ((player_sprite.position.Y - (x.thistle_sprite.position.Y - player_sprite.sprite_animation.rect.Height) > 6))
                        {
                            player_sprite.position.Y = x.thistle_sprite.position.Y - player_sprite.sprite_animation.rect.Height;
                        }
                        if (current_lcooldown >= life_cooldown)
                        {
                            lives--;
                            current_lcooldown = 0;
                        }
                    }
                }

                if (shortbread_hit != null)
                {
                    score += 100;
                    shortbreads.Remove(shortbread_hit);
                }

                if (haggis_hit != null)
                {
                    score += 15;
                    haggises.Remove(haggis_hit);
                }

                if (eagle_hit != null)
                {
                    score += 15;
                    eagles.Remove(eagle_hit);
                }

                if (lives < 1)
                {
                    gameon = false;
                    gameover = true;
                    gameover_sound.Play();

                    if (score > highscores[lasthighscore])
                        highscorenames[lasthighscore] = "";
                }

                //for (int i = 0; i < level_number; i++)
                //{
                //    MediaPlayer.Play(background_music[i]);
                //}
            }

            else if (gameover == true)
            {
                player_camera.Position = new Vector2(0, 0);
                IsMouseVisible = true;
                Rectangle restart_box = new Rectangle((int)restart_button.position.X, (int)restart_button.position.Y, restart_button.graphic.Width, restart_button.graphic.Height);

                // Game is over
                if (score > highscores[lasthighscore])
                {
                    keycounter -= timebetweenupdates; // Counter to delay between keys of the same value being entered
                    if (keyboardreleased)
                    {
                        if (keys.IsKeyDown(Keys.Back) && highscorenames[lasthighscore].Length > 0)
                        {
                            highscorenames[lasthighscore] = highscorenames[lasthighscore].Substring(0, highscorenames[lasthighscore].Length - 1);
                        }
                        else
                        {
                            char nextchar = sfunctions2d.getnextkey();
                            char lastchar = '!';
                            if (highscorenames[lasthighscore].Length > 0)
                                lastchar = Convert.ToChar(highscorenames[lasthighscore].Substring(highscorenames[lasthighscore].Length - 1, 1));
                            if (nextchar != '!' && (nextchar != lastchar || keycounter < 0))
                            {
                                keycounter = keystrokedelay;
                                highscorenames[lasthighscore] += nextchar;
                                if (highscorenames[lasthighscore].Length > maxnamelength)
                                    highscorenames[lasthighscore] = highscorenames[lasthighscore].Substring(0, maxnamelength);
                            }
                        }
                    }

                    // Allow game to return to the main menu
                    if (controller.Buttons.B == ButtonState.Pressed || keys.IsKeyDown(Keys.Enter))
                    {
                        if (score > highscores[lasthighscore])
                        {
                            highscores[lasthighscore] = score;
                        }

                        // Sort the high score table
                        Array.Sort(highscores, highscorenames);
                        Array.Reverse(highscores);
                        Array.Reverse(highscorenames);

                        returntomain();
                    }
                }

                if (hard == true)
                {
                    score = (int)((float)score * 1.3f);
                    hard = false;
                    speedup = false;
                }
                if (mouse_box.Intersects(restart_box))
                {
                    restart_button = new sprite(Content, "restart_selected", (screen_width * 3 / 8), (screen_height * 17 / 20 - screen_height / 10), 1);
                }
                else
                {
                    restart_button = new sprite(Content, "button_restart", (screen_width * 3 / 8), (screen_height * 17 / 20 - screen_height / 10), 1);
                }

                // Allow game to return to the main menu
                if (mouse_box.Intersects(restart_box) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    if (score > highscores[lasthighscore])
                    {
                        highscores[lasthighscore] = score;
                    }

                    // Sort the high score table
                    Array.Sort(highscores, highscorenames);
                    Array.Reverse(highscores);
                    Array.Reverse(highscorenames);

                    startgame();
                    lives = 3;
                }
            }

            // TODO: Add your update logic here
            lastkeystate = keys;                     // Read keyboard

            base.Update(gameTime);
        }

        void startgame()
        {
            gameover = false;
            gameon = true;
            menu = false;
            options = false;
            leaderboard = false;
            songplaying = false;
            MediaPlayer.Volume = 1;
            button_forward.Play();
            player_sprite.position = new Vector2(200, screen_height - 200);
            lives = defaultlives;
            score = 0;
            level_number = 0;
            Load_Level();
        }

        void returntomain()
        {
            gameon = false;
            options = false;
            leaderboard = false;
            menu = true;
            MediaPlayer.Volume = 0.1f;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: player_camera.GetPlayerView());
            // TODO: Add your drawing code here

            if (menu == true)
            {
                background_main.DrawRectangle(spriteBatch, screen_width, screen_height);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].DrawRectangle(spriteBatch, buttons[1].graphic.Width, buttons[1].graphic.Height);
                }
            }

            else if (leaderboard == true)
            {
                background_main.DrawRectangle(spriteBatch, screen_width, screen_height);
                spriteBatch.DrawString(info, "High Scores", new Vector2((screen_width * 4 / 7), (screen_height * 1 / 4 - screen_height / 10)), Color.Red);

                for (int x = 0; x < 10; x++)
                {
                    spriteBatch.DrawString(info, (x + 1).ToString("0") + ".         " + highscores[x] + "       " + highscorenames[x], new Vector2(300, 150 + (x * 20)), Color.Red);
                }

                back_button.DrawRectangle(spriteBatch, buttons[1].graphic.Width, buttons[1].graphic.Height);
            }

            else if (options == true)
            {
                background_main.DrawRectangle(spriteBatch, screen_width, screen_height);
                for (int i = 0; i < option_buttons.Length; i++)
                {
                    option_buttons[i].DrawRectangle(spriteBatch, buttons[1].graphic.Width, buttons[1].graphic.Height);
                }
            }

            else if (gameon == true)
            {

                for (int i = 0; i < level_number; i++)
                {
                    backgrounds[i].DrawRectangle(spriteBatch, 2400, 600);
                }

                player_sprite.DrawNormal(spriteBatch);
                bottled_juice.DrawNormal(spriteBatch);

                foreach (Eagle x in eagles)
                {
                    x.Draw_Eagle(spriteBatch);
                }

                foreach (Haggis x in haggises)
                {
                    x.Draw_Haggis(spriteBatch);
                }

                foreach (Shortbread x in shortbreads)
                {
                    x.Draw_Shortbread(spriteBatch);
                }

                foreach (Thistle x in thistles)
                {
                    x.Draw_Thistle(spriteBatch);
                }

                foreach (Block x in blocks)
                {
                    x.Draw_Block(spriteBatch);
                }
            }

            else if (gameover == true)
            {
                background_gameover.DrawNormal(spriteBatch);

                restart_button.DrawNormal(spriteBatch);

                spriteBatch.DrawString(info, "Your Score:    " + score, new Vector2(300, 300), Color.Red);

                if (score > highscores[numberofhighscores - 1])
                {
                    spriteBatch.DrawString(info, "New High Score Enter Name", new Vector2(100 + screen_width / 2 - (int)(info.MeasureString("New High Score Enter Name").X * (1f / 2f)), 350),
                            Color.Blue, MathHelper.ToRadians(0), new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(info, highscorenames[numberofhighscores - 1], new Vector2(100 + screen_width / 2 - (int)(info.MeasureString("New High Score Enter Name").X * (1f / 2f)), 390),
                            Color.AliceBlue, MathHelper.ToRadians(0), new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                }

            }

            spriteBatch.End();

            spriteBatch.Begin();
            if (gameon == true)
            {
                spriteBatch.DrawString(info, "Lives: " + lives, info_position,
                Color.White, MathHelper.ToRadians(0), new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);

                spriteBatch.DrawString(info, "Score: " + score, new Vector2(info_position.X + 200, info_position.Y),
                Color.White, MathHelper.ToRadians(0), new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }

        protected void Load_Level()
        {
            Clear();
            MediaPlayer.Stop();
            level_number++;

            if (level_number == 1)
            {
                bottled_juice.position.Y = 400;
                MediaPlayer.Play(grass_music);
                MediaPlayer.IsRepeating = true;
                for (int x = 0; x < 1; x++)
                {
                    Block blocks_17 = new Block(Content, "grass");
                    Haggis haggis_1 = new Haggis(Content);
                    Eagle eagle_1 = new Eagle(Content);
                    Thistle thistle_1 = new Thistle(Content);
                    blocks_17.block_position = new Vector2(1160, 200);
                    haggis_1.haggis_sprite.position = new Vector2(650, 470);
                    eagle_1.eagle_position = new Vector2(1685, 500);
                    thistle_1.thistle_sprite.position = new Vector2(1130, 332);
                    blocks.Add(blocks_17);
                    haggises.Add(haggis_1);
                    eagles.Add(eagle_1);
                    thistles.Add(thistle_1);
                }
                for (int x = 0; x < 12; x++)
                {
                    Block blocks_1 = new Block(Content, "dirt");
                    Block blocks_5 = new Block(Content, "dirt");
                    Block blocks_6 = new Block(Content, "dirt");
                    blocks_1.block_position = new Vector2(0 + x * 40, 580);
                    blocks_5.block_position = new Vector2(560 + x * 40, 580);
                    blocks_6.block_position = new Vector2(560 + x * 40, 540);
                    blocks.Add(blocks_1);
                    blocks.Add(blocks_5);
                    blocks.Add(blocks_6);
                }
                for (int x = 0; x < 10; x++)
                {
                    Block blocks_38 = new Block(Content, "grass");
                    blocks_38.block_position = new Vector2(0 + x * 40, 540);
                    blocks.Add(blocks_38);
                }
                for (int x = 0; x < 2; x++)
                {
                    Block blocks_3 = new Block(Content, "dirt");
                    Block blocks_4 = new Block(Content, "grass");
                    Block blocks_7 = new Block(Content, "dirt");
                    Block blocks_8 = new Block(Content, "grass");
                    Block blocks_11 = new Block(Content, "dirt");
                    Block blocks_12 = new Block(Content, "dirt");
                    Block blocks_13 = new Block(Content, "dirt");
                    Block blocks_14 = new Block(Content, "dirt");
                    Block blocks_15 = new Block(Content, "grass");
                    Block blocks_16 = new Block(Content, "grass");
                    Block blocks_23 = new Block(Content, "dirt");
                    Block blocks_24 = new Block(Content, "dirt");
                    Block blocks_25 = new Block(Content, "dirt");
                    Block blocks_26 = new Block(Content, "dirt");
                    Block blocks_27 = new Block(Content, "dirt");
                    Block blocks_28 = new Block(Content, "grass");
                    Block blocks_39 = new Block(Content, "dirt");
                    blocks_3.block_position = new Vector2(400 + x * 40, 500);
                    blocks_4.block_position = new Vector2(400 + x * 40, 460);
                    blocks_7.block_position = new Vector2(560 + x * 40, 500);
                    blocks_8.block_position = new Vector2(560 + x * 40, 460);
                    blocks_11.block_position = new Vector2(1120 + x * 40, 580);
                    blocks_12.block_position = new Vector2(1120 + x * 40, 540);
                    blocks_13.block_position = new Vector2(1120 + x * 40, 500);
                    blocks_14.block_position = new Vector2(1120 + x * 40, 460);
                    blocks_15.block_position = new Vector2(1120 + x * 40, 420);
                    blocks_16.block_position = new Vector2(960 + x * 40, 280);
                    blocks_23.block_position = new Vector2(1600 + x * 40, 580);
                    blocks_24.block_position = new Vector2(1600 + x * 40, 540);
                    blocks_25.block_position = new Vector2(1600 + x * 40, 500);
                    blocks_26.block_position = new Vector2(1600 + x * 40, 460);
                    blocks_27.block_position = new Vector2(1600 + x * 40, 420);
                    blocks_28.block_position = new Vector2(1600 + x * 40, 380);
                    blocks_39.block_position = new Vector2(400 + x * 40, 540);
                    blocks.Add(blocks_3);
                    blocks.Add(blocks_4);
                    blocks.Add(blocks_7);
                    blocks.Add(blocks_8);
                    blocks.Add(blocks_11);
                    blocks.Add(blocks_12);
                    blocks.Add(blocks_13);
                    blocks.Add(blocks_14);
                    blocks.Add(blocks_15);
                    blocks.Add(blocks_16);
                    blocks.Add(blocks_23);
                    blocks.Add(blocks_24);
                    blocks.Add(blocks_25);
                    blocks.Add(blocks_26);
                    blocks.Add(blocks_27);
                    blocks.Add(blocks_28);
                    blocks.Add(blocks_39);
                }
                for (int x = 0; x < 4; x++)
                {
                    Block blocks_9 = new Block(Content, "dirt");
                    Block blocks_10 = new Block(Content, "grass");
                    Block blocks_37 = new Block(Content, "dirt");
                    blocks_9.block_position = new Vector2(880 + x * 40, 460);
                    blocks_10.block_position = new Vector2(880 + x * 40, 420);
                    blocks_37.block_position = new Vector2(880 + x * 40, 500);
                    blocks.Add(blocks_9);
                    blocks.Add(blocks_10);
                    blocks.Add(blocks_37);
                }
                for (int x = 0; x < 6; x++)
                {
                    Block blocks_18 = new Block(Content, "dirt");
                    Block blocks_19 = new Block(Content, "dirt");
                    Block blocks_20 = new Block(Content, "dirt");
                    Block blocks_21 = new Block(Content, "dirt");
                    Block blocks_22 = new Block(Content, "grass");
                    Block blocks_29 = new Block(Content, "dirt");
                    Block blocks_30 = new Block(Content, "dirt");
                    Block blocks_31 = new Block(Content, "grass");
                    Block blocks_36 = new Block(Content, "grass");
                    blocks_18.block_position = new Vector2(1240 + x * 40, 580);
                    blocks_19.block_position = new Vector2(1240 + x * 40, 540);
                    blocks_20.block_position = new Vector2(1240 + x * 40, 500);
                    blocks_21.block_position = new Vector2(1240 + x * 40, 460);
                    blocks_22.block_position = new Vector2(1240 + x * 40, 420);
                    blocks_29.block_position = new Vector2(1760 + x * 40, 580);
                    blocks_30.block_position = new Vector2(1760 + x * 40, 540);
                    blocks_31.block_position = new Vector2(1760 + x * 40, 500);
                    blocks_36.block_position = new Vector2(640 + x * 40, 500);
                    blocks.Add(blocks_18);
                    blocks.Add(blocks_19);
                    blocks.Add(blocks_20);
                    blocks.Add(blocks_21);
                    blocks.Add(blocks_22);
                    blocks.Add(blocks_29);
                    blocks.Add(blocks_30);
                    blocks.Add(blocks_31);
                    blocks.Add(blocks_36);
                }
                for (int x = 0; x < 10; x++)
                {
                    Block blocks_32 = new Block(Content, "dirt");
                    Block blocks_33 = new Block(Content, "dirt");
                    Block blocks_34 = new Block(Content, "dirt");
                    Block blocks_35 = new Block(Content, "grass");
                    blocks_32.block_position = new Vector2(2080 + x * 40, 580);
                    blocks_33.block_position = new Vector2(2080 + x * 40, 540);
                    blocks_34.block_position = new Vector2(2080 + x * 40, 500);
                    blocks_35.block_position = new Vector2(2080 + x * 40, 460);
                    blocks.Add(blocks_32);
                    blocks.Add(blocks_33);
                    blocks.Add(blocks_34);
                    blocks.Add(blocks_35);
                }
            }

            else if (level_number == 2)
            {
                bottled_juice.position.Y = 300;
                MediaPlayer.Play(grass_music);
                MediaPlayer.IsRepeating = true;
                for (int x = 0; x < 1; x++)
                {
                    Block blocks_3 = new Block(Content, "grass");
                    Block blocks_11 = new Block(Content, "grass");
                    Block blocks_14 = new Block(Content, "grass");
                    Eagle eagle_1 = new Eagle(Content);
                    Eagle eagle_2 = new Eagle(Content);
                    Haggis haggis_1 = new Haggis(Content);
                    Haggis haggis_2 = new Haggis(Content);
                    Thistle thistle_1 = new Thistle(Content);
                    Thistle thistle_2 = new Thistle(Content);
                    Shortbread shortbread_1 = new Shortbread(Content);
                    blocks_3.block_position = new Vector2(280, 350);
                    blocks_11.block_position = new Vector2(480, 520);
                    blocks_14.block_position = new Vector2(480, 260);
                    eagle_1.eagle_position = new Vector2(385, 20);
                    eagle_2.eagle_position = new Vector2(545, 550);
                    haggis_1.haggis_sprite.position = new Vector2(730, 490);
                    haggis_2.haggis_sprite.position = new Vector2(1020, 490);
                    thistle_1.thistle_sprite.position = new Vector2(850, 142);
                    thistle_2.thistle_sprite.position = new Vector2(1475, 472);
                    shortbread_1.shortbread_position = new Vector2(1000, 160);
                    blocks.Add(blocks_3);
                    blocks.Add(blocks_11);
                    blocks.Add(blocks_14);
                    eagles.Add(eagle_1);
                    eagles.Add(eagle_2);
                    haggises.Add(haggis_1);
                    haggises.Add(haggis_2);
                    thistles.Add(thistle_1);
                    thistles.Add(thistle_2);
                    shortbreads.Add(shortbread_1);
                }

                for (int x = 0; x < 2; x++)
                {
                    Block blocks_4 = new Block(Content, "grass");
                    Block blocks_7 = new Block(Content, "dirt");
                    Block blocks_8 = new Block(Content, "dirt");
                    Block blocks_9 = new Block(Content, "grass");
                    Block blocks_10 = new Block(Content, "grass");
                    Block blocks_12 = new Block(Content, "grass");
                    Block blocks_15 = new Block(Content, "grass");
                    Block blocks_22 = new Block(Content, "grass");
                    blocks_4.block_position = new Vector2(90 + x * 40, 260);
                    blocks_7.block_position = new Vector2(640 + x * 40, 520);
                    blocks_9.block_position = new Vector2(640 + x * 40, 480);
                    blocks_8.block_position = new Vector2(1080 + x * 40, 520);
                    blocks_10.block_position = new Vector2(1080 + x * 40, 480);
                    blocks_12.block_position = new Vector2(290 + x * 40, 190);
                    blocks_15.block_position = new Vector2(750 + x * 40, 135);
                    blocks_22.block_position = new Vector2(1470 + x * 40, 560);
                    blocks.Add(blocks_4);
                    blocks.Add(blocks_7);
                    blocks.Add(blocks_8);
                    blocks.Add(blocks_9);
                    blocks.Add(blocks_10);
                    blocks.Add(blocks_12);
                    blocks.Add(blocks_15);
                    blocks.Add(blocks_22);
                }

                for (int x = 0; x < 3; x++)
                {
                    Block blocks_23 = new Block(Content, "grass");
                    blocks_23.block_position = new Vector2(1860 + x * 40, 480);
                    blocks.Add(blocks_23);
                }

                for (int x = 0; x < 4; x++)
                {
                    Block blocks_20 = new Block(Content, "grass");
                    Block blocks_21 = new Block(Content, "grass");
                    blocks_20.block_position = new Vector2(1280 + x * 40, 440);
                    blocks_21.block_position = new Vector2(1580 + x * 40, 530);
                    blocks.Add(blocks_20);
                    blocks.Add(blocks_21);
                }

                for (int x = 0; x < 9; x++)
                {
                    Block blocks_1 = new Block(Content, "dirt");
                    Block blocks_2 = new Block(Content, "grass");
                    Block blocks_6 = new Block(Content, "grass");
                    Block blocks_16 = new Block(Content, "dirt");
                    Block blocks_17 = new Block(Content, "dirt");
                    Block blocks_18 = new Block(Content, "dirt");
                    Block blocks_19 = new Block(Content, "grass");
                    blocks_1.block_position = new Vector2(0 + x * 40, 560);
                    blocks_2.block_position = new Vector2(0 + x * 40, 520);
                    blocks_6.block_position = new Vector2(720 + x * 40, 520);
                    blocks_16.block_position = new Vector2(2120 + x * 40, 580);
                    blocks_17.block_position = new Vector2(2120 + x * 40, 540);
                    blocks_18.block_position = new Vector2(2120 + x * 40, 500);
                    blocks_19.block_position = new Vector2(2120 + x * 40, 460);
                    blocks.Add(blocks_1);
                    blocks.Add(blocks_2);
                    blocks.Add(blocks_6);
                    blocks.Add(blocks_16);
                    blocks.Add(blocks_17);
                    blocks.Add(blocks_18);
                    blocks.Add(blocks_19);
                }

                for (int x = 0; x < 13; x++)
                {
                    Block blocks_5 = new Block(Content, "dirt");
                    Block blocks_13 = new Block(Content, "grass");
                    blocks_5.block_position = new Vector2(640 + x * 40, 560);
                    blocks_13.block_position = new Vector2(640 + x * 40, 230);
                    blocks.Add(blocks_5);
                    blocks.Add(blocks_13);
                }
            }

            else if (level_number == 3)
            {
                lives++;
                bottled_juice.position.Y = 370;
                MediaPlayer.Play(cave_music);
                MediaPlayer.IsRepeating = true;
                for (int x = 0; x < 9; x++)
                {
                    Block blocks_1 = new Block(Content, "rock");
                    Block blocks_2 = new Block(Content, "rock");
                    Block blocks_3 = new Block(Content, "rock");
                    Block blocks_4 = new Block(Content, "rock");
                    Block blocks_5 = new Block(Content, "rock");
                    Block blocks_6 = new Block(Content, "rock");
                    Block blocks_7 = new Block(Content, "rock");
                    Block blocks_8 = new Block(Content, "rock");
                    Block blocks_9 = new Block(Content, "rock");
                    Block blocks_10 = new Block(Content, "rock");
                    Block blocks_11 = new Block(Content, "rock");
                    Block blocks_12 = new Block(Content, "rock");
                    Block blocks_26 = new Block(Content, "rock");
                    Block blocks_33 = new Block(Content, "rock");
                    blocks_1.block_position = new Vector2(0 + x * 40, 560);
                    blocks_2.block_position = new Vector2(0 + x * 40, 520);
                    blocks_3.block_position = new Vector2(0 + x * 40, 480);
                    blocks_4.block_position = new Vector2(0 + x * 40, 440);
                    blocks_5.block_position = new Vector2(0 + x * 40, 400);
                    blocks_6.block_position = new Vector2(0 + x * 40, 360);
                    blocks_7.block_position = new Vector2(0 + x * 40, 320);
                    blocks_8.block_position = new Vector2(0 + x * 40, 280);
                    blocks_9.block_position = new Vector2(0 + x * 40, 240);
                    blocks_10.block_position = new Vector2(0 + x * 40, 200);
                    blocks_11.block_position = new Vector2(0 + x * 40, 160);
                    blocks_12.block_position = new Vector2(0 + x * 40, 120);
                    blocks_26.block_position = new Vector2(1160 + x * 40, 520);
                    blocks_33.block_position = new Vector2(1520 + x * 40, 560);
                    blocks.Add(blocks_1);
                    blocks.Add(blocks_2);
                    blocks.Add(blocks_3);
                    blocks.Add(blocks_4);
                    blocks.Add(blocks_5);
                    blocks.Add(blocks_6);
                    blocks.Add(blocks_7);
                    blocks.Add(blocks_8);
                    blocks.Add(blocks_9);
                    blocks.Add(blocks_10);
                    blocks.Add(blocks_11);
                    blocks.Add(blocks_12);
                    blocks.Add(blocks_26);
                    blocks.Add(blocks_33);
                }

                for (int x = 0; x < 40; x++)
                {
                    Block blocks_13 = new Block(Content, "rock");
                    Block blocks_14 = new Block(Content, "rock");
                    Block blocks_15 = new Block(Content, "rock");
                    blocks_13.block_position = new Vector2(800 + x * 40, 0);
                    blocks_14.block_position = new Vector2(800 + x * 40, 40);
                    blocks_15.block_position = new Vector2(800 + x * 40, 80);
                    blocks.Add(blocks_13);
                    blocks.Add(blocks_14);
                    blocks.Add(blocks_15);
                }

                for (int x = 0; x < 20; x++)
                {
                    Block blocks_16 = new Block(Content, "rock");
                    Block blocks_17 = new Block(Content, "rock");
                    Block blocks_18 = new Block(Content, "rock");
                    blocks_16.block_position = new Vector2(800 + x * 40, 120);
                    blocks_17.block_position = new Vector2(800 + x * 40, 160);
                    blocks_18.block_position = new Vector2(800 + x * 40, 200);
                    blocks.Add(blocks_16);
                    blocks.Add(blocks_17);
                    blocks.Add(blocks_18);
                }

                for (int x = 0; x < 1; x++)
                {
                    Block blocks_19 = new Block(Content, "rock");
                    Block blocks_20 = new Block(Content, "rock");
                    Block blocks_21 = new Block(Content, "rock");
                    Block blocks_22 = new Block(Content, "rock");
                    Block blocks_25 = new Block(Content, "rock");
                    Block blocks_31 = new Block(Content, "rock");
                    Block blocks_32 = new Block(Content, "rock");
                    Block blocks_37 = new Block(Content, "rock");
                    Block blocks_38 = new Block(Content, "rock");
                    Block blocks_39 = new Block(Content, "rock");
                    Block blocks_40 = new Block(Content, "rock");
                    Block blocks_41 = new Block(Content, "rock");
                    Eagle eagle_1 = new Eagle(Content);
                    Thistle thistle_1 = new Thistle(Content);
                    Haggis haggis_1 = new Haggis(Content);
                    Haggis haggis_2 = new Haggis(Content);
                    Haggis haggis_3 = new Haggis(Content);
                    Haggis haggis_4 = new Haggis(Content);
                    blocks_19.block_position = new Vector2(480, 160);
                    blocks_20.block_position = new Vector2(640, 320);
                    blocks_21.block_position = new Vector2(480, 520);
                    blocks_22.block_position = new Vector2(640, 520);
                    blocks_25.block_position = new Vector2(1160, 480);
                    blocks_31.block_position = new Vector2(1480, 480);
                    blocks_32.block_position = new Vector2(1480, 560);
                    blocks_37.block_position = new Vector2(800, 240);
                    blocks_38.block_position = new Vector2(800, 280);
                    blocks_39.block_position = new Vector2(800, 320);
                    blocks_40.block_position = new Vector2(760, -40);
                    blocks_41.block_position = new Vector2(799, 320);
                    eagle_1.eagle_position = new Vector2(545, 400);
                    thistle_1.thistle_sprite.position = new Vector2(1065, 525);
                    haggis_1.haggis_sprite.position = new Vector2(1530, 530);
                    haggis_2.haggis_sprite.position = new Vector2(1790, 530);
                    haggis_3.haggis_sprite.position = new Vector2(1210, 490);
                    haggis_4.haggis_sprite.position = new Vector2(1470, 490);
                    blocks.Add(blocks_19);
                    blocks.Add(blocks_20);
                    blocks.Add(blocks_21);
                    blocks.Add(blocks_22);
                    blocks.Add(blocks_25);
                    blocks.Add(blocks_31);
                    blocks.Add(blocks_32);
                    blocks.Add(blocks_37);
                    blocks.Add(blocks_38);
                    blocks.Add(blocks_39);
                    blocks.Add(blocks_40);
                    blocks.Add(blocks_41);
                    eagles.Add(eagle_1);
                    thistles.Add(thistle_1);
                    haggises.Add(haggis_1);
                    haggises.Add(haggis_2);
                    haggises.Add(haggis_3);
                    haggises.Add(haggis_4);
                }

                for (int x = 0; x < 6; x++)
                {
                    Block blocks_23 = new Block(Content, "rock");
                    blocks_23.block_position = new Vector2(800 + x * 40, 520);
                    blocks.Add(blocks_23);
                }

                for (int x = 0; x < 2; x++)
                {
                    Block blocks_24 = new Block(Content, "rock");
                    blocks_24.block_position = new Vector2(1060 + x * 40, 590);
                    blocks.Add(blocks_24);
                }

                for (int x = 0; x < 5; x++)
                {
                    Block blocks_27 = new Block(Content, "rock");
                    Block blocks_28 = new Block(Content, "rock");
                    Block blocks_29 = new Block(Content, "rock");
                    Block blocks_30 = new Block(Content, "rock");
                    Block blocks_34 = new Block(Content, "rock");
                    Block blocks_35 = new Block(Content, "rock");
                    Block blocks_36 = new Block(Content, "rock");
                    blocks_27.block_position = new Vector2(2200 + x * 40, 560);
                    blocks_28.block_position = new Vector2(2200 + x * 40, 520);
                    blocks_29.block_position = new Vector2(2200 + x * 40, 480);
                    blocks_30.block_position = new Vector2(2200 + x * 40, 440);
                    blocks_34.block_position = new Vector2(1880 + x * 40, 560);
                    blocks_35.block_position = new Vector2(1880 + x * 40, 520);
                    blocks_36.block_position = new Vector2(1880 + x * 40, 480);
                    blocks.Add(blocks_27);
                    blocks.Add(blocks_28);
                    blocks.Add(blocks_29);
                    blocks.Add(blocks_30);
                    blocks.Add(blocks_34);
                    blocks.Add(blocks_35);
                    blocks.Add(blocks_36);
                }
            }
            else if (level_number == 4)
            {
                player_sprite.position = new Vector2(50, screen_height - 200);
                bottled_juice.position.Y = 100;
                MediaPlayer.Play(cave_music);
                MediaPlayer.IsRepeating = true;
                for (int x = 0; x < 15; x++)
                {
                    Block blocks_1 = new Block(Content, "rock");
                    Block blocks_2 = new Block(Content, "rock");
                    Block blocks_3 = new Block(Content, "rock");
                    Block blocks_4 = new Block(Content, "rock");
                    Block blocks_5 = new Block(Content, "rock");
                    Block blocks_6 = new Block(Content, "rock");
                    Block blocks_7 = new Block(Content, "rock");
                    Block blocks_8 = new Block(Content, "rock");
                    Block blocks_13 = new Block(Content, "rock");
                    Block blocks_14 = new Block(Content, "rock");
                    Block blocks_15 = new Block(Content, "rock");
                    Block blocks_16 = new Block(Content, "rock");
                    Block blocks_17 = new Block(Content, "rock");
                    Block blocks_26 = new Block(Content, "rock");
                    Block blocks_27 = new Block(Content, "rock");
                    Block blocks_28 = new Block(Content, "rock");
                    Block blocks_29 = new Block(Content, "rock");
                    Block blocks_30 = new Block(Content, "rock");
                    Block blocks_31 = new Block(Content, "rock");
                    Block blocks_32 = new Block(Content, "rock");
                    Block blocks_33 = new Block(Content, "rock");
                    Block blocks_34 = new Block(Content, "rock");
                    Block blocks_35 = new Block(Content, "rock");
                    Block blocks_36 = new Block(Content, "rock");
                    blocks_1.block_position = new Vector2(0 + x * 40, 560);
                    blocks_2.block_position = new Vector2(0 + x * 40, 520);
                    blocks_3.block_position = new Vector2(0 + x * 40, 480);
                    blocks_4.block_position = new Vector2(0 + x * 40, 440);
                    blocks_5.block_position = new Vector2(0 + x * 40, 400);
                    blocks_6.block_position = new Vector2(0 + x * 40, 360);
                    blocks_7.block_position = new Vector2(0 + x * 40, 320);
                    blocks_8.block_position = new Vector2(0 + x * 40, 280);
                    blocks_13.block_position = new Vector2(920 + x * 40, 0);
                    blocks_14.block_position = new Vector2(920 + x * 40, 40);
                    blocks_15.block_position = new Vector2(920 + x * 40, 80);
                    blocks_16.block_position = new Vector2(920 + x * 40, 120);
                    blocks_17.block_position = new Vector2(920 + x * 40, 160);
                    blocks_26.block_position = new Vector2(1960 + x * 40, 560);
                    blocks_27.block_position = new Vector2(1960 + x * 40, 560);
                    blocks_28.block_position = new Vector2(1960 + x * 40, 520);
                    blocks_29.block_position = new Vector2(1960 + x * 40, 480);
                    blocks_30.block_position = new Vector2(1960 + x * 40, 440);
                    blocks_31.block_position = new Vector2(1960 + x * 40, 400);
                    blocks_32.block_position = new Vector2(1960 + x * 40, 360);
                    blocks_33.block_position = new Vector2(1960 + x * 40, 320);
                    blocks_34.block_position = new Vector2(1960 + x * 40, 280);
                    blocks_35.block_position = new Vector2(1960 + x * 40, 240);
                    blocks_36.block_position = new Vector2(1960 + x * 40, 200);
                    blocks.Add(blocks_1);
                    blocks.Add(blocks_2);
                    blocks.Add(blocks_3);
                    blocks.Add(blocks_4);
                    blocks.Add(blocks_5);
                    blocks.Add(blocks_6);
                    blocks.Add(blocks_7);
                    blocks.Add(blocks_8);
                    blocks.Add(blocks_13);
                    blocks.Add(blocks_14);
                    blocks.Add(blocks_15);
                    blocks.Add(blocks_16);
                    blocks.Add(blocks_17);
                    blocks.Add(blocks_26);
                    blocks.Add(blocks_27);
                    blocks.Add(blocks_28);
                    blocks.Add(blocks_29);
                    blocks.Add(blocks_30);
                    blocks.Add(blocks_31);
                    blocks.Add(blocks_32);
                    blocks.Add(blocks_33);
                    blocks.Add(blocks_34);
                    blocks.Add(blocks_35);
                    blocks.Add(blocks_36);
                }

                for (int x = 0; x < 2; x++)
                {
                    Block blocks_18 = new Block(Content, "rock");
                    Block blocks_19 = new Block(Content, "rock");
                    Block blocks_20 = new Block(Content, "rock");
                    Block blocks_21 = new Block(Content, "rock");
                    Block blocks_22 = new Block(Content, "rock");
                    blocks_18.block_position = new Vector2(920 + x * 40, 480);
                    blocks_19.block_position = new Vector2(1020 + x * 40, 580);
                    blocks_20.block_position = new Vector2(1120 + x * 40, 480);
                    blocks_21.block_position = new Vector2(1220 + x * 40, 580);
                    blocks_22.block_position = new Vector2(1320 + x * 40, 480);
                    blocks.Add(blocks_18);
                    blocks.Add(blocks_19);
                    blocks.Add(blocks_20);
                    blocks.Add(blocks_21);
                    blocks.Add(blocks_22);
                }

                for (int x = 0; x < 6; x++)
                {
                    Block blocks_9 = new Block(Content, "rock");
                    Block blocks_10 = new Block(Content, "rock");
                    blocks_9.block_position = new Vector2(0 + x * 40, 240);
                    blocks_10.block_position = new Vector2(0 + x * 40, 200);
                    blocks.Add(blocks_9);
                    blocks.Add(blocks_10);
                }

                for (int x = 0; x < 1; x++)
                {
                    Block blocks_11 = new Block(Content, "rock");
                    Block blocks_12 = new Block(Content, "rock");
                    Block blocks_23 = new Block(Content, "rock");
                    Block blocks_24 = new Block(Content, "rock");
                    Block blocks_25 = new Block(Content, "rock");
                    Block blocks_37 = new Block(Content, "rock");
                    Block blocks_38 = new Block(Content, "rock");
                    Haggis haggis_1 = new Haggis(Content);
                    Haggis haggis_2 = new Haggis(Content);
                    Eagle eagle_1 = new Eagle(Content);
                    Eagle eagle_2 = new Eagle(Content);
                    Eagle eagle_3 = new Eagle(Content);
                    Thistle thistle_1 = new Thistle(Content);
                    Thistle thistle_2 = new Thistle(Content);
                    blocks_11.block_position = new Vector2(560, 240);
                    blocks_12.block_position = new Vector2(720, 360);
                    blocks_23.block_position = new Vector2(1520, 520);
                    blocks_24.block_position = new Vector2(1640, 520);
                    blocks_25.block_position = new Vector2(1800, 440);
                    blocks_37.block_position = new Vector2(1640, 320);
                    blocks_38.block_position = new Vector2(1800, 240);
                    haggis_1.haggis_sprite.position = new Vector2(241, 250);
                    haggis_2.haggis_sprite.position = new Vector2(520, 250);
                    eagle_1.eagle_position = new Vector2(620, 100);
                    eagle_2.eagle_position = new Vector2(780, 500);
                    eagle_3.eagle_position = new Vector2(1700, 300);
                    thistle_1.thistle_sprite.position = new Vector2(1025, 485);
                    thistle_2.thistle_sprite.position = new Vector2(1225, 485);
                    blocks.Add(blocks_11);
                    blocks.Add(blocks_12);
                    blocks.Add(blocks_23);
                    blocks.Add(blocks_24);
                    blocks.Add(blocks_25);
                    blocks.Add(blocks_37);
                    blocks.Add(blocks_38);
                    haggises.Add(haggis_1);
                    haggises.Add(haggis_2);
                    eagles.Add(eagle_1);
                    eagles.Add(eagle_2);
                    eagles.Add(eagle_3);
                    thistles.Add(thistle_1);
                    thistles.Add(thistle_2);
                }
            }
            else if (level_number == 5)
            {
                lives++;
                player_sprite.position.X = 100;
                bottled_juice.position.Y = 340;
                MediaPlayer.Play(city_music);
                MediaPlayer.IsRepeating = true;
                for (int x = 0; x < 1; x++)
                {
                    Haggis haggis_1 = new Haggis(Content);
                    Haggis haggis_2 = new Haggis(Content);
                    Haggis haggis_3 = new Haggis(Content);
                    Haggis haggis_4 = new Haggis(Content);
                    Haggis haggis_5 = new Haggis(Content);
                    Haggis haggis_6 = new Haggis(Content);
                    Eagle eagle_1 = new Eagle(Content);
                    Eagle eagle_2 = new Eagle(Content);
                    Thistle thistle_1 = new Thistle(Content);
                    haggis_1.haggis_sprite.position = new Vector2(200, 370);
                    haggis_2.haggis_sprite.position = new Vector2(300, 370);
                    haggis_3.haggis_sprite.position = new Vector2(400, 370);
                    haggis_4.haggis_sprite.position = new Vector2(500, 370);
                    haggis_5.haggis_sprite.position = new Vector2(600, 370);
                    haggis_6.haggis_sprite.position = new Vector2(700, 370);
                    eagle_1.eagle_position = new Vector2(1100, 300);
                    eagle_2.eagle_position = new Vector2(2060, 400);
                    thistle_1.thistle_sprite.position = new Vector2(1380, 420);
                    haggises.Add(haggis_1);
                    haggises.Add(haggis_2);
                    haggises.Add(haggis_3);
                    haggises.Add(haggis_4);
                    haggises.Add(haggis_5);
                    haggises.Add(haggis_6);
                    eagles.Add(eagle_1);
                    eagles.Add(eagle_2);
                    thistles.Add(thistle_1);
                }
                for (int x = 0; x < 5; x++)
                {
                    Block blocks_1 = new Block(Content, "scaffolding_lower");
                    Block blocks_2 = new Block(Content, "scaffolding_lower");
                    Block blocks_3 = new Block(Content, "scaffolding_lower");
                    Block blocks_4 = new Block(Content, "scaffolding_lower");
                    Block blocks_5 = new Block(Content, "scaffolding_lower");
                    Block blocks_6 = new Block(Content, "scaffolding_lower");
                    Block blocks_7 = new Block(Content, "scaffolding_top");
                    blocks_1.block_position = new Vector2(0 + x * 40, 560);
                    blocks_2.block_position = new Vector2(0 + x * 40, 520);
                    blocks_3.block_position = new Vector2(0 + x * 40, 480);
                    blocks_4.block_position = new Vector2(0 + x * 40, 440);
                    blocks_5.block_position = new Vector2(0 + x * 40, 400);
                    blocks_6.block_position = new Vector2(0 + x * 40, 360);
                    blocks_7.block_position = new Vector2(0 + x * 40, 320);
                    blocks.Add(blocks_1);
                    blocks.Add(blocks_2);
                    blocks.Add(blocks_3);
                    blocks.Add(blocks_4);
                    blocks.Add(blocks_5);
                    blocks.Add(blocks_6);
                    blocks.Add(blocks_7);
                }
                for (int x = 0; x < 16; x++)
                {
                    Block blocks_8 = new Block(Content, "scaffolding_lower");
                    Block blocks_9 = new Block(Content, "scaffolding_lower");
                    Block blocks_10 = new Block(Content, "scaffolding_lower");
                    Block blocks_11 = new Block(Content, "scaffolding_lower");
                    Block blocks_12 = new Block(Content, "scaffolding_top");
                    blocks_8.block_position = new Vector2(200 + x * 40, 560);
                    blocks_9.block_position = new Vector2(200 + x * 40, 520);
                    blocks_10.block_position = new Vector2(200 + x * 40, 480);
                    blocks_11.block_position = new Vector2(200 + x * 40, 440);
                    blocks_12.block_position = new Vector2(200 + x * 40, 400);
                    blocks.Add(blocks_8);
                    blocks.Add(blocks_9);
                    blocks.Add(blocks_10);
                    blocks.Add(blocks_11);
                    blocks.Add(blocks_12);
                }
                for (int x = 0; x < 6; x++)
                {
                    Block blocks_13 = new Block(Content, "scaffolding_lower");
                    Block blocks_14 = new Block(Content, "scaffolding_lower");
                    Block blocks_15 = new Block(Content, "scaffolding_lower");
                    Block blocks_16 = new Block(Content, "scaffolding_lower");
                    Block blocks_17 = new Block(Content, "scaffolding_lower");
                    Block blocks_18 = new Block(Content, "scaffolding_lower");
                    Block blocks_19 = new Block(Content, "scaffolding_top");
                    Block blocks_31 = new Block(Content, "scaffolding_lower");
                    Block blocks_32 = new Block(Content, "scaffolding_lower");
                    Block blocks_33 = new Block(Content, "scaffolding_lower");
                    Block blocks_34 = new Block(Content, "scaffolding_lower");
                    Block blocks_35 = new Block(Content, "scaffolding_top");
                    blocks_13.block_position = new Vector2(840 + x * 40, 560);
                    blocks_14.block_position = new Vector2(840 + x * 40, 520);
                    blocks_15.block_position = new Vector2(840 + x * 40, 480);
                    blocks_16.block_position = new Vector2(840 + x * 40, 440);
                    blocks_17.block_position = new Vector2(840 + x * 40, 400);
                    blocks_18.block_position = new Vector2(840 + x * 40, 360);
                    blocks_19.block_position = new Vector2(840 + x * 40, 320);
                    blocks_31.block_position = new Vector2(2160 + x * 40, 560);
                    blocks_32.block_position = new Vector2(2160 + x * 40, 520);
                    blocks_33.block_position = new Vector2(2160 + x * 40, 480);
                    blocks_34.block_position = new Vector2(2160 + x * 40, 440);
                    blocks_35.block_position = new Vector2(2160 + x * 40, 400);
                    blocks.Add(blocks_13);
                    blocks.Add(blocks_14);
                    blocks.Add(blocks_15);
                    blocks.Add(blocks_16);
                    blocks.Add(blocks_17);
                    blocks.Add(blocks_18);
                    blocks.Add(blocks_19);
                    blocks.Add(blocks_31);
                    blocks.Add(blocks_32);
                    blocks.Add(blocks_33);
                    blocks.Add(blocks_34);
                    blocks.Add(blocks_35);
                }
                for (int x = 0; x < 4; x++)
                {
                    Block blocks_20 = new Block(Content, "scaffolding_lower");
                    Block blocks_21 = new Block(Content, "scaffolding_lower");
                    Block blocks_22 = new Block(Content, "scaffolding_lower");
                    Block blocks_23 = new Block(Content, "scaffolding_lower");
                    Block blocks_24 = new Block(Content, "scaffolding_top");
                    blocks_20.block_position = new Vector2(1200 + x * 40, 560);
                    blocks_21.block_position = new Vector2(1200 + x * 40, 520);
                    blocks_22.block_position = new Vector2(1200 + x * 40, 480);
                    blocks_23.block_position = new Vector2(1200 + x * 40, 440);
                    blocks_24.block_position = new Vector2(1200 + x * 40, 400);
                    blocks.Add(blocks_20);
                    blocks.Add(blocks_21);
                    blocks.Add(blocks_22);
                    blocks.Add(blocks_23);
                    blocks.Add(blocks_24);
                }
                for (int x = 0; x < 2; x++)
                {
                    Block blocks_27 = new Block(Content, "scaffolding_lower");
                    Block blocks_28 = new Block(Content, "scaffolding_top");
                    Block blocks_29 = new Block(Content, "scaffolding_lower");
                    Block blocks_30 = new Block(Content, "scaffolding_lower");
                    blocks_27.block_position = new Vector2(1960 + x * 40, 480);
                    blocks_28.block_position = new Vector2(1960 + x * 40, 440);
                    blocks_29.block_position = new Vector2(1960 + x * 40, 560);
                    blocks_30.block_position = new Vector2(1960 + x * 40, 520);
                    blocks.Add(blocks_27);
                    blocks.Add(blocks_28);
                    blocks.Add(blocks_29);
                    blocks.Add(blocks_30);
                }
                for (int x = 0; x < 14; x++)
                {
                    Block blocks_25 = new Block(Content, "scaffolding_lower");
                    Block blocks_26 = new Block(Content, "scaffolding_top");
                    blocks_25.block_position = new Vector2(1400 + x * 40, 560);
                    blocks_26.block_position = new Vector2(1400 + x * 40, 520);
                    blocks.Add(blocks_25);
                    blocks.Add(blocks_26);
                }

            }
            else if (level_number == 6)
            {
                MediaPlayer.Play(city_music);
                MediaPlayer.IsRepeating = true;
                player_sprite.position.X = 100;
                bottled_juice.position.Y = 300;
                for (int x = 0; x < 60; x++)
                {
                    Block blocks_1 = new Block(Content, "scaffolding_lower");
                    blocks_1.block_position = new Vector2(0 + x * 40, 560);
                    blocks.Add(blocks_1);
                }
                for (int x = 0; x < 51; x++)
                {
                    Block blocks_2 = new Block(Content, "scaffolding_lower");
                    Block blocks_3 = new Block(Content, "scaffolding_lower");
                    Block blocks_10 = new Block(Content, "scaffolding_lower");
                    Block blocks_11 = new Block(Content, "scaffolding_lower");
                    blocks_2.block_position = new Vector2(0 + x * 40, 520);
                    blocks_3.block_position = new Vector2(0 + x * 40, 480);
                    blocks_10.block_position = new Vector2(0 + x * 40, 440);
                    blocks_11.block_position = new Vector2(0 + x * 40, 400);
                    blocks.Add(blocks_2);
                    blocks.Add(blocks_3);
                    blocks.Add(blocks_10);
                    blocks.Add(blocks_11);
                }
                for (int x = 0; x < 4; x++)
                {
                    Block blocks_4 = new Block(Content, "scaffolding_top");
                    blocks_4.block_position = new Vector2(2040 + x * 40, 520);
                    blocks.Add(blocks_4);
                }
                for (int x = 0; x < 5; x++)
                {
                    Block blocks_5 = new Block(Content, "scaffolding_lower");
                    Block blocks_6 = new Block(Content, "scaffolding_lower");
                    Block blocks_7 = new Block(Content, "scaffolding_lower");
                    Block blocks_8 = new Block(Content, "scaffolding_lower");
                    Block blocks_9 = new Block(Content, "scaffolding_top");
                    Block blocks_12 = new Block(Content, "scaffolding_lower");
                    Block blocks_13 = new Block(Content, "scaffolding_lower");
                    Block blocks_14 = new Block(Content, "scaffolding_lower");
                    Block blocks_15 = new Block(Content, "scaffolding_lower");
                    Block blocks_16 = new Block(Content, "scaffolding_top");
                    Block blocks_40 = new Block(Content, "scaffolding_lower");
                    Block blocks_41 = new Block(Content, "scaffolding_lower");
                    Block blocks_42 = new Block(Content, "scaffolding_lower");
                    Block blocks_43 = new Block(Content, "scaffolding_lower");
                    Block blocks_44 = new Block(Content, "scaffolding_top");
                    blocks_5.block_position = new Vector2(2200 + x * 40, 520);
                    blocks_6.block_position = new Vector2(2200 + x * 40, 480);
                    blocks_7.block_position = new Vector2(2200 + x * 40, 440);
                    blocks_8.block_position = new Vector2(2200 + x * 40, 400);
                    blocks_9.block_position = new Vector2(2200 + x * 40, 360);
                    blocks_12.block_position = new Vector2(0 + x * 40, 360);
                    blocks_13.block_position = new Vector2(0 + x * 40, 320);
                    blocks_14.block_position = new Vector2(0 + x * 40, 280);
                    blocks_15.block_position = new Vector2(0 + x * 40, 240);
                    blocks_16.block_position = new Vector2(0 + x * 40, 200);
                    blocks_40.block_position = new Vector2(1280 + x * 40, 360);
                    blocks_41.block_position = new Vector2(1280 + x * 40, 320);
                    blocks_42.block_position = new Vector2(1280 + x * 40, 280);
                    blocks_43.block_position = new Vector2(1280 + x * 40, 240);
                    blocks_44.block_position = new Vector2(1280 + x * 40, 200);
                    blocks.Add(blocks_5);
                    blocks.Add(blocks_6);
                    blocks.Add(blocks_7);
                    blocks.Add(blocks_8);
                    blocks.Add(blocks_9);
                    blocks.Add(blocks_12);
                    blocks.Add(blocks_13);
                    blocks.Add(blocks_14);
                    blocks.Add(blocks_15);
                    blocks.Add(blocks_16);
                    blocks.Add(blocks_40);
                    blocks.Add(blocks_41);
                    blocks.Add(blocks_42);
                    blocks.Add(blocks_43);
                    blocks.Add(blocks_44);
                }
                for (int x = 0; x < 7; x++)
                {
                    Block blocks_17 = new Block(Content, "scaffolding_lower");
                    Block blocks_19 = new Block(Content, "scaffolding_lower");
                    Block blocks_18 = new Block(Content, "scaffolding_top");
                    blocks_17.block_position = new Vector2(200 + x * 40, 320);
                    blocks_19.block_position = new Vector2(200 + x * 40, 360);
                    blocks_18.block_position = new Vector2(200 + x * 40, 280);
                    blocks.Add(blocks_17);
                    blocks.Add(blocks_18);
                    blocks.Add(blocks_19);
                }
                for (int x = 0; x < 2; x++)
                {
                    Block blocks_20 = new Block(Content, "scaffolding_lower");
                    Block blocks_21 = new Block(Content, "scaffolding_lower");
                    Block blocks_22 = new Block(Content, "scaffolding_lower");
                    Block blocks_23 = new Block(Content, "scaffolding_lower");
                    Block blocks_24 = new Block(Content, "scaffolding_lower");
                    Block blocks_25 = new Block(Content, "scaffolding_top");
                    Block blocks_29 = new Block(Content, "scaffolding_lower");
                    Block blocks_30 = new Block(Content, "scaffolding_lower");
                    Block blocks_31 = new Block(Content, "scaffolding_lower");
                    Block blocks_32 = new Block(Content, "scaffolding_lower");
                    Block blocks_33 = new Block(Content, "scaffolding_top");
                    Block blocks_34 = new Block(Content, "scaffolding_lower");
                    Block blocks_35 = new Block(Content, "scaffolding_lower");
                    Block blocks_36 = new Block(Content, "scaffolding_top");
                    blocks_20.block_position = new Vector2(480 + x * 40, 360);
                    blocks_21.block_position = new Vector2(480 + x * 40, 320);
                    blocks_22.block_position = new Vector2(480 + x * 40, 280);
                    blocks_23.block_position = new Vector2(480 + x * 40, 240);
                    blocks_24.block_position = new Vector2(480 + x * 40, 200);
                    blocks_25.block_position = new Vector2(480 + x * 40, 160);
                    blocks_29.block_position = new Vector2(680 + x * 40, 360);
                    blocks_30.block_position = new Vector2(680 + x * 40, 320);
                    blocks_31.block_position = new Vector2(680 + x * 40, 280);
                    blocks_32.block_position = new Vector2(680 + x * 40, 240);
                    blocks_33.block_position = new Vector2(680 + x * 40, 200);
                    blocks_34.block_position = new Vector2(760 + x * 40, 360);
                    blocks_35.block_position = new Vector2(760 + x * 40, 320);
                    blocks_36.block_position = new Vector2(760 + x * 40, 280);
                    blocks.Add(blocks_20);
                    blocks.Add(blocks_21);
                    blocks.Add(blocks_22);
                    blocks.Add(blocks_23);
                    blocks.Add(blocks_24);
                    blocks.Add(blocks_25);
                    blocks.Add(blocks_29);
                    blocks.Add(blocks_30);
                    blocks.Add(blocks_31);
                    blocks.Add(blocks_32);
                    blocks.Add(blocks_33);
                    blocks.Add(blocks_34);
                    blocks.Add(blocks_35);
                    blocks.Add(blocks_36);
                }
                for (int x = 0; x < 10; x++)
                {
                    Block blocks_37 = new Block(Content, "scaffolding_top");
                    Block blocks_45 = new Block(Content, "scaffolding_lower");
                    Block blocks_46 = new Block(Content, "scaffolding_top");
                    blocks_37.block_position = new Vector2(840 + x * 40, 360);
                    blocks_45.block_position = new Vector2(1640 + x * 40, 360);
                    blocks_46.block_position = new Vector2(1640 + x * 40, 320);
                    blocks.Add(blocks_37);
                    blocks.Add(blocks_45);
                    blocks.Add(blocks_46);
                }
                for (int x = 0; x < 1; x++)
                {
                    Block blocks_38 = new Block(Content, "scaffolding_lower");
                    Block blocks_39 = new Block(Content, "scaffolding_top");
                    Haggis haggis_1 = new Haggis(Content);
                    Haggis haggis_2 = new Haggis(Content);
                    Haggis haggis_3 = new Haggis(Content);
                    Haggis haggis_4 = new Haggis(Content);
                    Thistle thistle_1 = new Thistle(Content);
                    Thistle thistle_2 = new Thistle(Content);
                    Thistle thistle_3 = new Thistle(Content);
                    Thistle thistle_4 = new Thistle(Content);
                    Thistle thistle_5 = new Thistle(Content);
                    Eagle eagle_1 = new Eagle(Content);
                    Eagle eagle_2 = new Eagle(Content);
                    Eagle eagle_3 = new Eagle(Content);
                    Eagle eagle_4 = new Eagle(Content);
                    Eagle eagle_5 = new Eagle(Content);
                    Shortbread shortbread_1 = new Shortbread(Content);
                    haggis_1.haggis_sprite.position = new Vector2(202, 250);
                    haggis_2.haggis_sprite.position = new Vector2(400, 250);
                    haggis_3.haggis_sprite.position = new Vector2(842, 330);
                    haggis_4.haggis_sprite.position = new Vector2(1160, 330);
                    thistle_1.thistle_sprite.position = new Vector2(560, 180);
                    thistle_2.thistle_sprite.position = new Vector2(1480, 300);
                    thistle_3.thistle_sprite.position = new Vector2(1515, 300);
                    thistle_4.thistle_sprite.position = new Vector2(2050, 420);
                    thistle_5.thistle_sprite.position = new Vector2(2130, 420);
                    eagle_1.eagle_position = new Vector2(600, 100);
                    eagle_2.eagle_position = new Vector2(1280, 10);
                    eagle_3.eagle_position = new Vector2(2040, 10);
                    eagle_4.eagle_position = new Vector2(2120, 10);
                    eagle_5.eagle_position = new Vector2(2070, 400);
                    shortbread_1.shortbread_position = new Vector2(2240, 330);
                    blocks_38.block_position = new Vector2(1240, 360);
                    blocks_39.block_position = new Vector2(1240, 320);
                    blocks.Add(blocks_38);
                    blocks.Add(blocks_39);
                    haggises.Add(haggis_1);
                    haggises.Add(haggis_2);
                    haggises.Add(haggis_3);
                    haggises.Add(haggis_4);
                    thistles.Add(thistle_1);
                    thistles.Add(thistle_2);
                    thistles.Add(thistle_3);
                    thistles.Add(thistle_4);
                    thistles.Add(thistle_5);
                    eagles.Add(eagle_1);
                    eagles.Add(eagle_2);
                    eagles.Add(eagle_3);
                    eagles.Add(eagle_4);
                    eagles.Add(eagle_5);
                    shortbreads.Add(shortbread_1);
                }
                for (int x = 0; x < 3; x++)
                {
                    Block blocks_26 = new Block(Content, "scaffolding_lower");
                    Block blocks_27 = new Block(Content, "scaffolding_lower");
                    Block blocks_28 = new Block(Content, "scaffolding_top");
                    blocks_26.block_position = new Vector2(560 + x * 40, 360);
                    blocks_27.block_position = new Vector2(560 + x * 40, 320);
                    blocks_28.block_position = new Vector2(560 + x * 40, 280);
                    blocks.Add(blocks_26);
                    blocks.Add(blocks_27);
                    blocks.Add(blocks_28);
                }
            }
            else if (level_number == 7)
            {
                level_number = 0;
                Load_Level();
                hard = true;
                speedup = true;
                lives = 1;
            }


            foreach (Haggis x in haggises)
            {
                if (speedup == true)
                {
                    x.haggis_velo = 0.4f;
                }
                else
                {
                    x.haggis_velo = 0.2f;
                }
            }

            foreach (Eagle x in eagles)
            {
                if (speedup == true)
                {
                    x.eagle_velo = 0.5f;
                }
                else
                {
                    x.eagle_velo = 0.25f;
                }
            }

        }

        protected void Clear()
        {
            blocks.Clear();
            haggises.Clear();
            eagles.Clear();
            thistles.Clear();
            shortbreads.Clear();
        }
    }
}
