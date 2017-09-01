using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSocietyProject
{
    public class HumanSociety
    {
        DataClasses1DataContext database = new DataClasses1DataContext();

        public HumanSociety()
        {
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
                    Console.WriteLine("You did not choose 1 or 2.");
                    GetCheckAdoptee();
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

            DateTime dob = DateOfBirth();

            string userName = GetNewUserName();

            Console.WriteLine("Please enter a password.");
            string password = Console.ReadLine();

            StoreNewUser(firstName, lastName, streetAddress, city, state, zip, phone, dob, userName, password);
        }

        public void StoreNewUser(string firstName, string lastName, string street, string city, string state, string zip, string phone, DateTime dob, string userName, string password)
        {
            Adopter adopter = new Adopter();
            adopter.FirstName = firstName;
            adopter.LastName = lastName;
            adopter.StreetAddress = street;
            adopter.City = city;
            adopter.State = state;
            adopter.Zip = zip;
            adopter.Phone = phone;
            adopter.DOB = dob;
            adopter.AdopterUserName = userName;
            adopter.AdopterPassword = password;

            database.SubmitChanges();

            Console.Clear();
            GetAdopteeLogin();
        }

        public DateTime DateOfBirth()
        {
            DateTime dateTime = new DateTime();
            Console.WriteLine("Please enter your date of birth. \"yyyy/mm/dd\" ");
            string dob = Console.ReadLine();
            try
            {
                dateTime = DateTime.Parse(dob);
            }
            catch(FormatException)
            {
                Console.WriteLine("You've enter the wrong format or illegal charcters.  Please use the \"yyyy/mm/dd\" with \"/\".");
                DateOfBirth();
            }
            return dateTime;
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

            CheckAdopteeLoginCredits(userName, password);
        }

        public void CheckAdopteeLoginCredits(string userName, string password)
        {
            var adoptee = from person in database.Adopters
                    where person.AdopterUserName == userName && person.AdopterPassword == password
                    select person;
            
            if(adoptee != null)
            {
                GetUserMenu(userName);
            }
            else
            {
                Console.WriteLine("User was not found or password was incorrect.");
                Console.ReadLine();
                Console.Clear();
                GetAdopteeLogin();
            }
        }

        public void GetUserMenu(string userName)
        {
            Console.WriteLine("Please choose: \n 1: User Information \n 2: Find Animal \n 3: Logout");
            string choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    DisplayUserInformation(userName);
                    break;
                case "2":
                    break;
                case "3":
                    Console.WriteLine("Thank you for visiting the Humane Society.  Goodbye.");
                    Console.ReadLine();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("You have entered an invalid selection.");
                    Console.ReadLine();
                    GetUserMenu(userName);
                    break;
            }
        }

        public void DisplayUserInformation(string userName)
        {
            var user = from person in database.Adopters
                       where person.AdopterUserName == userName
                       select person;

            foreach(Adopter person in user)
            {
                Console.WriteLine($"First Name: {person.FirstName} \n Last Name: {person.LastName} \n Address: {person.StreetAddress} \n City: {person.City} \n State: {person.State} \n Zip: {person.Zip} \n Phone: {person.Phone} \n DOB: {person.DOB}");
            }
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
