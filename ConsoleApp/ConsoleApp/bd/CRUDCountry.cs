using ConsoleApp.bd.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ConsoleApp.bd
{
    class CRUDCountry
    {
        public int CreateCity(String countryName) 
        {
            int id = -1;
            
            using (SqlConnection connection = DBConnection.dbConnect())
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Cities WHERE name= @CountryName";
                    command.Parameters.AddWithValue("@CountryName", countryName);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Console.WriteLine("ДАННЫЕ Города ЕСТЬ!");

                        if (reader.Read())
                        {
                            id = reader.GetInt32(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("ДАННЫХ Города НЕТУ!");

                        id = InsertData("Cities", countryName);

                    }
                    reader.Close();                    
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
            }
            return id;           
        }
     
        public int CreateRegion(string regionName)
        {
            int id = -1;
                    
            using (SqlConnection connection = DBConnection.dbConnect())
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "SELECT * FROM Regions WHERE name= @RegionName";

                    command.Parameters.AddWithValue("@RegionName", regionName);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Console.WriteLine("ДАННЫЕ Региона ЕСТЬ!");

                        if (reader.Read())
                        {
                            id = reader.GetInt32(0);

                        }
                    }
                    else
                    {
                        Console.WriteLine("ДАННЫХ Региона НЕТУ!");
                        id = InsertData("Regions", regionName);
                    }
                    reader.Close();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
            }
            return id;
        }

        public void CreateCountry(CountryInf countryInf, int cityId, int regionId) 
        {
            using (SqlConnection connection = DBConnection.dbConnect())
            {
                try
                {
                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "SELECT * FROM Countries WHERE AlphaCode = @AlphaCode";

                    command.Parameters.AddWithValue("@AlphaCode", countryInf.alphaCode);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Console.WriteLine("ДАННЫЕ Страны ЕСТЬ!");
                        if (reader.Read())
                        {
                            UpdateData(countryInf, cityId, regionId);
                        }
                    }
                    else
                    {
                        Console.WriteLine("ДАННЫХ Страны НЕТУ!");

                        CreateData(countryInf, cityId, regionId);

                    }

                    reader.Close();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
            }          
        }

        private int InsertData(string table, string name)
        {
            int id = -1;

            using (SqlConnection connection = DBConnection.dbConnect())
            {
                try
                {
                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "INSERT INTO "+ table + "(name) VALUES (@Name)";
                    command.Parameters.AddWithValue("@Name", name);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
            }           
            return id;
        }

        private void CreateData(CountryInf countryInf, int cityId, int regionId) 
        {
            using (SqlConnection connection = DBConnection.dbConnect())
            {
                try
                {
                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "INSERT INTO Countries " +
                        "(Name, AlphaCode, CapitalId, Area, Population, RegionId) " +
                        "VALUES(@Name, @AlphaCode, @CapitalId, @Area, @Population, @RegionId); ";

                    command.Parameters.AddWithValue("@Name", countryInf.name);
                    command.Parameters.AddWithValue("@AlphaCode", countryInf.alphaCode);
                    command.Parameters.AddWithValue("@CapitalId", cityId);
                    command.Parameters.AddWithValue("@Area", countryInf.area);
                    command.Parameters.AddWithValue("@Population", countryInf.population);
                    command.Parameters.AddWithValue("@RegionId", regionId);
                    command.Parameters.AddWithValue("@FindByAlphaCode", countryInf.alphaCode);


                    command.ExecuteReader();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();              
            }
        }

        private void UpdateData(CountryInf countryInf, int cityId, int regionId) 
        {
            using (SqlConnection connection = DBConnection.dbConnect())
            {
                try
                {
                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "UPDATE Countries SET " +
                                        "Name = @Name," +
                                        "AlphaCode = @AlphaCode," +
                                        "CapitalId = @CapitalId," +
                                        "Area = @Area," +
                                        "Population = @Population," +
                                        "RegionId = @RegionId" +

                                   " FROM Countries" +
                                   " INNER JOIN Cities ON Cities.id = Countries.CapitalId" +
                                   " INNER JOIN Regions ON Regions.id = Countries.RegionId" +
                                   " WHERE Countries.AlphaCode = @FindByAlphaCode;";

                    command.Parameters.AddWithValue("@Name", countryInf.name);
                    command.Parameters.AddWithValue("@AlphaCode", countryInf.alphaCode);
                    command.Parameters.AddWithValue("@CapitalId", cityId);
                    command.Parameters.AddWithValue("@Area", countryInf.area);
                    command.Parameters.AddWithValue("@Population", countryInf.population);
                    command.Parameters.AddWithValue("@RegionId", regionId);
                    command.Parameters.AddWithValue("@FindByAlphaCode", countryInf.alphaCode);


                    command.ExecuteReader();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();                
            }
        }

        public List<CountryInf> Read()
        {
           
            string sqlExpression = "SELECT Countries.Name, Countries.AlphaCode, " +
                                        "Cities.name as Capital, Countries.Area, " +
                                        "Countries.Population, Regions.Name as Region " +
                                    "FROM Countries INNER JOIN Cities ON Cities.id = Countries.CapitalId " +

                                    "INNER JOIN Regions ON Regions.id = Countries.RegionId";

            using (SqlConnection connection = DBConnection.dbConnect())
            {
                List<CountryInf> countries = new List<CountryInf>();
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CountryInf country = new CountryInf();
                            country.name = (string)reader.GetValue(0);
                            country.alphaCode = (string)reader.GetValue(1);
                            country.capital = (string)reader.GetValue(2);
                            country.area = Convert.ToDouble(reader.GetValue(3));
                            country.population = (int)reader.GetValue(4);
                            country.region = (string)reader.GetValue(5);

                            countries.Add(country);
                        }

                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
                return countries;
            }
        }

        public void Update()
        {

        }

        public void Delete()
        {

        }
    }
}
