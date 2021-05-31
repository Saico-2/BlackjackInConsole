using System;
using System.Linq;

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
                int randomCard = rnd.Next(CardRepo.deck.Count); // choose a random card from deck
                CardRepo.handPlayer.Add(CardRepo.deck[randomCard]); // add card to hand
                CardRepo.deck.RemoveAt(randomCard);             // remove added card from deck
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
    }
}
