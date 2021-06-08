using System;
using System.Linq;
using System.Threading;

namespace Blackjack
{
    class CardService
    {
        Random rnd = new Random();

        // (true = player, false = dealer)
        // deal a card
        public void DealCard(bool player)
        {
            // deal a card to player
            if (player)
            {
                int randomCard = rnd.Next(CardRepo.deck.Count);     // choose a random card from deck
                CardRepo.handPlayer.Add(CardRepo.deck[randomCard]); // add card to hand
                CardRepo.deck.RemoveAt(randomCard);                 // remove added card from deck
            }
            // deal a card to dealer
            else
            {
                int randomCard = rnd.Next(CardRepo.deck.Count);
                CardRepo.handDealer.Add(CardRepo.deck[randomCard]);
                CardRepo.deck.RemoveAt(randomCard);
            }
        }


        // show sum of cards in hand
        public int HandSum(bool player)
        {
            int x = 0;

            // sum up player's hand
            if (player)
            {
                // add up values in hand
                foreach (int i in CardRepo.handPlayer)
                {
                    x += i;
                }

                // check what value should each Ace be
                foreach (int i in Enumerable.Range(0, CardRepo.handPlayer.Count))
                {
                    if((CardRepo.handPlayer[i] == 0) && ((x + 11) > 21)) // if 11 would bust the hand, make Ace a 1
                    {
                        CardRepo.handPlayer[i] = 1;
                        x += 1; // adjust sum of cards for new Ace value
                    }
                    else if (CardRepo.handPlayer[i] == 0) // if 11 doesn't bust, make Ace an 11
                    {
                        CardRepo.handPlayer[i] = 11;
                        x += 11;
                    }
                    else if ((CardRepo.handPlayer[i] == 11) && (x > 21)) // if an Ace is already an 11 but it busts, make it a 1
                    {
                        CardRepo.handPlayer[i] = 1;
                        x -= 10;
                    }
                }

                return x;
            }
            // same stuff but for dealer's hand
            else
            {
                foreach (int i in CardRepo.handDealer)
                {
                    x += i;
                }

                foreach (int i in Enumerable.Range(0, CardRepo.handDealer.Count))
                {
                    if ((CardRepo.handDealer[i] == 0) && ((x + 11) > 21))
                    {
                        CardRepo.handDealer[i] = 1;
                        x += 1;
                    }
                    else if (CardRepo.handDealer[i] == 0)
                    {
                        CardRepo.handDealer[i] = 11;
                        x += 11;
                    }
                    else if ((CardRepo.handDealer[i] == 11) && (x > 21))
                    {
                        CardRepo.handDealer[i] = 1;
                        x -= 10;
                    }
                }

                return x;
            }
        }


        // show list of held cards
        public string HandEach(bool player)
        {
            string cards = "";

            if (player)
            {
                foreach (int i in CardRepo.handPlayer)
                {
                    cards += $"{i.ToString()}, "; // list cards separated by comma and space ", "
                }
                cards = cards.Remove(cards.Length - 2); // remove unnecessary comma and space at the end
                return cards;
            }
            else
            {
                foreach (int i in CardRepo.handDealer)
                {
                    cards += $"{i.ToString()}, ";
                }
                cards = cards.Remove(cards.Length - 2);
                return cards;
            }
        }
    

        // game-ending events
        public bool checkWin(bool playerStand)
        {
            // BEFORE player Stands
            if(!playerStand)
            {
                // player has over 21 points (bust)
                if (CardRepo.cardSumPlayer > 21)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\nPLAYER BUSTED\nDealer wins!");
                    return true;
                }

                // player and dealer were given equal hands
                else if ((CardRepo.handPlayer.Count == 2) && (CardRepo.handDealer.Count == 1) && (CardRepo.cardSumPlayer == CardRepo.cardSumDealer))
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\nPUSH\nNo one wins!");
                    return true;
                }

                // player got Blackjack (initial hand value of 21)
                else if ((CardRepo.handPlayer.Count == 2) && (CardRepo.cardSumPlayer == 21))
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\nBLACKJACK\nPlayer wins!");
                    return true;
                }

                // all conditions false, game continues
                else
                {
                    return false;
                }
            }

            // AFTER player Stands and dealer has 17 or more points
            else if(CardRepo.cardSumDealer >= 17)
            {
                // player has more points (21 or less)
                if (CardRepo.cardSumPlayer > CardRepo.cardSumDealer)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\nPlayer wins!");
                    return true;
                }

                // player and dealer have equal amounts of points (21 or less)
                else if (CardRepo.cardSumPlayer == CardRepo.cardSumDealer)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\nPUSH\nNo one wins!");
                    return true;
                }
                /// (if all above conditions are false, player must have less points than dealer)

                // dealer did not bust (21 points or less)
                else if (CardRepo.cardSumDealer <= 21)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\nDealer wins!");
                    return true;
                }
                /// (if previous condition is false, dealer must have more than 21 points)

                // dealer busted (more than 21 points)
                else
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("\nDEALER BUSTED\nPlayer wins!");
                    return true;
                }
            }

            // all conditions false, game continues
            else
            {
                return false;
            }
        }
    

        // shows sum of cards and each card held by dealer and player
        public void showHands()
        {
            Console.Clear();
            Console.WriteLine($"Dealer: {CardRepo.cardSumDealer} ({HandEach(false)})");
            Console.WriteLine($"Player: {CardRepo.cardSumPlayer} ({HandEach(true)})");
        }
    

        // prepare a new game
        public void resetGame()
        {
            Console.Clear();

            // replace used deck with a fresh one containing all cards
            CardRepo.deck = CardRepo.newDeck;

            // empty hands and choices
            CardRepo.handPlayer.Clear();
            CardRepo.handDealer.Clear();
            CardRepo.cardSumDealer = 0;
            CardRepo.cardSumPlayer = 0;
            CardRepo.playerStand = false;

            // deal initial hands
            DealCard(false); // false = dealer
            DealCard(true); // true = player
            DealCard(true);
        }
    }
}
