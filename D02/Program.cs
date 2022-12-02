using System.Diagnostics;

int P1(IEnumerable<string> lines) => lines.Select(
        line =>
        {
            int a = ToScore(line[0]);
            int b = ToScore(line[2]);
            return (a, b) switch
            {
                (1, 1) or (2, 2) or (3, 3) => 3 + b,
                (1, 2) or (2, 3) or (3, 1) => 6 + b,
                _ => 0 + b,
            };
        }
    ).Sum();

int P2(IEnumerable<string> lines) => lines.Select(
        line => line[2] switch
        {
            'X' => line[0] switch
            {
                'A' => 0 + ToScore('C'),
                'B' => 0 + ToScore('A'),
                _ => 0 + ToScore('B'),
            },
            'Z' => line[0] switch
            {
                'A' => 6 + ToScore('B'),
                'B' => 6 + ToScore('C'),
                _ => 6 + ToScore('A'),
            },
            _ => 3 + ToScore(line[0])
        }
    ).Sum();

int ToScore(char c) => c switch
{
    'A' or 'X' => 1,
    'B' or 'Y' => 2,
    _ => 3,
};

var test = @"A Y
B X
C Z".Split('\n', StringSplitOptions.TrimEntries);
Debug.Assert(P1(test) == 15);
Debug.Assert(P2(test) == 12);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 10310);
Debug.Assert(P2(input) == 14859);