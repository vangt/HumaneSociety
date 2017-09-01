using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSocietyProject
{
    public class HumanSociety
    {
        DataClasses1DataContext database;

        public HumanSociety()
        {
            database = new DataClasses1DataContext();
        }

        public void UserType()
        {
            Console.WriteLine("Are you an adoptee or employee? \n 1: Adoptee \n 2: Employee");
            string choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    Console.Clear();
                    GetCheckAdoptee();
                    break;
                case "2":
                    Console.Clear();
                    GetEmployee();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("You've inputed an incorrect option.");
                    UserType();
                    break;
            }
        }

        public void GetCheckAdoptee()
        {
            Console.WriteLine("Are you a new user? \n 1: Yes, I'm new and have don't have an account. \n 2: No, I'm an existing user and have an account.");
            string choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    Console.Clear();
                    Register();
                    break;
                case "2":
                    Console.Clear();
                    GetAdopteeLogin();
                    break;
                default:
                    Console.WriteLine("You choose ");
                    break;
            }
        }

        public void Register()
        {
            Console.WriteLine("Please enter your first name.");
            string firstName = Console.ReadLine();

            Console.WriteLine("Please enter your last name.");
            string lastName = Console.ReadLine();

            Console.WriteLine("Please enter just your street adress.");
            string streetAddress = Console.ReadLine();

            Console.WriteLine("Please enter your city name.");
            string city = Console.ReadLine();

            Console.WriteLine("Please enter your state initials.");
            string state = Console.ReadLine();

            Console.WriteLine("Please enter your 5-digit zip code.");
            string zip = Console.ReadLine();

            Console.WriteLine("Please enter your phone.");
            string phone = Console.ReadLine();

            Console.WriteLine("Please enter your date of birth.");
            string dob = Console.ReadLine();

            string userName = GetNewUserName();

            Console.WriteLine("Please enter a password.");
            string password = Console.ReadLine();

            StoreNewUser(firstName, lastName, streetAddress, city, state, zip, phone, dob, userName, password);
        }

        public void StoreNewUser(string firstName, string lastName, string street, string city, string state, string zip, string phone, string dob, string userName, string password)
        {

        }

        public string GetNewUserName()
        {
            Console.WriteLine("Please enter a Username.");
            string userName = Console.ReadLine();
            userName = UserNameCheck(userName);
            return userName;
        }

        public string UserNameCheck(string userName)
        {
            var user = database.Adopters.Select(x => x.AdopterUserName == userName).ToString();
            
            if(user != null)
            {
                Console.WriteLine("There is someone with this name;");
                GetNewUserName();
            }

            return userName;
        }

        public void GetAdopteeLogin()
        {
            Console.WriteLine("Please enter your user name.");
            string userName = Console.ReadLine();

            Console.WriteLine("Please enter your password.");
            string password = Console.ReadLine();
        }

        public void GetEmployee()
        {

        }

        public void ListOfAnimals()
        {
            var a = from b in database.Animals
                    select b;

            foreach(Animal c in a)
            {
                Console.WriteLine(c.Name);
            }
        }
    }
}
