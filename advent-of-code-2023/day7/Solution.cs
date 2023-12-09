namespace advent.of.code.day7;

internal static class Solution {

    private enum HandType {
        HighCard,
        OnePair,
        TwoPairs,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    internal static int Task1(StreamReader reader) {
        return GetTotalBid(ReadHands(reader, false), new List<char> {
            'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'
        });
    }

    internal static int Task2(StreamReader reader) {
        return GetTotalBid(ReadHands(reader, true), new List<char> {
            'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'
        });
    }

    private static Dictionary<HandType, List<(string hand, int bid)>> ReadHands(StreamReader reader, bool respectJoker) {
        var mappedHands = new Dictionary<HandType, List<(string hand, int bid)>>();

        while (!reader.EndOfStream) {
            var line = reader.ReadLine() ?? throw new Exception();

            var hand = line.Split(" ")[0];
            var bid = int.Parse(line.Split(" ")[1]);

            var handType = GetHandType(hand, respectJoker);
            if (!mappedHands.ContainsKey(handType)) mappedHands.Add(handType, new List<(string hand, int bid)>());
            mappedHands[handType].Add((hand, bid));
        }

        return mappedHands;
    }

    private static int GetTotalBid(IReadOnlyDictionary<HandType, List<(string hand, int bid)>> types, IList<char> strengthOrder) {
        var total = 0;
        var rank = 1;
        foreach (HandType type in Enum.GetValues(typeof(HandType))) {
            if (!types.ContainsKey(type)) continue;

            var handList = types[type];
            handList.Sort((a, b) => {
                for (var i = 0; i < a.hand.Length; i++) {
                    if (a.hand[i] != b.hand[i]) {
                        return strengthOrder.IndexOf(b.hand[i]).CompareTo(strengthOrder.IndexOf(a.hand[i]));
                    }
                }

                return 0;
            });

            foreach (var hand in handList) {
                total += rank * hand.bid;
                rank++;
            }
        }

        return total;
    }

    private static HandType GetHandType(string hand, bool respectJoker) {
        var countOfCards = hand.ToCharArray().Distinct().Select(curChar => hand.Count(c => c == curChar)).ToList();

        var handType = countOfCards.Count switch {
            5 => HandType.HighCard,
            4 => HandType.OnePair,
            3 => countOfCards.Max() == 2 ? HandType.TwoPairs : HandType.ThreeOfAKind,
            2 => countOfCards.Max() == 3 ? HandType.FullHouse : HandType.FourOfAKind,
            1 => HandType.FiveOfAKind,
            _ => throw new Exception()
        };

        if (!respectJoker || !hand.Contains('J')) return handType;

        return handType switch {
            HandType.HighCard => HandType.OnePair,
            HandType.OnePair => HandType.ThreeOfAKind,
            HandType.TwoPairs => hand.Count(c => c == 'J') == 1 ? HandType.FullHouse : HandType.FourOfAKind,
            HandType.ThreeOfAKind => HandType.FourOfAKind,
            HandType.FullHouse => HandType.FiveOfAKind,
            HandType.FourOfAKind => HandType.FiveOfAKind,
            HandType.FiveOfAKind => HandType.FiveOfAKind,
            _ => throw new Exception()
        };
    }
}