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

        private static Patient lastSeekPatient;

        private static ConcurrentQueue<Patient> patientSeats;

        private int processTime;
        private int helpDoctor;

        private Logger logger;

        public Observation(int d, int s, int T, int help, Logger l)
        {
            countDoctors = d;
            countSeats = s;
            processTime = T;
            helpDoctor = help;

            logger = l;

            doctorSemaphore = new SemaphoreSlim(countDoctors, countDoctors);
            observationSeats = new SemaphoreSlim(countSeats, countSeats);

            logger.PrintBlack($"количество свободных мест в смотровой: {observationSeats.CurrentCount}");
            logger.PrintBlack($"количество свободных докторов в смотровой: {doctorSemaphore.CurrentCount}");

            patientSeats = new ConcurrentQueue<Patient>();
        }

        public void NewPatient(Patient p)
        {
            observationSeats.Wait();
            if (p.IsSick())
            {
                lastSeekPatient = p;
            }

            patientSeats.Enqueue(p);

            logger.PrintBlack($"-- {p.GetName()} ({p.GetState()}) зашла в смотровую и села в ожидании доктора");

            Thread patientProcess = new Thread(PatientProcess);
            patientProcess.Start();
        }

        public bool isSickObservation()
        {
            if (lastSeekPatient != null)
                return true;
            return false;
        }

        public bool isFree()
        {
            if (observationSeats.CurrentCount == countSeats)
                return true;
            return false;
        }

        // проуесс пациента в смотровой
        // ожидает доктора
        // доктор занимается с ним рандомное время в от 1 до T
        // надо как-то добавить возможность доктора пригласить себе в помощь другого доктора
        public void PatientProcess()
        {
            logger.PrintBlack($"количество свободных мест в смотровой: {observationSeats.CurrentCount}");
            logger.PrintBlack($"количество свободных докторов в смотровой: {doctorSemaphore.CurrentCount}");

            doctorSemaphore.Wait(); // ждет доктора

            Patient patient;
            while (!patientSeats.TryDequeue(out patient))
            {
            }

            Random rndTime = new Random();
            int patientTime = rndTime.Next(processTime) + 1;

            logger.PrintBlack(
                $"-- доктор занимается с пациентом {patient.GetName()} ({patientTime} сек)");

            Thread.Sleep(patientTime * 1000);

            // в особых случаях доктору требуется помощь другого доктора

            if (doctorSemaphore.CurrentCount > 0 && rndTime.Next(helpDoctor) % 2 == 0)
            {
                doctorSemaphore.Wait();
                
                logger.PrintBlack(
                    $"-- доктор пришел на помощь к другому доктору с пациентом {patient.GetName()} ({patientTime} сек)");
                Thread.Sleep(1000 * (rndTime.Next(processTime) + 1));

                doctorSemaphore.Release();
            }

            if (lastSeekPatient == patient)
            {
                lastSeekPatient = null;
            }

            doctorSemaphore.Release();
            observationSeats.Release();

            logger.PrintBlack($"-- {patient.GetName()} покидает смотровую");
            logger.PrintBlack($"количество свободных мест в смотровой: {observationSeats.CurrentCount}");
            logger.PrintBlack($"количество свободных докторов в смотровой: {doctorSemaphore.CurrentCount}");
        }
    }
}