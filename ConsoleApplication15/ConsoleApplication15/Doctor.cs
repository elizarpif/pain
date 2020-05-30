namespace ConsoleApplication15
{
    public class Doctor
    {
        private static int sumID = 0;
        private int id = 0;

        Doctor()
        {
            sumID++;
            id = sumID;
        }

        public int GetID()
        {
            return id;
        }
    }
}