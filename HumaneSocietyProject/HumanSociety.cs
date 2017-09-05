﻿using System;
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

            database.Adopters.InsertOnSubmit(adopter);
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
            
            if(user == null)
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
                    FindAnimal(userName);
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

            Console.WriteLine("Please press enter to continue.");
            Console.ReadLine();
            Console.Clear();
            GetUserMenu(userName);
        }

        public void FindAnimal(string userName)
        {
            //TODO:
        }

        public void GetEmployee()
        {
            Console.WriteLine("Please enter your user name.");
            string employee = Console.ReadLine();
            Console.WriteLine("Please enter your password.");
            string employeePassword = Console.ReadLine();

            CheckEmployeeCredientals(employee, employeePassword);
        }

        public void CheckEmployeeCredientals(string employee, string employeePassword)
        {
            var employeeUser = from person in database.Employees
                          where person.EmployeeUserName == employee && person.EmployeePassword == employeePassword
                          select person;

            if (employeeUser != null)
            {
                GetEmployeeMenu(employee);
            }
            else
            {
                Console.WriteLine("User was not found or password was incorrect.");
                Console.ReadLine();
                Console.Clear();
                GetAdopteeLogin();
            }
        }

        public void GetEmployeeMenu(string employee)
        {
            Console.WriteLine("Welcome to work!  What would you like to do? \n 1: Employee info. \n 2: Check on an animal \n 3: Log off");
            string choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    Console.Clear();
                    EmployeeInformation(employee);
                    break;
                case "2":
                    Console.Clear();
                    CheckOnAnimals(employee);
                    break;
                case "3":
                    Console.WriteLine("Have a good day.  Goodbye.");
                    Console.ReadLine();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("You have entered an invalid choice.");
                    Console.Clear();
                    GetEmployeeMenu(employee);
                    break;
            }
        }

        public void EmployeeInformation(string employee)
        {
            var user = from person in database.Employees
                       where person.EmployeeUserName == employee
                       select person;

            foreach(Employee person in user)
            {
                Console.WriteLine($"First Name: {person.EmployeeFirstName} \n Last Name: {person.EmployeeLastName}");
            }

            Console.WriteLine("Please press enter to return to the menu screen.");
            Console.ReadLine();
            Console.Clear();
            GetEmployeeMenu(employee);
        }

        public void CheckOnAnimals(string employee)
        {
            Console.WriteLine("Please select what to check. \n 1: Add An Animal \n 2: Shots \n 3: Food \n 4: Collect Money \n 5: Verify Adoption \n 6: Check Rooms \n 7: User Menu \n 8: Log off");
            string choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    Console.Clear();
                    AddAnimal(employee);
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                case "6":
                    break;
                case "7":
                    Console.Clear();
                    GetEmployeeMenu(employee);
                    break;
                case "8":
                    Console.WriteLine("Thank you for your hard work.  Goodbye.");
                    Console.ReadLine();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("You have enter an invalid choice.");
                    Console.Clear();
                    CheckOnAnimals(employee);
                    break;
            }
        }

        public void AddAnimal(string employee)
        {
            Console.WriteLine("What is the animal's name?");
            string animalName = Console.ReadLine();

            bool vaccinated = CheckVaccinated();

            int age = CheckAge();

            int price = CheckPrice();

            bool adopted = CheckAdoptedStatus();

            int adopter = CheckAdopter();

            int animalType = GetAnimalType();

            int room = GetRoom();

            int color = GetAnimalColor();
        }

        public bool CheckVaccinated()
        {
            bool vaccinated = false;

            Console.WriteLine("Is the animal vaccinated, yes or no?");
            string answer = Console.ReadLine();

            switch(answer)
            {
                case "yes":
                    vaccinated = true;
                    break;
                case "no":
                    vaccinated = false;
                    break;
                default:
                    Console.WriteLine("You need to input yes or no.");
                    CheckVaccinated();
                    break;
            }

            return vaccinated;
        }

        public int CheckAge()
        {
            int age = 0;
            try
            {
                Console.WriteLine("What is the age of the animal, please enter 0.");
                age = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("Your entry was invalid.");
                Console.ReadLine();
                CheckAge();
            }

            return age;
        }

        public int CheckPrice()
        {
            int price = 0;

            try
            {
                Console.WriteLine("What is the price of the animal (no cents, decimals, or \"$\" needed).  If price is undecided enter 0.");
                price = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("You entry was invalid.");
                CheckPrice();
            }

            return price;
        }

        public bool CheckAdoptedStatus()
        {
            bool status = false;

            Console.WriteLine("Is this animal already adopted, yes or no?");
            string answer = Console.ReadLine();

            switch(answer)
            {
                case "yes":
                    status = true;
                    break;
                case "no":
                    status = false;
                    break;
                default:
                    Console.WriteLine("You did not enter yes or no.");
                    CheckAdoptedStatus();
                    break;
            }

            return status;
        }

        public int CheckAdopter()
        {
            int adopter = 0;

            Console.WriteLine("Is this animal adopted, yes or no.");
            string answer = Console.ReadLine();

            switch(answer)
            {
                case "yes":
                    adopter = GetAdopterID();
                    break;
                case "no":
                    adopter = 0;
                    break;
                default:
                    Console.WriteLine("You did not enter yes or no.");
                    CheckAdopter();
                    break;
            }

            return adopter;
        }

        public int GetAdopterID()
        {
            int id = 0;
            Console.WriteLine("From the list below, please enter the ID of the adopter.");

            var adopter = from person in database.Adopters
                          select person;

            foreach(Adopter person in adopter)
            {
                Console.WriteLine($"First Name: {person.FirstName} \t Last Name: {person.LastName} \t ID: {person.AdopterID}");
            }

            try
            {
                id = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("You did not enter a valid ID number.");
            }

            id = VerifyID(id);

            return id;
        }

        public int VerifyID(int id)
        {
            int check = id;
            var adopter = from person in database.Adopters
                          where person.AdopterID == check
                          select person;

            if(adopter == null)
            {
                Console.WriteLine("There is no adopter with this id number.");
                GetAdopterID();
            }
            else
            {
                check = id;
            }

            return check;
        }

        public int GetAnimalType()
        {
            int animalType = 0;

            Console.WriteLine("Please choose the ID associated with that represents this animal's type.");

            var types = from animal in database.AnimalTypes
                          select animal;

            foreach(AnimalType animal in types)
            {
                Console.WriteLine($"Type: {animal.TypeOfAnimal} \t ID: {animal.AnimalTypeID}");
            }

            try
            {
                animalType = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("Your input was invalid.");
                Console.ReadLine();
                Console.Clear();
                GetAdopteeLogin();
            }

            animalType = VerifyAnimalID(animalType);

            return animalType;
        }

        public int VerifyAnimalID(int id)
        {
            int animalId = id;

            var animalType = from animal in database.AnimalTypes
                             where animal.AnimalTypeID == animalId
                             select animal;

            if(animalType == null)
            {
                Console.WriteLine("The ID you entered was not found.");
                Console.ReadLine();
                Console.Clear();
                GetAnimalType();
            }
            else
            {
                animalId = id;
            }

            return animalId;
        }

        public int GetRoom()
        {
            int newRoom = 0;
            Console.WriteLine("All rooms below are taken, please enter a new room number for this animal.");

            var roomNumber = from room in database.Rooms
                             select room;

            foreach(Room number in roomNumber)
            {
                Console.WriteLine($"Room: {number.RoomNumber}");
            }

            try
            {
                newRoom = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("You entry was invalid.");
                Console.ReadLine();
                Console.Clear();
                GetRoom();
            }

            newRoom = VerifyRoom(newRoom);

            return newRoom;
        }

        public int VerifyRoom(int room)
        {
            int roomNumber = room;

            var number = from rooms in database.Rooms
                         where rooms.RoomNumber == roomNumber
                         select rooms;

            if(number != null)
            {
                Console.WriteLine("That room is already taken.");
                Console.ReadLine();
                Console.Clear();
                GetRoom();
            }
            else
            {
                roomNumber = room;
            }

            return roomNumber;
        }

        public int GetAnimalColor()
        {
            int color = 0;

            Console.WriteLine("Here is a list of colors.  Please choose an ID of the color.");

            var colorTypes = from colors in database.Colors
                        select colors;

            foreach(Color type in colorTypes)
            {
                Console.WriteLine($"Color: {type.Color1} \t ID: {type.ColorID}");
            }

            try
            {
                color = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {

                Console.WriteLine("You have an invalid input.");
                Console.ReadLine();
                Console.Clear();
                GetAnimalColor();
            }

            color = VerifyColorID(color);

            return color;
        }

        public int VerifyColorID(int id)
        {
            int colorID = id;

            var idNumbers = from color in database.Colors
                            where color.ColorID == id
                            select color;

            if(idNumbers == null)
            {
                Console.WriteLine("That color does not exist in the database.");
                Console.ReadLine();
                Console.Clear();
                GetAnimalColor();
            }
            else
            {
                colorID = id;
            }

            return colorID;
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
