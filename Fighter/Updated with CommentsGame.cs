/*
 * For every new battle the Strength is randomly generated for both the player and opponent.
 * For the First battle the Health is 10(Full) for the player and decreases by 2 after every match.
 * If the Health of the Player gets to 0(Dead) the player dies.
 * The player gets an option if he/she wants to increase his/her Health or Strength after every Win.
 * For every battle opponent is generated randomly and the health is =10.
 */


using System;
using System.Collections.Generic;
using System.Text;

namespace Fighter
{
    enum BattleResult
    {
        Lost,
        Draw,
        Won
    }

    //Creates Characters to fight and initializes their health and strength
    class Character
    {
        public string name;
        public int strength;
        public int health;       

        public Character(string Name, int Health)
        {
            name = Name;
            health = Health;
        }
        public void InitializeAttributes(int addStrength, int addHealth)
        {
            var rand = new Random();            
            strength = rand.Next(1, 11);
            strength = strength + addStrength;
            health += addHealth;
        }
    }

    /*Starts the battle and checks for the strength of both players and 
     * adds the value of the randomly generated dice roll for both players. 
    The player with higher strength wins the battle.
    */
    class Battle
    {
        public Character Player, Opponent;
        public string result;
        public Battle(Character player, Character opponent)
        {
            Player = player;
            Opponent = opponent;
        }

        public string StartBattle()
        {
            Round round_1 = new Round();
            Round round_2 = new Round();

            int playerrolls = round_1.RollDice();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"{Player.name} rolls the dice and gets value {playerrolls}");
            Player.strength += playerrolls;
            Console.WriteLine($"{Player.name} power is {Player.strength}");

            int opponentrolls = round_2.RollDice();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"{Opponent.name} rolls the dice and gets value {opponentrolls}");            
            Opponent.strength += opponentrolls;            
            Console.WriteLine($"{Opponent.name} power is {Opponent.strength}");

            if (Player.strength > Opponent.strength)
            {
                result = "Won";
                return result;
            }

            else if (Player.strength == Opponent.strength)
            {
                result = "Draw";
                return result;
            }

