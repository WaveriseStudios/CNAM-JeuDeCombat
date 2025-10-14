using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using Button = System.Windows.Forms.Button;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Panel = System.Windows.Forms.Panel;
using GumButton = Gum.Forms.Controls.Button;
using Orientation = Gum.Forms.Controls.Orientation;
using Text = RenderingLibrary.Graphics.Text;
using TextBox = Gum.Forms.Controls.TextBox;
using RenderingLibrary.Graphics;

namespace JeuDeCombat;
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D ballTexture;
     private Vector2 ballPosition;
    private string nameUser;
    // SpriteFont font;
    // private Vector2 textPosition;
    // private Button button;
    // private Texture2D buttonTexture;
    GumService gumService => GumService.Default;

    // Initialisation des variables
    bool gameStarted = false;
    bool stopPartie = false;
    bool playerDead = false;
    bool iaDead = false;
    int tour = 1;
    private Dictionary<int, Personnage> availableClasses;
    private Personnage player;
    Random rnd = new Random();
    private int randomIa;
    Personnage iaChoosenClass;
    Personnage ia_player;
    
    public enum ClassePersonnage
    {
        Tank,
        Damager,
        Healer,
        Assassin
    }
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowAltF4 = true;
        Window.Title = "Jeu De Combat";
        Window.AllowUserResizing = true;
        _graphics.PreferredBackBufferHeight = 844;
        _graphics.PreferredBackBufferWidth = 1390;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        gumService.Initialize(this);
        Console.WriteLine("Initializing Game1");
         ballPosition = new Vector2(300, 300);
        
        nameUser = Environment.UserName;
        Console.Write(nameUser);
        // textPosition = new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2, _graphics.GraphicsDevice.Viewport.Height / 2);

        OpenMenu();
        CreationClass();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        
         ballTexture = Content.Load<Texture2D>("ball");
        // font = Content.Load<SpriteFont>("Arial");
        // buttonTexture = new Texture2D(GraphicsDevice, 200, 60);
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        gumService.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin();
        
        _spriteBatch.Draw(
            ballTexture,
            new Vector2(600, 600),
            null,
            Color.White,
            0f,
            new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
            4f, 
            SpriteEffects.None,
            0f
        );
        
        
        
        _spriteBatch.Draw(
            ballTexture,
            new Vector2(1200, 200),
            null,
            Color.White,
            0f,
            new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
            4f, 
            SpriteEffects.None,
            0f
        );


        //_spriteBatch.DrawString(font, "Jeu De Combat", textPosition, Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
        
        gumService.Draw();
        _spriteBatch.End();
        base.Draw(gameTime);
    }


    void openCharacterSelection()
    {
        
        Console.WriteLine("1 - Tank (PV:5, Attaque:1, Spéciale: Attaque puissante -> sacrifie 1 PV pour +1 attaque ce tour)");
        Console.WriteLine("2 - Damager (PV:3, Attaque:2, Spéciale: Rage -> renvoie les dégâts reçus ce tour)");
        Console.WriteLine("3 - Healer (PV:4, Attaque:1, Spéciale: Soin -> récupère 2 PV)");
        Console.WriteLine("4 - Assasin (PV: 2, Attaque:3, Spéciale: Double Tranchant -> Si l'adversaire se défend, il prend 2 dégts. S'il ne défend pas, vous avez 1/2 chance de prendre un dégât \n");
        
        GumService.Default.Root.Children.Clear();
        var characterPanel = new StackPanel();
        characterPanel.Anchor(Anchor.Center);
        characterPanel.Spacing = 150;
        //characterPanel.X = 50;
        characterPanel.Orientation = Orientation.Horizontal;
        characterPanel.AddToRoot();
        
        var textPanel = new StackPanel();
        textPanel.Anchor(Anchor.Top);
        textPanel.AddToRoot();
        
        var title = new MonoGameGum.Forms.Controls.Label();
        title.Anchor(Anchor.Top);
        title.Text = "Choose your character";
        title.Visual.Height = 150;
        title.Visual.Width = 500;
        title.Visual.X = 0;
        title.Visual.Y = 100;
        textPanel.AddChild(title);

        
        var player1 = new GumButton();
        //player1.Anchor(Anchor.Left);
        player1.Text = "Player 1";
        player1.Visual.Width = 200;
        player1.Visual.Height = 200;

        player1.Click += (sender, args) =>
        {
            player = availableClasses[1];
            Console.WriteLine("Type de joueur : " + player.GetClassName());
            randomIa = rnd.Next(1, availableClasses.Count + 1);
            iaChoosenClass = availableClasses[randomIa];
            ia_player = new Personnage(iaChoosenClass.GetClassName(), iaChoosenClass.GetHp(), iaChoosenClass.GetDamage());
            Console.WriteLine("Type de l'ia : " + ia_player.GetClassName());
            StartGame();
        };
        
        var player2 = new GumButton();
        //player2.Anchor(Anchor.Center);
        player2.Text = "Player 2";
        player2.Visual.Width = 200;
        player2.Visual.Height = 200;

        player2.Click += (sender, args) =>
        {
            player = availableClasses[2];
            Console.WriteLine("Type de joueur : " + player.GetClassName());
            randomIa = rnd.Next(1, availableClasses.Count + 1);
            iaChoosenClass = availableClasses[randomIa];            
            ia_player = new Personnage(iaChoosenClass.GetClassName(), iaChoosenClass.GetHp(), iaChoosenClass.GetDamage());
            Console.WriteLine("Type de l'ia : " + ia_player.GetClassName());
            StartGame();
        };
        
        
        var player3 = new GumButton();
        //player3.Anchor(Anchor.Right);
        player3.Text = "Player 3";
        player3.Visual.Width = 200;
        player3.Visual.Height = 200;

        player3.Click += (sender, args) =>
        {
            player = availableClasses[3];
            Console.WriteLine("Type de joueur : " + player.GetClassName());
            randomIa = rnd.Next(1, availableClasses.Count + 1);
            iaChoosenClass = availableClasses[randomIa];
            ia_player = new Personnage(iaChoosenClass.GetClassName(), iaChoosenClass.GetHp(), iaChoosenClass.GetDamage());
            Console.WriteLine("Type de l'ia : " + ia_player.GetClassName());
            StartGame();
        };
        
        var player4 = new GumButton();
        //player3.Anchor(Anchor.Right);
        player4.Text = "Player 4";
        player4.Visual.Width = 200;
        player4.Visual.Height = 200;

        player4.Click += (sender, args) =>
        {
            player = availableClasses[4];
            Console.WriteLine("Type de joueur : " + player.GetClassName());
            randomIa = rnd.Next(1, availableClasses.Count + 1);
            iaChoosenClass = availableClasses[randomIa];
            ia_player = new Personnage(iaChoosenClass.GetClassName(), iaChoosenClass.GetHp(), iaChoosenClass.GetDamage());
            Console.WriteLine("Type de l'ia : " + ia_player.GetClassName());
            StartGame();
        };
        
        var backPanel = new StackPanel();
        backPanel.Anchor(Anchor.BottomRight);
        backPanel.AddToRoot();
        
        var back = new GumButton();
        //player3.Anchor(Anchor.Right);
        back.Text = "Back";
        back.Visual.Width = 150;
        back.Visual.Height = 50;

        back.Click += (sender, args) =>
        {
            OpenMenu();
        };
        backPanel.AddChild(back);
        characterPanel.AddChild(player1);
        characterPanel.AddChild(player2);
        characterPanel.AddChild(player3);
        characterPanel.AddChild(player4);
    }

    void OpenMenu()
    {
        GumService.Default.Root.Children.Clear();
        var mainMenuPanel = new StackPanel();
        mainMenuPanel.Anchor(Anchor.Center);
        mainMenuPanel.Spacing = 250;
        mainMenuPanel.AddToRoot();
        
        var playButton = new GumButton();
        playButton.Anchor(Anchor.Top);
        playButton.Text = "Play";
        playButton.Visual.Width = 550;
        playButton.Visual.Height = 50;
        
        mainMenuPanel.AddChild(playButton);

        playButton.Click += (sender, args) =>
        {
            Console.Write(nameUser);
            openCharacterSelection();
        };
        
        var quitButton = new GumButton();
        quitButton.Anchor(Anchor.Bottom);
        quitButton.Text = "Quit";
        quitButton.Visual.Width = 550;
        quitButton.Visual.Height = 50;
        
        mainMenuPanel.AddChild(quitButton);

        quitButton.Click += (sender, args) =>
        {
            Exit();
        };
    }

    void StartGame()
    {
        GumService.Default.Root.Children.Clear();
        
        var playPanel = new StackPanel();
        playPanel.Anchor(Anchor.BottomLeft);
        playPanel.Spacing = 50;
        //characterPanel.X = 50;
        playPanel.Orientation = Orientation.Horizontal;
        playPanel.AddToRoot();
        
        var AttackButton = new GumButton();
        AttackButton.Text = "Play";
        AttackButton.Visual.Width = 250;
        AttackButton.Visual.Height = 100;
        
        playPanel.AddChild(AttackButton);

        AttackButton.Click += (sender, args) =>
        {
            Console.Write(nameUser);
            openCharacterSelection();
        };
        
        var DefendButton = new GumButton();
        DefendButton.Text = "Play";
        DefendButton.Visual.Width = 250;
        DefendButton.Visual.Height = 100;
        
        playPanel.AddChild(DefendButton);

        DefendButton.Click += (sender, args) =>
        {
            Console.Write(nameUser);
            openCharacterSelection();
        };
        
        var SpecialAttackButton = new GumButton();
        SpecialAttackButton.Text = "Play";
        SpecialAttackButton.Visual.Width = 250;
        SpecialAttackButton.Visual.Height = 100;
        
        playPanel.AddChild(SpecialAttackButton);

        SpecialAttackButton.Click += (sender, args) =>
        {
            Console.Write(nameUser);
            openCharacterSelection();
        };
        
        
        
        var statsPanel = new StackPanel();
        statsPanel.Anchor(Anchor.BottomRight);
        statsPanel.Spacing = 3;
        statsPanel.Y = -30;
        statsPanel.AddToRoot();
        
        var statsHp = new GumButton();
        statsHp.Text = "HP : " + player.GetHp();
        //statsHp.Anchor(Anchor.Center);
        statsHp.Visual.Width = 100;
        statsHp.Visual.Height = 50;
        statsHp.IsEnabled = false;
        
        var statsDamage = new GumButton();
        statsDamage.Text = "Damage : " + player.GetDamage();
        //statsDamage.Anchor(Anchor.Center);
        statsDamage.Visual.Width = 100;
        statsDamage.Visual.Height = 50;
        statsDamage.IsEnabled = false;
        
        var statsDegats = new GumButton();
        statsDegats.Text = "SA : " + player.GetSpecialAbility();
        //statsDegats.Anchor(Anchor.Bottom);
        statsDegats.Visual.Width = 100;
        statsDegats.Visual.Height = 50;
        statsDegats.IsEnabled = false;
        
        statsPanel.AddChild(statsHp);
        statsPanel.AddChild(statsDamage);
        statsPanel.AddChild(statsDegats);
        
        
        var classPanel = new StackPanel();
        classPanel.Anchor(Anchor.Center);
        classPanel.Spacing = 3;
        classPanel.Y = 150;
        classPanel.X = 150;
        classPanel.AddToRoot();
        
        // var playerFace = new GumButton();
        // //statsClass.Anchor(Anchor.Top);
        // playerFace.Visual.Width = 100;
        // playerFace.Visual.Height = 50;
        // playerFace.IsEnabled = false;
        // classPanel.AddChild(playerFace);
        
        var statsClass = new GumButton();
        statsClass.Text = "" + player.GetClassName();
        //statsClass.Anchor(Anchor.Top);
        statsClass.Visual.Width = 100;
        statsClass.Visual.Height = 50;
        statsClass.IsEnabled = false;
        classPanel.AddChild(statsClass);
    }

    void CreationClass()
    {
        Personnage Tank = new Personnage(ClassePersonnage.Tank, 5, 1);
        Personnage Damager = new Personnage(ClassePersonnage.Damager, 3, 2);
        Personnage Healer = new Personnage(ClassePersonnage.Healer, 4, 1);
        Personnage Assassin = new Personnage(ClassePersonnage.Assassin, 2, 3);

        availableClasses = new Dictionary<int, Personnage>();
        availableClasses.Add(1, Tank);
        availableClasses.Add(2, Damager);
        availableClasses.Add(3, Healer);
        availableClasses.Add(4, Assassin);
    }
    
    
    
    
    public class Personnage
    {
        protected ClassePersonnage className { get; set; }
        protected int damage { get; set; }

        protected int receivedDamage { get; set; }
        protected int hp { get; set; }

        public Personnage(ClassePersonnage name, int maxHealth, int attackPower)
        {
            this.className = name;
            this.damage = attackPower;
            this.hp = maxHealth;
        }


        // Références
        public ClassePersonnage GetClassName() => className;
        public int GetDamage() => damage;
        public int GetHp() => hp;
        public int SetHp(int newhp)
        {
            receivedDamage = hp - newhp;
            hp = newhp;
            return receivedDamage;
        }

        public string GetSpecialAbility()
        {
            switch (className)
            {
                case ClassePersonnage.Tank:
                    return "AttaquePuissante";
                case ClassePersonnage.Healer:
                    return "Soin";
                case ClassePersonnage.Damager:
                    return "Rage";
                case ClassePersonnage.Assassin:
                    return "DoubleTranchant";
            }
            return "null";
        }


        // Attaque, Défense, Attaque Spéciale

        public void Attaque(Personnage otherPlayer)
        {
            otherPlayer.TakeDamage(GetDamage());
        }

        public void Defense(Personnage otherPlayer)
        {
            //this.TakeDamage(-receivedDamage);
        }

        public void AttaqueSpe(Personnage otherPlayer, string action = "")
        {
            Random rnd = new Random();
            int randomDamage = rnd.Next(0, 2);

            switch (GetSpecialAbility())
            {
                case "AttaquePuissante":
                    if (action == "d")
                    {
                        this.TakeDamage(1);
                        otherPlayer.TakeDamage(1); return;
                    }
                    else
                    {
                        this.TakeDamage(1);
                        otherPlayer.TakeDamage(GetDamage() + 1); return;
                    }
                case "Soin":
                    this.TakeDamage(-2);
                    return;
                case "Rage":
                    otherPlayer.TakeDamage(this.receivedDamage);
                    return;
                case "DoubleTranchant":
                    if (action == "d")
                    {
                        otherPlayer.TakeDamage(2); return;
                    }
                    else
                    {
                        this.TakeDamage(randomDamage); return;
                    }
            }
        }

        public void TakeDamage(int damageToDeal)
        {
            SetHp(GetHp() - damageToDeal);
        }

        public bool isDead()
        {
            if (GetHp() <= 0) return true;
            else return false;
        }
    }

    void gameFonction()
    {
        while (!stopPartie)
            {
                Console.WriteLine("\n==============================Tour {0}==============================\n", tour);
                // Choix du joueur
                Console.WriteLine("C'est a vous de jouer !");
                Console.WriteLine("A - Atttaquer // D - Défendre // S - Capacité Spéciale");
                string playerAction = Console.ReadLine().ToLower();
                Console.WriteLine(" ");
                if (!playerDead)
                {
                    if (playerAction == "a")
                    {
                        Console.WriteLine("Le joueur attaque");
                    }
                    else if (playerAction == "d")
                    {
                        Console.WriteLine("Le joueur défend");
                    }
                    else if (playerAction == "s")
                    {
                        Console.WriteLine("Le joueur utilise sa capacité spéciale : " + player.GetSpecialAbility());
                    }
                    else
                    {
                        Console.WriteLine("Action invalide, veuillez réessayer");
                        continue;
                    }
                }

                Console.WriteLine("\n L'IA réfléchit...\n");
                System.Threading.Thread.Sleep(1000);  // attend 1 seconde (1000 ms)

                // Choix de l'ia
                string iaActionString = "";
                if (!iaDead)
                {
                    int iaAction = rnd.Next(1, 4);
                    if (iaAction == 1)
                    {
                        Console.WriteLine("Le bot attaque");
                        iaActionString = "a";
                    }
                    else if (iaAction == 2)
                    {
                        Console.WriteLine("Le bot défend");
                        iaActionString = "d";
                    }
                    else if (iaAction == 3)
                    {
                        Console.WriteLine("Le bot utilise sa capacité spéciale : " + ia_player.GetSpecialAbility());
                        iaActionString = "s";
                    }

                }

                // Tour de combat

                if (playerAction == iaActionString)
                {
                    if (playerAction == "a")
                    {
                        player.Attaque(ia_player);
                        ia_player.Attaque(player);

                    }
                    else if (playerAction == "s")
                    {
                        player.AttaqueSpe(ia_player);
                        ia_player.AttaqueSpe(player);

                    }
                }

                if (playerAction != iaActionString)
                {
                    if ((playerAction == "a" && iaActionString == "d") || (playerAction == "d" && iaActionString == "a"))
                    {
                        // Rien du tout

                    }
                    else if ((playerAction == "a" && iaActionString == "s") || (playerAction == "s" && iaActionString == "a"))
                    {
                        if (playerAction == "a")
                        {
                            player.Attaque(ia_player);
                            ia_player.AttaqueSpe(player);

                        }
                        else
                        {
                            ia_player.Attaque(player);
                            player.AttaqueSpe(ia_player);

                        }

                    }
                    else if ((playerAction == "s" && iaActionString == "d") || (playerAction == "d" && iaActionString == "s"))
                    {
                        if (playerAction == "s")
                        {
                            if (player.GetSpecialAbility() == "AttaquePuissante" || player.GetSpecialAbility() == "DoubleTranchant")
                            {
                                player.AttaqueSpe(ia_player, iaActionString);

                            }
                        }
                        else
                        {
                            if (ia_player.GetSpecialAbility() == "AttaquePuissante" || player.GetSpecialAbility() == "DoubleTranchant")
                            {
                                ia_player.AttaqueSpe(player, playerAction);
                            }
                        }

                    }

                }
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("\nLe joueur a {0}pv", player.GetHp());
                Console.WriteLine("Le bot a {0}pv", ia_player.GetHp());
                Console.BackgroundColor = ConsoleColor.Black;

                if (player.isDead() || ia_player.isDead())
                {
                    stopPartie = true;
                }
                tour++;
                System.Threading.Thread.Sleep(1000);  // attend 1 seconde (1000 ms)
            }
            Console.WriteLine("==============================FIN DU COMBAT==============================\n");

            if (player.GetHp() == ia_player.GetHp())
            {
                Console.WriteLine("Egalité !");
            }
            else if (player.GetHp() < ia_player.GetHp())
            {
                Console.WriteLine("Le bot à gagné !");
            }
            else if (player.GetHp() > ia_player.GetHp())
            {
                Console.WriteLine("Tu as gagné !");
            }
    }

}
