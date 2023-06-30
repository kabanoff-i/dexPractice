using System;

namespace ModelsDb
{
    public abstract class Person
    {
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string? passportNumber { get; set; }
        public Guid id { get; set; }
        public Person(string name, string surname, DateTime date, Guid ID)
        {
            this.name = name;
            this.surname = surname;
            dateOfBirth = date;
            this.id = ID;
        }
    }
    public class Employee : Person
    {
        public Employee(string name, string surname, DateTime date, Guid ID, string contract, string jobTitle, int salary, DateTime dateOfHire) : base(name, surname, date, ID)
        {
            this.contract = contract;
            this.jobTitle = jobTitle;
            this.salary = salary;
            this.dateOfHire = dateOfHire;
        }
        public string contract { get; set; }
        public string jobTitle { get; set; }
        public int salary { get; set; }
        public DateTime dateOfHire { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (!(obj is Employee)) return false;

            var other = (Employee)obj;
            return (other.name == name && other.surname == surname && other.salary == salary && other.dateOfBirth == dateOfBirth && other.dateOfHire == dateOfHire && other.jobTitle == jobTitle && other.contract == contract && other.id == id);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + name.GetHashCode();
                hash = hash * 23 + surname.GetHashCode();
                hash = hash * 23 + dateOfBirth.GetHashCode();
                hash = hash * 23 + salary.GetHashCode();
                hash = hash * 23 + dateOfHire.GetHashCode();
                hash = hash * 23 + jobTitle.GetHashCode();
                hash = hash * 23 + contract.GetHashCode();
                hash = hash * 23 + id.GetHashCode();
                return hash;
            }
        }
    }
    public class Client : Person
    {
        public Client(string name, string surname, DateTime date, Guid clientID, string phoneNumber, string email) : base(name, surname, date, clientID)
        {
            id = clientID;
            this.phoneNumber = phoneNumber;
            this.email = email;
        }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (!(obj is Client)) return false;

            var other = (Client)obj;
            return (other.name == name && other.surname == surname && other.id == id && other.dateOfBirth == dateOfBirth && other.phoneNumber == phoneNumber && other.email == email);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + name.GetHashCode();
                hash = hash * 23 + surname.GetHashCode();
                hash = hash * 23 + dateOfBirth.GetHashCode();
                hash = hash * 23 + id.GetHashCode();
                hash = hash * 23 + phoneNumber.GetHashCode();
                hash = hash * 23 + email.GetHashCode();
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
        public Guid accountNumber { get; set; }
        public string currencyName { get; set; }
        public Guid clientId { get; set; }
        public int amount { get; set; }
        public Account(Guid accountNumber, string currency, Guid guid, int amount)
        {
            this.accountNumber = accountNumber;
            clientId = guid;
            this.currencyName = currency;
            this.amount = amount;
        }
    }
}
