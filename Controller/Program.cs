namespace WakeApp.Controller
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    using WakeApp.Type;

    public sealed class Program
    {
        private static string filePath;

        private static void Main(string[] args)
        {
            filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "XMLFile\\SaveFile.Xml");

            Console.WriteLine(CustomString.Start);
            Console.WriteLine(CustomString.Space);
            Console.WriteLine(CustomString.GetValues);

            Model model = LoadModel();

            Console.WriteLine($"Ankunftszeit: {model.Arrival} \nReisezeit in Minuten: {model.PrepTimeInMin} Min\nVorbereitungszeit in Minuten: {model.TravelTimeInMin} Min\nEingeplante Verzögerzungen in Minuten: {model.Delay} Min\nWakeTime: {model.WakeTime}");

            if (YesOrNo(CustomString.Correct))
            {
                Order();
            }
            
            Console.WriteLine(CustomString.ThankYou);
            Console.ReadKey();
        }

        private static void Order()
        {
            var arrival = GetArrivalTime();

            var travelTime = GetTravelTime();

            var prepTime = GetPrepTime();

            var delayNeeded = YesOrNo(CustomString.Delay);

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
            if (YesOrNo(CustomString.Happy))
            {
                XDocument document = XDocument.Load(filePath);
                XElement xElement = document.Element("Model");

                xElement.ReplaceWith(
                    new XElement("Model",
                        new XAttribute("Arrival", model.Arrival),
                        new XAttribute("TravelTime", model.TravelTimeInMin),
                        new XAttribute("PrepTime", model.PrepTimeInMin),
                        new XAttribute("Delay", model.Delay),
                        new XAttribute("WakeTime", model.WakeTime)));

                document.Save(filePath);

                Console.WriteLine(CustomString.Save);
                Console.ReadKey();

                return;
            }

            Console.Clear();
            Order();
        }

        private static Model LoadModel()
        {
            XmlDocument document = new XmlDocument();
            document.Load(filePath);
            var xdoc = document.DocumentElement;

            var arrival = xdoc.GetAttribute("Arrival");
            var travel = xdoc.GetAttribute("TravelTime");
            var prep = xdoc.GetAttribute("PrepTime");
            var delay = xdoc.GetAttribute("Delay");
            var wakeUp = xdoc.GetAttribute("WakeTime");

            Model model = new Model()
            {
                Arrival = FormatDateTime(arrival),
                TravelTimeInMin = ConvertStringToInt(travel),
                PrepTimeInMin = ConvertStringToInt(prep),
                Delay = ConvertStringToInt(delay),
                WakeTime = FormatDateTime(wakeUp)
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

        private static bool YesOrNo(string nextString)
        {
            while (true)
            {
                Console.WriteLine(nextString);
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
            try
            {
                DateTime formattedDate;

                formattedDate = Convert.ToDateTime(dateTimeString).ToLocalTime();
                return formattedDate;
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
    }
}
