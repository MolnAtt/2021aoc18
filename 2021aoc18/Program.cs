using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021aoc18
{

    class Program
    {
        static void Main(string[] args)
        {
            /*
            Szám.Parse("[1,2]").Diagnosztika();
            Szám.Parse("[[[[[9,8],1],2],3],4]").Diagnosztika();
            Szám.Parse("[7,[6,[5,[4,[3,2]]]]]").Diagnosztika();
            Szám.Parse("[[6,[5,[4,[3,2]]]],1]").Diagnosztika();
            Szám.Parse("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]").Diagnosztika();
            Szám.Parse("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]").Diagnosztika();

            Console.WriteLine("------------------------");
            Szám a = Szám.Parse("[[[[4,3],4],4],[7,[[8,4],9]]]");
            Szám b = Szám.Parse("[1, 1]");
            Console.WriteLine($"{a} + {b}");
            (a + b).Diagnosztika();
            */
            string lista = @"[1,1]
[2,2]
[3,3]
[4,4]";
            lista = @"[1,1]
[2,2]
[3,3]
[4,4]
[5,5]";
            lista = @"[1,1]
[2,2]
[3,3]
[4,4]
[5,5]
[6,6]";
            lista = @"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]
[7,[5,[[3,8],[1,4]]]]
[[2,[2,2]],[8,[8,1]]]
[2,9]
[1,[[[9,3],9],[[9,0],[0,7]]]]
[[[5,[7,4]],7],1]
[[[[4,2],2],6],[8,7]]";

            // lista.Split('\n').Select(Szám.Parse).Sum().Diagnosztika();

            /*
            Szám.Parse("[[1,2],[[3,4],5]]").Diagnosztika();
            Szám.Parse("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]").Diagnosztika();
            Szám.Parse("[[[[1,1],[2,2]],[3,3]],[4,4]]").Diagnosztika();
            Szám.Parse("[[[[3,0],[5,3]],[4,4]],[5,5]]").Diagnosztika();
            Szám.Parse("[[[[5,0],[7,4]],[5,5]],[6,6]]").Diagnosztika();
            Szám.Parse("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]").Diagnosztika();

            */
            lista = @"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]";

            // lista.Split('\n').Select(Szám.Parse).Sum().Diagnosztika();


            List<Szám> számlista = lista.Split('\n').Select(Szám.Parse).ToList();
            foreach (Szám item in számlista)
            {
                item.Diagnosztika();
            }
            Console.WriteLine("------------------------");
            foreach (Szám item in számlista)
            {
                item.Diagnosztika();
            }
            
            Console.WriteLine("--------------------");

            foreach ((Szám, Szám) pár in számlista.Párok())
            {
                (Szám a, Szám b) = pár;
                Szám ö = a + b;
                Console.WriteLine($"{ö.Magnitúdó} :  {a}   +   {b}   =   {ö}");
                //Console.WriteLine($"{a}   +   {b}   =   ");
            }
            Console.WriteLine("--------------------");
            foreach (Szám item in számlista.Összegek().OrderBy(x=>x.Magnitúdó))
            {
                item.Diagnosztika();
            }
            /*
             */

           


            /** /
            System.IO.File.ReadAllText("input.txt").Split('\n').Select(Szám.Parse).Sum().Diagnosztika();
            /*/
            List<Szám> összegek = System.IO.File.ReadAllText("input.txt").Split('\n').Select(Szám.Parse).Összegek();

            // Console.WriteLine(String.Join("\n",összegek.Select(szám=>szám.Magnitúdó).OrderBy(x=>x)));
            foreach (var item in összegek.OrderBy(x => x.Magnitúdó))
            {
                item.Diagnosztika();
            }
            /**/

            Console.ReadLine();
        }


    }
}
