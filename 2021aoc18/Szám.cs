using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021aoc18
{
    class Szám
    {
        Szám bal;
        Szám jobb;
        Szám szülő;
        int mélység;
        int tartalom;
        bool Levél { get => tartalom != -1; }



        public Szám(int tartalom, Szám szülő=null) : this(null, null, szülő, Mélység_származtatása(szülő), tartalom) { }
        public Szám((Szám, Szám) t, Szám szülő) : this(t.Item1, t.Item2, szülő, Mélység_származtatása(szülő), -1) { }
        public Szám(Szám bal, Szám jobb, Szám szülő, int mélység, int tartalom)
        {
            this.bal = bal;
            this.jobb = jobb;
            this.szülő = szülő;
            this.mélység = mélység;
            this.tartalom = tartalom;

            Óvatosan(Szülő_Update,bal);
            Óvatosan(Szülő_Update,jobb);
            Óvatosan(Mélység_Update,bal);
            Óvatosan(Mélység_Update,jobb);
        }
        public static Szám Parse(string str) => Parse(str.Trim(), null);
        public static Szám Parse(string str, Szám szülő) => Parse(str, str.Legkülső_vessző_helye(), szülő);
        static Szám Parse(string str, int i, Szám szülő) => i == str.Length ? new Szám(int.Parse(str), szülő) : new Szám(str.Kettétörése(i).Számai(), szülő);
        public static Szám Copy(Szám szám) => szám == null ? null : new Szám(Copy(szám.bal), Copy(szám.jobb), null, szám.mélység, szám.tartalom).Szülő_Teljes_Update();

        Szám Szülő_Teljes_Update()
        {
            if (bal != null) // nem írható át Óvatosannal, mert az már végtelen ciklushoz vezet!
            {
                bal.szülő = this;
                bal.Szülő_Teljes_Update();
            }
            if (jobb != null)
            {
                jobb.szülő = this;
                jobb.Szülő_Teljes_Update();
            }
            return this;
        }

        public static Szám operator +(Szám a, Szám b) => new Szám(Copy(a), Copy(b), null, 0, -1).Egyszerűsít();
        public static Szám operator +(Szám a, int b) => Copy(a) + new Szám(b);
        public static Szám operator +(int a, Szám b) => new Szám(a) + Copy(b);

        public static void Óvatosan(Action f, Szám szám) { if (szám != null) f(); }
        public static void Óvatosan(Action<Szám> f, Szám szám) { if (szám != null) f(szám); }
        public static void Óvatosan(Action<Szám, int> f, Szám szám, int x) { if (szám != null) f(szám,x); }
        public static Szám Óvatosan(Func<Szám, Szám> f, Szám szám) => szám == null ? null : f(szám);

        public override string ToString() => Levél ? tartalom.ToString() : $"[{bal},{jobb}]";
        public string ToGraphViz() => Levél ? "" : bal.ToGraphViz() + jobb.ToGraphViz() + $"\"{this}\"->\"{bal}\";\n\"{this}\"->\"{jobb}\";\n";

        public Szám Egyszerűsít()
        {
            // Console.Error.WriteLine($"Egyszerűsítés: {this}");
            Szám egyszerű_pár = Legbalsó_levélszülő();
            if (egyszerű_pár != null)
                return egyszerű_pár.Robbant().Egyszerűsít();

            Szám hagyományos_szám = Legbalsó_nagy_szám();
            if (hagyományos_szám != null)
                return hagyományos_szám.Szétesik().Egyszerűsít();

            return Gyökér;
        }


        static int Mélység_származtatása(Szám szülő) => szülő == null ? 0 : szülő.mélység + 1;
        void Mélység_Update()
        {
            mélység = Mélység_származtatása(szülő);
            if (bal != null) // nem írható át Óvatosannal, mert az már végtelen ciklushoz vezet!
                bal.Mélység_Update();
            if (jobb != null)
                jobb.Mélység_Update();
        }

        void Szülő_Update(Szám szám) => szám.szülő = this;
        bool Gyerek { get => szülő != null; }
        bool Balgyerek { get => Gyerek && szülő.bal == this; }
        bool Jobbgyerek { get => Gyerek && szülő.jobb == this; }
        Szám Gyökér { get => szülő == null ? this : szülő.Gyökér; }
        int Magasság { get => Levél ? 0 : Math.Max(bal.Magasság, jobb.Magasság) + 1; }
        public int Magnitúdó { get => Levél ? tartalom : 3 * bal.Magnitúdó + 2 * jobb.Magnitúdó; }
        Szám Jobbszomszéd { get => Óvatosan(Legbalsó_levél,Óvatosan(Jobb,Jobbszomszéddal_közös_ős())); }
        Szám Balszomszéd { get => Óvatosan(Legjobbsó_levél,Óvatosan(Bal,Balszomszéddal_közös_ős())); }
        static Szám Jobb(Szám szám) => szám.jobb;
        static Szám Bal(Szám szám) => szám.bal;

        void Csere(Szám szám) { if (Balgyerek) szülő.bal = szám; else szülő.jobb = szám; }
        Szám Szétesik() { Csere(Parse($"[{tartalom / 2},{tartalom / 2 + tartalom % 2}]", szülő)); return Gyökér; }
        Szám Robbant()
        {
            Óvatosan(Tartalomnövelés, Balszomszéd, bal.tartalom);
            Óvatosan(Tartalomnövelés, Jobbszomszéd, jobb.tartalom);
            Csere(new Szám(0,szülő));
            return Gyökér;
        }
        static void Tartalomnövelés(Szám target, int ezzel) { target.tartalom += ezzel; }
        static Szám Legbalsó_levél(Szám szám) => szám.Levél ? szám : (szám.bal.Levél ? szám.bal : Legbalsó_levél(szám.bal));
        static Szám Legjobbsó_levél(Szám szám) => szám.Levél ? szám : (szám.jobb.Levél ? szám.jobb : Legjobbsó_levél(szám.jobb));
        Szám Balszomszéddal_közös_ős() => Balgyerek ? szülő.Balszomszéddal_közös_ős() : szülő;
        Szám Jobbszomszéddal_közös_ős() => Jobbgyerek ? szülő.Jobbszomszéddal_közös_ős() : szülő;
        public Szám Legbalsó_levélszülő() => Legbalsó_levélszülő(4);
        //Szám Legbalsó_levélszülő(int m) => (bal.Levél && jobb.Levél) ? (mélység < m ? null : this) : (bal.Levél ? jobb.Legbalsó_levélszülő(m) : Ha_itt_nincs_levélszülő_irány_jobbra(m, bal.Legbalsó_levélszülő(m)));
        Szám Legbalsó_levélszülő(int m) 
        {
            if (Levél)
                return null;
            if (bal.Levél && jobb.Levél)
                return mélység < m ? null : this;
            if (bal.Levél)
                return jobb.Legbalsó_levélszülő(m);
            return Ha_itt_nincs_levélszülő_irány_jobbra(m, bal.Legbalsó_levélszülő(m));
        }
        Szám Ha_itt_nincs_levélszülő_irány_jobbra(int m, Szám balresult) => balresult == null ? jobb.Legbalsó_levélszülő(m) : balresult;

        public Szám Legbalsó_nagy_szám() => Legbalsó_nagy_szám(10);
        Szám Legbalsó_nagy_szám(int n) => n <= tartalom ? this : (Levél ? null : Ha_itt_nincs_nagy_szám_jobbra_keresd(n, bal.Legbalsó_nagy_szám(n)));
        /*
        * /
        Szám Legbalsó_nagy_szám(int n)
        {
            if (n <= tartalom)
                return this;
            if (Levél)
                return null;
            return Ha_itt_nincs_nagy_szám_jobbra_keresd(n,bal.Legbalsó_nagy_szám(n));
        }
        /*
        */
        Szám Ha_itt_nincs_nagy_szám_jobbra_keresd(int n, Szám balresult) => balresult == null ? jobb.Legbalsó_nagy_szám(n) : balresult;
        /*
        Szám Ha_balra_nincs_jobbra_keresd(int n, Szám balresult)
        {
            if (balresult == null)
                return jobb.Legbalsó_nagy_szám(n);
            return balresult;
        }
        */

        public void Diagnosztika()
        {
            //Console.WriteLine(ToGraphViz());
            // Console.WriteLine($"==== {this} ====");
            //Console.WriteLine($"Legbalsó egyszerű párja: {Legbalsó_levélszülő()}");
            // Console.WriteLine($"Legbalsó nagy száma: {Legbalsó_nagy_szám()}");
            Console.WriteLine($"Magnitúdó: {Magnitúdó} -- {this}");
        }

        
    }

    static class Stringkiterjesztések
    {
        public static int Legkülső_vessző_helye(this string str)
        {
            int nyitott_zárójelek_száma = 0;
            int i = 0;
            while (i < str.Length && !(nyitott_zárójelek_száma == 1 && str[i] == ','))
            {
                if (str[i] == '[')
                    nyitott_zárójelek_száma++;
                else if (str[i] == ']')
                    nyitott_zárójelek_száma--;
                i++;
            }
            return i;
        }
        public static (string, string) Kettétörése(this string str, int i) => (str.Substring(1, i - 1), str.Substring(i + 1, str.Length - 1 - (i + 1)));
        public static (Szám, Szám) Számai(this (string, string) p) => (Szám.Parse(p.Item1), Szám.Parse(p.Item2));
        public static Szám Sum(this IEnumerable<Szám> lista)
        {
            Szám s = lista.First();
            foreach (Szám szám in lista.Skip(1))
                s += szám;
            return s;
        }

        public static List<Szám> Összegek(this IEnumerable<Szám> lista)
        {
            List<Szám> result = new List<Szám>();
            Szám[] tömb = lista.ToArray();
            for (int i = 0; i < tömb.Length; i++)
                for (int j = i + 1; j < tömb.Length; j++)
                {
                    Szám ij = tömb[i] + tömb[j];
                    Szám ji = tömb[j] + tömb[i];
                    Console.WriteLine($"Magnitúdó = {ij.Magnitúdó} --- {tömb[i]}   +   {tömb[j]}   =   {ij}");
                    Console.WriteLine($"Magnitúdó = {ji.Magnitúdó} --- {tömb[j]}   +   {tömb[i]}  ===   {ji}");
                    result.Add(ij);
                    result.Add(ji);
                }
            return result;
        }

        public static List<(Szám, Szám)> Párok(this IEnumerable<Szám> lista)
        {
            List<(Szám, Szám)> result = new List<(Szám, Szám)>();
            Szám[] tömb = lista.ToArray();
            for (int i = 0; i < tömb.Length; i++)
                for (int j = i + 1; j < tömb.Length; j++)
                {
                    result.Add((tömb[i], tömb[j]));
                    result.Add((tömb[j], tömb[i]));
                }
            return result;
        }
    }
}