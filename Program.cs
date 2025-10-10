using System;
using System.Net.NetworkInformation;

namespace JeuDeCombat
{
    public class Personnage
    {
        protected string className { get; set;}
        protected int damage { get; set;}

        protected int hp { get; set;}

        public Personnage(string name, int maxHealth, int attackPower)
        {
            this.className = name;
            this.damage = attackPower;
            this.hp = maxHealth;
        }

        public string GetClassName() => className;
        public int GetDamage() => damage;
        public int GetHp() => hp;
    }

    internal class Program
    {
        static void Main(string[] args)
        {

            // Creéation des classes
            Personnage Tank = new Personnage("Tank", 5,1);
            Personnage Damager = new Personnage("Damager", 3, 2);
            Personnage Healer = new Personnage("Healer", 4, 1);

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
            int randomIa = rnd.Next(1,availableClasses.Count+1);
            Personnage iaChoosenClass = availableClasses[randomIa];
            Personnage ia_player = new Personnage(iaChoosenClass.GetClassName(), iaChoosenClass.GetHp(), iaChoosenClass.GetDamage());

            // Affichage
            Console.WriteLine("le joueur est un {0} et a {1}pv", player.GetClassName(),player.GetHp());
            Console.WriteLine("le bot est un {0} et a {1}pv", ia_player.GetClassName(), ia_player.GetHp());

        }
    }
}