using NLog;
using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace PaintMeter.Classes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class PeriodicBackgroundWorker : BackgroundWorker
    {
        public bool InitializationPerformed { get; set; } = false;
        public bool GenerateLog { get; set; } = false;
        private DispatcherTimer timer = new DispatcherTimer();
        Logger logger = LogManager.GetCurrentClassLogger();

        public PeriodicBackgroundWorker()
        {
            ProgressChanged += new ProgressChangedEventHandler(CustomBackgroundWorker_ProgressChanged);
            RunWorkerCompleted += new RunWorkerCompletedEventHandler(CustomBackgroundWorker_RunWorkerCompleted);
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        public void InitializeTimer(int minutes, int seconds, int miliseconds)
        {
            timer = new DispatcherTimer();

            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, minutes, seconds, miliseconds);
            timer.Start();
        }

        public TimeSpan GetTimerInterval()
        {
            return timer.Interval;
        }

        private void CustomBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (WorkerSupportsCancellation == true)
                CancelAsync();
        }

        public void Work(Action initializationAction, Action executionAction, Action postInitializationAction = null)
        {
            if (!InitializationPerformed)
                Initialize(initializationAction, postInitializationAction);

            if (executionAction != null)
                Execute(executionAction);
        }

        private void Initialize(Action initializationAction, Action postInitializationAction = null)
        {
            try
            {
                initializationAction();
                InitializationPerformed = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
            }
            if (postInitializationAction != null)
            {
                postInitializationAction();
            }
        }

        public void Execute(Action executionAction)
        {
            try
            {
                executionAction();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
            }
        }

        private void CustomBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (IsBusy != true)
                RunWorkerAsync();
        }

        public void SetInterval(TimeSpan timeSpan)
        {
            timer.Interval = timeSpan;
        }

        public void StartTimer()
        {
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        public void ExecuteSingleTick()
        {
            Timer_Tick(new object(), new EventArgs());
        }
    }
}
