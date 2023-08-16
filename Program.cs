using ClasseLlistesAvancades;

internal class Program
{
    private static void Main(string[] args)
    {
        TaulaHash<object> taula = new TaulaHash<object>();
        StreamReader srLlibes = new StreamReader("Llibres.csv");
        string registre = srLlibes.ReadLine();
        registre = srLlibes.ReadLine();
        while (registre != null)
        {
            string[] campsRegistre = registre.Split(";");
            KeyValuePair<String, object> parella = new KeyValuePair<string, object>(campsRegistre[0], campsRegistre[3]);
            taula.Afegeix(parella);
            //Console.WriteLine("Afegit: " + campsRegistre[0]);
            registre = srLlibes.ReadLine();
        }
        taula.Estadistica();
        taula.Clear();
        taula.Estadistica();
        taula.Afegeix(new KeyValuePair<string, object>("a4558867", "Gozos á San Juan de Peñagolosa termino de Vistabella"));
        taula.Estadistica();
        taula.Elimina("a4558867");
        taula.Estadistica();
    }
}