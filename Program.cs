using System;
using System.Net.NetworkInformation;

namespace JeuDeCombat
{

    public enum ClassePersonnage
    {
        Tank,
        Damager,
        Healer
    }

    public class Personnage
    {
        protected ClassePersonnage className { get; set; }
        protected int damage { get; set; }

        protected int hp { get; set; }

        public Personnage(ClassePersonnage name = ClassePersonnage.Tank, int maxHealth = 3, int attackPower = 2)
        {
            this.className = name;
            this.damage = attackPower;
            this.hp = maxHealth;
        }


        // Références
        public ClassePersonnage GetClassName() => className;
        public int GetDamage() => damage;
        public int GetHp() => hp;
        public int SetHp(int newhp) => hp = newhp;

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

        }

        public void AttaqueSpe(Personnage otherPlayer)
        {
            switch (GetSpecialAbility())
            {
                case "AttaquePuissante":
                    otherPlayer.TakeDamage(GetDamage());
                    break;
                case "Soin":
                    otherPlayer.TakeDamage(GetDamage());
                    break;
                case "":
                    break;
            }
        }

        public void TakeDamage(int damageToDeal)
        {
            SetHp(GetHp() - damageToDeal);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {

            // Creéation des classes
            Personnage Tank = new Personnage(ClassePersonnage.Tank, 5, 1);
            Personnage Damager = new Personnage(ClassePersonnage.Tank, 3, 2);
            Personnage Healer = new Personnage(ClassePersonnage.Tank, 4, 1);

            Dictionary<int, Personnage> availableClasses = new Dictionary<int, Personnage>();
            availableClasses.Add(1, Tank);
            availableClasses.Add(2, Damager);
            availableClasses.Add(3, Healer);

            // Choix du joueur
            Console.WriteLine("Quelle classe voulez vouc choisir");
            int playerChoosenClass = int.Parse(Console.ReadLine()); // à changer 
            Personnage player = availableClasses[playerChoosenClass];


            // Choix de la classe de l'ia
            Random rnd = new Random();
            int randomIa = rnd.Next(1, availableClasses.Count + 1);
            Personnage iaChoosenClass = availableClasses[randomIa];
            Personnage ia_player = new Personnage(iaChoosenClass.GetClassName(), iaChoosenClass.GetHp(), iaChoosenClass.GetDamage());

            // Affichage
            Console.WriteLine("le joueur est un {0} et a {1}pv", player.GetClassName(), player.GetHp());
            Console.WriteLine("l'attaque spéciale est {0}", player.GetSpecialAbility());
            Console.WriteLine("le bot est un {0} et a {1}pv", ia_player.GetClassName(), ia_player.GetHp());

        }
    }
}