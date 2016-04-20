using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Media;

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

        sprite player_sprite;

        sprite gerard_sprite;

        sprite nessie_sprite;

        sprite[] backgrounds = new sprite[7];

        sprite[] buttons = new sprite[4];
        sprite[] buttons_selected = new sprite[4];

        sprite[] option_buttons = new sprite[3];
        sprite[] options_selected = new sprite[3];

        sprite back_button;
        sprite back_selected;

        Song[] background_music = new Song[7];

        Song main_music;

        List<Haggis> haggises = new List<Haggis>();
        List<Thistle> thistles = new List<Thistle>();
        List<Eagle> eagles = new List<Eagle>();
        List<Shortbread> shortbreads = new List<Shortbread>();
        List<Block> blocks = new List<Block>();

        int screen_width = 800;
        int screen_height = 600;

        int level_number = 0;
        float gravity = 0.1f;
        int current_time;

        int defaultlives = 3;
        int lives = 3;
        int score = 0;

        const int button_timer = 500;
        int button_timercount = 0;
        const int life_cooldown = 2000;
        int current_lcooldown = 0;
        const int jump_timer = 300;
        int jump_current = 0;

        bool menu = true;
        bool leaderboard = false;
        bool options = false;
        bool gameon = false;

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

            info = Content.Load<SpriteFont>("SpriteFont");

            player_camera = new Camera(GraphicsDevice.Viewport);

            background_main = new sprite(Content, "main_menu", 0,0, 1);
            backgrounds[0] = new sprite(Content, "background_blueclouds", 0, 0, 1);
            backgrounds[1] = new sprite(Content, "Background2", 0, 0, 1);
            backgrounds[2] = new sprite(Content, "Background2", 0, 0, 1);
            backgrounds[3] = new sprite(Content, "Background2", 0, 0, 1);
            backgrounds[4] = new sprite(Content, "Background2", 0, 0, 1);
            backgrounds[5] = new sprite(Content, "Background2", 0, 0, 1);
            backgrounds[6] = new sprite(Content, "Background2", 0, 0, 1);

            //main_music = Content.Load<Song>("");
            //background_music[0] = Content.Load<Song>("");
            //background_music[1] = Content.Load<Song>("");
            //background_music[2] = Content.Load<Song>("");
            //background_music[3] = Content.Load<Song>("");
            //background_music[4] = Content.Load<Song>("");
            //background_music[5] = Content.Load<Song>("");
            //background_music[6] = Content.Load<Song>("");


            buttons[0] = new sprite(Content, "button_play", (screen_width * 3/8), (screen_height * 1/5 - 30), 1);
            buttons[1] = new sprite(Content, "button_leaderboard", (screen_width * 3/8), (screen_height * 2 / 5 - 30), 1);
            buttons[2] = new sprite(Content, "button_options", (screen_width * 3 / 8), (screen_height * 3 / 5 - 30), 1);
            buttons[3] = new sprite(Content, "button_exit", (screen_width * 3 / 8), (screen_height * 4 / 5 - 30), 1);

            back_button = new sprite(Content, "button_back", (screen_width * 3 / 8), (screen_height * 17 / 20 - screen_height / 10), 1);

            option_buttons[0] = new sprite(Content, "button_easy", (screen_width * 3 / 8), (screen_height * 1 / 4 - 30), 1);
            option_buttons[1] = new sprite(Content, "button_hard", (screen_width * 3 / 8), (screen_height * 1 / 2 - 30), 1);
            option_buttons[2] = new sprite(Content, "button_back", (screen_width * 3 / 8), (screen_height * 3 / 4 - 30), 1);

            player_sprite = new sprite(Content, "player", 200, screen_height - 500, 1);
            //gerard_sprite = new sprite(Content, "", 40, screen_height - 50, 1);
            //nessie_sprite = new sprite(Content, "", 100, screen_height - 50, 1);

            info_position = new Vector2(50, 20);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gameon = false;
                options = false;
                leaderboard = false;
                menu = true;
            }
            current_time = gameTime.ElapsedGameTime.Milliseconds;
            var controller = GamePad.GetState(PlayerIndex.One);
            var controls = Keyboard.GetState();
            var mouse = Mouse.GetState();
            
            Rectangle mouse_box = new Rectangle((int)mouse.X,(int)mouse.Y, 1, 1);
            button_timercount += current_time;

            if(menu == true)
            {
                player_camera.Position = new Vector2(0, 0);
                IsMouseVisible = true;
                Rectangle button_box1 = new Rectangle((int)buttons[0].position.X, (int)buttons[0].position.Y, buttons[0].graphic.Width, buttons[0].graphic.Height);
                Rectangle button_box2 = new Rectangle((int)buttons[1].position.X, (int)buttons[1].position.Y, buttons[1].graphic.Width, buttons[1].graphic.Height);
                Rectangle button_box3 = new Rectangle((int)buttons[2].position.X, (int)buttons[2].position.Y, buttons[2].graphic.Width, buttons[2].graphic.Height);
                Rectangle button_box4 = new Rectangle((int)buttons[3].position.X, (int)buttons[3].position.Y, buttons[3].graphic.Width, buttons[3].graphic.Height);

                //MediaPlayer.Play(main_music);

                //if((mouse_box.Intersects(button_box1)))
                //{
                //    buttons[0] = new sprite(Content, "play_selected", (screen_width * 3/8), (screen_height * 1/5 - 30), 1);
                //}
                if((mouse_box.Intersects(button_box1)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    gameon = true;
                    menu = false;

                    level_number = 0;
                    score = 0;
                    lives = defaultlives;
                    Load_Level();
                }

                //if((mouse_box.Intersects(button_box2)))
                //{
                //    buttons[1] = new sprite(Content, "leaderboard_selected", (screen_width * 3 / 8), (screen_height * 2 / 5 - 30), 1);
                //}
                if((mouse_box.Intersects(button_box2)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    leaderboard = true;
                    menu = false;
                }

                //if ((mouse_box.Intersects(button_box3)))
                //{
                //    buttons[2] = new sprite(Content, "options_selected", (screen_width * 3 / 8), (screen_height * 3 / 5 - 30), 1);
                //}
                if ((mouse_box.Intersects(button_box3)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    options = true;
                    menu = false;
                    button_timercount = 0;
                }

                //if ((mouse_box.Intersects(button_box4)))
                //{
                //    buttons[3] = new sprite(Content, "exit_selected", (screen_width * 3 / 8), (screen_height * 4 / 5 - 30), 1);
                //}
                if ((mouse_box.Intersects(button_box4)) && (mouse.LeftButton == ButtonState.Pressed) && (button_timercount >= button_timer))
                {
                    Exit();
                }
            }

            else if (leaderboard == true)
            {

                Rectangle back_box = new Rectangle((int)back_button.position.X, (int)back_button.position.Y, back_button.graphic.Width, back_button.graphic.Height);

                //if ((mouse_box.Intersects(back_box)))
                //{
                //    back_button = new sprite(Content, "back_selected", (screen_width * 3 / 8), (screen_height * 17 / 20 - screen_height / 10), 1);
                //}
                if ((mouse_box.Intersects(back_box)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    menu = true;
                    leaderboard = false;
                    button_timercount = 0;
                }

                //MediaPlayer.Play(main_music);

                //if (File.Exists(@"Gamehighscores.txt"))
                //{
                //    StreamReader file = new StreamReader(@"Gamehighscores.txt");

                //}
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

                //MediaPlayer.Play(main_music);

                //if ((mouse_box.Intersects(option_box1)))
                //{
                //    option_buttons[0] = new sprite(Content, "easy_selected", (screen_width * 3 / 8), (screen_height * 1 / 4 - 30), 1);
                //}
                if ((mouse_box.Intersects(option_box1)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    defaultlives = 3;
                }

                //if ((mouse_box.Intersects(option_box2)))
                //{
                //    option_buttons[1] = new sprite(Content, "hard_selected", (screen_width * 3 / 8), (screen_height * 1 / 2 - 30), 1);
                //}
                if ((mouse_box.Intersects(option_box2)) && (mouse.LeftButton == ButtonState.Pressed) && (button_timercount >= button_timer))
                {
                    defaultlives = 1;
                }

                //if ((mouse_box.Intersects(option_box3)))
                //{
                //    option_buttons[2] = new sprite(Content, "back_selected", (screen_width * 3 / 8), (screen_height * 3 / 4 - 30), 1);
                //}
                if ((mouse_box.Intersects(option_box3)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    menu = true;
                    options = false;
                    button_timercount = 0;
                }

            }


            else if (gameon == true)
            {
                IsMouseVisible = false;
                player_camera.Position = new Vector2(player_sprite.position.X - 200, 0);
                Rectangle player_box = new Rectangle((int)player_sprite.position.X, (int)player_sprite.position.Y, player_sprite.graphic.Width, player_sprite.graphic.Height);

                Eagle eagle_hit = null;
                Haggis haggis_hit = null;
                Shortbread shortbread_hit = null;
                Thistle thistle_hit = null;
                Block block_hit = null;

                player_sprite.position.Y += gravity * current_time;

                if (player_sprite.position.X < 200)
                {
                    player_camera.Position = new Vector2 (0,0);
                    info_position.X = 50;
                }
                if (player_sprite.position.X < 0)
                {
                    player_sprite.position.X = 0;
                }

                if (player_sprite.position.X > 1800)
                {
                    player_camera.Position = new Vector2(1600, 0);
                    info_position = new Vector2(1650, 20);
                }
                if (player_sprite.position.X > 2300)
                {
                    player_camera.Position = new Vector2(0, 0);
                    player_sprite.position.X = 200;
                    info_position = new Vector2(50, 20);
                    score += 200;
                    Load_Level();
                }

                if (player_sprite.position.Y < 0)
                {
                    player_sprite.position.Y = 0;
                }

                foreach (Block x in blocks)
                {
                    x.block_box = new Rectangle((int)x.block_position.X, (int)x.block_position.Y, (int)x.block_sprite.Width, (int)x.block_sprite.Height);

                    if (player_box.Intersects(x.block_box))
                    {
                        gravity = 0;
                        jump_current = 0;
                        block_hit = x;
                    }
                    //else
                    //{
                    //    gravity = 0.1f;
                    //}

                    //if (player_box.Intersects(x.block_box) && (player_sprite.position.X > x.block_position.X - player_sprite.graphic.Width))
                    //{
                    //    player_sprite.position.X = x.block_position.X - player_sprite.graphic.Width;
                    //}
                }


                if (controls.IsKeyDown(Keys.Space) || (controller.Buttons.A == ButtonState.Pressed))
                {
                    jump_current += current_time;

                    if (jump_current <= jump_timer)
                    {
                        gravity = -0.25f;
                    }
                }

                if (controls.IsKeyDown(Keys.A) || (controller.DPad.Left == ButtonState.Pressed) || (controller.ThumbSticks.Left.X < -0.1))
                {
                    player_sprite.position.X -= 1.5f;
                    player_camera.Position -= new Vector2(1.5f,0);
                    info_position.X -= 1.5f;
                }

                if (controls.IsKeyDown(Keys.D) || (controller.DPad.Right == ButtonState.Pressed) || (controller.ThumbSticks.Left.X > 0.1))
                {
                    player_sprite.position.X += 1.5f;
                    player_camera.Position += new Vector2(1.5f, 0);
                    info_position.X += 1.5f;
                }

                foreach (Eagle x in eagles)
                {
                    x.eagle_box = new Rectangle ((int)x.eagle_position.X, (int)x.eagle_position.Y, x.eagle_sprite.Width, x.eagle_sprite.Height);

                    if(x.eagle_box.Intersects(player_box) && current_lcooldown >= life_cooldown)
                    {
                        lives -= 1;
                        eagle_hit = x;
                        current_lcooldown = 0;
                    }
                }

                foreach (Haggis x in haggises)
                {
                    x.haggis_box = new Rectangle((int)x.haggis_position.X, (int)x.haggis_position.Y, x.haggis_sprite.Width, x.haggis_sprite.Height);

                    x.haggis_position.X += x.haggis_velo * current_time;

                    if(x.haggis_box.Intersects(player_box) && (player_sprite.position.Y + player_sprite.graphic.Height == x.haggis_position.Y))
                    {
                        haggis_hit = x;
                    }

                    if (x.haggis_box.Intersects(player_box) && (player_sprite.position.Y + player_sprite.graphic.Height != x.haggis_position.Y) && current_lcooldown >= life_cooldown)
                    {
                        lives--;
                        current_lcooldown = 0;
                    }

                    foreach(Block b in blocks)
                    {
                        b.block_box = new Rectangle((int)b.block_position.X, (int)b.block_position.Y, (int)b.block_sprite.Width, (int)b.block_sprite.Height);

                        if (x.haggis_box.Intersects(b.block_box))
                        {
                            x.haggis_velo *= -1;
                        }
                    }

                }

                foreach (Shortbread x in shortbreads)
                {
                    x.sbread_box = new Rectangle((int)x.shortbread_position.X, (int)x.shortbread_position.Y, x.shortbread_sprite.Width, x.shortbread_sprite.Height);

                    if (x.sbread_box.Intersects(player_box))
                    {
                        shortbread_hit = x;
                    }
                }

                foreach (Thistle x in thistles)
                {
                    x.thistle_box = new Rectangle((int)x.thistle_position.X, (int)x.thistle_position.Y, x.thistle_sprite.Width, x.thistle_sprite.Height);
                }

                if (shortbread_hit != null)
                {
                    score += 50;
                    shortbreads.Remove(shortbread_hit);
                }

                if (haggis_hit != null)
                {
                    score += 15;
                    haggises.Remove(haggis_hit);
                }

                //for (int i = 0; i < level_number; i++)
                //{
                //    MediaPlayer.Play(background_music[i]);
                //}
            }



            // TODO: Add your update logic here

            base.Update(gameTime);
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

            if (leaderboard == true)
            {
                background_main.DrawRectangle(spriteBatch, screen_width, screen_height);
                spriteBatch.DrawString(info, "High Scores", new Vector2 ((screen_width / 2) - (screen_width / 20), (screen_height * 1/4 - screen_height / 10)), Color.Red);
                back_button.DrawRectangle(spriteBatch, buttons[1].graphic.Width, buttons[1].graphic.Height);
            }

            if (options == true)
            {
                background_main.DrawRectangle(spriteBatch, screen_width, screen_height);
                for (int i = 0; i < option_buttons.Length; i++)
                {
                    option_buttons[i].DrawRectangle(spriteBatch, buttons[1].graphic.Width, buttons[1].graphic.Height);
                }
            }

            if (gameon == true)
            {

                for (int i = 0; i < level_number; i++)
                {
                    backgrounds[i].DrawRectangle(spriteBatch, 2400, 600);
                }

                    player_sprite.DrawNormal(spriteBatch);

                    if (level_number == 7)
                    {
                        gerard_sprite.DrawNormal(spriteBatch);
                        nessie_sprite.DrawNormal(spriteBatch);
                    }

                    spriteBatch.DrawString(info, "Lives: " + lives, info_position, Color.Black);
                    spriteBatch.DrawString(info, "Score: " + score, new Vector2(info_position.X + 200, info_position.Y), Color.Black);

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

            spriteBatch.End(); 
            base.Draw(gameTime);
        }

        protected void Load_Level()
        {
            Clear();
            level_number++;

            if (level_number == 1)
            {
                for (int x = 0; x < 1; x++)
                {
                    Block blocks_17 = new Block(Content);
                    blocks_17.block_position = new Vector2(1100 + x * 40, 240);
                    blocks.Add(blocks_17);
                }
                for (int x = 0; x < 12; x++)
                {
                    Block blocks_1 = new Block(Content);
                    Block blocks_2 = new Block(Content);
                    Block blocks_5 = new Block(Content);
                    Block blocks_6 = new Block(Content);
                    Block blocks_7 = new Block(Content);
                    blocks_1.block_position = new Vector2(20 + x * 40, 580);
                    blocks_2.block_position = new Vector2(20 + x * 40, 540);
                    blocks_5.block_position = new Vector2(580 + x * 40, 580);
                    blocks_6.block_position = new Vector2(580 + x * 40, 540);
                    blocks_7.block_position = new Vector2(580 + x * 40, 500);
                    blocks.Add(blocks_1);
                    blocks.Add(blocks_2);
                    blocks.Add(blocks_5);
                    blocks.Add(blocks_6);
                    blocks.Add(blocks_7);
                }
                for (int x = 0; x < 2; x++)
                {
                    Block blocks_3 = new Block(Content);
                    Block blocks_4 = new Block(Content);
                    Block blocks_8 = new Block(Content);
                    Block blocks_11 = new Block(Content);
                    Block blocks_12 = new Block(Content);
                    Block blocks_13 = new Block(Content);
                    Block blocks_14 = new Block(Content);
                    Block blocks_15 = new Block(Content);
                    Block blocks_16 = new Block(Content);
                    Block blocks_23 = new Block(Content);
                    Block blocks_24 = new Block(Content);
                    Block blocks_25 = new Block(Content);
                    Block blocks_26 = new Block(Content);
                    Block blocks_27 = new Block(Content);
                    Block blocks_28 = new Block(Content);
                    blocks_3.block_position = new Vector2(420 + x * 40, 500);
                    blocks_4.block_position = new Vector2(420 + x * 40, 460);
                    blocks_8.block_position = new Vector2(580 + x * 40, 460);
                    blocks_11.block_position = new Vector2(1140 + x * 40, 580);
                    blocks_12.block_position = new Vector2(1140 + x * 40, 540);
                    blocks_13.block_position = new Vector2(1140 + x * 40, 500);
                    blocks_14.block_position = new Vector2(1140 + x * 40, 460);
                    blocks_15.block_position = new Vector2(1140 + x * 40, 420);
                    blocks_16.block_position = new Vector2(980 + x * 40, 300);
                    blocks_23.block_position = new Vector2(1620 + x * 40, 580);
                    blocks_24.block_position = new Vector2(1620 + x * 40, 540);
                    blocks_25.block_position = new Vector2(1620 + x * 40, 500);
                    blocks_26.block_position = new Vector2(1620 + x * 40, 460);
                    blocks_27.block_position = new Vector2(1620 + x * 40, 420);
                    blocks_28.block_position = new Vector2(1620 + x * 40, 380);
                    blocks.Add(blocks_3);
                    blocks.Add(blocks_4);
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
                }
                for (int x = 0; x < 4; x++)
                {
                    Block blocks_9 = new Block(Content);
                    Block blocks_10 = new Block(Content);
                    blocks_9.block_position = new Vector2(900 + x * 40, 460);
                    blocks_10.block_position = new Vector2(900 + x * 40, 420);
                    blocks.Add(blocks_9);
                    blocks.Add(blocks_10);
                }
                for (int x = 0; x < 6; x++)
                {
                    Block blocks_18 = new Block(Content);
                    Block blocks_19 = new Block(Content);
                    Block blocks_20 = new Block(Content);
                    Block blocks_21 = new Block(Content);
                    Block blocks_22 = new Block(Content);
                    Block blocks_29 = new Block(Content);
                    Block blocks_30 = new Block(Content);
                    Block blocks_31 = new Block(Content);
                    blocks_18.block_position = new Vector2(1300 + x * 40, 580);
                    blocks_19.block_position = new Vector2(1300 + x * 40, 540);
                    blocks_20.block_position = new Vector2(1300 + x * 40, 500);
                    blocks_21.block_position = new Vector2(1300 + x * 40, 460);
                    blocks_22.block_position = new Vector2(1300 + x * 40, 420);
                    blocks_29.block_position = new Vector2(1780 + x * 40, 580);
                    blocks_30.block_position = new Vector2(1780 + x * 40, 540);
                    blocks_31.block_position = new Vector2(1780 + x * 40, 500);
                    blocks.Add(blocks_18);
                    blocks.Add(blocks_19);
                    blocks.Add(blocks_20);
                    blocks.Add(blocks_21);
                    blocks.Add(blocks_22);
                    blocks.Add(blocks_29);
                    blocks.Add(blocks_30);
                    blocks.Add(blocks_31);
                }
                for (int x = 0; x < 10; x++)
                {
                    Block blocks_32 = new Block(Content);
                    Block blocks_33 = new Block(Content);
                    Block blocks_34 = new Block(Content);
                    Block blocks_35 = new Block(Content);
                    blocks_32.block_position = new Vector2(2100 + x * 40, 580);
                    blocks_33.block_position = new Vector2(2100 + x * 40, 540);
                    blocks_34.block_position = new Vector2(2100 + x * 40, 500);
                    blocks_35.block_position = new Vector2(2100 + x * 40, 460);
                    blocks.Add(blocks_32);
                    blocks.Add(blocks_33);
                    blocks.Add(blocks_34);
                    blocks.Add(blocks_35);
                }
            }

            else if (level_number == 2)
            { 
            
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
