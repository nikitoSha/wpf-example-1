using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpf_example_1.helpers;
using wpf_example_1.models.posts;
using System.Data.SQLite;

namespace wpf_example_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PostModel> posts = new List<PostModel>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void addPostsToLocalDB()
        {
            SQLiteManager.deletePosts();

            int currentPostIndex = 0;
            //string result = "";
            new AsyncHelper().doBackgroundWorker(
                //exec code in background
                () => {
                    SQLiteManager.addRowToPost(posts[currentPostIndex]);
                    currentPostIndex++;
                },
                //onProgress
                (int current, int total, double percent) => {
                    string currentProgressLine = string.Format("{0}/{1} ({2}%)", current, total, percent);
                    Console.WriteLine(currentProgressLine);
                    //result += currentProgressLine + "\n";
                },
                //onComplete
                () => {
                    MessageBox.Show("Сохранение постов в локальную БД SQLite завершено.");
                },
                posts.Count, 0
                );
        }

        private void loadPostsFromLocalDB()
        {
            mDataGrid.ItemsSource = null;
            if (!File.Exists("database.db"))
            {
                SQLiteManager.createDB();
                SQLiteManager.createTable_post();
                MessageBox.Show("В локальной базе нет данных. Загрузите данные с сервера и сохраните их локально.");
            }
            else
            {
                posts.Clear();
                posts = SQLiteManager.getPosts();
                updateTable();
                if (posts == null || posts.Count == 0)
                {
                    MessageBox.Show("В локальной базе нет данных. Загрузите данные с сервера и сохраните их локально.");                    
                }
            }

        }

        #region Menu

        private void loadFromDB_Click(object sender, RoutedEventArgs e)
        {
            mDataGrid.ItemsSource = null;
            loadPostsFromLocalDB();
        }

        private void saveToDB_Click(object sender, RoutedEventArgs e)
        {
            addPostsToLocalDB();
        }

        private void clearDB_Click(object sender, RoutedEventArgs e)
        {
            SQLiteManager.deletePosts();
            loadPostsFromLocalDB();
        }

        private void LoadFromNet_Click(object sender, RoutedEventArgs e)
        {
            getPosts();
        }

        private void createPostOnServer_Click(object sender, RoutedEventArgs e)
        {
            addNewPostOnServer();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion


        #region API-calls

        /// <summary>
        /// Обновить список постов из интернета
        /// </summary>
        private void getPosts()
        {
            mDataGrid.ItemsSource = null;
            RestApiManager.getPosts(
                (object answer) => {
                    AsyncHelper.doInMainThread(() => {
                        posts = (List<PostModel>)answer;
                        updateTable();
                    });
                },
                (string errMsg) =>
                {
                    AsyncHelper.doInMainThread(() => {
                        mDataGrid.ItemsSource = null;
                        MessageBox.Show(errMsg);
                    });
                });
        }

        /// <summary>
        /// Добавить новый пост на сервере
        /// </summary>
        private void addNewPostOnServer()
        {
            PostModel post = new PostModel();
            post.title = "test";
            post.body = "test body";
            post.userId = 1;

            RestApiManager.createPost(
                post,
                (object answer) => {                    
                    MessageBox.Show("Пост добавлен: id = " + ((PostModel)answer).id);
                },
                (string errMsg) =>
                {
                    mDataGrid.ItemsSource = null;
                    MessageBox.Show(errMsg);
                });
        }

        #endregion


        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void mDataGrid_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void mDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void mDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadPostsFromLocalDB();
        }

        private void updateTable()
        {
            mDataGrid.ItemsSource = null;
            mDataGrid.ItemsSource = posts;
        }


    }
}
