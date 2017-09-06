using System;
using System.Collections.Generic;
using System.IO;
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
            
            if(user.Equals(null))
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
            var adoptee = database.Adopters.Where(x => x.AdopterUserName == userName && x.AdopterPassword == password).ToList();
            
            if(adoptee.Count == 0)
            {
                Console.WriteLine("User was not found or password was incorrect.");
                Console.ReadLine();
                Console.Clear();
                GetAdopteeLogin();
            }
            else
            {
                GetUserMenu(userName);
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
            int type = 0;
            Console.WriteLine("What type of animal are you looking for? Please enter the ID.");
            ListAnimalTypes();

            try
            {
                type = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("You have inputed an invalid selection.");
                FindAnimal(userName);
            }

            type = VerifyTypeID(userName, type);
            GetColor(userName, type);
        }

        public void GetColor(string userName, int type)
        {
            int colorID = 0;
            Console.WriteLine("What color are you looking for? Please enter the ID.");
            ListColor();

            try
            {
                colorID = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("You have inputed an invalid selection.");
                GetColor(userName, type);
            }

            GetUserPrice(userName, type, colorID);
        }

        public void GetUserPrice(string userName, int type, int colorID)
        {
            int price = 0;
            Console.WriteLine("What is the price limit?  Please enter a number. (Do not include \"$\").");
            
            try
            {
                price = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("You have an invalid entry.");
                Console.ReadLine();
                GetUserPrice(userName, type, colorID);
            }

            LookForAnimal(userName, type, colorID, price);
        }

        public void LookForAnimal(string userName, int type, int colorID, int price)
        {
            var animals = from animal in database.Animals
                          where animal.AnimalTypeID.Value == type && animal.ColorID.Value == colorID && animal.Price.Value < price
                          select animal;

            Console.WriteLine("Here is a list of animals you might like. If you have any questions about purchasing an animal please contact an employee.");

            foreach(Animal animal in animals)
            {
                Console.WriteLine($"Name: {animal.Name} \t Price: {animal.Price}");
            }

            Console.WriteLine("Please press enter to return the the user menu.");
            Console.ReadLine();
            Console.Clear();
            GetUserMenu(userName);
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
            var employeeUser = database.Employees.Where(x => x.EmployeeUserName == employee && x.EmployeePassword == employeePassword).ToList();

            if(employeeUser.Count == 0)
            {
                Console.WriteLine("User was not found or password was incorrect.");
                Console.ReadLine();
                Console.Clear();
                GetEmployee();
            }
            else
            {
                GetEmployeeMenu(employee);
            }
        }

        public void GetEmployeeMenu(string employee)
        {
            Console.WriteLine("Welcome to work!  What would you like to do? \n 1: Employee info. \n 2: Check on an animal \n 3: List of Animals \n 4: Log off");
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
                    Console.Clear();
                    ListAnimalsInOrder(employee);
                    break;
                case "4":
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
            Console.WriteLine("Please select what to check. \n 0: Load CSV file. \n 1: Add An Animal \n 2: Shots \n 3: Food \n 4: Collect Money \n 5: Verify Adoption \n 6: Check Rooms \n 7: User Menu \n 8: Log off");
            string choice = Console.ReadLine();

            switch(choice)
            {
                case "0":
                    Console.Clear();
                    LoadCSVFile(employee);
                    break;
                case "1":
                    Console.Clear();
                    AddAnimal(employee);
                    break;
                case "2":
                    Console.Clear();
                    GetShotMenu(employee);
                    break;
                case "3":
                    Console.Clear();
                    GetFoodMenu(employee);
                    break;
                case "4":
                    Console.Clear();
                    CollectMoney(employee);
                    break;
                case "5":
                    Console.Clear();
                    GetAdoptedStatus(employee);
                    break;
                case "6":
                    Console.Clear();
                    GetRoomList(employee);
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

            int gender = GetGender();

            AdmitAnimal(employee, animalName, vaccinated, age, price, adopted, adopter, animalType, room, color, gender);
        }

        public void AdmitAnimal(string employee, string name, bool vaccinated, int age, int price, bool adopted, int adopterID, int animalTypeID, int room, int color, int gender)
        {
            Animal animal = new Animal();

            animal.Name = name;
            animal.Vaccinated = vaccinated;
            animal.Age = age;
            animal.Price = price;
            animal.Adopted = adopted;
            animal.AdopterID = adopterID;
            animal.AnimalTypeID = animalTypeID;

            animal.RoomID = GetRoomID(room);
            animal.ColorID = color;
            animal.GenderID = gender;

            database.Animals.InsertOnSubmit(animal);
            database.SubmitChanges();

            Console.WriteLine("The animal as been added to the database. \n Press enter to return to the menu.");
            Console.ReadLine();
            Console.Clear();
            GetEmployeeMenu(employee);

        }

        public int GetRoomID(int room)
        {
            int roomNumberID = room;

            var roomID = from rooms in database.Rooms
                         where rooms.RoomNumber == room
                         select rooms;

            foreach(Room id in roomID)
            {
                roomNumberID = id.RoomID;
            }

            return roomNumberID;
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
            int adopter = 2;

            Console.WriteLine("Does this animal have an adopter, yes or no.");
            string answer = Console.ReadLine();

            switch(answer)
            {
                case "yes":
                    adopter = GetAdopterID();
                    break;
                case "no":
                    adopter = 2;
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
            var adopter = database.Adopters.Where(x => x.AdopterID == check).ToList();

            if(adopter.Count == 0)
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

            animalType = VerifyTypeID(null, animalType);

            return animalType;
        }

        public int VerifyAnimalID(int id)
        {
            int animalId = id;

            var animals = database.Animals.Where(x => x.AnimalID == animalId).ToList();

            if(animals.Count == 0)
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
                Console.WriteLine($"Room: {number.RoomNumber} \t ID: {number.RoomID}");
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

            var number = database.Rooms.Where(x => x.RoomNumber == roomNumber).ToList();
            
                if (number.Count == 0)
                {
                    Room newRoom = new Room();
                    newRoom.RoomNumber = room;

                    database.Rooms.InsertOnSubmit(newRoom);
                    database.SubmitChanges();
                    roomNumber = room;
                }
                else
                {
                    Console.WriteLine("That room is already taken.");
                    Console.ReadLine();
                    Console.Clear();
                    GetRoom();
                }
   
            return roomNumber;
        }

        public int GetAnimalColor()
        {
            int color = 0;

            Console.WriteLine("Here is a list of colors.  Please choose an ID of the color that matches closest to the animals color.");

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

            var idNumbers = database.Colors.Where(x => x.ColorID == colorID).ToList();

            if(idNumbers.Count == 0)
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

        public int GetGender()
        {
            int gender = 0;
            Console.WriteLine("Please select the gender: \n 1: Male \n 2: Female");
            string choice = Console.ReadLine();
            
            switch(choice)
            {
                case "1":
                    gender = 1;
                    break;
                case "2":
                    gender = 2;
                    break;
                default:
                    Console.WriteLine("You did not input 1 or 2.");
                    Console.ReadLine();
                    Console.Clear();
                    GetGender();
                    break;
            }

            return gender;
        }

        public void GetShotMenu(string employee)
        {
            int animalID = 0;
            Console.WriteLine("Please select the ID of the animal you wish to check?");

            ListOfAnimals();

            try
            {
                animalID = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("You have an invalid entry.");
                Console.ReadLine();
                Console.Clear();
                GetShotMenu(employee);
            }

            animalID = VerifyAnimalID(animalID);
            GetVaccination(employee, animalID);
        }

        public void GetVaccination(string employee, int animalID)
        {
            var vaccinated = from animals in database.Animals
                              where animals.AnimalID == animalID
                              select animals;

            foreach(Animal animal in vaccinated)
            {
                if(animal.Vaccinated.Equals(false))
                {
                    GetVaccinated(employee, animalID);
                }
                else
                {
                    Console.WriteLine("This animal is already vaccinated. \n Please press enter to return to main menu.");
                    Console.ReadLine();
                    Console.Clear();
                    GetEmployeeMenu(employee);
                }
            }
        }

        public void GetVaccinated(string employee, int animalID)
        {
            Console.WriteLine("This animal does not have it's shots. \n Do you wish to give it shots, yes or no?");
            string choice = Console.ReadLine();
            
            switch(choice)
            {
                case "yes":
                    ChangeVaccinatedStatus(employee, animalID);
                    break;
                case "no":
                    Console.WriteLine("You have choosen \"no\".  Please press enter to return to the main menu.");
                    Console.ReadLine();
                    Console.Clear();
                    GetEmployeeMenu(employee);
                    break;
                default:
                    Console.WriteLine("You did not input yes or no.");
                    GetVaccinated(employee, animalID);
                    break;
            }
        }

        public void ChangeVaccinatedStatus(string employee, int animalID)
        {
            var animal = from animals in database.Animals
                         where animals.AnimalID == animalID
                         select animals;

            foreach(Animal animals in animal)
            {
                animals.Vaccinated = true;
                database.SubmitChanges();
            }

            Console.WriteLine("You have given this animal it's shots. \n Please press enter to return to the main menu.");
            Console.ReadLine();
            Console.Clear();
            GetEmployeeMenu(employee);
        }

        public void GetFoodMenu(string employee)
        {
            int id = 0;
            Console.WriteLine("Here is a list of the type of animals.  Please choose the ID of the animal you plan to feed.");
            ListAnimalTypes();
            
            try
            {
                id = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("Your input was not valid.");
                Console.ReadLine();
                Console.Clear();
                GetFoodMenu(employee);
            }

            id = VerifyTypeID(employee, id);

            GetFoodAmount(employee, id);
        }

        public int VerifyTypeID(string employee, int id)
        {
            int typeID = id;

            var type = database.AnimalTypes.Where(x => x.AnimalTypeID == typeID).ToList();

            if(type.Count == 0)
            {
                Console.WriteLine("The type you selected was not found.");
                Console.ReadLine();
                Console.Clear();
                GetFoodMenu(employee);
            }
            else
            {
                typeID = id;
            }

            return typeID;
        }

        public void GetFoodAmount(string employee, int id)
        {
            var animalyType = from types in database.AnimalTypes
                              where types.AnimalTypeID == id
                              select types;

            foreach(AnimalType type in animalyType)
            {
                Console.WriteLine($"Animal Type: {type.TypeOfAnimal} \t Food Type: {type.FoodType}");
            }

            GetAmountPerWeek(employee, id);
        }

        public void GetAmountPerWeek(string employee, int id)
        {
            int weeks = 0;
            Console.WriteLine("How many weeks are you feeding this animal for?");
            
            try
            {
                weeks = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("You did not input a number.");
                Console.ReadLine();
                Console.Clear();
                GetAmountPerWeek(employee, id);
            }

            GetServingSize(employee, id, weeks);
        }

        public void GetServingSize(string employee, int id, int weeks)
        {
            int servingSize = 0;
            var animalType = from type in database.AnimalTypes
                             where type.AnimalTypeID == id
                             select type;

            foreach(AnimalType animal in animalType)
            {
                var foodtypes = from food in database.FoodTypes
                               where food.FoodTypeID == animal.FoodTypeID
                               select food;

                foreach(FoodType type in foodtypes)
                {
                    servingSize = type.WeeklyServing * weeks;
                    Console.WriteLine($"You would need {servingSize} per week for this animal.");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Please press enter to return to the main menu.");
            Console.ReadLine();
            Console.Clear();
            GetEmployeeMenu(employee);
        }

        public void CollectMoney(string employee)
        {
            int adopterID = 0;
            Console.WriteLine("Which adopter ID is adopting a new animal?");
            ListOfAdopters();

            try
            {
                adopterID = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("You have an invalid entry.");
                Console.ReadLine();
                Console.Clear();
                CollectMoney(employee);
            }

            adopterID = VerifyID(adopterID);
            GetAnimal(employee, adopterID);
        }

        public void GetAnimal(string employee, int adopterID)
        {
            int animalID = 0;
            Console.WriteLine("Which animal ID belongs to the animal that is being adopted?");
            ListOfAnimals();
            
            try
            {
                animalID = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("You have an invalid entry.");
                Console.ReadLine();
                Console.Clear();
                GetAnimal(employee, adopterID);
            }

            animalID = VerifyAnimalID(animalID);
            GetPrice(employee, adopterID, animalID);
        }

        public void GetPrice(string employee, int adopterID, int animalID)
        {
            int price = 0;
            var animals = from animal in database.Animals
                        where animal.AnimalID == animalID
                        select animal;

            foreach(Animal animal in animals)
            {
                price = animal.Price.Value;
            }

            Console.WriteLine($"The price of the animal is ${price}.");
            Console.ReadLine();

            PayForAnimal(employee, adopterID, animalID, price);
        }

        public void PayForAnimal(string employee, int adopterID, int animalID, int price)
        {
            int payment = 0;
            Console.WriteLine("How much is the adoptee paying?");

            try
            {
                payment = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("Invalid input.");
                Console.ReadLine();
                PayForAnimal(employee, adopterID, animalID, price);
            }

            if(payment > price)
            {
                int change = payment - price;
                Console.WriteLine($"The change will be {change}.");
                PaymentProcess(employee, adopterID, animalID, price);
            }
            else if(payment < price)
            {
                DeniedProcess(employee, adopterID, animalID, price);
            }
            else if(payment == price)
            {
                Console.WriteLine("Congratulations to the new pet owner.");
                PaymentProcess(employee, adopterID, animalID, price);
            }
        }

        public void PaymentProcess(string employee, int adopterID, int animalID, int price)
        {
            Payment payment = new Payment();
            Animal animal = new Animal();
            DateTime dateTime = DateTime.Today;
            payment.AmountPaid = price;
            payment.AnimalID = animalID;
            payment.DatePaid = dateTime;
            ChangeAdoptionStatus(adopterID, animalID);

            database.Payments.InsertOnSubmit(payment);
            database.SubmitChanges();

            Console.WriteLine("Please press enter to proceed back to the main menu.");
            Console.ReadLine();
            Console.Clear();
            GetEmployeeMenu(employee);
        }

        public void ChangeAdoptionStatus(int adopterId, int animalID)
        {
            var animals = from animal in database.Animals
                         where animal.AnimalID == animalID
                         select animal;
             
            foreach(Animal animal in animals)
            {
                animal.Adopted = true;
                animal.AdopterID = adopterId;

                database.Animals.InsertOnSubmit(animal);
                database.SubmitChanges();
            }
        }

        public void DeniedProcess(string employee, int adopterID, int animalID, int price)
        {
            Console.WriteLine("This is not enough for this animal. Does the adopter wish to change the amount, yes or no?");
            string choice = Console.ReadLine();

            switch(choice)
            {
                case "yes":
                    PayForAnimal(employee, adopterID, animalID, price);
                    break;
                case "no":
                    Console.WriteLine("Please press enter to go back to the main screen.");
                    Console.ReadLine();
                    Console.Clear();
                    GetEmployeeMenu(employee);
                    break;
                default:
                    Console.WriteLine("You did not input a valid option.");
                    Console.ReadLine();
                    DeniedProcess(employee, adopterID, animalID, price);
                    break;
            }
        }

        public void GetAdoptedStatus(string employee)
        {
            int animalID = 0;
            Console.WriteLine("What is the ID of the animal you wish to check on.");
            ListOfAnimals();

            try
            {
                animalID = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("You have an invalid entry.");
                Console.ReadLine();
                Console.Clear();
                GetAdoptedStatus(employee);
            }

            animalID = VerifyAnimalID(animalID);
            CheckStatus(employee, animalID);
        }

        public void CheckStatus(string employee, int animalID)
        {
            var animals = from animal in database.Animals
                          where animal.AnimalID == animalID
                          select animal;

            foreach(Animal animal in animals)
            {
                if(animal.Adopted.Equals(false))
                {
                    Console.WriteLine($"{animal.Name} has not been adopted yet.");
                }
                else
                {
                    Console.WriteLine($"{animal.Name} is already adopted.");
                }
            }

            Console.WriteLine("Please press enter to return to the main menu.");
            Console.ReadLine();
            Console.Clear();
            GetEmployeeMenu(employee);
        }

        public void GetRoomList(string employee)
        {
            Console.WriteLine("Here is a list of animals and their rooms.");

            var animal = from animals in database.Animals
                         select animals;

            

            foreach (Animal animals in animal)
            {
                var room = from rooms in database.Rooms
                           where rooms.RoomID == animals.RoomID
                           select rooms;

                foreach(Room rooms in room)
                {
                    Console.WriteLine($"Name: {animals.Name} \t Room: {rooms.RoomNumber}");
                }
            }

            Console.WriteLine("Please press enter to return to the main menu.");
            Console.ReadLine();
            Console.Clear();
            GetEmployeeMenu(employee);
        }

        public void ListOfAnimals()
        {
            var animal = from animals in database.Animals
                    select animals;

            foreach(Animal animals in animal)
            {
                Console.WriteLine($"Name: {animals.Name} \t ID: {animals.AnimalID}");
            }
        }

        public void ListAnimalTypes()
        {
            var animalType = from types in database.AnimalTypes
                             select types;

            foreach(AnimalType type in animalType)
            {
                Console.WriteLine($"Type: {type.TypeOfAnimal} \t ID: {type.AnimalTypeID}");
            }
        }

        public void ListOfAdopters()
        {
            var adoptee = from adopter in database.Adopters
                          select adopter;

            foreach(Adopter adopter in adoptee)
            {
                Console.WriteLine($"First Name: {adopter.FirstName} \t Last Name: {adopter.LastName} \t ID: {adopter.AdopterID}");
            }
        }

        public void ListColor()
        {
            var colors = from color in database.Colors
                         select color;

            foreach(Color color in colors)
            {
                Console.WriteLine($"{color.Color1} \t ID: {color.ColorID}");
            }
        }

        public void ListAnimalsInOrder(string employee)
        {
            int type = 0;
            Console.WriteLine("Here is a list of animal types, which animal of the specific type do you wish to see, please input the type ID? If you wish to see entire list input 0.");
            ListAnimalTypes();

            try
            {
                type = int.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Console.WriteLine("You have an invalid input.");
                Console.ReadLine();
                Console.Clear();
                ListAnimalsInOrder(employee);
            }

            if (type == 0)
            {
                ListAllTypes(employee);
            }
            else
            {
                type = VerifyTypeID(employee, type);
                OrderAnimals(employee, type);
            }      
        }

        public void ListAllTypes(string employee)
        {
            var animals = database.Animals.Where(x=>x.AnimalType==x.AnimalType).Select(x=>x).ToList();

            foreach (Animal animal in animals.OrderBy(x=>x.AnimalTypeID))
            {
                Console.WriteLine($"Name: {animal.Name} \t Type: {animal.AnimalType.TypeOfAnimal} \t Age: {animal.Color.Color1} \t Price: {animal.Price} \t Vaccinated: {animal.Vaccinated} \t Gender: {animal.Gender.GenderType} \t Adopted: {animal.Adopted}");
            }
            Console.WriteLine("Please press enter to return to he main menu.");
            Console.ReadLine();
            Console.Clear();
            GetEmployeeMenu(employee);
        }

        public void OrderAnimals(string employee, int type)
        {
            var animal = database.Animals.Where(x => x.AnimalTypeID == type).Select(x=>x).ToList();

            foreach(Animal animals in animal)
            {
                Console.WriteLine($"Name: {animals.Name} \t Type: {animals.AnimalType.TypeOfAnimal} \t Age: {animals.Color.Color1} \t Price: {animals.Price} \t Vaccinated: {animals.Vaccinated} \t Gender: {animals.Gender.GenderType} \t Adopted: {animals.Adopted}");
            }
            Console.WriteLine("Please press enter to return to he main menu.");
            Console.ReadLine();
            Console.Clear();
            GetEmployeeMenu(employee);
        }

        public void LoadCSVFile(string employee)
        {
            Console.WriteLine("Location of the csv file? Please include drive \"c:\\\".");
            string location = Console.ReadLine();

            var csv = File.ReadLines($@"{location}").Select(x => x.Split(','));

            foreach(var info in csv)
            {
                Animal animal = new Animal();
                animal.Name = info[0];
                animal.Vaccinated = Convert.ToBoolean(info[1]);
                animal.Age = int.Parse(info[2]);
                animal.Price = int.Parse(info[3]);
                animal.Adopted = Convert.ToBoolean(info[4]);
                animal.AdopterID = int.Parse(info[5]);
                animal.AnimalTypeID = int.Parse(info[6]);
                animal.RoomID = int.Parse(info[7]);
                animal.ColorID = int.Parse(info[8]);
                animal.GenderID = int.Parse(info[9]);

                database.Animals.InsertOnSubmit(animal);
                database.SubmitChanges();
            }
        }
    }
}
