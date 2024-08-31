using Dapper;
using Microsoft.Data.SqlClient;
using _1._ORM.Models.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._ORM.Models
{
    public class Db : IDisposable
    {
        public SqlConnection Connection { get; set; }

        public void Dispose()
        {
            Connection.Dispose();
        }

        public void ReturnAllBooks()
        {
            Console.WriteLine();
            var query = "Select * From Books Where Quantity > 0";
            var Result = Connection.Query<Book>(query);
            foreach (var item in Result)
            {
                Console.WriteLine(item);
            }
        }

        public Db(string c) {
            Connection = new SqlConnection(c);
        }
        int idd;
        public Students Login(int StudentId)
        {
            var query = "Select * From Students Where Id = @StudentId";
            var result = Connection.QuerySingle<Students>(query, param: new { StudentId });
            Console.WriteLine($"Welcome {result.FirstName}");
            idd = StudentId;
            return result;
        }

        public void TakeBook()
        {
            var quary1 = "DISABLE TRIGGER All ON S_Cards";
            Connection.Execute(quary1);
            var insertQuary1 = "Insert Into S_Cards(Id,Id_Student,Id_Book,DateOut,Id_Lib) Values(@Id,@Id_Student,@Id_Book,@DateOut,@Id_Lib)";
            var Quary1 = "Select Max(Id) From S_Cards";
            var y = Connection.ExecuteScalar<int>(Quary1);
            ReturnAllBooks();
        TakeBook:
                Console.Write("Which Book Take : ");
                var query1 = "Select Books.Id From Books Where Quantity > 0";
                var result1 = Connection.Query(query1).ToList();
            try
            {
                int ReadChoise = int.Parse(Console.ReadLine());
                foreach (var item in result1)
                {
                    if (ReadChoise == item.Id)
                    {
                        var t = item.Id;
                        Connection.Execute(insertQuary1, param: new { Id = y + 1, Id_Student = idd, Id_Book = item.Id, DateOut = DateTime.Now, Id_Lib = 1 });
                        var commant = "Update Books Set Quantity = Quantity - 1 Where Id = @Id";
                        var Result = Connection.Execute(commant, param: new { Id = t });
                        Console.WriteLine("Taked Book");
                        Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception ex) {
                Console.Clear();
                ReturnAllBooks();
                Console.WriteLine(ex.Message);
                goto TakeBook;
            }
        }
        int Check = -1;
        public void TakenBooks()
        {
            Console.WriteLine();
            var query = "Select S_Cards.Id,Books.Name From Books Join S_Cards On Books.Id = S_Cards.Id_Book Join Students On S_Cards.Id_Student = Students.Id Where Students.Id = @Id"; // bu butun goturduyu Kitablardir Yeni dateinleri olan ve olmayan kitablar
            var result = Connection.Query(query,param: new { Id = idd});
            Console.Write("You Taked This Books(DateIn IS Full And DateIn Is Null(All)) : \n");
            foreach (var item in result) {
                Console.WriteLine($"{item.Id}. {item.Name}");
            }
            Console.WriteLine("\n1. Return Book\n2. LogOut\nMakeChoise:");
            int choise = int.Parse(Console.ReadLine());
            if (choise == 1)
            {
                Console.Clear();
                var query1 = "Select S_Cards.Id,Books.Name From Books Join S_Cards On Books.Id = S_Cards.Id_Book Join Students On S_Cards.Id_Student = Students.Id Where S_Cards.DateIn is Null and Students.Id = @Id";//bu ise secime bagli olaraq dateinleri olmayan yeni qayatarilmayan kitablardir. burada dateinleri null olanlar lazimdir!
                var res = Connection.Query(query1, param: new { Id = idd }).ToList();
                foreach (var item1 in res)
                {
                    Console.WriteLine($"{item1.Id} {item1.Name}");
                    Check = item1.Id;
                }
                if (Check != -1)
                {
                    Console.Write("Enter Id For Return : ");
                }

                else if (Check == -1)
                {
                    Console.Write("There are no unreturned books or You didn't Take a book or Datein Fulll!"); // yeniki kitab goturmusduse bele onu evvel qayatarib datein is not null
                }

                if (int.TryParse(Console.ReadLine(), out int bookId))
                {
                    var bookToReturn = res.FirstOrDefault(b => b.Id == bookId);
                    if (bookToReturn != null)
                    {
                        var cmd = "UPDATE S_Cards SET DateIn = @DateIn WHERE Id = @Id";
                        var resultUpdate = Connection.Execute(cmd, param: new { DateIn = DateTime.Now, Id = bookId });
                        Console.WriteLine(resultUpdate > 0 ? "Returned Book. SuccessFul!." : "Have Problem . The book did not returned");
                        Console.WriteLine("Click Enter For Continue!");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("False Id!");
                    }
                }
            }
            if (choise == 2) {
                return;
            }
        }
    }
}
