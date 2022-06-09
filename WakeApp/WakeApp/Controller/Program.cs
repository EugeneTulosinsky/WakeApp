namespace WakeApp.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SqlServer.Server;
    using WakeApp.Type;

    public sealed class Program
    {
        private static void Main(string[] args)
        {
            for (int stringIndex = 0; stringIndex <= 7; stringIndex++)
            {
                Console.WriteLine(GetStringOutput(stringIndex));
            }
        }

        private static string GetStringOutput(int index, string userInput = null)
        {
            switch (index)
            {
                case (int)StringIndex.Start:
                    return CustomString.Start;
                case (int)StringIndex.Space:
                    return CustomString.Space;
                case (int)StringIndex.GetValues:
                    return CustomString.GetValues;
                case (int)StringIndex.NotFound:
                    return CustomString.NotFound;
                case (int)StringIndex.ArrivalTime:
                    return CustomString.ArrivalTime;
                case (int)StringIndex.ArrivalTimeAnswer:
                    return $"Sie wollen also um {userInput} ankommen.";
                case (int)StringIndex.TravelTime:
                    return CustomString.TravelTime;
                case (int)StringIndex.ReadyTime: 
                    return CustomString.ReadyTime;
                default:
                    return string.Empty;
            }
        }
    }
}
