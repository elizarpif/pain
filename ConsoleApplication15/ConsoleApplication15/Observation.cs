using System;
using System.Collections.Generic;
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
        private Dictionary<Patient, int> patientSeats;

        private SemaphoreSlim doctorSemaphore;
        private SemaphoreSlim observationSeats;

        private int processTime = 60;

        public Observation(int d, int s)
        {
            countDoctors = d;
            countSeats = s;
            patientSeats = new Dictionary<Patient, int>();
            doctorSemaphore = new SemaphoreSlim(countDoctors, countDoctors);
            observationSeats = new SemaphoreSlim(countSeats, countSeats);
        }

        public void NewPatient(Patient p)
        {
            observationSeats.Wait();
            Thread patientProcess = new Thread(PatientProcess);
            patientProcess.Start(p);
        }

        // проуесс пациента в смотровой
        // ожидает доктора
        // доктор занимается с ним рандомное время в от 1 до минуты
        public void PatientProcess(Object patientObject)
        {
            Patient patient = (Patient) patientObject;

            patientSeats.Add(patient, patientSeats.Count + 1);
            Console.WriteLine($"-- {patient.GetName()} зашла в смотровую и села в ожидании доктора");

            doctorSemaphore.Wait(); // ждет доктора

            Random rndTime = new Random();
            int patientTime = rndTime.Next(processTime - 1) + 1;

            Console.WriteLine($"-- доктор занимается с пациентом {patient.GetName()} ({patientTime} сек)");
            
            Thread.Sleep(patientTime * 1000);

            doctorSemaphore.Release();

            Console.WriteLine($"-- {patient.GetName()} покидает смотровую");

            observationSeats.Release();
        }
    }
}