using System;

namespace ModelsDb
{
    public abstract class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PassportNumber { get; set; }
        public Guid ID { get; set; }
        public Person(string name, string surname, DateTime date, Guid ID)
        {
            Name = name;
            Surname = surname;
            DateOfBirth = date;
            this.ID = ID;
        }
    }
    public class Employee : Person
    {
        public Employee(string name, string surname, DateTime date, int ID, string contract, string jobTitle, int salary, DateTime dateOfHire) : base(name, surname, date, ID)
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
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (!(obj is Employee)) return false;

            var other = (Employee)obj;
            return (other.Name == Name && other.Surname == Surname && other.Salary == Salary && other.DateOfBirth == DateOfBirth && other.DateOfHire == DateOfHire && other.JobTitle == JobTitle && other.Contract == Contract && other.ID == ID);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + Surname.GetHashCode();
                hash = hash * 23 + DateOfBirth.GetHashCode();
                hash = hash * 23 + Salary.GetHashCode();
                hash = hash * 23 + DateOfHire.GetHashCode();
                hash = hash * 23 + JobTitle.GetHashCode();
                hash = hash * 23 + Contract.GetHashCode();
                hash = hash * 23 + ID.GetHashCode();
                return hash;
            }
        }
    }
    public class Client : Person
    {
        public Client(string name, string surname, DateTime date, int clientID, string phoneNumber, string email) : base(name, surname, date, clientID)
        {
            ID = ID;
            PhoneNumber = phoneNumber;
            Email = email;
        }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (!(obj is Client)) return false;

            var other = (Client)obj;
            return (other.Name == Name && other.Surname == Surname && other.ID == ID && other.DateOfBirth == DateOfBirth && other.PhoneNumber == PhoneNumber && other.Email == Email);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + Surname.GetHashCode();
                hash = hash * 23 + DateOfBirth.GetHashCode();
                hash = hash * 23 + ID.GetHashCode();
                hash = hash * 23 + PhoneNumber.GetHashCode();
                hash = hash * 23 + Email.GetHashCode();
                return hash;
            }
        }
    }
    public class Currency
    {
        public string code;
        public string symbol;
        public Currency(string name, string symbol)
        {
            this.code = name;
            this.symbol = symbol;
        }
    }
    public class Account
    {
        public string AccountNumber { get; set; }
        public string CurrencyName { get; set; }
        public int Amount { get; set; }
        public Account(string accountNumber, string currency, int amount)
        {
            AccountNumber = accountNumber;
            CurrencyName = currency;
            Amount = amount;
        }
    }
}
