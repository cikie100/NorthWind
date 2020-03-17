using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Northwind
{
    internal class Program
    {
        //1.Lijst van producten me categorienaam, gesorteerd per categorienaam en daarna per productnaam
        static private string q1 = "select p.*, c.CategoryName FROM Products p join Categories c ON p.CategoryID = c.CategoryID ORDER BY c.CategoryName ASC, p.ProductName ASC; ";

        //2.De tien meest kostelijke producten in aflopende ordevan kostelijkheid
        static private string q2 = "SELECT ProductName, UnitPrice FROM Products WHERE ( UnitPrice IN (  SELECT TOP(10) UnitPrice  FROM Products as table1  GROUP BY UnitPrice  ORDER BY UnitPrice DESC )) ORDER BY UnitPrice DESC;";

        //3.Klanten en leveranciers per stad, gesorteerd per stadsnaam
        static private string q3 = "SELECT City, CompanyName, ContactName, 'Customer' FROM Customers UNION ALL SELECT City, CompanyName, ContactName, 'Klant' FROM Suppliers ORDER BY CITY ASC";

        //4.Subtotaal voor elk order, oplopend per orderId
        static private string q4 = "SELECT distinct o.OrderID, cast(round(SUM(od.UnitPrice*Quantity),0) as int)AS 'subtotaal' FROM ORDERS o JOIN[Order Details] od ON o.OrderID = od.OrderID GROUP BY o.OrderID ";

        //5.Subtotaal voor elk order tussen 24/12/1996 en 30/9/1997
        //met shipped date, orderId, subtotaalen jaar, oplopend volgens shipped date
        static private string q5 = "SELECT  ShippedDate, o.OrderID, SUM(od.UnitPrice*Quantity)AS TotalPrice,  YEAR(ShippedDate) as 'Year' FROM Orders o JOIN[Order Details] od ON o.OrderID = od.OrderID WHERE ShippedDate IS NOT NULL AND  ShippedDate BETWEEN '1996-12-24' AND'1997-09-30' GROUP BY  o.OrderID, ShippedDate ORDER BY ShippedDate";

       static  private string[] theArray = { q1, q2, q3, q4, q5 };

       
        static void Main(string[] args)
        {
            var con = new SqlConnection("Data Source=./SQLEXPRESS; Initial Catalog = Northwind; Integrated Security = True;Connect Timeout=30");
           
            using (con)
                foreach (string quer in theArray)
            {
                try
                {
                    SqlCommand comm = new SqlCommand(quer, con);
                        con.Open();

                    SqlDataReader reader = comm.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                comm.ExecuteNonQuery();
                                DataTable x = new DataTable();
                               

                            }
                        }
                        else
                        {
                            Console.WriteLine("Geen (records) gevonden.");
                        }
                    reader.Close();
                }

                catch (Exception)
                {
                    throw;
                }
                finally
                {
                        con.Close(); 
                        con?.Dispose(); 
                        con = null;
                    }
            }
        }
}
}