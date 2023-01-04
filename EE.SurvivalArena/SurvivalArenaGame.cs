using EE.PoolingSystem;
using EE.ScoreSystem;
using EE.SoundSystem;
using EE.SpriteRendererSystem;
using MainMenuSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SurvivalArena.ColliderSystem;
using SurvivalArena.GameObjects;
using SurvivalArena.TileSystem;

namespace SurvivalArena {
    public class GameSettings {
        public int GAMEWIDTH = 640;
        public int GAMEHEIGHT = 480;
        public int SCREENWIDTH = 1920;
        public int SCREENHEIGHT = 1080;

        public event Action<int,int,int,int> NewScrren;
        public event Action FullScreenEvent;

        public int currentSetting = 0;

        public void ChangeSize() {
            NewScrren?.Invoke(GAMEWIDTH, GAMEHEIGHT, SCREENWIDTH, SCREENHEIGHT);
        }
        public void ChangeFullScreen() {
            FullScreenEvent?.Invoke();
        }
        public GameSettings(int gAMEWIDTH, int gAMEHEIGHT, int sCREENWIDTH, int sCREENHEIGHT) {
            SCREENWIDTH = sCREENWIDTH;
            SCREENHEIGHT = sCREENHEIGHT;
            GAMEWIDTH = gAMEWIDTH;
            GAMEHEIGHT = gAMEHEIGHT;
        }
    }
    public class SurvivalArenaGame : IGame {

        public enum GameState { 
            Running,
            GameOver,
            Win,
            MainMenu,
            Pause
        }
        Level level;
        SpriteFont font;
        Vector2 scorePosition;

        ContentManager contentManager;
        public static GameState gameState = GameState.MainMenu;

        public static TextInputComponent textInputComponent;

        UICanvas uICanvas;
        RenderTarget2D screen;
        MainMenuManager menuManager;
        PauseMenuManager pauseManager;
        bool escapeRelease = true;
        GameSettings settings;
        Action IGame.Quit { get => QuitEvent; set => QuitEvent = value; }

        public event Action QuitEvent;



        public SurvivalArenaGame(RenderTarget2D screen, GameSettings settings) : base() {
            this.screen = screen;
            this.settings = settings;
        }

        public void Initialize() {
        }
        public void LoadContent(IServiceProvider serviceProvider, GraphicsDeviceManager graphicsDeviceManager, GameWindow gameWindow) {
            contentManager = new ContentManager(serviceProvider, "Content");
            ColliderComponent.rectangeTexture = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1);
            ColliderComponent.rectangeTexture.SetData(new[] { Color.White });
            level = new Level(contentManager);

            font = contentManager.Load<SpriteFont>("FontTest");


            var screenScaleWitdh = graphicsDeviceManager.PreferredBackBufferWidth / screen.Width;
            scorePosition = new Vector2((screen.Width / 2  -145) * screenScaleWitdh, 0);



            var backgroundMusicController = new BackgroundMusicController(contentManager);
            backgroundMusicController.Play();

            ScoreManager.LoadHighScore();

            textInputComponent = new TextInputComponent(font, gameWindow);
            uICanvas = new UICanvas(screen.Width, screen.Height, font);

            menuManager = new MainMenuManager(contentManager, graphicsDeviceManager, screen);
            menuManager.GameStarted += ChangeToRunning;
            menuManager.start.Clicked += ChangeToRunning;
            menuManager.quit.Clicked += QuitEvent;


