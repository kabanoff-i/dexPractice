using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsDb
{
    public abstract class Person
    {
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string? passportNumber { get; set; }
        [Key]
        public Guid id { get; set; }
        public Person(string name, string surname, DateTime dateOfBirth, Guid id)
        {
            this.name = name;
            this.surname = surname;
            this.dateOfBirth = dateOfBirth;
            this.id = id;
        }
        public Person() { }
    }
    public class Employee : Person
    {
        public Employee(string name, string surname, DateTime dateOfBirth, Guid id, string contract, string jobTitle, int salary, DateTime dateOfHire) : base(name, surname, dateOfBirth, id)
        {
            this.contract = contract;
            this.jobTitle = jobTitle;
            this.salary = salary;
            this.dateOfHire = dateOfHire;
        }
        public Employee() { }
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
        public Client(string name, string surname, DateTime dateOfBirth, Guid id, string phoneNumber, string email) : base(name, surname, dateOfBirth, id)
        {
            this.id = id;
            this.phoneNumber = phoneNumber;
            this.email = email;
        }
        public Client() { }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public virtual ICollection<Account> accounts { get; set; }
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
        [Key]
        public string code { get; set; }
        public char symbol { get; set; }
        [InverseProperty("currency")]
        public virtual ICollection<Account> accounts { get; set; }
        public Currency(string code, char symbol)
        {
            this.code = code;
            this.symbol = symbol;
        }
        public Currency() { }
    }
    public class Account
    {
        [Key]
        public Guid accountNumber { get; set; }
        [Display(Name = "currency")]
        public string currencyName { get; set; }
        [Display(Name = "client")]
        public Guid clientId { get; set; }
        public int amount { get; set; }
        [ForeignKey("clientId")]
        [InverseProperty("accounts")]
        public virtual Client client { get; set; }
        [ForeignKey("currencyName")]
        [InverseProperty("accounts")]
        public virtual Currency currency { get; set; }
        public Account(Guid accountNumber, string currencyName, Guid clientId, int amount, Client client, Currency currency)
        {
            this.accountNumber = accountNumber;
            this.clientId = clientId;
            this.currencyName = currencyName;
            this.amount = amount;
            this.client = client;
            this.currency = currency;
        }
        public Account() { }
    }
}
