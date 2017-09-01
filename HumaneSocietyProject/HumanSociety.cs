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
                    GetAdoptee();
                    break;
                case "2":
                    GetEmployee();
                    break;
                default:
                    break;
            }
        }

        public void GetAdoptee()
        {

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
