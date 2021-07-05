using ConsoleApp.api;
using ConsoleApp.bd;
using ConsoleApp.bd.model;
using ConsoleTables;
using System;
using System.Threading.Tasks;

namespace ConsoleApp.console
{
    class ConsoleClient
    {
        private ConsoleTable table;
        private CRUDCountry crud;
        private API api;

        public ConsoleClient()
        {
            crud = new CRUDCountry();
            api = new API();
            table = new ConsoleTable("Название", "Код страны", 
                "Столица", "Площадь", "Население", "Регион");
        }

        public async Task ConsoleInputAsync()
        {
            var f = true;
            while (f)
            {
                Console.WriteLine("Выберете действие: ");
                Console.WriteLine("1 - Ввод названия страны");
                Console.WriteLine("2 - Вывод всех стран с БД");
                Console.WriteLine("3 - Выход");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("Введите название страны на английском языке");
                        var countryName = Console.ReadLine();

                        if (!String.IsNullOrEmpty(countryName) &&
                            countryName.Length > 1 &&
                            Foo(countryName))
                        {
                            try
                            {
                                var country = await api.GET(countryName);
                                if (country == null)
                                {
                                    Console.WriteLine("Ошибка! Страна не найдена!");
                                    break;
                                }

                                Console.WriteLine("Сохранить информацию в базу?");
                                Console.WriteLine("y - Да");
                                Console.WriteLine("n - Нет");

                                switch (Console.ReadLine())
                                {
                                    case "y":                                       
                                        int cityId = crud.CreateCity(country.capital);
                                        int regionId = crud.CreateRegion(country.region);
                                        if (regionId == -1 && cityId == -1)
                                        {
                                            Console.WriteLine("error!");
                                            break;
                                        }
                                        crud.CreateCountry(country, cityId, regionId);
                                        break;
                                    case "n":
                                        Console.WriteLine("Отмена!");
                                        break;
                                    default:
                                        Console.WriteLine("Неверный ввод!");
                                        break;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                        }
                        else
                        {
                            Console.WriteLine("Страна не найдена!");
                        }

                        break;
                    case "2":
                        if (crud.Read() == null)
                        {
                            Console.WriteLine("Error");
                            break;
                        }

                        foreach (CountryInf c in crud.Read())
                        {
                            table.AddRow(c.name, c.alphaCode, c.capital,
                            c.area, c.population, c.region);
                        }

                        table.Write();
                        table.Rows.Clear();
                        break;
                    case "3":
                        Console.WriteLine("Выход!");
                        f = false;
                        break;
                    default:
                        Console.WriteLine("Неверный ввод!");
                        break;
                }
            }
        }

        private static bool Foo(string text)
        {
            for (int i = 0; i < text.Length - 1; i++)
            {
                if (!(text[i] >= 65 && text[i] <= 90 || text[i] >= 97 && text[i] <= 122 || text[i] == 32))
                    return false;
            }
            return true;
        }
    }
}
