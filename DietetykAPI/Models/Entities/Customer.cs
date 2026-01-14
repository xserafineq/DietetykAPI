using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DietetykAPI.Models.Entities
{
    [Table("customers")]
    public class Customer
    {
        public Customer(string pesel, string firstName, string lastName, int age, string email, string residentialAddress, double weightDestination, string phone)
        {
            this.pesel = pesel;
            this.firstName = firstName;
            this.lastName = lastName;
            this.age = age;
            this.email = email;
            this.residentialAddress = residentialAddress;
            this.weightDestination = weightDestination;
            this.phone = phone;
        }

        [Column("pesel")]
        [Key]
        public string pesel {  get; set; }

        [Column("firstname")]
        public string firstName {  get; set; }
        [Column("lastname")]
        public string lastName { get; set; }
        [Column("age")]
        public int age { get; set; }
        [Column("email")]
        public string email { get; set; }
        [Column("residential_address")]
        public string residentialAddress { get; set; }
        [Column("weight_destination")]
        public double weightDestination { get; set; }

        [Column("phone")]
        public string phone { get; set; }

        public ICollection<Visit> Visits { get; set; } = new List<Visit>();

    }
}
