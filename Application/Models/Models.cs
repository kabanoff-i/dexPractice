using System;

namespace Models
{
    public abstract class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Person(string name, string surname, DateTime date)
        {
            Name = name;
            Surname = surname;
            DateOfBirth = date;
        }
    }
    public class Employee : Person
    {
        public Employee(string name, string surname, DateTime date, string contract, string jobTitle, int salary, DateTime dateOfHire) : base(name, surname, date)
        {
            Contract = contract;
            JobTitle = jobTitle;
            Salary = salary;
            DateOfHire = dateOfHire;
        }

        public string Contract { get; set; }
        public string JobTitle { get; set; }
        public int Salary { get; set; }
        public DateTime DateOfHire { get; set; }
    }
    public class Client : Person
    {
        public Client(string name, string surname, DateTime date, string clientID, string phoneNumber, string email) : base(name, surname, date)
        {
            ClientID = clientID;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public string ClientID { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
    public struct Currency
    {
        public string name;
        public string symbol;
        public string code;
        public string country;
        public double exchangeRateToUSD;
        public Currency(string name, string symbol, string code, string country, double exchangeRateToUSD)
        {
            this.name = name;
            this.symbol = symbol;
            this.code = code;
            this.country = country;
            this.exchangeRateToUSD = exchangeRateToUSD;
        }
    }
}
