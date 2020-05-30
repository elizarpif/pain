namespace ConsoleApplication15
{
    public class Params
    {
        private int seatsCount;

        public int Seats
        {
            get { return seatsCount; }
            set { seatsCount = value; }
        }

        private int doctorsCount;

        public int Doctors
        {
            get { return doctorsCount; }
            set { doctorsCount = value; }
        }

        private int genPatientTime;

        public int GeneratePatientTime
        {
            get { return genPatientTime; }
            set { genPatientTime = value; }
        }

        private int genSickTime;

        public int GenerateSickTime
        {
            get { return genSickTime; }
            set { genSickTime = value; }
        }

        private int processTime;

        public int DoctorProcessTime
        {
            get { return processTime; }
            set { processTime = value; }
        }

        private int genHelpDoctor;

        public int GenerateHelpDoctor
        {
            get { return genHelpDoctor; }
            set { genHelpDoctor = value; }
        }

        public Params()
        {
        }
    }
}