            else
            {
                result = "Lost";
                return result;
            }

        }

    }

    //Generates random number for the dice roll.
    class Round
    {
        public int RollDice()
        {
            var rnd = new Random();
            return rnd.Next(1, 7);
        }


    }

    class Game
    {
        static List<Battle> battleList = new List<Battle>();
        static string playerName;
        static string aliveStatus = "";
        public static int money = 0;
        public static int level = 0;
        public static void Main(string[] args)
        {
            int addStrength=0, addHealth=0, playerHealth=10;
            Console.Write("Enter player name:");
            playerName = Console.ReadLine();
            string[] Opponents = new string[] { "The Hulk", "Batman", "Superman", "Cat Woman", "Wonder Woman", "Spider Man","Black Widow","Iron Man","Thor" };
            while (true)
            {
                Character player = new Character(playerName,playerHealth);
                player.InitializeAttributes(addStrength, addHealth);

                string opponentName = Opponents[new Random().Next(0, 8)];
                Character opponent = new Character(opponentName,10);
                opponent.InitializeAttributes(0,0);

                Console.WriteLine("\n********************************");
                Console.WriteLine($"Your match starts with {opponent.name}");

                Battle currentBattle = new Battle(player, opponent);
                battleList.Add(currentBattle);
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"{player.name} strength is {player.strength}");
                Console.WriteLine($"{player.name} health is {player.health}");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"{opponent.name} strength is {opponent.strength}");                
                Console.WriteLine($"{opponent.name} health is {opponent.health}");                
                if (player.health<=0)
                {
                    Console.WriteLine("\nYou lost the game, your health is less than 0");
                    DisplaySummary();
                    Console.ReadKey();
                    return;
                }
                level++;
                string result = currentBattle.StartBattle();

                //Player health decreases after every battle
                playerHealth =player.health- 2;
                
                switch (result)
                {
                    case "Won":
                        money += 100;                        
                        Console.WriteLine($"\n{player.name} wins and deals damage to {opponent.name}");                        
                        Console.WriteLine("Continue to next level? press Y/y OR to Retire press R/r");
                        string response = Console.ReadLine();
                        if (response == "Y" || response == "y")
                        {
                            int updateResponse = Update();
                            
                            if (updateResponse == 1)                            
                                addHealth = UpdateHealth();                                                         
                            else if (updateResponse == 2)
                                addStrength = UpdateStrength();
                            else if (updateResponse == 3)
                                Console.WriteLine("\n Your value will not be updated.");
                            else
                                Console.WriteLine("\nYou didnt select proper option or you dont have enough money, no update");

                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                            continue;
                        }

                        else if (response == "R" || response == "r")
                        {
                            Console.WriteLine("\nYou have Withdrawn from the matches");
                            aliveStatus = "Alive and Retired";
                            DisplaySummary();
                            return;
                        }
                        break;
                    case "Draw":

                        Console.WriteLine($"\n{player.name} and {opponent.name} have equal strength, battle is draw");                        
                        Console.WriteLine("Continue to next level? press Y/y OR to Retire press R/r");
                        string response1 = Console.ReadLine();
                        if (response1 == "Y" || response1 == "y")
                        {
                            int updateResponse = Update();
                            if (updateResponse == 1)
                                addHealth = UpdateHealth();
                            else if (updateResponse == 2)
                                addStrength = UpdateStrength();
                            else if(updateResponse == 3)
                                Console.WriteLine("\n Your value will not be updated.");
                            else
                                Console.WriteLine("You didnt select proper option or you dont have enough money,no update");

                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                            continue;
                        }

                        else if (response1 == "R" || response1 == "r")
                        {
                            Console.WriteLine("\nYou have Withdrawn from the matches");
                            aliveStatus = "Alive and Retired";
                            DisplaySummary();
                            return;
                        }
                        break;
                    case "Lost":
                        Console.WriteLine($"\n{opponent.name} wins and deals damage to {player.name}, you lost the battle");
                        aliveStatus = "Dead";
                        DisplaySummary();
                        return;
                    default:
                        Console.WriteLine("You entered wrong input, Batlle quits");
                        DisplaySummary();
                        return;
                }
            }
        }

        //Displays the Log Summary of all the battles 
        public static void DisplaySummary()
        {
            Console.WriteLine("\nYour game summary");
            Console.WriteLine($"\nYou played {battleList.Count} number of battles");
            int win = 0;
            int draw = 0;
            int loss = 0;
            int counter = 1;
            foreach (Battle battle in battleList)
            {

                Console.WriteLine($"\n Battle {counter} Summary");
                Console.WriteLine("********************************");
                Console.WriteLine($"Player {battle.Player.name} power {battle.Player.strength}");
                Console.WriteLine($"Opponent {battle.Opponent.name} power {battle.Opponent.strength}");
                if (battle.result == Convert.ToString(BattleResult.Won))
                    win++;
                else if (battle.result == Convert.ToString(BattleResult.Draw))
                    draw++;
                else if (battle.result == Convert.ToString(BattleResult.Lost))
                    loss++;
                counter++;
            }

            Console.WriteLine("\n Total summary of Battles");
            Console.WriteLine("********************************");
            Console.WriteLine($"{playerName} is {aliveStatus}");
            Console.WriteLine($"{playerName} WON {win} number of battles");
            Console.WriteLine($"{draw} Number of DRAW battles");
            Console.WriteLine($"{playerName} LOST {loss} number of battle");
            Console.WriteLine($"{playerName} has reached level {level}");
            Console.WriteLine($"{playerName} has scored {win*10} points");
            Console.Read();
        }

        //Updates the Health of the Player if chooses to update.
        public static int UpdateHealth()
        {
            Console.WriteLine("\nSelect number to Update Health" +
                        "\n Blue Potion Increses health by 2 points for 20Kr : 1" +
                        "\n Green Potion Increses health by 3 points for 30Kr: 2" +
                        "\n Red Potion Increses health by 5 points for 50Kr : 3");
            int potion = Convert.ToInt32(Console.ReadLine());

            if (potion == 1)
            {                
                Console.WriteLine("Your Health will be updated by 2 points for next battle" );
                money = money - 20;
                return 2;
            }
            else if (potion == 2)
            {
                Console.WriteLine("Your Health will be updated by 3 points for next battle");
                money = money - 30;
                return 3;
            }
            else if (potion == 3)
            {
                Console.WriteLine("Your Health will be updated by 5 points for next battle");
                money = money - 50;
                return 5;
            }
            else
            {
                Console.WriteLine("Wrong input");
                return 0;
            }
            
            
        }

        //Updates the Strength of the Player if chooses to update.
        public static int UpdateStrength()
        {
            Console.WriteLine("\nSelect gear number to Update Strength" +
                        "\n Update Shield to Increses strength by 2 points for 20Kr : 1" +
                        "\n Update Sword to Increses strength by 3 points  for 30Kr : 2" +
                        "\n Update Gun to Increses strength by 5 points for 50Kr : 3");
            int gear = Convert.ToInt32(Console.ReadLine());

            if (gear == 1)
            {
                Console.WriteLine("Your Strength will be updated by 2 points for next battle");
                money = money - 20;
                return 2;
            }
            else if (gear == 2)
            {
                Console.WriteLine("Your Strength will be updated by 3 points for next battle");
                money = money - 30;
                return 3;
            }
            else if (gear == 3)
            {
                Console.WriteLine("Your Strength will be updated by 5 points for next battle");
                money = money - 50;
                return 5;
            }
            else
            {
                Console.WriteLine("Wrong input");
                return 0;
            }

        }

        //Displays the reward money(100SEK) the Player wins after every match.
        //Giving the player an option to increase Health or Strength after every Win.
        public static int Update()
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"\nYou have {money}SEK in your account");
            if (money >= 20)
            {
                Console.WriteLine("What would you like to update ,Select an option number" +
                    "\n 1 : Health" +
                    "\n 2 : Strength" +
                    "\n 3 : No update");
                int choice = Convert.ToInt32(Console.ReadLine());
                return choice;
            }
            else
            {
                Console.WriteLine("You have no money in your account , you cannot update ypur gear");
                return 0;
            }

            
            
        }
    }
}
