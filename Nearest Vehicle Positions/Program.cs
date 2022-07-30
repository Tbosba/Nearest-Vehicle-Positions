using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nearest_Vehicle_Positions
{
    class Program
    {

        const string fileName = "VehiclePositions.dat";

        //const string filePath = @$"C:\{fileName}";
        const string filePath = @"C:\VehiclePositions.dat";

        static List<VehiclePosition> vehiclePositions = new List<VehiclePosition>();

        static void Main(string[] args)

        {
            ReadAndProcessVehicleFromFile();
           /* PrintNearestVehiclePositions();
            Console.ReadLine();*/
        }

        static void ReadAndProcessVehicleFromFile(long whereToStartReading = 0)
        {

            Console.WriteLine("Start:: Reading vehicle list from file");

            const int megabyte = 1024 * 1024;

            try
            {
                if (System.IO.File.Exists(filePath))
                {

                    FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    using (fileStream)
                    {

                        byte[] buffer = new byte[megabyte];

                        fileStream.Seek(whereToStartReading, SeekOrigin.Begin);

                        int bytesRead = fileStream.Read(buffer, 0, megabyte);

                        while (bytesRead > 0)
                        {
                            try
                            {
                                ProcessFileChunk(buffer, bytesRead);
                            }

                            catch (Exception e)
                            {
                                postException($"{e.ToString()}");
                            }

                            bytesRead = fileStream.Read(buffer, 0, megabyte);
                        }

                    }

                    Console.WriteLine("End:: Reading vehicle list from file");

                    PrintNearestVehiclePositions();
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine($"Please copy and past {fileName} file to your C drive");
                }

            }
            catch(Exception e)
            {
                postException($"{e.ToString()}");
                Console.WriteLine($" {e.ToString()}");
            }

        }

        //We should be storing the expection logs to AWS CloudWatch for bugs fixing
        static void postException(string exception)
        {

        }

        static void ProcessFileChunk(byte[] buffer, int bytesRead)
        {
            Stream stream = new MemoryStream(buffer);

            using (var reader = new BinaryReader(stream))
            {

                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    VehiclePosition vehiclePosition = new VehiclePosition() { Position = new Position() };

                    vehiclePosition.PositionId = reader.ReadInt32();
                    vehiclePosition.VehicleRegistraton = UnsafeAsciiBytesToString(reader.ReadString());
                    vehiclePosition.Position.Latitude = reader.ReadSingle();
                    vehiclePosition.Position.Longitude = reader.ReadSingle();
                    vehiclePosition.RecordedTimeUTC = reader.ReadUInt64();
                    vehiclePositions.Add(vehiclePosition);
                    //Console.WriteLine(vehiclePosition.VehicleRegistraton);
                }
            }
        }


        static string UnsafeAsciiBytesToString(string S)

        {

            S += '\0'; // Add null terminator for C strings.

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(S); // Since we convert to bytes the '\0' is crucial, otherwise it will be lost

            return System.Text.Encoding.ASCII.GetString(bytes);

        }


        static void PrintNearestVehiclePositions()
        {
            foreach (var location in GetPositions())
            {

                var vehiclePosition = GetNearestVehicle(location);
                Console.WriteLine($"Closest vehicle to Position {location.Name} is vehicle {GetCoordinates(vehiclePosition?.Position)} (Reg: {vehiclePosition.VehicleRegistraton})");
            }
        }


        static List<Position> GetPositions()
        {
            List<Position> positions = new List<Position>();

            positions.Add(new Position(34.544909f, -102.100843f, "Position 1"));

            positions.Add(new Position(32.345544f, -99.123124f, "Position 2"));

            positions.Add(new Position(33.234235f, -100.214124f, "Position 3"));

            positions.Add(new Position(35.195739f, -95.348899f, "Position 4"));

            positions.Add(new Position(31.895839f, -97.789573f, "Position 5"));

            positions.Add(new Position(32.895839f, -101.789573f, "Position 6"));

            positions.Add(new Position(34.115839f, -100.225732f, "Position 7"));

            positions.Add(new Position(32.335839f, -99.992232f, "Position 8"));

            positions.Add(new Position(33.535339f, -94.792232f, "Position 9"));

            positions.Add(new Position(32.234235f, -100.222222f, "Position 10"));

            return positions;
        }


        static VehiclePosition GetNearestVehicle(Position position)
        {

            return vehiclePositions

                          .GroupBy(x => Math.Pow((position.Latitude - (double)x.Position?.Latitude), 2) + Math.Pow(position.Longitude - (double)x.Position?.Longitude, 2))

                          .OrderBy(x => x.Key)

                          .First()?.FirstOrDefault();

        }


        static string GetCoordinates(Position position)
        {
            return $"{{ {position?.Latitude},{position?.Longitude} }}";
        }
    }
}
