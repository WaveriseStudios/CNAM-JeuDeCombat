using System;
using System.Net.NetworkInformation;

namespace JeuDeCombat
{
    public enum ClassePersonnage
    {
        Tank,
        Damager,
        Healer,
        Assassin
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
            int randomDamage = rnd.Next(0,1);

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
                    if(action == "d")
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

    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("==============================JEU DE COMBAT============================== \n");
            Console.WriteLine("Vous allez devoir choisir une classe parmi les 3 proposées \n");
            System.Threading.Thread.Sleep(1000);  // attend 1 seconde (1000 ms)
            Console.WriteLine("1 - Tank (PV:5, Attaque:1, Spéciale: Attaque puissante -> sacrifie 1 PV pour +1 attaque ce tour)");
            Console.WriteLine("2 - Damager (PV:3, Attaque:2, Spéciale: Rage -> renvoie les dégâts reçus ce tour)");
            Console.WriteLine("3 - Healer (PV:4, Attaque:1, Spéciale: Soin -> récupère 2 PV) \n");
            Console.WriteLine("4 - Assassin (PV: 2, Attaque:3, Spéciale: Double Tranchant");

            // Creéation des classes
            Personnage Tank = new Personnage(ClassePersonnage.Tank, 5, 1);
            Personnage Damager = new Personnage(ClassePersonnage.Damager, 3, 2);
            Personnage Healer = new Personnage(ClassePersonnage.Healer, 4, 1);
            Personnage Assassin = new Personnage(ClassePersonnage.Assassin, 2, 3);

            Dictionary<int, Personnage> availableClasses = new Dictionary<int, Personnage>();
            availableClasses.Add(1, Tank);
            availableClasses.Add(2, Damager);
            availableClasses.Add(3, Healer);
            availableClasses.Add(4, Assassin);

            // Choix du joueur
            bool chosen = false;
            System.Threading.Thread.Sleep(1000);  // attend 1 seconde (1000 ms)
            Console.WriteLine("Quelle classe voulez vouc choisir ? (1-3)");
            int playerClasssInt = 1;

            while (!chosen)
            {
                string playerChoosenClass = Console.ReadLine();
                if (playerChoosenClass == "1" || playerChoosenClass == "2" || playerChoosenClass == "3")
                {
                    chosen = true;
                    playerClasssInt = int.Parse(playerChoosenClass);
                }
                else if (playerChoosenClass.ToLower() == "exit" || playerChoosenClass.ToLower() == "quit")
                {
                    Console.WriteLine("Fermeture du jeu...");
                    return;
                }
                else
                {
                    Console.WriteLine("Veuillez faire un choix valide.");
                    continue;
                }
            }

            Personnage player = availableClasses[playerClasssInt];


            // Choix de la classe de l'ia
            Random rnd = new Random();
            int randomIa = rnd.Next(1, availableClasses.Count + 1);
            Personnage iaChoosenClass = availableClasses[randomIa];
            Personnage ia_player = new Personnage(iaChoosenClass.GetClassName(), iaChoosenClass.GetHp(), iaChoosenClass.GetDamage());

            // Affichage
            Console.WriteLine("==============================COMBAT COMMENCE============================== \n");
            Console.WriteLine("le joueur est un {0}, et a {1}pv", player.GetClassName(), player.GetHp());
            Console.WriteLine("le bot est un {0}, et a {1}pv", ia_player.GetClassName(), ia_player.GetHp());

            // Initialisation des variables
            bool stopPartie = false;
            bool playerDead = false;
            bool iaDead = false;
            int tour = 1;

            // Boucle de jeu
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
                            if (player.GetSpecialAbility() == "AttaquePuissante")
                            {
                                player.AttaqueSpe(ia_player, iaActionString);

                            }
                        }
                        else
                        {
                            if (ia_player.GetSpecialAbility() == "AttaquePuissante")
                            {
                                ia_player.AttaqueSpe(player, playerAction);

                            }
                        }

                    }

                }
                Console.WriteLine("\nLe joueur a {0}pv", player.GetHp());
                Console.WriteLine("Le bot a {0}pv", ia_player.GetHp());

                if (player.isDead() || ia_player.isDead())
                {
                    stopPartie = true;
                }
                tour++;
                System.Threading.Thread.Sleep(1000);  // attend 1 seconde (1000 ms)
            }
            Console.WriteLine("\n==============================FIN DU COMBAT==============================\n");

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
}