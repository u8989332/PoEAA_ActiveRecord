using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace PoEAA_ActiveRecord
{
    class Program
    {
        private const string FindAllPersonsStatementString = @"
            SELECT id, lastname, firstname, numberOfDependents
            FROM person
            ";

        static void Main(string[] args)
        {
            InitializeData();

            Console.WriteLine("Get persons");
            var people = FindPersons();
            PrintPerson(people);

            Console.WriteLine("Insert a new person");
            new Person(0, "Rose", "Jackson", 60).Insert();
            people = FindPersons();
            PrintPerson(people);

            Console.WriteLine("Update a person's first name");
            var firstPerson = Person.Find(1);
            firstPerson.FirstName = "Jack";
            firstPerson.Update();

            Console.WriteLine("Update a person's number of dependents");
            var secondPerson = Person.Find(2);
            secondPerson.NumberOfDependents = 0;
            secondPerson.Update();

            Console.WriteLine("Get persons again");
            people = FindPersons();
            PrintPerson(people);
        }

        private static List<Person> FindPersons()
        {
            List<Person> result = new List<Person>();
            try
            {
                using var conn = DbManager.CreateConnection();
                conn.Open();
                using IDbCommand comm = new SQLiteCommand(FindAllPersonsStatementString, conn);
                using IDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(Person.Load(reader));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private static void PrintPerson(IEnumerable<Person> people)
        {
            foreach (var person in people)
            {
                Console.WriteLine($"ID: {person.Id}, " +
                                  $"last name: {person.LastName}, " +
                                  $"first name: {person.FirstName}, " +
                                  $"number of dependents: {person.NumberOfDependents}, " +
                                  $"exemption: {person.GetExemption().Amount}");
            }
        }

        private static void InitializeData()
        {
            using (var connection = DbManager.CreateConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"
                        DROP TABLE IF EXISTS person;
                    ";
                    command.ExecuteNonQuery();


                    command.CommandText =
                        @"
                        CREATE TABLE person (Id int primary key, lastname TEXT, firstname TEXT, numberOfDependents int);
                    ";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        @"
                       
                    INSERT INTO person
                        VALUES (1, 'Sean', 'Reid', 5);

                    INSERT INTO person
                        VALUES (2, 'Madeleine', 'Lyman', 13);

                    INSERT INTO person
                        VALUES (3, 'Oliver', 'Wright', 66);
                    ";
                    command.ExecuteNonQuery();
                }

            }
        }
    }
}
