using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_example_1.models.posts;

namespace wpf_example_1.helpers
{
    public class SQLiteManager
    {

        public static void createDB()
        {
            SQLiteConnection.CreateFile("database.db");
        }

        public static void createTable_post()
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=database.db; Version=3;"))
            {
                string commandText = "CREATE TABLE IF NOT EXISTS [Post] ( [id] INTEGER PRIMARY KEY NOT NULL, [user_id] INTEGER, [title] TEXT, [body] TEXT)";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery();
                Connect.Close();
            }
        }

        public static void deletePosts()
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=database.db; Version=3;"))
            {
                string commandText = "DELETE FROM [Post]";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery();
                Connect.Close();
            }
        }

        public static void addRowToPost(PostModel post)
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=database.db; Version=3;"))
            {
                string commandText = string.Format("INSERT INTO [Post](id, user_id, title, body) " +
                    "VALUES({0}, {1}, '{2}', '{3}')", post.id, post.userId, post.title, post.body);
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery();
                Connect.Close();
            }
        }

        public static List<PostModel> getPosts()
        {
            List<PostModel> posts = new List<PostModel>();

            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=database.db; Version=3;"))
            {
                string commandText = "SELECT id, user_id, title, body FROM [Post]";

                Connect.Open();
                SQLiteCommand Command = new SQLiteCommand
                {
                    Connection = Connect,
                    CommandText = commandText
                };                
                SQLiteDataReader sqlReader = Command.ExecuteReader();
                while (sqlReader.Read())
                {
                    PostModel post = new PostModel();
                    post.id = (Int64)sqlReader["id"];
                    post.userId = (Int64)sqlReader["user_id"];
                    post.title = (string)sqlReader["title"];
                    post.body = (string)sqlReader["body"];
                    posts.Add(post);
                }
                Connect.Close();
            }

            return posts;
        }
    }
}
