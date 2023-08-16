using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasseLlistesAvancades
{
    public class TaulaHash<TB>
    {
        public const int MIDA = 73533;

        List<KeyValuePair<String, TB>>[] dades;

        public TaulaHash()
        {
            dades = new List<KeyValuePair<String, TB>>[MIDA];
            for(int i = 0; i < MIDA; i++) dades[i] = new List<KeyValuePair<string, TB>>();
        }

        #region Propietats
        /// <summary>
        /// Retorna el número de parelles clau valor que hi ha emmagatzemades a la taula hash
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                foreach(List<KeyValuePair<String, TB>> llista in dades)
                {
                    count += llista.Count();
                }
                return count;
            }
        }

        /// <summary>
        /// permet consultar un valor donada la seva clau (get), o substituir el valor existent associant amb aquest clau (set).
        /// Si la clau no existeix genera una KeyNotFoundException
        /// </summary>
        /// <param name="clau"></param>
        /// <returns></returns>
        public TB this[String clau]
        {
            get
            {
                TB valor = default(TB);
                try
                {
                    foreach (List<KeyValuePair<String, TB>> llista in dades)
                    {
                        int index = llista.FindIndex(n => n.Key.Equals(clau));
                        if (index != -1) valor = llista[index].Value;
                    }
                    if (valor.Equals(default(TB))) throw new KeyNotFoundException();
                }
                catch (KeyNotFoundException ke) { Console.WriteLine(ke); }
                return valor;
            }
            set
            {
                KeyValuePair<String, TB> newKvp = new KeyValuePair<String, TB>(clau, value);
                foreach (List<KeyValuePair<String, TB>> llista in dades)
                {
                    int index = llista.FindIndex(n => n.Key.Equals(clau));
                    if (index != -1) llista[index] = newKvp;
                }
            }
        }

        /// <summary>
        /// Retorna un ICollection amb les claus emmagatzemandes a la taula hash
        /// </summary>
        public ICollection<String> Claus
        {
            get
            {
                List<String> claus = new List<string>();
                foreach (List<KeyValuePair<String, TB>> llista in dades)
                {
                    List<String> lst = (from kvp in llista select kvp.Key).Distinct().ToList();
                    foreach (String clau in lst) claus.Append(clau);
                }
                return claus;
            }
        }

        /// <summary>
        /// Retorna un ICollection amb els valors emmagatzemandes a la taula hash
        /// </summary>
        public ICollection<TB> Valors
        {
            get
            {
                List<TB> valors = new List<TB>();
                foreach (List<KeyValuePair<String, TB>> llista in dades)
                {
                    List<TB> lst = (from kvp in llista select kvp.Value).Distinct().ToList();
                    foreach (TB valor in lst) valors.Append(valor);
                }
                return valors;
            }
        }
        #endregion

        #region Mètodes
        /// <summary>
        /// Retorna l'index per inserir a la taula calculat amb un hash.
        /// </summary>
        /// <param name="clau"></param>
        /// <returns></returns>
        public long Hash(String clau)
        {
            long resultat = 0, counter = 0;
            foreach (char c in clau)
            {
                resultat += (int)c * (long)Math.Pow(2, counter);
                counter++;
            }
            resultat %= MIDA;
            return resultat;
        }

        /// <summary>
        /// Afegeix aquesta parella clau valor a la taula hash. En cas que la clau fos duplicada, genera una ArgumentException
        /// </summary>
        /// <param name="parella"></param>
        public void Afegeix(KeyValuePair<String, TB> parella)
        {
            try
            {
                if (ConteClau(parella.Key)) throw new ArgumentException();
                else dades[Hash(parella.Key)].Add(parella);
            }
            catch (ArgumentException) { Console.WriteLine("Aquesta clau ja està dins de la taula."); }
        }

        /// <summary>
        /// Elimina tots els elements de la taula hash
        /// </summary>
        public void Clear()
        {
            foreach (List<KeyValuePair<String, TB>> llista in dades) if (llista != null) llista.Clear();
        }

        /// <summary>
        /// Determina si hi ha algun element amb aquesta clau
        /// </summary>
        /// <param name="clau"></param>
        /// <returns></returns>
        public bool ConteClau(String clau)
        {
            bool resultat = false;
            long r = Hash(clau);
            foreach (KeyValuePair<String, TB> kvp in dades[r])
            {
                if(kvp.Key == clau)
                {
                    resultat = true;
                    break;
                }
            }
            return resultat;
        }

        /// <summary>
        /// Elimina l'element amb aquesta clau retornant si existia o no
        /// </summary>
        /// <param name="clau"></param>
        /// <returns></returns>
        public bool Elimina(String clau)
        {
            bool eliminat = false;
            if (ConteClau(clau))
            {
                foreach (List<KeyValuePair<String, TB>> llista in dades)
                {
                    foreach(KeyValuePair<String, TB> kvp in llista.ToList())
                    {
                        if (kvp.Key == clau) llista.Remove(kvp);
                    }
                }
                eliminat = true;
            }
            return eliminat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Estadistica()
        {
            int[] nPosicions = new int[MIDA];
            foreach (List<KeyValuePair<String, TB>> llista in dades)
            {
                nPosicions[llista.Count]++;
            }
            for(int i = 0; i < MIDA; i++)
            {
                if(nPosicions[i] != 0) Console.WriteLine(nPosicions[i] + " posicions tenen " + i + " elements\n");
            }
        }
        #endregion
    }
}
