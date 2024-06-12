using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;


namespace KursWork
{
    public partial class Form1 : Form
    {

        public SqlConnection sqlConnection = null;
        public static string LoggedInUsername { get; private set; } // Сохранение имени пользователя
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["LangDB"].ConnectionString);

            sqlConnection.Open();

            if (sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Подключение установлено!");
            }
            else
                MessageBox.Show("Не удалось установить подключение(");

        }

        private void RegButt_Click(object sender, EventArgs e)
        {
            string username = UsernameBox.Text;
            string query = "SELECT COUNT(1) FROM [User] WHERE Username=@username";
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@username", username);

                int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                if (userCount > 0)
                {
                    MessageBox.Show("Пользователь ужэ существует. Выберите другое имя.");
                    return;
                }
            }

            // Insert the new user into the database
            query = "INSERT INTO [user] (username) VALUES (@username)";
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Пользователь успешно зарегестрирован.");
        }

        private void EnterButt_Click(object sender, EventArgs e)
        {
            string username = UsernameBox.Text;
            string query = "SELECT COUNT(1) FROM [user] WHERE [username]=@username";
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@username", username);

                int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                if (userCount > 0)
                {
                    LoggedInUsername = username;


                    // Переход на Form2
                    FormLang form2 = new FormLang();
                    form2.Show();
                    this.Hide();
                    //sqlConnection.Close();
                }
                else
                {
                    MessageBox.Show("Несуществующее имя. Пожалуйства зарегстрируйтесь.");
                }
            }
        }
    }
}
