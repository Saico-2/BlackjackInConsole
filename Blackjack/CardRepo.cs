using System.Collections.Generic;

namespace Blackjack
{
    class CardRepo
    {
        // deck of available cards, should remain unchanged
        // each card is worth its face value
        // Jack, Queen, King are each worth 10
        // Ace (marked as 0) is worth either 1 or 11 (whichever is most beneficial to the holder)
        public static List<int> newDeck = new List<int>() { 2,  2,  2,  2,  3,  3,  3,  3,
                                                     4,  4,  4,  4,  5,  5,  5,  5,
                                                     6,  6,  6,  6,  7,  7,  7,  7,
                                                     8,  8,  8,  8,  9,  9,  9,  9,
                                                     10, 10, 10, 10, 10, 10, 10, 10,
                                                     10, 10, 10, 10, 10, 10, 10, 10,
                                                     0,  0,  0,  0 };

        // deck in use during game
        public static List<int> deck = new List<int>() { };

        // cards currently in hand
        public static List<int> handPlayer = new List<int>() { };
        public static List<int> handDealer = new List<int>() { };

        // sum of cards' values in hand of dealer/player
        public static int cardSumDealer;
        public static int cardSumPlayer;

        public static bool playerStand; // false until player chooses to Stand
    }
}
