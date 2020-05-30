using System;
using System.Collections.Generic;
using System.Net;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace ConsoleApplication15
{
    // смотровая
    // может зайти
    // 1. любой человек, если есть свободное место
    // 2. не заболевший человек, если в смотровой нет заболевших
    // 3. при наличии свободных мест заболевший человек может войти в смотровую, если там только заболевшие.
    public class Observation
    {
        private int countSeats;
        private int countDoctors;

        private SemaphoreSlim doctorSemaphore;
        private SemaphoreSlim observationSeats;
        private static Mutex m = new Mutex();
        private static Patient lastSeekPatient;

        private static ConcurrentQueue<Patient> patientSeats;

        private int processTime = 60;

        public Observation(int d, int s)
        {
            countDoctors = d;
            countSeats = s;
            
            doctorSemaphore = new SemaphoreSlim(countDoctors, countDoctors);
            observationSeats = new SemaphoreSlim(countSeats, countSeats);
            
            Console.WriteLine($"количество свободных мест в смотровой: {observationSeats.CurrentCount}");
            Console.WriteLine($"количество свободных докторов в смотровой: {doctorSemaphore.CurrentCount}");

            
            patientSeats = new ConcurrentQueue<Patient>();
        }
        // Процесс доктор-пациент - это поток
        // у одного пациента может быть несколько докторов
        
        public void NewPatient(Patient p)
        {
            observationSeats.Wait();
            if (p.IsSick())
            {
                lastSeekPatient = p;
            }
            patientSeats.Enqueue(p);
            Console.WriteLine($"-- {p.GetName()} зашла в смотровую и села в ожидании доктора");
            Thread patientProcess = new Thread(PatientProcess);
            patientProcess.Start();
        }

        // проуесс пациента в смотровой
        // ожидает доктора
        // доктор занимается с ним рандомное время в от 1 до минуты
        // надо как-то добавить возможность доктора пригласить себе в помощь другого доктора
        public void PatientProcess()
        {
            Console.WriteLine($"количество свободных мест в смотровой: {observationSeats.CurrentCount}");
            Console.WriteLine($"количество свободных докторов в смотровой: {doctorSemaphore.CurrentCount}");
            
            Console.WriteLine();
            doctorSemaphore.Wait(); // ждет доктора

            Patient patient;
            while (!patientSeats.TryDequeue(out patient)){}
            

            Random rndTime = new Random();
            int patientTime = rndTime.Next(processTime - 1) + 1;

            Console.WriteLine($"-- доктор занимается с пациентом {patient.GetName()} ({patientTime} сек)");

            Thread.Sleep(patientTime * 1000);

            if (lastSeekPatient == patient)
            {
                lastSeekPatient = null;
            }
            
            //while (!patientSeats.TryDequeue(out patient)){}

            doctorSemaphore.Release();
            observationSeats.Release();
            
            Console.WriteLine($"-- {patient.GetName()} покидает смотровую");
            Console.WriteLine($"количество свободных мест в смотровой: {observationSeats.CurrentCount}");
            Console.WriteLine($"количество свободных докторов в смотровой: {doctorSemaphore.CurrentCount}");
        }
    }
}