            pauseManager = new PauseMenuManager(contentManager, graphicsDeviceManager, screen);
            pauseManager.ReturnEvent += REturn;
            pauseManager.start.Clicked += REturn;
            pauseManager.changeResolution.Clicked += ChangeResolutionSettings;
            pauseManager.changeFullScreen.Clicked += settings.ChangeFullScreen;
            pauseManager.changeVolume.Clicked += backgroundMusicController.ChangeVolume;
            pauseManager.quit.Clicked += QuitEvent;

        }
        public void Pause() {
            gameState = GameState.Pause;
        }
        public void REturn() {
            gameState = GameState.Running;
        }
        public void ChangeToRunning() {
            gameState = GameState.Running;
            SpriteRendererComponent.spriteRendererComponents = new List<IEEDrawable>();
            ColliderComponent.ColliderComponents = new List<ColliderComponent>();
            PoolManager.gameObjects = new List<IUpdater>();
            GameObjectSpawner.currentWaves = 0;
            level = new Level(contentManager);
            ScoreManager.Score = 0;
            ScoreManager.ScoreSaved = false;
        }
        public static void ChangeToMainMenu() {
            gameState = GameState.MainMenu;
        }
        public void UnloadContent() {
        }
        public void Update(GameTime gameTime) {
            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyUp(Keys.Escape)) {
                escapeRelease = true;
            }
            switch (gameState) {
                case GameState.Pause:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && escapeRelease || ControllerStartPressed()) {
                        REturn();
                        escapeRelease = false;
                    }
                    pauseManager.Update(time);
                    break;
                case GameState.MainMenu:
                    menuManager.Update(time);
                    break;
                case GameState.Running:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && escapeRelease) {
                        Pause();
                        escapeRelease = false;
                    }
                    RunGame(time);
                    break;
                case GameState.GameOver:
                case GameState.Win:
                    textInputComponent.Update(time);

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) || ControllerStartPressed()) {
                        ScoreManager.Name = textInputComponent.GetText();
                        ScoreManager.SaveCurrentScore();
                        ScoreManager.StoreScore();
                        
                        ChangeToRunning();
                        ScoreManager.Score = 0;
                        ScoreManager.ScoreSaved = false;

                    }
                    break;
                default:
                    break;
            }

        }
        private bool ControllerStartPressed() {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected) {
                GamePadState state = GamePad.GetState(PlayerIndex.One);
                if (state.IsButtonDown(Buttons.Start)) {
                    return true;
                }
            }
            return false;
        }
        private void RunGame(float gameTime) {

            level.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            int startY = 150;
            int offset = 15;

            switch (gameState) {
                case GameState.Pause:
                    pauseManager.Draw(spriteBatch);
                    break;
                case GameState.MainMenu:
                    menuManager.Draw(spriteBatch);
                    break;
                case GameState.Running:
                    level.Draw(spriteBatch);
                    spriteBatch.DrawString(font, $"Score: {ScoreManager.Score}", scorePosition, Color.White);
                    break;
                case GameState.GameOver:
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY), "Game Over!");
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY + offset), $"Score: {ScoreManager.Score}");
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY + (offset * 2)), "Press Enter to Restart.");

                    startY += (offset * 4);
                    textInputComponent.Position = new Vector2(screen.Width/2, startY);
                    textInputComponent.Draw(spriteBatch);
                    DrawHighScores(spriteBatch, startY, offset);
                    break;
                case GameState.Win:
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY), "Win!");
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY + offset),  $"Score: {ScoreManager.Score}");
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY + (offset * 2)), "Press Enter to Restart.");
                    startY += (offset * 4);
                    textInputComponent.Position = new Vector2(screen.Width / 2, startY);
                    textInputComponent.Draw(spriteBatch);

                    DrawHighScores(spriteBatch, startY, offset);
                    break;
                default:
                    break;
            }


            //for (int i = ColliderComponent.ColliderComponents.Count - 1; i >= 0; i--) {
            //    if (!ColliderComponent.ColliderComponents[i].IsActive) {
            //        continue;
            //    }
            //    spriteBatch.Draw(ColliderComponent.rectangeTexture, ColliderComponent.ColliderComponents[i].Rectangle,
            //Color.Chocolate);
            //}
        }

        private int DrawHighScores(SpriteBatch spriteBatch, int startY, int offset) {
            startY += (offset * 4);
            uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY), $"High Scores");
            startY += offset;
            foreach (var score in ScoreManager.Highscores) {
                uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY + offset), score.name + " " + score.number);
                startY += offset;
            }

            return startY;
        }

        public static void GameOver() {
            textInputComponent.ResetText();
            gameState = GameState.GameOver;
        }
        public static void Win() {
            textInputComponent.ResetText();
            gameState = GameState.Win;
        }

        ushort[] widths = new ushort[] { 3840, 2560, 2560, 1920, 1366, 1280, 1280 };
        ushort[] heights = new ushort[] { 2160, 1440, 1080, 1080, 768, 1024, 720 };
        public void ChangeResolutionSettings() {
            if (settings.currentSetting >= widths.Length) {
                settings.currentSetting = 0;
            }
            var width = widths[settings.currentSetting];
            var height = heights[settings.currentSetting];
            settings.currentSetting++;
            settings.SCREENWIDTH = width;
            settings.SCREENHEIGHT = height;
            settings.ChangeSize();
        }

    }
}
