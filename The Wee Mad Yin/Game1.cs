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

        sprite[] option_buttons = new sprite[3];

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

        int lives = 3;
        int score = 0;

        const int button_timer = 1000;
        int button_timercount = 0;

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


            buttons[0] = new sprite(Content, "Button", (screen_width / 2) - (screen_width / 10), (screen_height * 1/5 - screen_height / 10), 1);
            buttons[1] = new sprite(Content, "Button", (screen_width / 2) - (screen_width / 10), (screen_height * 2/5 - screen_height / 10), 1);
            buttons[2] = new sprite(Content, "Button", (screen_width / 2) - (screen_width / 10), (screen_height * 3/5 - screen_height / 10), 1);
            buttons[3] = new sprite(Content, "Button", (screen_width / 2) - (screen_width / 10), (screen_height * 4/5 - screen_height / 10), 1);

            option_buttons[0] = new sprite(Content, "Button", (screen_width / 2) - (screen_width / 10), (screen_height * 1/4 - screen_height / 10), 1);
            option_buttons[1] = new sprite(Content, "Button", (screen_width / 2) - (screen_width / 10), (screen_height * 1/2 - screen_height / 10), 1);
            option_buttons[2] = new sprite(Content, "Button", (screen_width / 2) - (screen_width / 10), (screen_height * 3/4 - screen_height / 10), 1);

            player_sprite = new sprite(Content, "Cactus", 0, screen_height - 50, 1);
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

            var controls = Keyboard.GetState();
            var mouse = Mouse.GetState();
            
            Rectangle mouse_box = new Rectangle((int)mouse.X,(int)mouse.Y, 1, 1);
            button_timercount += gameTime.ElapsedGameTime.Milliseconds;

            if(menu == true)
            {
                IsMouseVisible = true;
                Rectangle button_box1 = new Rectangle((int)buttons[0].position.X, (int)buttons[0].position.Y, buttons[0].graphic.Width, buttons[0].graphic.Height);
                Rectangle button_box2 = new Rectangle((int)buttons[1].position.X, (int)buttons[1].position.Y, buttons[1].graphic.Width, buttons[1].graphic.Height);
                Rectangle button_box3 = new Rectangle((int)buttons[2].position.X, (int)buttons[2].position.Y, buttons[2].graphic.Width, buttons[2].graphic.Height);
                Rectangle button_box4 = new Rectangle((int)buttons[3].position.X, (int)buttons[3].position.Y, buttons[3].graphic.Width, buttons[3].graphic.Height);

                //MediaPlayer.Play(main_music);

                if((mouse_box.Intersects(button_box1)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    gameon = true;
                    menu = false;
                    level_number = 1;
                }

                if((mouse_box.Intersects(button_box2)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    leaderboard = true;
                    menu = false;
                }

                if ((mouse_box.Intersects(button_box3)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    options = true;
                    menu = false;
                }

                if ((mouse_box.Intersects(button_box4)) && (mouse.LeftButton == ButtonState.Pressed) && (button_timercount >= button_timer))
                {
                    Exit();
                }
            }

            ////if (leaderboard == true)
            ////{

            //      MediaPlayer.Play(main_music);

            ////    if (File.Exists(@"Gamehighscores.txt"))
            ////    {
            ////        StreamReader file = new StreamReader(@"Gamehighscores.txt");

            ////    }
            ////}

            if (options == true)
            {
                IsMouseVisible = true;
                Rectangle option_box1 = new Rectangle((int)option_buttons[0].position.X, (int)option_buttons[0].position.Y,
                    option_buttons[0].graphic.Width, option_buttons[0].graphic.Height);
                Rectangle option_box2 = new Rectangle((int)option_buttons[1].position.X, (int)option_buttons[1].position.Y,
                    option_buttons[1].graphic.Width, option_buttons[1].graphic.Height);
                Rectangle option_box3 = new Rectangle((int)option_buttons[2].position.X, (int)option_buttons[2].position.Y,
                    option_buttons[2].graphic.Width, option_buttons[2].graphic.Height);

                //MediaPlayer.Play(main_music);

                if ((mouse_box.Intersects(option_box1)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    lives = 3;
                }

                if ((mouse_box.Intersects(option_box2)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    lives = 1;
                }

                if ((mouse_box.Intersects(option_box3)) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    menu = true;
                    options = false;
                    button_timercount = 0;
                }

            }


            if (gameon == true)
            {

                if (player_sprite.position.X < 0)
                {
                    player_sprite.position.X = 0;
                }

                if (player_sprite.position.X == 1600)
                {
                    player_camera.Position = new Vector2(0, 0);
                    player_sprite.position.X = 0;
                    level_number += 1;
                }

                if (controls.IsKeyDown(Keys.A) && player_sprite.position.X > 0)
                {
                    player_sprite.position.X -= 1;
                    player_camera.Position -= new Vector2(1,0);
                    info_position.X -= 1;
                    //level_number = 1;
                }
                if (controls.IsKeyDown(Keys.D) && player_sprite.position.X < 2000)
                {
                    player_sprite.position.X += 1;
                    player_camera.Position += new Vector2(1, 0);
                    info_position.X += 1;
                    //level_number = 2;
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
                    buttons[i].DrawRectangle(spriteBatch, screen_width / 5, screen_height / 5);
                }
            }

            if (leaderboard == true)
            {
                background_main.DrawRectangle(spriteBatch, screen_width, screen_height);
                spriteBatch.DrawString(info, "High Scores", new Vector2 ((screen_width / 2) - (screen_width / 20), (screen_height * 1/4 - screen_height / 10)), Color.Red);
            }

            if (options == true)
            {
                background_main.DrawRectangle(spriteBatch, screen_width, screen_height);
                for (int i = 0; i < option_buttons.Length; i++)
                {
                    option_buttons[i].DrawRectangle(spriteBatch, screen_width / 5, screen_height / 5);
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

                    spriteBatch.DrawString(info, "Lives: " + lives, info_position, Color.White);
                    spriteBatch.DrawString(info, "Score: " + score, new Vector2(info_position.X + 200, info_position.Y), Color.White);
            }

            foreach(Eagle x in eagles)
            {
                x.Draw_Eagle(spriteBatch);
            }

            foreach(Haggis x in haggises)
            {
                x.Draw_Haggis(spriteBatch);
            }

            foreach(Shortbread x in shortbreads)
            {
                x.Draw_Shortbread(spriteBatch);
            }

            foreach(Thistle x in thistles)
            {
                x.Draw_Thistle(spriteBatch);
            }

            foreach(Block x in blocks)
            {
                x.Draw_Block(spriteBatch);
            }

            spriteBatch.End(); 
            base.Draw(gameTime);
        }

        protected void Level_1()
        {

        }

    }
}
