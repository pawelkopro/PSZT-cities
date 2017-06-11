using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSZT
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                //string fileName = Console.ReadLine();

                string fileName = "Cities.csv";
                string filePath = Path.GetFullPath(Directory.GetCurrentDirectory() + @"\" + fileName);

                IList<City> cities = new List<City>();

                using (StreamReader sr = File.OpenText(filePath))
                {
                    string line = string.Empty;
                    line = sr.ReadLine();

                    while ((line = sr.ReadLine()) != null)
                    {
                        var lineList = line.Split(';');
                        cities.Add(new City
                        {
                            Id = Convert.ToInt32(lineList[0]),
                            Name = Convert.ToString(lineList[1]),
                            Latitude = Convert.ToDouble(lineList[2].Replace('.', ',')),
                            Longitude = Convert.ToDouble(lineList[3].Replace('.', ','))
                        });
                    }
                }

                var lines = new List<string>();

                string header = "0";
                for (int i = 0; i < cities.Count; i++)
                {
                    header += ";" + (i + 1);
                }

                lines.Add(header);

                int i1, i2;
                for (i1 = 0; i1 < cities.Count; i1++)
                {
                    var item1 = cities.ElementAt(i1);

                    string line = Convert.ToString(i1 + 1);
                    for (i2 = 0; i2 < cities.Count; i2++)
                    {
                        var item2 = cities.ElementAt(i2);
                        if (i2 < i1 + 1)
                            line += ";x";
                        else if (i2 < cities.Count)
                        {
                            var g1 = new GeoCoordinate(item1.Latitude, item1.Longitude);
                            var g2 = new GeoCoordinate(item2.Latitude, item2.Longitude);
                            var distance = g1.GetDistanceTo(g2);
                            line += ";" + Convert.ToInt32(distance);
                        }
                    }
                    lines.Add(line);
                }

                filePath = filePath.Replace("Cities", "CitiesOutput");
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    foreach (var line in lines)
                        sw.WriteLine(line);
                }

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
