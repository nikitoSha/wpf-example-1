using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace wpf_example_1.helpers
{
    public class AsyncHelper
    {

        public delegate void AsyncCompletedEventHandler();
        public delegate void AsyncProgressEventHandler(int current, int total, double percent);

        #region параметры для фоновой работы через воркер

        private BackgroundWorker _backgroundWorker;
        Action backgroundWorkingArea;
        AsyncProgressEventHandler onProgress;
        AsyncCompletedEventHandler onCompleted;
        int total = 0; 
        int start = 0;
        int current = 0;

        public int Total { get => total; set => total = value; }
        public int Start { get => start; set => start = value; }

        #endregion

        public AsyncHelper()
        {
            _backgroundWorker = new BackgroundWorker();
        }

        /// <summary>
        /// Выполнить в главном потоке
        /// </summary>
        /// <param name="action">Фрагмент кода (процедура)</param>
        public static void doInMainThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        /// <summary>
        /// Выполнить в фоновом потоке
        /// </summary>
        /// <param name="action">Фрагмент кода (процедура)</param>
        public static void doInBackgroundThread(Action action)
        {
            Thread backThread = new Thread(new ThreadStart(action));
            backThread.Start();
        }



        /// <summary>
        /// Начать выполнение кода в фоновом процессе
        /// </summary>
        /// <param name="executableCode">Блок кода, который надо выполнить в фоне</param>
        /// <param name="onProgress"></param>
        /// <param name="onCompleted"></param>
        /// <param name="total"></param>
        /// <param name="start">Начинается с 0</param>
        public void doBackgroundWorker(
            Action executableCode, AsyncProgressEventHandler onProgress, AsyncCompletedEventHandler onCompleted, 
            int total = 0, int start = 0)
        {
            this.onProgress = onProgress;
            this.onCompleted = onCompleted;
            Total = total;
            Start = start;
            current = start;
            backgroundWorkingArea = executableCode;

            _backgroundWorker.WorkerReportsProgress = false;
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += new DoWorkEventHandler(bw_DoWork);
            _backgroundWorker.RunWorkerAsync();
        }        

        public void stopBackgroundWorker()
        {
            _backgroundWorker.CancelAsync();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_backgroundWorker != null)
            {
                if (total > 0)
                {
                    if (current < Total)
                    {
                        while (current < Total)
                        {
                            if (_backgroundWorker.CancellationPending == true)
                            {
                                e.Cancel = true;
                                break;
                            }
                            else
                            {
                                //Выполним блок кода в месте запуска (родительский экземпляр класса)
                                backgroundWorkingArea.Invoke();
                                current++;
                                double percent = (double)current / (double)Total * 100.0;
                                //уведомим о прогрессе
                                onProgress.Invoke(current, Total, percent);
                            }
                        }
                        if (onCompleted != null)
                        {
                            onCompleted.Invoke();
                        }
                    }
                    else
                    {
                        throw new Exception("Total должен быть больше start.");
                    }
                }
                else
                {
                    backgroundWorkingArea.Invoke();
                    onCompleted.Invoke();
                }                
            }
            else
            {
                throw new Exception("Не получилось запустить фоновый процесс.");
            }
        }

    }
}
