using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

namespace ConsoleApplication15
{
    public class Clinic
    {
        private Observation observation;
        private ConcurrentQueue<Patient> patientsQueue;
        private Logger logger;

        private Params pParams;
        private Patient lastSickPatient;

        public Clinic(Params p)
        {
            pParams = p;
            patientsQueue = new ConcurrentQueue<Patient>();
            logger = new Logger();
            
            observation = new Observation(p.Doctors, p.Seats, p.DoctorProcessTime, p.GenerateHelpDoctor, logger);
        }


        public void Start()
        {
            Thread patientsObservation = new Thread(WaitSeatInObservation);
            patientsObservation.Start();

            Thread sicker = new Thread(Infection);
            sicker.Start();
            
            GenNewPatients();
        }

        public void WaitSeatInObservation()
        {
            while (true)
            {
                if (patientsQueue.Count > 0)
                {
                    Patient p;
                    while (!patientsQueue.TryPeek(out p)) {}

                    bool free = observation.isFree();
                    bool isSickObs = observation.isSickObservation();
                    bool isSickPat = p.IsSick();
                    
                    if (free || (isSickObs && isSickPat) || (!isSickObs && !isSickPat))
                    {
                        // чтобы отследить очередь распечатаем
                        if (patientsQueue.Count > 1)
                            printQueue();

                        while (!patientsQueue.TryDequeue(out p)) {}

                        if (p == lastSickPatient)
                        {
                            lastSickPatient = null;
                        }
                        
                        observation.NewPatient(p);
                    }
                }
            }
        }

        public void GenNewPatients()
        {
            while (true)
            {
                Patient patient = new Patient();

                patientsQueue.Enqueue(patient);
                
                if (patient.IsSick())
                {
                    lastSickPatient = patient;
                }
                
                Random rnd = new Random();
                int time = rnd.Next(pParams.GeneratePatientTime) + 1;
                
                Thread.Sleep(1000 * time);
            }
        }

        // все в очереди заражаюdтся
        public void Infection()
        {
            Random rnd = new Random();
            int time = (rnd.Next(pParams.GenerateSickTime) + 1) * 1000;
            while (true)
            {
                Thread.Sleep(time);
                if (lastSickPatient != null)
                {
                    printQueue();

                    bool isHealthy = true;
                    
                    foreach (Patient p in patientsQueue)
                    {
                        if (isHealthy && !p.IsSick())
                        {
                            isHealthy = false;
                            
                            // цвет консоли только для текущего потока ?
                            logger.PrintRed($"заболевший: {lastSickPatient.GetName()} заражает всех!");
                        }
                        p.SetSick();
                        lastSickPatient = p;
                    }

                    time = (rnd.Next(pParams.GenerateSickTime) + 1) * 1000;
                }
            }
        }

        public void printQueue()
        {
            if (patientsQueue.Count == 0)
            {
                return;
            }

            int i = 1;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Очередь: \n");

            foreach (Patient p in patientsQueue)
            {
                stringBuilder.Append($"{i}) {p.GetName()} : {p.GetState()}\n");

                i++;
            }

            logger.PrintBlack(stringBuilder.ToString());
        }
    }
}