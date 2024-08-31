using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _1._ORM.Models.Menu
{
    public class Menu
    {
        public static void menu()
        {
            Db db = new Db("Data Source=DESKTOP-NB7MT4D\\SQLEXPRESS;Initial Catalog=Library;Integrated Security = true;TrustServerCertificate=True");
        check_Choise:
            try
            {
                Console.Clear();
                Console.Write("Enter Your Id : ");
                int CRL = int.Parse(Console.ReadLine());
                db.Login(CRL);
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("False Student Id!");
                goto check_Choise;
            }
        Choise:
            Console.Write($"1. Take Book\n2. Taken Books\nEnter You Choise : ");
            try
            {
                var t = Console.ReadKey();
                if (t.Key == ConsoleKey.D1)
                {
                    db.TakeBook();
                }
                if (t.Key == ConsoleKey.D2)
                {
                    db.TakenBooks();
                }
                else
                {
                    throw new Exception("False Choise !");
                };
            }
            catch (Exception ex)
            {
                Console.Clear();
                goto Choise;
            }
        } 
    }
}
