using System;
using System.Text;

namespace ConsoleApplication15
{
    public class Patient
    {
        private PatientStates state;
        private string name;
        private static int num = 0;

        public Patient()
        {
            Random rnd = new Random();
            if (rnd.Next() % 2 == 0)
            {
                state = PatientStates.Healthy;
            }
            else
            {
                state = PatientStates.Sick;
            }

            num++;
            name = GenerateName();
        }

        public static string GenerateName()
        {
            string[] firstNames = {"Тая", "Надя", "Айгуль", "Лиза", "Катя", "Лия"};
            string[] lastNames = {"Гольцова", "Прошутина", "Якупова", "Пивоварова", "Аладина", "Пянко"};

            Random rnd = new Random();
            StringBuilder name = new StringBuilder();

            name.Append(firstNames[rnd.Next(firstNames.Length)]);
            name.Append(" ");
            name.Append(lastNames[rnd.Next(lastNames.Length)]);
            name.Append(num);

            return name.ToString();
        }

        public string GetName()
        {
            return name;
        }

        public void SetSick()
        {
            state = PatientStates.Sick;
        }

        public string GetState()
        {
            if (state == PatientStates.Healthy)
            {
                return "здорова";
            }

            return "больна";
        }


        public bool IsSick()
        {
            switch (state)
            {
                case PatientStates.Sick:
                    return true;
                default:
                    return false;
            }
        }
    }
}