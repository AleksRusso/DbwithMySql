using Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MySql.Data.MySqlClient;  // 0) для работы с MySql.Data нужно сначала скачать библиотеку MySql.Data из Nuget 
                               //    если там не получается скачать, можно с основного сайта https://downloads.mysql.com/archives/installer/
namespace Database.Controllers //    затем добавить ссылку этой библиотеки в проект
{
    public class HomeController : Controller
    {
        public MySqlConnection connectionstring = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=dbexample;");   //1) строка соединения бд 
        string query; //2) строка запроса Sql
        MySqlCommand command;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {

            _logger = logger;
        }

        public IActionResult Index()
        {

            List<string> a = new List<string>();
            query = "SELECT * FROM fanho";
            command = new MySqlCommand(query, connectionstring); //3) создание объекта команды которая соединяет 1 и 2
            try //4) ВОЗМОЖНЫЕ ИСКЛЮЧЕНИЯ
            {
                connectionstring.Open(); //5) открытие бд для выполнения команды
                MySqlDataReader reader = command.ExecuteReader();  //6) выполнение команды 
                                                                   //ExecuteReader возвращает объект MySqlDataReader
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string nom = reader.GetValue(1).ToString();   // GetValue(1)-здесь возвращает второй столбец в из таблицы
                        a.Add(nom);
                    }
                }
                else
                {
                    ViewBag.M = "No rows found.";
                }
                connectionstring.Close();  //7) закрытие бд после окончания работы
            }
            catch (Exception ex)
            {
                ViewBag.M =ex.Message;
            }
            return View(a);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//ExecuteNonQuery: просто выполняет sql - выражение и возвращает количество измененных записей. Подходит для sql-выражений INSERT, UPDATE, DELETE.
//ExecuteReader: выполняет sql-выражение и возвращает строки из таблицы. Подходит для sql-выражения SELECT.
//ExecuteScalar: выполняет sql-выражение и возвращает одно скалярное значение, например, число. Подходит для sql-выражения SELECT в паре с одной из встроенных функций SQL, как например, Min, Max, Sum, Count.

//использованные сайты:
//https://downloads.mysql.com/archives/installer/
//https://ourcodeworld.com/articles/read/218/how-to-connect-to-mysql-with-c-sharp-winforms-and-xampp
//https://metanit.com/sharp/adonet/2.6.php


// Проект работает если включён xampp, а бд находится в sqlyog!!!
// Без xampp sqlyog не подключиться.

//наглядные фото находятся в ~/wwwroot/photo/