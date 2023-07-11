using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceSimManager
{
    public class Driver
    {
        public string Name { get; set; }
        public double Speed { get; set; }
        public double CorneringAbility { get; set; }
        public double CurrentLapTime { get; set; }
        public double PreviousLapTime {  get; set; }
        public bool isRainTiresEnabled { get; set; }

        public class Race
        {
            public enum Weather
            {
                Sunny,
                Rainy
            }

            public Weather CurrentWeather { get; set; }

            private Weather GetRandomWeather()
            {
                Random random = new Random();
                int weatherIndex = random.Next(0, 1); // Generates a random number between 0 and 1 for sunny or rain
                return (Weather)weatherIndex;
            }
            private List<Driver> drivers;
            public int NumberOfLaps { get; set; }
            public class Track
            {
                public string Name { get; set; }
                public double baseTime { get; set; }
            }

            private List<Track> tracks;

            public void InitializeTracks()
            {
                Track tracks1 = new Track { Name = "Laguna Seca", baseTime = 100.2d };
                Track tracks2 = new Track { Name = "Sonoma Raceway", baseTime = 111.4d };

                // Add the tracks to the list
                tracks.Add(tracks1);
                tracks.Add(tracks2);
            }

            public Race()
            {
                drivers = new List<Driver>();
                tracks = new List<Track>();
            }

            public void AddDriver(Driver driver)
            {
                drivers.Add(driver);
            }

            public double SimulateLapPerformance(double baseTime, Driver driver, Weather currentWeather, bool isRainTiresEnabled)
            {
                double speedFactor = 1.0 - (driver.Speed / 100.0);
                double corneringFactor = 1.0 - (driver.CorneringAbility / 100.0);
                double lapTime;


                // Handle if driver is 100 skill level in speed and cornering
                if (driver.Speed == 100.0 && driver.CorneringAbility == 100.0)
                {
                    lapTime = baseTime;
                }
                // Handle if only driver speed is 100 skill level
                else if (driver.Speed == 100.0)
                {
                    lapTime = baseTime * corneringFactor;
                }
                // Handle if only driver cornering is 100 skill level
                else if (driver.CorneringAbility == 100.0)
                {
                    lapTime = baseTime * speedFactor;
                }
                // handle all others
                else
                {
                    lapTime = baseTime * speedFactor * corneringFactor;
                }

                // Adjust lap time based on the weather condition
                if (currentWeather == Weather.Rainy)
                {
                    double rainTime = isRainTiresEnabled ? 5.0 : 10.0;
                    lapTime += rainTime;
                }

                // Add a random factor to account for the randomness of all races
                Random random = new Random();
                double randomFactor = random.NextDouble() * 0.2 + 0.9;
                lapTime *= randomFactor;

                return lapTime;
            }

            public void SimulateRace(Track currentTrack)
            {
                // Generate random weather for race
                CurrentWeather = GetRandomWeather();

                for (int lap = 1; lap <= NumberOfLaps; lap++)
                {
                    Console.WriteLine("Lap " + lap);

                    // Simulate lap performance for each driver
                    foreach (var driver in drivers)
                    {
                        driver.PreviousLapTime = driver.CurrentLapTime; // Store the previous lap time
                        driver.CurrentLapTime = SimulateLapPerformance(currentTrack.baseTime, driver, CurrentWeather, driver.isRainTiresEnabled);
                        Console.WriteLine(driver.Name + " Lap time: " + driver.CurrentLapTime);
                    }

                    // Update Race progress and positions based on lap times
                    UpdateRacePositions(drivers);

                    // Display race progress and driver positions
                    DisplayRaceProgress();
                }
            }

            private void UpdateRacePositions(List<Driver> drivers)
            {
                // Sort the drivers based on the current lap times (ascending order)
                drivers.Sort((driver1, driver2) =>
                {
                    int compareResult = driver1.CurrentLapTime.CompareTo(driver2.CurrentLapTime);

                    // If the current lap times are the same, compare to the previous lap times
                    if (compareResult == 0)
                    {
                        return driver1.PreviousLapTime.CompareTo(driver2.PreviousLapTime);
                    }
                    return compareResult;
                });

                // Update the positions based on sorted order
                for (int i = 0; i < drivers.Count; i++)
                {
                    // Check if the driver can overtake the previous driver
                    if (i > 0 && drivers[i].CurrentLapTime < drivers[i - 1].CurrentLapTime)
                    {
                        // Perform the overtaking maneuver for mulitple drivers
                        for (int j = i; j > 0; j--)
                        {
                            if (drivers[j].CurrentLapTime < drivers[j - 1].CurrentLapTime)
                            {
                                var temp = drivers[j];
                                drivers[j] = drivers[j - 1];
                                drivers[j - 1] = temp;
                            }
                            else
                            {
                                break; // Stop overtaking if the previous driver is not slower
                            }
                        }
                    }
                }
            }

            private void DisplayRaceProgress()
            {
                Console.WriteLine("Race progress:");

                // Display headers
                Console.WriteLine("Lap\tDriver\tPosition\tDifference\tTime Behind Leader\tRemaining Laps");

                // Display race progress for each driver
                for (int i = 0; i < drivers.Count;  i++)
                {
                    Driver driver = drivers[i];

                    string lap = (i + 1).ToString();
                    string positions = (i +1).ToString();
                    string difference = (driver.CurrentLapTime - driver.PreviousLapTime).ToString("0.000");
                    string timeBehindLeader = (driver.CurrentLapTime - drivers[0].CurrentLapTime).ToString("0.000");

                    Console.WriteLine($"{lap}\t{driver.Name}\t{positions}\t{difference}\t{timeBehindLeader}\t{NumberOfLaps - (i + 1)}");
                }
            }
        }
    }
}
