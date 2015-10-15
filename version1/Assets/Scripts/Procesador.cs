using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Procesador  {

    public string Reindex_(string fin)
    {
        if (!fin.Contains("_")) return fin;
        for (int i = 0; i < fin.Count(); i++)
        {
            if (fin[i] != '_') continue;
            if (fin[i - 1] != '#') continue;
            fin = fin.Remove(i, 1);
            fin = fin.Insert(i + 1, "_");
        }

        return fin;
    }

        public List<int> findposelemt(string x)
    {
        var posiciones = new List<int>();
        var cont = 0;
        foreach (var c in x)
        {
            if (c == '_')
            {

                posiciones.Add(cont);

            }
            cont++;
        }

        return posiciones;
    }

        public string Removepista(string palabraa)
        {
            return palabraa.Where(l => l != '#').Aggregate("", (current, l) => current + l);
        }

        public string Remove_(string pal)
        {
            return pal.Where(variable => variable != '_').Aggregate("", (current, variable) => current + variable);
        }
}
