using MySql.Data.MySqlClient;
namespace PompeAEssenceToDB;
class Program //dotnet new console --framework net6.0 --use-program-main
{
    static void Main(string[] args)
    {
        double stockEssence = 5000;
        double prixEssence = 2.033;
        double stockGasoil = 1500;
        double prixGasoil = 1.718;
        string reponse ="non";
        List<string> historique = new List<string>();
        while (reponse !="oui"){
            Console.WriteLine("Actions : \n[1] - Carburants\n[2] - Historique\n[3] - Quitter");
            switch (Console.ReadLine())
            {
                case "1" or "[1]":
                    Console.Clear();
                    Console.WriteLine("Vous avez choisi l'essense");
                    if (ChoixCarburant())
                    {
                        Console.WriteLine("Choisissez votre quantié d'essence\nEssence : " + prixEssence + " euros/L");
                        double quantiteEssenceChoisie = choixQuantité();
                        stockEssence = stockEssence - quantiteEssenceChoisie;
                        Console.WriteLine("Vous avez pris "+ quantiteEssenceChoisie + "L d'essence pour " + quantiteEssenceChoisie * prixEssence+" euros");
                        Console.WriteLine(stockEssence);
                        string historiqueItem = DateTime.Now + "" + quantiteEssenceChoisie + " litres d'essence achetée"+ prixEssence +" euros";
                        historique.Add(historiqueItem);
                        WriteHisto("essence",quantiteEssenceChoisie);
                    }
                    else
                    {
                        Console.WriteLine("Choisissez votre quantié de gasoil\nGasoil : " + prixGasoil + " euros/L");
                        double quantiteGasoilChoisie = choixQuantité();
                        stockGasoil = stockGasoil - quantiteGasoilChoisie;
                        Console.WriteLine("Vous avez pris " + quantiteGasoilChoisie + "L de gasoil pour " + quantiteGasoilChoisie * prixGasoil + " euros");
                        Console.WriteLine(stockGasoil);
                        string historiqueItem = DateTime.Now + "" + quantiteGasoilChoisie + " litres de gasoil acheté"+ prixGasoil +" euros";
                        historique.Add(historiqueItem);
                        WriteHisto("gasoile",quantiteGasoilChoisie);
                    }
                    break;
                case "2" or "[2]":
                    Console.Clear();
                    Console.WriteLine("Voici l'historique :");
                    LectureHisto();
                    break;
                case "3" or "[3]":
                    Console.Clear();
                    Console.WriteLine("Voulez-vous quitter ? [oui] ou [non]");
                    reponse = Console.ReadLine();
                    break;
                default:
                    break;
            }
        }
    }
    public static bool ChoixCarburant()
        {
            Console.WriteLine("Choisissez votre carburant : \n[1] - Essence\n[2] - Gasoil");
            switch (Console.ReadLine())
            {
                case "1" or "[1]":
                    Console.WriteLine("Vous avez choisi l'essense");
                    return true;
                    break;
                case "2" or "[2]":
                    Console.WriteLine("Vous avez choisi le gasoil");
                    return false;
                    break;
                default:
                    return ChoixCarburant();
                    break;
            }
        }
        public static double choixQuantité()
        {
            Console.WriteLine("Veuillez choisir votre quantité de carburant");
            string entreeQuantiteChoisie = Console.ReadLine();
            if (entreeQuantiteChoisie.All(char.IsDigit)){
                return double.Parse(entreeQuantiteChoisie);
            }
            else
            {
                return choixQuantité();
            }
        }
    static void Connection()
    {
        try
        {
            string connectionString = GetConnectionString();
            using(MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection à la BDD");
            }
        }
        catch(Exception e)
        {
            Console.WriteLine("Impossible de se connecter à la BDD " + e.Message);
        }
    }
    static void LectureHisto(){
        string donnees = "";
        Console.WriteLine("Lecture historique\r\n");
        try{
            string connectionString = GetConnectionString();
            string queryString = "SELECT * FROM historiqueAchat;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(queryString, connection);
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader()){
                    while (reader.Read()){
                        donnees="";
                        for (int z = 0; z<=3; z++){
                            donnees = donnees + reader[z].ToString()+" ";
                        }
                        //Afichage
                        Console.WriteLine(donnees);
                    }
                }
            }
        }
        catch(Exception e)
        {
            Console.WriteLine("Impossible de se connecter à la BDD " + e.Message);
        }
    }
    static private string GetConnectionString(){
        return "SERVER=localhost;DATABASE=C#;UID=root;password=;";
    }
    static void WriteHisto(string carburant,double quantite)
        {
            string connectionString = GetConnectionString();
            var date = DateTime.Now.ToString("yyyyMMddhhmmss");

            try
            {
                string queryString = "INSERT historiqueachat(date,carburant,quantite) VALUES(";

                queryString += "'" + date + "','" + carburant + "','" + quantite + "'";
                queryString += ");";
                Console.WriteLine(queryString);
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(queryString, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Achat ajouté à l'historique ! ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("La liste n'a pas été exportée " + e.Message);
            }
        }
    }