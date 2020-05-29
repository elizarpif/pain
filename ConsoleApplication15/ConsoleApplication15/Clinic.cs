using System;
using System.Collections;
using System.Threading;

namespace ConsoleApplication15
{
    public class Clinic
    {
        public const int Second = 60;

        private Observation _observation;
        private Queue _queue;
        private int T;
        private int doctorsCount;
        private int seatsCount;


        public void SetRandomTime()
        {
            Random rnd = new Random();
            T = rnd.Next() % 60 + 1;
        }

        public Clinic(int doctors, int patients)
        {
            _observation = new Observation(doctors, patients);
            _queue = new Queue();
        }


        public void Start()
        {
            Thread patientsObservation = new Thread(WaitSeatInObservation);
            patientsObservation.Start();
            GenNewPatients();
        }

        public void WaitSeatInObservation()
        {
            while (true)
            {
                if (_queue.Count > 0)
                {
                    try
                    {
                        Patient p = (Patient) _queue.Dequeue();
                        _observation.NewPatient(p);
                    }
                    catch (ObservationException obsEx)
                    {
                        Console.WriteLine(obsEx.ToString());
                    }
                }
            }
        }

        public void GenNewPatients()
        {
            while (true)
            {
                Patient patient = new Patient();

                _queue.Enqueue(patient);
                printQueue();
                Thread.Sleep(3000);
            }
        }

        public void printQueue()
        {
            Mutex m = new Mutex();
            m.WaitOne();
            if (_queue.Count==0)
            {
                return;
            }
            
            int i = 1;
            Console.WriteLine("Очередь:");
            
            foreach (var obj in _queue)
            {
                Patient p = (Patient) obj;
                Console.WriteLine($"{i}) {p.GetName()} : {p.GetState()}");
                i++;
            }
            m.ReleaseMutex();
        }
    }
}