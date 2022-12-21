using System.Diagnostics;

long P1(IEnumerable<string> lines)
{
    var nodes = Parse(lines);
    return nodes["root"].Num;
}

long P2(IEnumerable<string> lines)
{
    var nodes = Parse(lines);
    var root = (OpNode)nodes["root"];
    var left = nodes[root.Left];
    var right = nodes[root.Right];
    if (left.HasH)
        left.Num = right.Num;
    else if (right.HasH)
        right.Num = left.Num;
    return nodes["humn"].Num;
}

Dictionary<string, Node> Parse(IEnumerable<string> lines)
{
    var res = new Dictionary<string, Node>();
    foreach (var line in lines)
    {
        var items = line.Split(' ', StringSplitOptions.TrimEntries);
        var name = items[0].Substring(0, 4);
        if (items.Length == 2)
        {
            var num = int.Parse(items[1]);
            res.Add(name, new NumNode { Name = name, Num = num });
        }
        else
        {
            res.Add(name, new OpNode
            {
                Left = items[1],
                Right = items[3],
                Op = items[2][0],
                Dict = res,
            });
        }
    }
    return res;
}

var test = @"root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32".Split('\n');
Debug.Assert(P1(test) == 152);
Debug.Assert(P2(test) == 301);

var input = File.ReadLines("input.txt");
Debug.Assert(P1(input) == 152479825094094);
Debug.Assert(P2(input) == 3360561285172);

abstract class Node
{
    public abstract long Num { get; set; }
    public abstract bool HasH { get; }
}

sealed class NumNode : Node
{
    public string Name { get; init; } = null!;
    public override long Num { get; set; }
    public override bool HasH => Name == "humn";
}

sealed class OpNode : Node
{
    public override long Num
    {
        get => Op switch
        {
            '+' => Dict[Left].Num + Dict[Right].Num,
            '-' => Dict[Left].Num - Dict[Right].Num,
            '*' => Dict[Left].Num * Dict[Right].Num,
            _ => Dict[Left].Num / Dict[Right].Num,
        };
        set
        {
            if (Dict[Left].HasH)
            {
                Dict[Left].Num = Op switch
                {
                    '+' => value - Dict[Right].Num,
                    '-' => value + Dict[Right].Num,
                    '*' => value / Dict[Right].Num,
                    _ => value * Dict[Right].Num,
                };
            }
            else if (Dict[Right].HasH)
            {
                Dict[Right].Num = Op switch
                {
                    '+' => value - Dict[Left].Num,
                    '-' => Dict[Left].Num - value,
                    '*' => value / Dict[Left].Num,
                    _ => value / Dict[Left].Num,
                };
            }
        }
    }

    public string Left { get; init; } = null!;
    public string Right { get; init; } = null!;
    public char Op { get; init; }
    public IReadOnlyDictionary<string, Node> Dict { get; init; } = null!;
    public override bool HasH => Dict[Left].HasH || Dict[Right].HasH;
}