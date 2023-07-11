using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceSimManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize the race
            Driver.Race race = new Driver.Race();
            race.InitializeTracks();

            // Select drivers
            List<Driver> drivers = SelectDrivers();

            // Generate random AI teams
            List<Driver> nonUserTeams = GenerateRandomTeams();

            // Add selected drivers and AI drivers to the race
            foreach ( var driver in drivers )
            {
                race.AddDriver(driver);
            }
            foreach ( var driver in nonUserTeams )
            {
                race.AddDriver(driver);
            }

            // Randomly select a track for the race
            Driver.Race.Track currentTrack = race.GetRandomTrack();

            // Start the race sim
            race.SimulateRace(currentTrack);

            // Race finished

        }

        static List<Driver> SelectDrivers()
        {
            List<Driver> selectedDrivers = new List<Driver>();

            Console.WriteLine("Select your drivers:");

            // Display available drivers

            // Get user input for driver selection

            // Add selected drivers to the list

            return selectedDrivers;
        }

        static List<Driver> GenerateRandomTeams()
        {
            List<Driver> randomTeams = new List<Driver>();

            Console.WriteLine("Other teams racing today are:");

            // Generate random drivers and add them to the list

            return randomTeams;
        }
    }
}
