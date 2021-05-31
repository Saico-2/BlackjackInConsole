using System;
using System.Threading;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            // welcome message
            Console.Clear();
            Console.WriteLine("Welcome to Blackjack!");
            Thread.Sleep(2000);

            // access to classes
            CardService service = new CardService();

            char choice = ' '; // hold player's choice, used when asking for Hit or Stand, or whether to play a new game
            bool playerEnd = false; // false until player chooses to Stand

            // sum of cards' values in hand of dealer/player
            int cardSumDealer;
            int cardSumPlayer;

            while (true)
            {// reset game
                Console.Clear();
                CardRepo.handPlayer.Clear();
                CardRepo.handDealer.Clear();
                CardRepo.deck = CardRepo.newDeck;
                cardSumDealer = 0;
                cardSumPlayer = 0;
                playerEnd = false;

                // deal initial hands
                service.DealCard(false); // false = dealer
                service.DealCard(false);
                service.DealCard(true); // true = player
                service.DealCard(true);

                // the actual game happens in this loop
                while (true)
                {
                    // calculate sum of hand in advance
                    cardSumDealer = service.HandSum(false);
                    cardSumPlayer = service.HandSum(true);

                    // show hands
                    Console.Clear();
                    Console.WriteLine($"Dealer: {cardSumDealer} ({service.HandEach(false)})");
                    Console.WriteLine($"Player: {cardSumPlayer} ({service.HandEach(true)})");

                    // when player Stands, dealer gets cards until has value 17 or more
                    if (playerEnd)
                    {
                        while (cardSumDealer < 17)
                        {
                            Thread.Sleep(1000);
                            service.DealCard(false);
                            cardSumDealer = service.HandSum(false);
                            Console.Clear();
                            Console.WriteLine($"Dealer: {cardSumDealer} ({service.HandEach(false)})");
                            Console.WriteLine($"Player: {cardSumPlayer} ({service.HandEach(true)})");
                        }

                        if (cardSumDealer >= 17)
                        {
                            if (cardSumPlayer > cardSumDealer)
                            {
                                Thread.Sleep(1000);
                                Console.WriteLine("\nPlayer wins!");
                                break;
                            }
                            else if (cardSumPlayer == cardSumDealer)
                            {
                                Thread.Sleep(1000);
                                Console.WriteLine("\nPUSH\nNo one wins!");
                                break;
                            }
                            else if (cardSumDealer <= 21)
                            {
                                Thread.Sleep(1000);
                                Console.WriteLine("\nDealer wins!");
                                break;
                            }
                            else
                            {
                                Thread.Sleep(1000);
                                Console.WriteLine("\nDEALER BUSTED\nPlayer wins!");
                                break;
                            }
                        }
                    }

                    // game-ending events
                    if (cardSumPlayer > 21)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("\nPLAYER BUSTED\nDealer wins!");
                        break;
                    }
                    else if (cardSumDealer > 21)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("\nDEALER BUSTED\nPlayer wins!");
                        break;
                    }
                    else if ((cardSumDealer == cardSumPlayer) && !(cardSumDealer <= 17))
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("\nPUSH\nNo one wins!");
                        break;
                    }
                    else if ((cardSumDealer == 21) && (CardRepo.handDealer.Count == 2))
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("\nBLACKJACK\nDealer wins!");
                        break;
                    }
                    else if ((cardSumPlayer == 21) && (CardRepo.handPlayer.Count == 2))
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("\nBLACKJACK\nPlayer wins!");
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
                        } while ((choice != 'y') && (choice != 'n'));
                    }

                    // if player presses Y, they're dealt another card
                    if (choice == 'y')
                    {
                        service.DealCard(true);
                    }
                    // player presses N, they stop taking cards
                    else
                    {
                        playerEnd = true;
                    }
                }

                // game is over
                choice = ' ';

                Thread.Sleep(1000);

                Console.WriteLine("\nPlay again? (y/n)");
                do
                {
                    choice = Console.ReadKey().KeyChar;
                } while ((choice != 'y') && (choice != 'n'));

                if (choice == 'y')
                {
                    continue;
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
