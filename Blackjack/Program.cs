using System;
using System.Threading;

namespace Blackjack
{
    class Program
    {
        static void Main()
        {
            // welcome message
            Console.Clear();
            Console.WriteLine("Welcome to Blackjack!");
            Thread.Sleep(2000);

            // access to classes
            CardService service = new CardService();

            char choice = ' '; // hold player's choice, used when asking for Hit or Stand, or whether to play a new game
            bool gameEnded = false; // used so there's no need to needlessly call the method twice in a row

            while (true)
            {
                service.resetGame();

                // the actual game happens in this loop
                while (true)
                {
                    // calculate sum of hand in advance
                    CardRepo.cardSumDealer = service.HandSum(false);
                    CardRepo.cardSumPlayer = service.HandSum(true);

                    service.showHands();

                    // when player Stands, dealer gets cards until has value 17 or more
                    if (CardRepo.playerStand)
                    {
                        while (CardRepo.cardSumDealer < 17)
                        {
                            Thread.Sleep(1000);

                            // deal card to dealer and calculate sum of dealer's hand
                            service.DealCard(false);
                            CardRepo.cardSumDealer = service.HandSum(false);

                            service.showHands();
                        }

                        // check if player or dealer wins/loses
                        if (service.checkWin(CardRepo.playerStand))
                        {
                            gameEnded = true;
                            break;
                        }
                    }

                    // check if player or dealer wins/loses
                    if (gameEnded || service.checkWin(CardRepo.playerStand))
                    {
                        gameEnded = false;
                        break;
                    }

                    Thread.Sleep(1000);

                    // player chooses whether to Hit (y) or Stand (n)
                    if (choice != 'n')
                    {
                        Console.WriteLine("\nAnother card? (y/n)");
                        do
                        {
                            choice = Console.ReadKey().KeyChar;
                        } while ((choice != 'y') && (choice != 'n')); // do nothing if player presses keys other than Y or N
                    }

                    // if player presses Y, they're dealt another card
                    if (choice == 'y')
                    {
                        service.DealCard(true);
                    }
                    // player presses N, they stop taking cards
                    else
                    {
                        CardRepo.playerStand = true;
                    }

                    choice = ' ';
                }

                // game is over
                Thread.Sleep(1000);

                Console.WriteLine("\nPlay again? (y/n)");
                do
                {
                    choice = Console.ReadKey().KeyChar;
                } while ((choice != 'y') && (choice != 'n'));

                if (choice == 'y')
                {
                    choice =  ' ';
                }
                else
                {
                    break;
                }
            }

            Console.Clear();
            Console.Write("Goodbye!");
            Thread.Sleep(1000);
        }
    }
}
