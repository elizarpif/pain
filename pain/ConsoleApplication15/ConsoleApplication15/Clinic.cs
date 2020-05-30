using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

namespace ConsoleApplication15
{
    public class Clinic
    {
        public const int Second = 60;

        private Observation _observation;
        private ConcurrentQueue<Patient> _queue;
        private static Mutex m = new Mutex();
        private int doctorsCount;
    
        private int seatsCount;

        private Patient lastSickPatient;
        public Clinic(int doctors, int patients)
        {
            _observation = new Observation(doctors, patients);
            _queue = new ConcurrentQueue<Patient>();
        }


        public void Start()
        {
            Thread patientsObservation = new Thread(WaitSeatInObservation);
            patientsObservation.Start();
            
            Thread sicker = new Thread(Sicker);
            sicker.Start();
            GenNewPatients();
        }

        public void WaitSeatInObservation()
        {
            while (true)
            {
                if (_queue.Count > 0)
                {
                    m.WaitOne();
                    // чтобы отследить очередь распечатаем
                    if (_queue.Count>1)
                        printQueue();
                    Patient p;
                    while (!_queue.TryDequeue(out p))
                    {
                        
                    }

                    if (p == lastSickPatient)
                    {
                        lastSickPatient = null;
                    }
                    printQueue();
                    m.ReleaseMutex();
                    _observation.NewPatient(p);

                }
            }
        }

        public void GenNewPatients()
        {
            while (true)
            {
                Patient patient = new Patient();

                _queue.Enqueue(patient);
                if (patient.IsSick())
                {
                    lastSickPatient = patient;
                }
               // printQueue();
                Random rnd = new Random();
                int time = rnd.Next(5) + 1;
                Thread.Sleep(1000*time);
            }
        }

        // все в очереди заражаюdтся
        public void Sicker()
        {
            Random rnd = new Random();
            int time = (rnd.Next(60) + 1)*1000;
            while (true)
            {
                Thread.Sleep(time);
                if (lastSickPatient != null)
                {
                    foreach (Patient p in _queue)
                    {
                        p.SetSick();
                        lastSickPatient = p;
                    }
                    time = (rnd.Next(60) + 1)*1000;
                    
                }
            }
        }

        public void printQueue()
        {
            if (_queue.Count == 0)
            {
                return;
            }
            
            int i = 1;
            
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Очередь: \n");
            
            foreach (var obj in _queue)
            {
                Patient p = (Patient) obj;
                
                stringBuilder.Append($"{i}) {p.GetName()} : {p.GetState()}\n");

                i++;
            }
            
            Console.WriteLine(stringBuilder.ToString());
        }
    }
}