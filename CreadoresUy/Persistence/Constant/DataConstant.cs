using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Constant
{
    public class DataConstant
    {

        public const int UserQuantity = 500;
        public const int CreatorQuantity = 20;
        public static int MaxPlans = 10;
        public static int MaxBenefits = 4;
        public static int MaxTags = 5;


        public static int MaxContent = 20;
        //100 = 10, el numero más a la derecha será un numero despues de la coma
        public static int MaxPricePlan = 15;



        static readonly string nameFile = "NameData.txt";
        static readonly string adjetiveFile = "AdjetiveData.txt";
        static readonly string descriptionFile = "DescriptionData.txt";
        static readonly string contentTitleFile = "ContentTitleData.txt";
        static readonly string contentTextFile = "ContentTextData.txt";
        static readonly string TagFile = "TagData.txt";
        static readonly string BenefitsFile = "BenefitsData.txt";



        static readonly string NAME_PATH = Path.Combine(Environment.CurrentDirectory.Replace("Api","Persistence"), @"Data\", nameFile);

        static readonly string ADJETIVE_PATH = Path.Combine(Environment.CurrentDirectory.Replace("Api", "Persistence"), @"Data\", adjetiveFile);

        static readonly string DESCRIPTION_PATH = Path.Combine(Environment.CurrentDirectory.Replace("Api", "Persistence"), @"Data\", descriptionFile);

        static readonly string CONTENTITLE_PATH = Path.Combine(Environment.CurrentDirectory.Replace("Api", "Persistence"), @"Data\", contentTitleFile);

        static readonly string CONTENTTEXT_PATH = Path.Combine(Environment.CurrentDirectory.Replace("Api", "Persistence"), @"Data\", contentTextFile);

        static readonly string TAG_PATH = Path.Combine(Environment.CurrentDirectory.Replace("Api", "Persistence"), @"Data\", TagFile);

        static readonly string BENEFITS_PATH = Path.Combine(Environment.CurrentDirectory.Replace("Api", "Persistence"), @"Data\", BenefitsFile);


        static readonly public List<String> CategoryDescription = new List<string>()
        {
            "Para los que le gusta ejercitarce",
            "Cantar y crear nuevas melodías",
            "Expreciones Artisticas varias",
            "Dibujo en todas sus expreciones",
            "Hacer reír al publico",
            "Creracion de nuevos lugares",
            "Desarrollo de videojuegos o de plataformas de aprendizaje",
            "El septimo arte"
        };

        public List<String> Names = new List<string>();

        public List<String> Adjetives = new List<string>();

        public List<String> Theme = new List<string>();

        public List<String> Description = new List<string>();

        public List<String> ContentTiles = new List<string>();

        public List<String> ContentTexts = new List<string>();

        public List<String> Tags = new List<string>();

        public List<String> Benefits = new List<string>();




        public List<String> Domains = new List<string>()
        {
            "gmail.com",
            "yahoo.com",
            "hotmail.com",
            "aol.com",
            "hotmail.co.uk",
            "hotmail.fr",
            "live.com",
            "outlook.com"
        };



        public DataConstant()
        {

            if (File.Exists(NAME_PATH))
            {
                Names = File.ReadAllLines(NAME_PATH).ToList();

            }

            if (File.Exists(ADJETIVE_PATH))
            {
                Adjetives = File.ReadAllLines(ADJETIVE_PATH).ToList();

            }
            
            if (File.Exists(DESCRIPTION_PATH))
            {
                Description = File.ReadAllLines(DESCRIPTION_PATH).ToList();

            }
            if (File.Exists(CONTENTITLE_PATH))
            {
                ContentTiles = File.ReadAllLines(CONTENTITLE_PATH).ToList();

            }
            if (File.Exists(CONTENTTEXT_PATH))
            {
                ContentTexts = File.ReadAllLines(CONTENTTEXT_PATH).ToList();

            }
            if (File.Exists(TAG_PATH))
            {
                Tags = File.ReadAllLines(TAG_PATH).ToList();

            }
            if (File.Exists(BENEFITS_PATH))
            {
                Benefits = File.ReadAllLines(BENEFITS_PATH).ToList();

            }
        }

    }
}
