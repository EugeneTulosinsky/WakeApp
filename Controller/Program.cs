namespace WakeApp.Controller
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    using WakeApp.Type;

    public sealed class Program
    {
        private static string filePath;
        private static char[] filterOptions = { '/', '<', '>' };


        private static void Main(string[] args)
        {
            filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "XMLFile\\SaveFile.Xml");

            Console.WriteLine(CustomString.Start);
            Console.WriteLine(CustomString.Space);
            Console.WriteLine(CustomString.GetValues);

            Model model = LoadModel();

            if (model == null)
            {
                Console.WriteLine(CustomString.NotFound);
                Console.WriteLine(CustomString.Continue);
                Console.Clear();
                Order();
            }
            else
            {
                Console.WriteLine($"Ankunftszeit: {model.Arrival} \nReisezeit in Minuten: {model.PrepTimeInMin} Min\nVorbereitungszeit in Minuten: {model.TravelTimeInMin} Min\nEingeplante Verzögerzungen in Minuten: {model.Delay} Min\nWakeTime: {model.WakeTime}");
                Console.ReadKey();
            }

        }

        private static void Order()
        {
            var arrival = GetArrivalTime();

            var travelTime = GetTravelTime();

            var prepTime = GetPrepTime();

            var delayNeeded = DelayNeeded();

            if (delayNeeded)
            {
                var delay = GetDelayTime();
                FillModel(arrival, travelTime, prepTime, true, delay);
            }

            FillModel(arrival, travelTime, prepTime);
        }

        private static void FillModel(DateTime arrival, int travelTime, int prepTime, bool delayNeeded = false, int delay = 0)
        {
            if (delayNeeded)
            {
                DateTime wakeTime = GetWakeTime(arrival, travelTime, prepTime, delay);

                Model model = new Model(arrival, travelTime, prepTime, delay, wakeTime);

                SaveModel(model);
            }
            else
            {
                DateTime wakeTime = GetWakeTime(arrival, travelTime, prepTime, delay);

                Model model = new Model(arrival, travelTime, prepTime, wakeTime);

                SaveModel(model);
            }
        }

        private static void SaveModel(Model model)
        {
            if (UserInputCorrect())
            {
                XDocument document = XDocument.Load(filePath);
                document.Element("metadata")?.Add(
                    new XElement("Model",
                        new XAttribute("Arrival", model.Arrival),
                        new XAttribute("TravelTime", model.TravelTimeInMin),
                        new XAttribute("PrepTime", model.PrepTimeInMin),
                        new XAttribute("Delay", model.Delay),
                        new XAttribute("WakeTime", model.WakeTime)));

                document.Save(filePath);
            }

            Console.Clear();
            Order();
        }

        private static Model LoadModel()
        {
            XmlDocument document = new XmlDocument();
            document.Load(filePath);

            XmlElement element = document.DocumentElement;
            var xmlData = SplitStrings(element?.InnerXml, filterOptions);


            Model model = new Model()
            {
                Arrival = FormatDateTime(xmlData[1]),
                TravelTimeInMin = ConvertStringToInt(xmlData[4]),
                PrepTimeInMin = ConvertStringToInt(xmlData[7]),
                Delay = ConvertStringToInt(xmlData[10]),
                WakeTime = FormatDateTime(xmlData[13])
            };

            return model;
        }

        private static DateTime GetArrivalTime()
        {
            Console.WriteLine(CustomString.ArrivalTime);
            string arrival = Console.ReadLine();
            DateTime arrivalDateTime = FormatDateTime(arrival);

            return arrivalDateTime;
        }

        private static int GetTravelTime()
        {
            Console.WriteLine(CustomString.TravelTime);
            string userTravel = Console.ReadLine();
            int travelTime = ConvertStringToInt(userTravel);

            return travelTime;
        }

        private static int GetPrepTime()
        {
            Console.WriteLine(CustomString.PrepTime);
            string userPrep = Console.ReadLine();
            int prepTime = ConvertStringToInt(userPrep);

            return prepTime;
        }

        private static int GetDelayTime()
        {
            Console.WriteLine(CustomString.DelayTime);
            string userDelay = Console.ReadLine();
            int delay = ConvertStringToInt(userDelay);

            return delay;
        }

        private static DateTime GetWakeTime(DateTime arrival, int travelTime, int prepTime, int delay)
        {
            var timeNeeded = travelTime + prepTime + delay;

            DateTime wakeTime = arrival.AddMinutes(-timeNeeded);

            return wakeTime;
        }

        private static bool UserInputCorrect()
        {
            while (true)
            {
                Console.WriteLine(CustomString.Correct);
                var userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "Y":
                        return true;
                    case "N":
                        return false;
                    default:
                        Console.WriteLine("Error: Wrong Input. Try Again");
                        break;
                }
            }
        }

        private static bool DelayNeeded()
        {
            while (true)
            {
                Console.WriteLine(CustomString.Delay);
                var userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "Y":
                        return true;
                    case "N":
                        return false;
                    default:
                        Console.WriteLine("Error: Wrong Input. Try Again");
                        break;
                }
            }
        }

        private static DateTime FormatDateTime(string dateTimeString)
        {
            DateTime formatedDate;

            try
            {
                formatedDate = Convert.ToDateTime(dateTimeString);
                return formatedDate;
            }
            catch (FormatException)
            {
                Console.WriteLine(CustomString.FormatError, dateTimeString);
                throw;
            }
        }

        private static int ConvertStringToInt(string userInput)
        {
            try
            {
                return Convert.ToInt32(userInput);
            }
            catch (FormatException)
            {
                Console.WriteLine(CustomString.FormatError, userInput);
                throw;
            }
        }

        private static string[] SplitStrings(string stringToFilter, char[] filter)
        {
            var splittedString = stringToFilter.Split(filter).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            return splittedString;
        }
    }
}
